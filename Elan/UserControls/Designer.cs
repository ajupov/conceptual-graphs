using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Elan.Actions;
using Elan.Content;
using Elan.Controllers.Contracts;
using Elan.Enums;
using Elan.Events;
using Elan.Forms;
using Elan.Helpers;
using Elan.Models.Base;
using Elan.Models.Contracts;
using Elan.Models.Domain;
using Elan.Models.Implementations.Elements;
using Elan.Models.Implementations.Nodes;
using Elan.Models.Implementations.Tuples;
using Elan.Services;
using Document = Elan.Models.Implementations.Containers.Document;
using DomainDocument = Elan.Models.Domain.Document;

namespace Elan.UserControls
{
    public sealed class Designer : UserControl
    {
        public Designer()
        {
            AutoScroll = true;
            BackColor = SystemColors.Window;
            Name = "Designer";

            // This change control to not flick
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            // Selection Area Properties
            _selectionArea.FillColor = SystemColors.Control;

            // Label Edit
            _labelTextBox.BorderStyle = BorderStyle.FixedSingle;
            _labelTextBox.Multiline = true;
            _labelTextBox.Hide();
            Controls.Add(_labelTextBox);

            //EventsHandlers
            RecreateEventsHandlers();

            Document.Id = FictitiousIdHelper.NextId;
        }

        public new void Invalidate()
        {
            if (Document.Elements.Count > 0)
            {
                for (var i = 0; i <= Document.Elements.Count - 1; i++)
                {
                    var baseElement = Document.Elements[i];

                    Invalidate(baseElement);

                    var element = baseElement as ILabelElement;
                    if (element != null)
                    {
                        Invalidate(element.Label);
                    }
                }
            }
            else
            {
                base.Invalidate();
            }

            if ((_moveAction != null) && _moveAction.IsMoving)
            {
                AutoScrollMinSize = new Size(
                    (int) ((Document.Location.X + Document.Size.Width)*Document.Zoom),
                    (int) ((Document.Location.Y + Document.Size.Height)*Document.Zoom));
            }
        }

        private void Invalidate(BaseElement element, bool force = false)
        {
            if (element == null)
            {
                return;
            }

            if (force || element.IsInvalidated)
            {
                var invalidateRec = Goc2Gsc(element.InvalidateRec);
                invalidateRec.Inflate(10, 10);
                Invalidate(invalidateRec);
            }
        }

        private void RecreateEventsHandlers()
        {
            Document.PropertyChanged += DocumentPropertyChanged;
            Document.AppearancePropertyChanged += DocumentAppearancePropertyChanged;
            Document.ElementPropertyChanged += DocumentElementPropertyChanged;
            Document.ElementSelection += DocumentElementSelection;
        }

        #region Designer Control Initialization
        // Drag and Drop
        private MoveAction _moveAction;

        // Selection
        private BaseElement _selectedElement;
        private bool _isMultiSelection;
        private readonly RectangleElement _selectionArea = new RectangleElement(0, 0, 0, 0);
        private IController[] _controllers;
        private BaseElement _mousePointerElement;

        // Resize
        private ResizeAction _resizeAction;

        // Add Element
        private bool _isAddSelection;

        // Link
        private bool _isAddLink;
        private ConnectorElement _connStart;
        private ConnectorElement _connEnd;
        private BaseLinkElement _linkLine;

        // Label
        private bool _isEditLabel;
        private readonly TextBox _labelTextBox = new TextBox();
        private EditLabelAction _editLabelAction;

        //Undo
        [NonSerialized]
        private readonly UndoManager _undo = new UndoManager();

        private bool _changed;
        #endregion

        #region Events Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.PageUnit = GraphicsUnit.Pixel;

            var scrollPoint = AutoScrollPosition;
            graphics.TranslateTransform(scrollPoint.X, scrollPoint.Y);

