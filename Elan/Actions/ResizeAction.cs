using System.Drawing;
using System.Windows.Forms;
using Elan.Controllers.Contracts;
using Elan.Enums;
using Elan.Events;
using Elan.Helpers;
using Elan.Models.Base;
using Elan.Models.Contracts;
using Elan.Models.Implementations.Containers;

namespace Elan.Actions
{
    public class ResizeAction
    {
        public delegate void OnElementResizingDelegate(ElementEventArgs e);

        private Document _document;

        private OnElementResizingDelegate _onElementResizingDelegate;

        private IResizeController _resizeController;

        public bool IsResizing { get; private set; }

        public bool IsResizingLink => _resizeController?.OwnerElement is BaseLinkElement;

        public void Select(Document document)
        {
            _document = document;

            if ((document.SelectedElements.Count == 1) && document.SelectedElements[0] is IControllable)
            {
                var controller = ((IControllable) document.SelectedElements[0]).GetController();
                if (controller is IResizeController)
                {
                    controller.OwnerElement.Invalidate();
                    _resizeController = (IResizeController) controller;
                }
            }
            else
            {
                _resizeController = null;
            }
        }

        public void Start(Point mousePoint, OnElementResizingDelegate onElementResizingDelegate)
        {
            IsResizing = false;

            if (_resizeController == null)
            {
                return;
            }

            _onElementResizingDelegate = onElementResizingDelegate;

            _resizeController.OwnerElement.Invalidate();

            var corPos = _resizeController.HitTestCorner(mousePoint);

            if (corPos != CornerPosition.Nothing)
            {
                //Events
                var eventResizeArg = new ElementEventArgs(_resizeController.OwnerElement);
                onElementResizingDelegate(eventResizeArg);

                _resizeController.Start(mousePoint, corPos);

                UpdateResizeCorner();

                IsResizing = true;
            }
        }

        public void Resize(Point dragPoint)
        {
            if ((_resizeController != null) && _resizeController.CanResize)
            {
                //Events
                var eventResizeArg = new ElementEventArgs(_resizeController.OwnerElement);
                _onElementResizingDelegate(eventResizeArg);

                _resizeController.OwnerElement.Invalidate();

                _resizeController.Resize(dragPoint);

                var labelController = ControllerHelper.GetLabelController(_resizeController.OwnerElement);
                if (labelController != null)
                {
                    labelController.SetLabelPosition();
                }
                else
                {
                    if (_resizeController.OwnerElement is ILabelElement)
                    {
                        var label = ((ILabelElement) _resizeController.OwnerElement).Label;
                        label.PositionBySite(_resizeController.OwnerElement);
                    }
                }

                UpdateResizeCorner();
            }
        }

        public void End(Point endPoint)
        {
            if (_resizeController != null)
            {
                _resizeController.OwnerElement.Invalidate();

                _resizeController.End(endPoint);

                //Events
                var eventResizeArg = new ElementEventArgs(_resizeController.OwnerElement);
                _onElementResizingDelegate(eventResizeArg);

                IsResizing = false;
            }
        }

        public void DrawResizeCorner(Graphics graphics)
        {
            if (_resizeController != null)
            {
                foreach (var corner in _resizeController.Corners)
                {
                    switch (_document.Action)
                    {
                        case DesignerAction.Select:
                            corner.Draw(graphics);
                            break;
                        case DesignerAction.Connect:
                            if (_resizeController.OwnerElement is BaseLinkElement)
                            {
                                corner.Draw(graphics);
                            }
                            break;
                    }
                }
            }
        }

        public void UpdateResizeCorner()
        {
            _resizeController?.UpdateCornersPos();
        }

        public Cursor UpdateResizeCornerCursor(Point mousePoint)
        {
            if ((_resizeController == null) || !_resizeController.CanResize)
            {
                return Cursors.Default;
            }

            var cornerPosition = _resizeController.HitTestCorner(mousePoint);

            switch (cornerPosition)
            {
                case CornerPosition.TopLeft:
                    return Cursors.SizeNWSE;

                case CornerPosition.TopCenter:
                    return Cursors.SizeNS;

                case CornerPosition.TopRight:
                    return Cursors.SizeNESW;

                case CornerPosition.MiddleLeft:
                case CornerPosition.MiddleRight:
                    return Cursors.SizeWE;

                case CornerPosition.BottomLeft:
                    return Cursors.SizeNESW;

                case CornerPosition.BottomCenter:
                    return Cursors.SizeNS;

                case CornerPosition.BottomRight:
                    return Cursors.SizeNWSE;
                default:
                    return Cursors.Default;
            }
        }
    }
}