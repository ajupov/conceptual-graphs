using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Elan.Controllers.Contracts;
using Elan.Enums;
using Elan.Events;
using Elan.Helpers;
using Elan.Models.Base;
using Elan.Models.Contracts;
using Elan.Models.Implementations.Collections;
using Elan.Models.Implementations.Elements;
using IContainer = Elan.Models.Base.IContainer;
using DomainDocument = Elan.Models.Domain.Document;

namespace Elan.Models.Implementations.Containers
{
    [Serializable]
    public class Document : IDeserializationCallback
    {
        #region Для БД
        [Browsable(false)]
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        public DomainDocument GetDocument()
        {
            var document = new DomainDocument
            {
                Id = Id,
                Name = Name,
                Width = Size.Width,
                Height = Size.Height,
                Nodes = Elements.GetNodes(),
                Links = Elements.GetLinks()
            };

            return document;
        }
        #endregion

        #region Свойства скрытые
        private DesignerAction _action = DesignerAction.Select;
        private bool _canFireEvents = true;
        private CompositingQuality _compositingQuality = CompositingQuality.AssumeLinear;
        private readonly ElementCollection _elements = new ElementCollection();
        private ElementType _elementType = ElementType.RectangleNode;
        private Size _gridSize = new Size(50, 50);
        private LinkType _linkType = LinkType.RightAngle;
        private Point _location = new Point(100, 100);
        private PixelOffsetMode _pixelOffsetMode = PixelOffsetMode.Default;
        private SmoothingMode _smoothingMode = SmoothingMode.HighQuality;
        private Size _windowSize = new Size(0, 0);
        private float _zoom = 1.0f;
        #endregion