            //Zoom
            using (var matrix = graphics.Transform)
            {
                var graphicsContainer = graphics.BeginContainer();

                graphics.SmoothingMode = Document.SmoothingMode;
                graphics.PixelOffsetMode = Document.PixelOffsetMode;
                graphics.CompositingQuality = Document.CompositionQuality;

                graphics.ScaleTransform(Document.Zoom, Document.Zoom);

                var clipRectangle = Gsc2Goc(e.ClipRectangle);

                Document.DrawElements(graphics, clipRectangle);

                if (!((_resizeAction != null) && _resizeAction.IsResizing))
                {
                    Document.DrawSelections(graphics, e.ClipRectangle);
                }

                if (_isMultiSelection || _isAddSelection)
                {
                    DrawSelectionRectangle(graphics);
                }

                if (_isAddLink)
                {
                    _linkLine.CalcLink();
                    _linkLine.Draw(graphics);
                }
                if ((_resizeAction != null) && !((_moveAction != null) && _moveAction.IsMoving))
                {
                    _resizeAction.DrawResizeCorner(graphics);
                }

                if (_mousePointerElement != null)
                {
                    if (_mousePointerElement is IControllable)
                    {
                        var ctrl = ((IControllable) _mousePointerElement).GetController();
                        ctrl.DrawSelection(graphics);
                    }
                }

                graphics.EndContainer(graphicsContainer);
                graphics.Transform = matrix;
            }

            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            var graphics = e.Graphics;
            graphics.PageUnit = GraphicsUnit.Pixel;
            using (var matrix = graphics.Transform)
            {
                var graphicsContainer = graphics.BeginContainer();
                graphics.EndContainer(graphicsContainer);
                graphics.Transform = matrix;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //Delete element
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedElements();
                EndGeneralAction();
                base.Invalidate();
            }

            //Undo
            if (e.Control && e.KeyCode == Keys.Z)
            {
                if (_undo.CanUndo)
                    Undo();
            }

            //Copy
            if (e.Control && (e.KeyCode == Keys.C))
            {
                Copy();
            }

            //Paste
            if (e.Control && (e.KeyCode == Keys.V))
            {
                Paste();
            }

            //Cut
            if (e.Control && (e.KeyCode == Keys.X))
            {
                Cut();
            }

            base.OnKeyDown(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Document.WindowSize = Size;
        }

        #region Mouse Events

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point mousePoint;

            //ShowSelectionCorner((document.Action==DesignerAction.Select));

