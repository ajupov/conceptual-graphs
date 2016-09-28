using System.Drawing;
using Elan.Controllers.Contracts;
using Elan.Events;
using Elan.Helpers;
using Elan.Models.Base;
using Elan.Models.Contracts;
using Elan.Models.Implementations.Containers;
using Elan.Models.Implementations.Elements;

namespace Elan.Actions
{
    public class MoveAction
    {
        public delegate void OnElementMovingDelegate(ElementEventArgs e);

        private Document _document;

        private IMoveController[] _moveControllers;

        private OnElementMovingDelegate _onElementMovingDelegate;

        private Point _upperSelPoint = Point.Empty;

        private Point _upperSelPointDragOffset = Point.Empty;

        public bool IsMoving { get; private set; }

        public void Start(Point mousePoint, Document document, OnElementMovingDelegate onElementMovingDelegate)
        {
            _document = document;
            _onElementMovingDelegate = onElementMovingDelegate;

            _moveControllers = new IMoveController[document.SelectedElements.Count];
            var moveLabelCtrl = new IMoveController[document.SelectedElements.Count];
            for (var i = document.SelectedElements.Count - 1; i >= 0; i--)
            {
                _moveControllers[i] = ControllerHelper.GetMoveController(document.SelectedElements[i]);

                if ((_moveControllers[i] != null) && _moveControllers[i].CanMove)
                {
                    onElementMovingDelegate(new ElementEventArgs(document.SelectedElements[i]));
                    _moveControllers[i].Start(mousePoint);

                    //ILabelElement - Move Label inside the element
                    if (document.SelectedElements[i] is ILabelElement &&
                        (ControllerHelper.GetLabelController(document.SelectedElements[i]) == null))
                    {
                        var label = ((ILabelElement) document.SelectedElements[i]).Label;
                        moveLabelCtrl[i] = ControllerHelper.GetMoveController(label);

                        if ((moveLabelCtrl[i] != null) && moveLabelCtrl[i].CanMove)
                        {
                            moveLabelCtrl[i].Start(mousePoint);
                        }
                        else
                        {
                            moveLabelCtrl[i] = null;
                        }
                    }
                }
                else
                {
                    _moveControllers[i] = null;
                }
            }

            _moveControllers = (IMoveController[]) ArrayHelper.Append(_moveControllers, moveLabelCtrl);
            _moveControllers = (IMoveController[]) ArrayHelper.Shrink(_moveControllers, null);

            // Can't move only links
            var isOnlyLink = true;
            foreach (var ctrl in _moveControllers)
            {
                // Verify
                if (ctrl != null)
                {
                    ctrl.OwnerElement.Invalidate();

                    if (!(ctrl.OwnerElement is BaseLinkElement) && !(ctrl.OwnerElement is LabelElement))
                    {
                        isOnlyLink = false;
                        break;
                    }
                }
            }
            if (isOnlyLink)
            {
                //End Move the Links
                foreach (var ctrl in _moveControllers)
                {
                    ctrl?.End();
                }
                _moveControllers = new IMoveController[]
                {
                    null
                };
            }

            //Upper selecion point controller
            UpdateUpperSelectionPoint();
            _upperSelPointDragOffset.X = _upperSelPoint.X - mousePoint.X;
            _upperSelPointDragOffset.Y = _upperSelPoint.Y - mousePoint.Y;

            IsMoving = true;
        }

        public void Move(Point dragPoint)
        {
            //Upper selecion point controller
            var dragPointEl = dragPoint;
            dragPointEl.Offset(_upperSelPointDragOffset.X, _upperSelPointDragOffset.Y);

            _upperSelPoint = dragPointEl;

            if (dragPointEl.X < 0)
            {
                dragPointEl.X = 0;
            }
            if (dragPointEl.Y < 0)
            {
                dragPointEl.Y = 0;
            }

            //Move Controller
            if (dragPointEl.X == 0)
            {
                dragPoint.X = dragPoint.X - _upperSelPoint.X;
            }
            if (dragPointEl.Y == 0)
            {
                dragPoint.Y = dragPoint.Y - _upperSelPoint.Y;
            }

            foreach (var moveController in _moveControllers)
            {
                if (moveController != null)
                {
                    moveController.OwnerElement.Invalidate();

                    _onElementMovingDelegate(new ElementEventArgs(moveController.OwnerElement));

                    moveController.Move(dragPoint);

                    if (moveController.OwnerElement is NodeElement)
                    {
                        UpdateLinkPosition((NodeElement) moveController.OwnerElement);
                    }

                    var controller = ControllerHelper.GetLabelController(moveController.OwnerElement);
                    controller?.SetLabelPosition();
                }
            }
        }

        public void End()
        {
            _upperSelPoint = Point.Empty;
            _upperSelPointDragOffset = Point.Empty;

            foreach (var moveController in _moveControllers)
            {
                if (moveController != null)
                {
                    if (moveController.OwnerElement is NodeElement)
                    {
                        UpdateLinkPosition((NodeElement) moveController.OwnerElement);
                    }

                    moveController.End();

                    _onElementMovingDelegate(new ElementEventArgs(moveController.OwnerElement));
                }
            }

            IsMoving = false;
        }

        private void UpdateUpperSelectionPoint()
        {
            var points = new Point[_document.SelectedElements.Count];
            var p = 0;
            foreach (BaseElement element in _document.SelectedElements)
            {
                points[p] = element.Location;
                p++;
            }
            _upperSelPoint = DiagramHelper.GetUpperPoint(points);
        }

        private static void UpdateLinkPosition(NodeElement node)
        {
            foreach (var connectorElement in node.Connectors)
            {
                foreach (BaseElement element in connectorElement.Links)
                {
                    var linkElement = (BaseLinkElement) element;
                    var controller = ((IControllable) linkElement).GetController();
                    if (controller is IMoveController)
                    {
                        var moveController = (IMoveController) controller;
                        if (!moveController.IsMoving) linkElement.NeedCalcLink = true;
                    }
                    else
                    {
                        linkElement.NeedCalcLink = true;
                    }

                    if (linkElement is ILabelElement)
                    {
                        var label = ((ILabelElement) linkElement).Label;

                        var labelController = ControllerHelper.GetLabelController(linkElement);

                        if (labelController != null)
                        {
                            labelController.SetLabelPosition();
                        }
                        else
                        {
                            label.PositionBySite(linkElement);
                        }

                        label.Invalidate();
                    }
                }
            }
        }
    }
}