        #region Свойства
        [Browsable(false)]
        public ElementCollection Elements => _elements;
        [Browsable(false)]
        public ElementCollection SelectedElements { get; } = new ElementCollection();
        [Browsable(false)]
        public ElementCollection SelectedNodes { get; } = new ElementCollection();
        [Browsable(false)]
        public Point Location => _elements.WindowLocation;
        [Browsable(false)]
        public Size Size => _elements.WindowSize;
        [Browsable(false)]
        internal Size WindowSize
        {
            set { _windowSize = value; }
        }
        [Browsable(false)]
        public SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set
            {
                _smoothingMode = value;
                OnAppearancePropertyChanged(new EventArgs());
            }
        }
        [Browsable(false)]
        public PixelOffsetMode PixelOffsetMode
        {
            get { return _pixelOffsetMode; }
            set
            {
                _pixelOffsetMode = value;
                OnAppearancePropertyChanged(new EventArgs());
            }
        }
        [Browsable(false)]
        public CompositingQuality CompositingQuality
        {
            get { return _compositingQuality; }
            set
            {
                _compositingQuality = value;
                OnAppearancePropertyChanged(new EventArgs());
            }
        }
        [Browsable(false)]
        public DesignerAction Action
        {
            get { return _action; }
            set
            {
                _action = value;
                OnPropertyChanged(new EventArgs());
            }
        }
        [Browsable(false)]
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                OnPropertyChanged(new EventArgs());
            }
        }
        [Browsable(false)]
        public ElementType ElementType
        {
            get { return _elementType; }
            set
            {
                _elementType = value;
                OnPropertyChanged(new EventArgs());
            }
        }
        [Browsable(false)]
        public LinkType LinkType
        {
            get { return _linkType; }
            set
            {
                _linkType = value;
                OnPropertyChanged(new EventArgs());
            }
        }
        [Browsable(false)]
        public Size GridSize
        {
            get { return _gridSize; }
            set
            {
                _gridSize = value;
                OnAppearancePropertyChanged(new EventArgs());
            }
        }
        #endregion

        #region Добавление
        public void AddElement(BaseElement element)
        {
            element.Id = FictitiousIdHelper.NextId;
            _elements.Add(element);
            element.AppearanceChanged += ElementAppearanceChanged;
            OnAppearancePropertyChanged(new EventArgs());
        }
        public void AddElements(ElementCollection collection)
        {
            AddElements(collection.GetArray());
        }
        public void AddElements(BaseElement[] elements)
        {
            _elements.EnabledCalc = false;
            foreach (var element in elements)
            {
                AddElement(element);
            }
            _elements.EnabledCalc = true;
        }
        internal bool CanAddLink(ConnectorElement start, ConnectorElement end)
        {
            return (start != end) && (start.ParentElement != end.ParentElement);
        }
        public BaseLinkElement AddLink(ConnectorElement start, ConnectorElement end)
        {
            if (CanAddLink(start, end))
            {
                var linkElement = new StraightLinkElement(start, end);
                _elements.Add(linkElement);
                linkElement.AppearanceChanged += ElementAppearanceChanged;
                OnAppearancePropertyChanged(new EventArgs());
                return linkElement;
            }
            return null;
        }
        #endregion

        #region Удаление
        public void DeleteElement(BaseElement element)
        {
            if ((element != null) && !(element is ConnectorElement))
            {
                //Delete link
                if (element is BaseLinkElement)
                {
                    var linkElement = (BaseLinkElement) element;
                    DeleteLink(linkElement);
                    return;
                }

                //Delete node
                if (element is NodeElement)
                {
                    var conn = (NodeElement) element;
                    foreach (var connectorElement in conn.Connectors)
                    {
                        for (var i = connectorElement.Links.Count - 1; i >= 0; i--)
                        {
                            var linkElement = (BaseLinkElement) connectorElement.Links[i];
                            DeleteLink(linkElement);
                        }
                    }

                    if (SelectedNodes.Contains(element))
                    {
                        SelectedNodes.Remove(element);
                    }
                }

                if (SelectedElements.Contains(element))
                {
                    SelectedElements.Remove(element);
                }

                _elements.Remove(element);

                OnAppearancePropertyChanged(new EventArgs());
            }
        }
        public void DeleteElement(Point point)
        {
            var selectedElement = FindElement(point);
            DeleteElement(selectedElement);
        }
        public void DeleteSelectedElements()
        {
            SelectedElements.EnabledCalc = false;
            SelectedNodes.EnabledCalc = false;

            for (var i = SelectedElements.Count - 1; i >= 0; i--)
            {
                DeleteElement(SelectedElements[i]);
            }

            SelectedElements.EnabledCalc = true;
            SelectedNodes.EnabledCalc = true;
        }
        public void DeleteLink(BaseLinkElement linkElement)
        {
            if (linkElement != null)
            {
                linkElement.Connector1.RemoveLink(linkElement);
                linkElement.Connector2.RemoveLink(linkElement);

                if (_elements.Contains(linkElement))
                {
                    _elements.Remove(linkElement);
                }
                if (SelectedElements.Contains(linkElement))
                {
                    SelectedElements.Remove(linkElement);
                }
                OnAppearancePropertyChanged(new EventArgs());
            }
        }
        #endregion

        #region Выбор
        public void ClearSelection()
        {
            SelectedElements.Clear();
            SelectedNodes.Clear();
            OnElementSelection(this, new ElementSelectionEventArgs(SelectedElements));
        }
        public void SelectElement(BaseElement element)
        {
            SelectedElements.Add(element);
            if (element is NodeElement)
            {
                SelectedNodes.Add(element);
            }
            if (_canFireEvents)
            {
                OnElementSelection(this, new ElementSelectionEventArgs(SelectedElements));
            }
        }
        public void SelectElements(BaseElement[] elements)
        {
            SelectedElements.EnabledCalc = false;
            SelectedNodes.EnabledCalc = false;

            _canFireEvents = false;

            try
            {
                foreach (var element in elements)
                {
                    SelectElement(element);
                }
            }
            finally
            {
                _canFireEvents = true;
            }
            SelectedElements.EnabledCalc = true;
            SelectedNodes.EnabledCalc = true;

            OnElementSelection(this, new ElementSelectionEventArgs(SelectedElements));
        }
        public void SelectElements(Rectangle selectionRectangle)
        {
            SelectedElements.EnabledCalc = false;
            SelectedNodes.EnabledCalc = false;

            // Add all "hitable" elements
            foreach (BaseElement element in _elements)
            {
                if (element is IControllable)
                {
                    var controller = ((IControllable) element).GetController();
                    if (controller.HitTest(selectionRectangle))
                    {
                        if (!(element is ConnectorElement))
                            SelectedElements.Add(element);

                        if (element is NodeElement)
                            SelectedNodes.Add(element);
                    }
                }
            }

            //if the seleciont isn't a expecific link, remove links
            // without 2 elements in selection
            if (SelectedElements.Count > 1)
            {
                foreach (BaseElement element in _elements)
                {
                    var lnk = element as BaseLinkElement;
                    if (lnk == null) continue;

                    if (!SelectedElements.Contains(lnk.Connector1.ParentElement) ||
                        !SelectedElements.Contains(lnk.Connector2.ParentElement))
                    {
                        SelectedElements.Remove(lnk);
                    }
                }
            }

            SelectedElements.EnabledCalc = true;
            SelectedNodes.EnabledCalc = true;

            OnElementSelection(this, new ElementSelectionEventArgs(SelectedElements));
        }
        public void SelectAllElements()
        {
            SelectedElements.EnabledCalc = false;
            SelectedNodes.EnabledCalc = false;

            foreach (BaseElement element in _elements)
            {
                if (!(element is ConnectorElement))
                {
                    SelectedElements.Add(element);
                }

                if (element is NodeElement)
                {
                    SelectedNodes.Add(element);
                }
            }

            SelectedElements.EnabledCalc = true;
            SelectedNodes.EnabledCalc = true;
        }
        public BaseElement FindElement(Point point)
        {
            if ((_elements != null) && (_elements.Count > 0))
            {
                // First, find elements
                BaseElement element;
                for (var i = _elements.Count - 1; i >= 0; i--)
                {
                    element = _elements[i];

                    if (element is BaseLinkElement)
                    {
                        continue;
                    }

                    //Find element in a Connector array
                    if (element is NodeElement)
                    {
                        var nodeElement = (NodeElement) element;
                        foreach (var connectorElement in nodeElement.Connectors)
                        {
                            var controller = ((IControllable) connectorElement).GetController();
                            if (controller.HitTest(point))
                            {
                                return connectorElement;
                            }
                        }
                    }

                    //Find element in a Container Element
                    if (element is IContainer)
                    {
                        var inner = FindInnerElement((IContainer) element, point);
                        if (inner != null)
                        {
                            return inner;
                        }
                    }

                    //Find element by hit test
                    if (element is IControllable)
                    {
                        var controller = ((IControllable) element).GetController();
                        if (controller.HitTest(point))
                        {
                            return element;
                        }
                    }
                }

                // Then, find links
                for (var i = _elements.Count - 1; i >= 0; i--)
                {
                    element = _elements[i];

                    if (!(element is BaseLinkElement))
                    {
                        continue;
                    }

                    if (element is IControllable)
                    {
                        var controller = ((IControllable) element).GetController();
                        if (controller.HitTest(point))
                        {
                            return element;
                        }
                    }
                }
            }
            return null;
        }
        private static BaseElement FindInnerElement(IContainer parent, Point point)
        {
            foreach (BaseElement element in parent.Elements)
            {
                if (element is IContainer)
                {
                    var innerElement = FindInnerElement((IContainer) element, point);
                    if (innerElement != null)
                    {
                        return innerElement;
                    }
                }

                if (element is IControllable)
                {
                    var controller = ((IControllable) element).GetController();

                    if (controller.HitTest(point))
                    {
                        return element;
                    }
                }
            }
            return null;
        }
        #endregion

        #region Позиции
        internal void CalcWindow(bool forceCalc)
        {
            _elements.CalcWindow(forceCalc);
            SelectedElements.CalcWindow(forceCalc);
            SelectedNodes.CalcWindow(forceCalc);
        }
        public void MoveUpElement(BaseElement element)
        {
            var i = _elements.IndexOf(element);
            if (i != _elements.Count - 1)
            {
                _elements.ChangeIndex(i, i + 1);
                OnAppearancePropertyChanged(new EventArgs());
            }
        }
        public void MoveDownElement(BaseElement element)
        {
            var i = _elements.IndexOf(element);
            if (i != 0)
            {
                _elements.ChangeIndex(i, i - 1);
                OnAppearancePropertyChanged(new EventArgs());
            }
        }
        public void BringToFrontElement(BaseElement element)
        {
            var i = _elements.IndexOf(element);
            for (var x = i + 1; x <= _elements.Count - 1; x++)
            {
                _elements.ChangeIndex(i, x);
                i = x;
            }
            OnAppearancePropertyChanged(new EventArgs());
        }
        public void SendToBackElement(BaseElement element)
        {
            var i = _elements.IndexOf(element);
            for (var x = i - 1; x >= 0; x--)
            {
                _elements.ChangeIndex(i, x);
                i = x;
            }
            OnAppearancePropertyChanged(new EventArgs());
        }
        #endregion

        #region Рисование
        internal void DrawElements(Graphics graphics, Rectangle clippingRegion)
        {
            for (var i = 0; i <= _elements.Count - 1; i++)
            {
                var element = _elements[i];
                if (element is BaseLinkElement && NeedDrawElement(element, clippingRegion))
                {
                    element.Draw(graphics);
                }

                if (element is ILabelElement)
                {
                    ((ILabelElement) element).Label.Draw(graphics);
                }
            }

            //Draw the other elements
            for (var i = 0; i <= _elements.Count - 1; i++)
            {
                var element = _elements[i];

                if (!(element is BaseLinkElement) && NeedDrawElement(element, clippingRegion))
                {
                    if (element is NodeElement)
                    {
                        var n = (NodeElement) element;
                        n.Draw(graphics, _action == DesignerAction.Connect);
                    }
                    else
                    {
                        element.Draw(graphics);
                    }

                    if (element is ILabelElement)
                    {
                        ((ILabelElement) element).Label.Draw(graphics);
                    }
                }
            }
        }
        private static bool NeedDrawElement(BaseElement element, Rectangle clippingRegion)
        {
            var elRectangle = element.GetUnsignedRectangle();
            elRectangle.Inflate(5, 5);
            return clippingRegion.IntersectsWith(elRectangle);
        }
        internal void DrawSelections(Graphics graphics, Rectangle clippingRegion)
        {
            for (var i = SelectedElements.Count - 1; i >= 0; i--)
            {
                if (SelectedElements[i] is IControllable)
                {
                    var ctrl = ((IControllable) SelectedElements[i]).GetController();
                    ctrl.DrawSelection(graphics);

                    if (SelectedElements[i] is BaseLinkElement)
                    {
                        var link = (BaseLinkElement) SelectedElements[i];
                        ctrl = ((IControllable) link.Connector1).GetController();
                        ctrl.DrawSelection(graphics);

                        ctrl = ((IControllable) link.Connector2).GetController();
                        ctrl.DrawSelection(graphics);
                    }
                }
            }
        }
        internal void DrawGrid(Graphics graphics, Rectangle clippingRegion)
        {
            using (var pen = new Pen(new HatchBrush(HatchStyle.LargeGrid | HatchStyle.Percent90,
                Color.LightGray, Color.Transparent), 1))
            {
                var maxX = _location.X + Size.Width;
                var maxY = _location.Y + Size.Height;

                if (_windowSize.Width/_zoom > maxX)
                {
                    maxX = (int) (_windowSize.Width/_zoom);
                }

                if (_windowSize.Height/_zoom > maxY)
                {
                    maxY = (int) (_windowSize.Height/_zoom);
                }

                for (var i = 0; i < maxX; i += _gridSize.Width)
                {
                    graphics.DrawLine(pen, i, 0, i, maxY);
                }

                for (var i = 0; i < maxY; i += _gridSize.Height)
                {
                    graphics.DrawLine(pen, 0, i, maxX, i);
                }
            }
        }
        #endregion

        #region Возбуждение событий
        [field: NonSerialized]
        public event EventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(EventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        [field: NonSerialized]
        public event EventHandler AppearancePropertyChanged;
        protected virtual void OnAppearancePropertyChanged(EventArgs e)
        {
            OnPropertyChanged(e);
            AppearancePropertyChanged?.Invoke(this, e);
        }
        [field: NonSerialized]
        public event EventHandler ElementPropertyChanged;
        protected virtual void OnElementPropertyChanged(object sender, EventArgs e)
        {
            ElementPropertyChanged?.Invoke(sender, e);
        }
        public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);
        [field: NonSerialized]
        public event ElementSelectionEventHandler ElementSelection;
        protected virtual void OnElementSelection(object sender, ElementSelectionEventArgs e)
        {
            ElementSelection?.Invoke(sender, e);
        }
        #endregion

        #region Обработка событий
        public void OnDeserialization(object sender)
        {
            RecreateEventsHandlers();
        }
        private void RecreateEventsHandlers()
        {
            foreach (BaseElement element in _elements)
            {
                element.AppearanceChanged += ElementAppearanceChanged;
            }
        }
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private void ElementAppearanceChanged(object sender, EventArgs e)
        {
            OnElementPropertyChanged(sender, e);
        }
        #endregion
    }
}