            switch (Document.Action)
            {
                // SELECT
                case DesignerAction.Connect:
                case DesignerAction.Select:
                    if (e.Button == MouseButtons.Left)
                    {
                        mousePoint = Gsc2Goc(new Point(e.X, e.Y));

                        //Verify resize action
                        StartResizeElement(mousePoint);
                        if ((_resizeAction != null) && _resizeAction.IsResizing) break;

                        //Verify LabelElement editing
                        if (_isEditLabel)
                        {
                            EndEditLabel();
                        }

                        // Search element by click
                        _selectedElement = Document.FindElement(mousePoint);

                        if (_selectedElement != null)
                        {
                            //Events
                            var eventMouseDownArg = new ElementMouseEventArgs(_selectedElement, e.X, e.Y);
                            OnElementMouseDown(eventMouseDownArg);

                            // Double-click to edit Label
                            if ((e.Clicks == 2) && _selectedElement is ILabelElement)
                            {
                                StartEditLabel();
                                break;
                            }

                            // Element selected
                            if (_selectedElement is ConnectorElement)
                            {
                                StartAddLink((ConnectorElement) _selectedElement, mousePoint);
                                _selectedElement = null;
                            }
                            else
                                StartSelectElements(_selectedElement, mousePoint);
                        }
                        else
                        {
                            // If click is on neutral area, clear selection
                            Document.ClearSelection();
                            var p = Gsc2Goc(new Point(e.X, e.Y));
                            ;
                            _isMultiSelection = true;
                            _selectionArea.Location = p;
                            _selectionArea.Size = new Size(0, 0);
                        }
                        base.Invalidate();
                    }
                    break;

                // ADD
                case DesignerAction.Add:

                    if (e.Button == MouseButtons.Left)
                    {
                        mousePoint = Gsc2Goc(new Point(e.X, e.Y));
                        StartAddElement(mousePoint);
                    }
                    break;

                // DELETE
                case DesignerAction.Delete:
                    if (e.Button == MouseButtons.Left)
                    {
                        mousePoint = Gsc2Goc(new Point(e.X, e.Y));
                        DeleteElement(mousePoint);
                    }
                    break;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
                Cursor = Cursors.Arrow;
                var mousePoint = Gsc2Goc(new Point(e.X, e.Y));

                if ((_resizeAction != null)
                    && ((Document.Action == DesignerAction.Select)
                        || ((Document.Action == DesignerAction.Connect)
                            && _resizeAction.IsResizingLink)))
                {
                    Cursor = _resizeAction.UpdateResizeCornerCursor(mousePoint);
                }

                if (Document.Action == DesignerAction.Connect)
                {
                    var mousePointerElementTMP = Document.FindElement(mousePoint);
                    if (_mousePointerElement != mousePointerElementTMP)
                    {
                        if (mousePointerElementTMP is ConnectorElement)
                        {
                            _mousePointerElement = mousePointerElementTMP;
                            _mousePointerElement.Invalidate();
                            Invalidate(_mousePointerElement, true);
                        }
                        else if (_mousePointerElement != null)
                        {
                            _mousePointerElement.Invalidate();
                            Invalidate(_mousePointerElement, true);
                            _mousePointerElement = null;
                        }
                    }
                }
                else
                {
                    Invalidate(_mousePointerElement, true);
                    _mousePointerElement = null;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                var dragPoint = Gsc2Goc(new Point(e.X, e.Y));

                if ((_resizeAction != null) && _resizeAction.IsResizing)
                {
                    _resizeAction.Resize(dragPoint);
                    Invalidate();
                }

                if ((_moveAction != null) && _moveAction.IsMoving)
                {
                    _moveAction.Move(dragPoint);
                    Invalidate();
                }

                if (_isMultiSelection || _isAddSelection)
                {
                    var p = Gsc2Goc(new Point(e.X, e.Y));
                    _selectionArea.Size = new Size(p.X - _selectionArea.Location.X, p.Y - _selectionArea.Location.Y);
                    _selectionArea.Invalidate();
                    Invalidate(_selectionArea, true);
                }

                if (_isAddLink)
                {
                    _selectedElement = Document.FindElement(dragPoint);
                    if (_selectedElement is ConnectorElement
                        && Document.CanAddLink(_connStart, (ConnectorElement) _selectedElement))
                    {
                        _linkLine.Connector2 = (ConnectorElement) _selectedElement;
                    }
                    else
                    {
                        _linkLine.Connector2 = _connEnd;
                    }

                    var moveController = (IMoveController) ((IControllable) _connEnd).GetController();
                    moveController.Move(dragPoint);
                    
                    base.Invalidate();
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            var selectionRectangle = _selectionArea.GetUnsignedRectangle();

            if ((_moveAction != null) && _moveAction.IsMoving)
            {
                var eventClickArg = new ElementEventArgs(_selectedElement);
                OnElementClick(eventClickArg);

                _moveAction.End();
                _moveAction = null;

                var eventMouseUpArg = new ElementMouseEventArgs(_selectedElement, e.X, e.Y);
                OnElementMouseUp(eventMouseUpArg);

                if (_changed)
                    AddUndo();
            }

            // Select
            if (_isMultiSelection)
            {
                EndSelectElements(selectionRectangle);
            }
            // Add element
            else if (_isAddSelection)
            {
                EndAddElement(selectionRectangle);
            }

            // Add link
            else if (_isAddLink)
            {
                EndAddLink();
                AddUndo();
            }

            // Resize
            if (_resizeAction != null)
            {
                if (_resizeAction.IsResizing)
                {
                    var mousePoint = Gsc2Goc(new Point(e.X, e.Y));
                    _resizeAction.End(mousePoint);

                    AddUndo();
                }
                _resizeAction.UpdateResizeCorner();
            }

            RestartInitValues();

            base.Invalidate();

            base.OnMouseUp(e);
        }

        #endregion

        #endregion

        #region Events Raising

        // element handler
        public delegate void ElementEventHandler(object sender, ElementEventArgs e);

        #region Element Mouse Events

        // CLICK
        [Category("Element")]
        public event ElementEventHandler ElementClick;

        private void OnElementClick(ElementEventArgs e)
        {
            if (ElementClick != null)
            {
                ElementClick(this, e);
            }
        }

        // mouse handler
        public delegate void ElementMouseEventHandler(object sender, ElementMouseEventArgs e);

        // MOUSE DOWN
        [Category("Element")]
        public event ElementMouseEventHandler ElementMouseDown;

        private void OnElementMouseDown(ElementMouseEventArgs e)
        {
            if (ElementMouseDown != null)
            {
                ElementMouseDown(this, e);
            }
        }

        // MOUSE UP
        [Category("Element")]
        public event ElementMouseEventHandler ElementMouseUp;

        private void OnElementMouseUp(ElementMouseEventArgs e)
        {
            if (ElementMouseUp != null)
            {
                ElementMouseUp(this, e);
            }
        }

        #endregion

        #region Element Move Events

        // Before Move
        [Category("Element")]
        public event ElementEventHandler ElementMoving;

        private void OnElementMoving(ElementEventArgs e)
        {
            if (ElementMoving != null)
            {
                ElementMoving(this, e);
            }
        }

        // After Move
        [Category("Element")]
        public event ElementEventHandler ElementMoved;

        private void OnElementMoved(ElementEventArgs e)
        {
            if (ElementMoved != null)
            {
                ElementMoved(this, e);
            }
        }

        #endregion

        #region Element Resize Events

        // Before Resize
        [Category("Element")]
        public event ElementEventHandler ElementResizing;

        private void OnElementResizing(ElementEventArgs e)
        {
            if (ElementResizing != null)
            {
                ElementResizing(this, e);
            }
        }

        // After Resize
        [Category("Element")]
        public event ElementEventHandler ElementResized;

        private void OnElementResized(ElementEventArgs e)
        {
            if (ElementResized != null)
            {
                ElementResized(this, e);
            }
        }

        #endregion

        #region Element Connect Events

        // connect handler
        public delegate void ElementConnectEventHandler(object sender, ElementConnectEventArgs e);

        // Before Connect
        [Category("Element")]
        public event ElementConnectEventHandler ElementConnecting;

        private void OnElementConnecting(ElementConnectEventArgs e)
        {
            if (ElementConnecting != null)
            {
                ElementConnecting(this, e);
            }
        }

        // After Connect
        [Category("Element")]
        public event ElementConnectEventHandler ElementConnected;

        private void OnElementConnected(ElementConnectEventArgs e)
        {
            if (ElementConnected != null)
            {
                ElementConnected(this, e);
            }
        }

        #endregion

        #region Element Selection Events
        public delegate void ElementSelectionEventHandler(object sender, ElementSelectionEventArgs e);
        [Category("Element")]
        public event ElementSelectionEventHandler ElementSelection;
        private void OnElementSelection(ElementSelectionEventArgs e)
        {
            ElementSelection?.Invoke(this, e);
        }

        #endregion

        #endregion

        #region Events Handling

        private void DocumentPropertyChanged(object sender, EventArgs e)
        {
            if (!IsChanging())
            {
                base.Invalidate();
            }
        }

        private void DocumentAppearancePropertyChanged(object sender, EventArgs e)
        {
            if (!IsChanging())
            {
                AddUndo();
                base.Invalidate();
            }
        }

        private void DocumentElementPropertyChanged(object sender, EventArgs e)
        {
            _changed = true;

            if (!IsChanging())
            {
                AddUndo();
                base.Invalidate();
            }
        }

        private void DocumentElementSelection(object sender, ElementSelectionEventArgs e)
        {
            OnElementSelection(e);
        }

        #endregion

        #region Properties

        public Document Document { get; private set; } = new Document();

        public bool CanUndo
        {
            get { return _undo.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _undo.CanRedo; }
        }


        private bool IsChanging()
        {
            return ((_moveAction != null) && _moveAction.IsMoving) //IsDragging
                   || _isAddLink || _isMultiSelection ||
                   ((_resizeAction != null) && _resizeAction.IsResizing);
        }

        #endregion

        #region Draw Methods

        /// <summary>
        ///     Graphic surface coordinates to graphic object coordinates.
        /// </summary>
        /// <param name="p">Graphic surface point.</param>
        /// <returns></returns>
        public Point Gsc2Goc(Point gsp)
        {
            var zoom = Document.Zoom;
            gsp.X = (int) ((gsp.X - AutoScrollPosition.X)/zoom);
            gsp.Y = (int) ((gsp.Y - AutoScrollPosition.Y)/zoom);
            return gsp;
        }

        public Rectangle Gsc2Goc(Rectangle gsr)
        {
            var zoom = Document.Zoom;
            gsr.X = (int) ((gsr.X - AutoScrollPosition.X)/zoom);
            gsr.Y = (int) ((gsr.Y - AutoScrollPosition.Y)/zoom);
            gsr.Width = (int) (gsr.Width/zoom);
            gsr.Height = (int) (gsr.Height/zoom);
            return gsr;
        }

        public Rectangle Goc2Gsc(Rectangle rectangle)
        {
            var zoom = Document.Zoom;
            rectangle.X = (int) ((rectangle.X + AutoScrollPosition.X)*zoom);
            rectangle.Y = (int) ((rectangle.Y + AutoScrollPosition.Y)*zoom);
            rectangle.Width = (int) (rectangle.Width*zoom);
            rectangle.Height = (int) (rectangle.Height*zoom);
            return rectangle;
        }

        internal void DrawSelectionRectangle(Graphics graphics)
        {
            _selectionArea.DrawSelection(graphics);
        }

        #endregion

        #region Open/Save File

        public void SaveFile(string fileName, FileExtensionType type)
        {
            FileManager.Save(fileName, Document.GetDocument(), type);
        }

        public void OpenFile(string fileName, FileExtensionType type)
        {
            var document = FileManager.Open(fileName, type);
            CreateDocument(document);
            RestartInitValues();
            Document.SetCurrentId();
            RecreateEventsHandlers();
            Invalidate();
        }

        public void SaveDb()
        {
            if (string.IsNullOrEmpty(Document.Name))
            {
                var form = new InputForm();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    Document.Name = form.DocumentName;
                }
            }

            if (!string.IsNullOrEmpty(Document.Name))
            {
                if (!DataBaseManager.IsDocumentExists(Document.Name)
                    || MessageBox.Show(Strings.ReplaceInDbText, Strings.ReplaceInDbTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var document = Document.GetDocument();
                    DataBaseManager.SaveDocument(document);
                    MessageBox.Show(Strings.SuccessSaveToDbText, Strings.ApplicationTitle, MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        public void OpenDb()
        {
            var documents = DataBaseManager.GetDocumentNames();
            var form = new DocumentListForm(documents);

            if (form.ShowDialog() == DialogResult.OK)
            {
                var documentId = form.SelectedDocumentId;
                var domainDocument = DataBaseManager.GetDocumentById(documentId);

                CreateDocument(domainDocument);
            }

            RestartInitValues();
            Invalidate();
        }

        public void CreateDocument(DomainDocument domainDocument)
        {
            Document = new Document
            {
                Id = domainDocument.Id,
                Name = domainDocument.Name,
                WindowSize = Size
            };

            foreach (var node in domainDocument.Nodes)
            {
                var rectangle = new Rectangle(node.X, node.Y, node.Width, node.Height);

                switch (node.Type)
                {
                    case NodeType.Concept:
                        var rectangleNode = new RectangleNode(rectangle)
                        {Id = node.Id, Label = new LabelElement {Text = node.Label}};
                        rectangleNode.Label.PositionBySite(rectangleNode);
                        Document.AddElement(rectangleNode);
                        break;
                    case NodeType.Relation:
                        var ellipseNode = new EllipseNode(rectangle)
                        {Id = node.Id, Label = new LabelElement {Text = node.Label}};
                        ellipseNode.Label.PositionBySite(ellipseNode);
                        Document.AddElement(ellipseNode);
                        break;
                    case NodeType.Comment:
                        var commentBoxElement = new CommentBoxElement(rectangle)
                        {Id = node.Id, Label = new LabelElement {Text = node.Label}};
                        commentBoxElement.Label.PositionBySite(commentBoxElement);
                        Document.AddElement(commentBoxElement);
                        break;
                }
            }

            foreach (var link in domainDocument.Links)
            {
                ConnectorElement startConnectorElement, endConnectorElement;
                GetConnectors(link, out startConnectorElement, out endConnectorElement);

                if (startConnectorElement != null && endConnectorElement != null)
                {
                    var linkElement = new StraightLinkElement(startConnectorElement, endConnectorElement)
                    {
                        Id = link.Id,
                        Label = new LabelElement
                        {
                            Text = link.Label
                        }
                    };
                    linkElement.Label.Size = EditLabelAction.GetTextSize(linkElement);
                    linkElement.Label.PositionBySite(linkElement);
                    Document.Elements.Add(linkElement);
                }
            }

            Document.SetCurrentId();
            RecreateEventsHandlers();
        }

        private void GetConnectors(Link link, out ConnectorElement startConnectorElement, out ConnectorElement endConnectorElement)
        {
            startConnectorElement = (ConnectorElement)Document.FindElement(new Point(link.StartPointX, link.StartPointY));
            endConnectorElement = (ConnectorElement)Document.FindElement(new Point(link.EndPointX, link.EndPointY));

            if (startConnectorElement == null || endConnectorElement == null)
            {
                var startNode = Document.Elements.GetArray().FirstOrDefault(n => n.Id == link.StartNodeId);
                var endNode = Document.Elements.GetArray().FirstOrDefault(n => n.Id == link.EndNodeId);

                if (startNode != null && endNode != null)
                {
                    var listTuples = new List<LineVariantTuple>();
                    foreach (var connector1 in ((NodeElement)startNode).Connectors)
                    {
                        foreach (var connector2 in ((NodeElement)endNode).Connectors)
                        {
                            listTuples.Add(new LineVariantTuple
                            {
                                StartPoint = connector1.Location,
                                EndPoint = connector2.Location
                            });
                        }
                    }

                    var minLengthLine = listTuples.OrderBy(l => l.Length).FirstOrDefault();
                    if (minLengthLine != null)
                    {
                        startConnectorElement = (ConnectorElement)Document.FindElement(minLengthLine.StartPoint);
                        endConnectorElement = (ConnectorElement)Document.FindElement(minLengthLine.EndPoint);
                    }
                }
            }
        }

        #endregion

        #region Copy/Paste

        public void Copy()
        {
            if (Document.SelectedElements.Count == 0) return;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, Document.SelectedElements.GetArray());
            var data = new DataObject(DataFormats.GetFormat("Diagram.NET Element Collection").Name, stream);
            Clipboard.SetDataObject(data);
        }

        public void Paste()
        {
            const int pasteStep = 20;

            _undo.Enabled = false;
            var iData = Clipboard.GetDataObject();
            var format = DataFormats.GetFormat("Diagram.NET Element Collection");
            if (iData.GetDataPresent(format.Name))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = (MemoryStream) iData.GetData(format.Name);
                var elCol = (BaseElement[]) formatter.Deserialize(stream);
                stream.Close();

                foreach (var el in elCol)
                {
                    el.Location = new Point(el.Location.X + pasteStep, el.Location.Y + pasteStep);
                }

                Document.AddElements(elCol);
                Document.ClearSelection();
                Document.SelectElements(elCol);
            }
            _undo.Enabled = true;

            AddUndo();
            EndGeneralAction();
        }

        public void Cut()
        {
            Copy();
            DeleteSelectedElements();
            EndGeneralAction();
        }

        #endregion

        #region Start/End Actions and General Functions

        #region General

        private void EndGeneralAction()
        {
            RestartInitValues();
        }

        public void RestartInitValues()
        {
            // Reinitialize status
            _moveAction = null;

            _isMultiSelection = false;
            _isAddSelection = false;
            _isAddLink = false;

            _changed = false;

            _connStart = null;

            _selectionArea.FillColor = SystemColors.Control;

            Document.CalcWindow(true);
        }

        #endregion

        #region Selection

        private void StartSelectElements(BaseElement selectedElement, Point mousePoint)
        {
            // Vefiry if element is in selection
            if (!Document.SelectedElements.Contains(selectedElement))
            {
                //Clear selection and add new element to selection
                Document.ClearSelection();
                Document.SelectElement(selectedElement);
            }

            _changed = false;


            _moveAction = new MoveAction();
            MoveAction.OnElementMovingDelegate onElementMovingDelegate = OnElementMoving;
            _moveAction.Start(mousePoint, Document, onElementMovingDelegate);


            // Get Controllers
            _controllers = new IController[Document.SelectedElements.Count];
            for (var i = Document.SelectedElements.Count - 1; i >= 0; i--)
            {
                if (Document.SelectedElements[i] is IControllable)
                {
                    // Get General Controller
                    _controllers[i] = ((IControllable) Document.SelectedElements[i]).GetController();
                }
                else
                {
                    _controllers[i] = null;
                }
            }

            _resizeAction = new ResizeAction();
            _resizeAction.Select(Document);
        }

        private void EndSelectElements(Rectangle selectionRectangle)
        {
            Document.SelectElements(selectionRectangle);
        }

        #endregion

        #region Resize

        private void StartResizeElement(Point mousePoint)
        {
            if ((_resizeAction != null) && ((Document.Action == DesignerAction.Select) || ((Document.Action == DesignerAction.Connect) && _resizeAction.IsResizingLink)))
            {
                ResizeAction.OnElementResizingDelegate onElementResizingDelegate = OnElementResizing;
                _resizeAction.Start(mousePoint, onElementResizingDelegate);
                if (!_resizeAction.IsResizing)
                    _resizeAction = null;
            }
        }

        #endregion

        #region Link

        private void StartAddLink(ConnectorElement connStart, Point mousePoint)
        {
            if (Document.Action == DesignerAction.Connect)
            {
                _connStart = connStart;
                _connEnd = new ConnectorElement(connStart.ParentElement) {Location = connStart.Location};

                var controller = (IMoveController) ((IControllable) _connEnd).GetController();
                controller.Start(mousePoint);

                _isAddLink = true;

                _linkLine = new StraightLinkElement(connStart, _connEnd) {BorderWidth = 1};

                Invalidate(_linkLine, true);

                OnElementConnecting(new ElementConnectEventArgs(connStart.ParentElement, null, _linkLine));
            }
        }

        private void EndAddLink()
        {
            if (_connEnd != _linkLine.Connector2)
            {
                _linkLine.Connector1.RemoveLink(_linkLine);
                _linkLine = Document.AddLink(_linkLine.Connector1, _linkLine.Connector2);
                OnElementConnected(new ElementConnectEventArgs(_linkLine.Connector1.ParentElement, _linkLine.Connector2.ParentElement, _linkLine));
            }

            _connStart = null;
            _connEnd = null;
            _linkLine = null;
        }

        #endregion

        #region Add Element

        private void StartAddElement(Point mousePoint)
        {
            Document.ClearSelection();

            //Change Selection Area Color
            _selectionArea.FillColor = Color.LightSteelBlue;

            _isAddSelection = true;
            _selectionArea.Location = mousePoint;
            _selectionArea.Size = new Size(0, 0);
        }

        private void EndAddElement(Rectangle selectionRectangle)
        {
            BaseElement el;
            switch (Document.ElementType)
            {
                case ElementType.Rectangle:
                    el = new RectangleElement(selectionRectangle);
                    break;
                case ElementType.RectangleNode:
                    el = new RectangleNode(selectionRectangle);
                    break;
                case ElementType.Ellipse:
                    el = new EllipseElement(selectionRectangle);
                    break;
                case ElementType.EllipseNode:
                    el = new EllipseNode(selectionRectangle);
                    break;
                case ElementType.CommentBox:
                    el = new CommentBoxElement(selectionRectangle);
                    break;
                default:
                    el = new RectangleNode(selectionRectangle);
                    break;
            }

            Document.AddElement(el);

            Document.Action = DesignerAction.Select;
        }

        #endregion

        #region Edit Label

        private void StartEditLabel()
        {
            _isEditLabel = true;

            // Disable resize
            if (_resizeAction != null)
            {
                _resizeAction = null;
            }

            _editLabelAction = new EditLabelAction();
            _editLabelAction.StartEdit(_selectedElement, _labelTextBox);
        }

        private void EndEditLabel()
        {
            if (_editLabelAction != null)
            {
                _editLabelAction.EndEdit();
                _editLabelAction = null;
            }
            _isEditLabel = false;
        }

        #endregion

        #region Delete

        private void DeleteElement(Point mousePoint)
        {
            Document.DeleteElement(mousePoint);
            _selectedElement = null;
            Document.Action = DesignerAction.Select;
        }

        private void DeleteSelectedElements()
        {
            Document.DeleteSelectedElements();
        }

        #endregion

        #endregion

        #region Undo/Redo

        public void Undo()
        {
            Document = (Document) _undo.Undo();
            RecreateEventsHandlers();
            _resizeAction?.UpdateResizeCorner();
            base.Invalidate();
        }

        public void Redo()
        {
            Document = (Document) _undo.Redo();
            RecreateEventsHandlers();
            _resizeAction?.UpdateResizeCorner();
            base.Invalidate();
        }

        private void AddUndo()
        {
            _undo.AddUndo(Document);
        }

        #endregion
    }
}