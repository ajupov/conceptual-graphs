using System.Drawing;
using System.Drawing.Drawing2D;
using Elan.Controllers.Contracts;
using Elan.Enums;
using Elan.Models.Base;
using Elan.Models.Implementations.Elements;

namespace Elan.Controllers.Implementations
{
    internal class RectangleController : IMoveController, IResizeController
    {
        public RectangleController(BaseElement element)
        {
            Element = element;
            for (var i = 0; i < SelectionCorner.Length; i++)
            {
                SelectionCorner[i] = new RectangleElement(0, 0, SelCornerSize*2, SelCornerSize*2)
                {
                    FillColor = Color.White
                };
            }
        }

        protected const int SelCornerSize = 3;

        protected bool CanMove = true;
        protected bool CanResize = true;
        protected bool IsDragging;
        protected Point DragOffset = new Point(0);
        protected BaseElement Element;
        protected CornerPosition SelCorner = CornerPosition.Nothing;
        protected RectangleElement[] SelectionCorner = new RectangleElement[9];

        public BaseElement OwnerElement => Element;
        public RectangleElement[] Corners => SelectionCorner;
        bool IMoveController.IsMoving => IsDragging;
        bool IMoveController.CanMove => CanMove;
        bool IResizeController.CanResize => CanResize;
        bool IResizeController.IsResizing => SelCorner != CornerPosition.Nothing;

        void IMoveController.Start(Point startPoint)
        {
            var location = Element.Location;
            DragOffset.X = location.X - startPoint.X;
            DragOffset.Y = location.Y - startPoint.Y;
            IsDragging = true;
        }
        void IMoveController.Move(Point currentPoint)
        {
            if (IsDragging)
            {
                var point = currentPoint;
                point.Offset(DragOffset.X, DragOffset.Y);
                if (point.X < 0)
                {
                    point.X = 0;
                }
                if (point.Y < 0)
                {
                    point.Y = 0;
                }

                Element.Location = point;
            }
        }
        void IMoveController.End()
        {
            IsDragging = false;
        }
        void IResizeController.UpdateCornersPos()
        {
            var rectangle = new Rectangle(Element.Location, Element.Size);

            SelectionCorner[(int)CornerPosition.TopLeft].Location =
                new Point(rectangle.Location.X - SelCornerSize, rectangle.Location.Y - SelCornerSize);

            SelectionCorner[(int)CornerPosition.TopRight].Location =
                new Point(rectangle.Location.X + rectangle.Size.Width - SelCornerSize, rectangle.Location.Y - SelCornerSize);

            SelectionCorner[(int)CornerPosition.TopCenter].Location =
                new Point(rectangle.Location.X + rectangle.Size.Width / 2 - SelCornerSize, rectangle.Location.Y - SelCornerSize);

            SelectionCorner[(int)CornerPosition.BottomLeft].Location = new Point(rectangle.Location.X - SelCornerSize,
                rectangle.Location.Y + rectangle.Size.Height - SelCornerSize);

            SelectionCorner[(int)CornerPosition.BottomRight].Location =
                new Point(rectangle.Location.X + rectangle.Size.Width - SelCornerSize, rectangle.Location.Y + rectangle.Size.Height - SelCornerSize);

            SelectionCorner[(int)CornerPosition.BottomCenter].Location =
                new Point(rectangle.Location.X + rectangle.Size.Width / 2 - SelCornerSize, rectangle.Location.Y + rectangle.Size.Height - SelCornerSize);

            SelectionCorner[(int)CornerPosition.MiddleLeft].Location =
                new Point(rectangle.Location.X - SelCornerSize, rectangle.Location.Y + rectangle.Size.Height / 2 - SelCornerSize);

            SelectionCorner[(int)CornerPosition.MiddleCenter].Location =
                new Point(rectangle.Location.X + rectangle.Size.Width / 2 - SelCornerSize, rectangle.Location.Y + rectangle.Size.Height / 2 - SelCornerSize);

            SelectionCorner[(int)CornerPosition.MiddleRight].Location =
                new Point(rectangle.Location.X + rectangle.Size.Width - SelCornerSize, rectangle.Location.Y + rectangle.Size.Height / 2 - SelCornerSize);
        }
        void IResizeController.Start(Point startPoint, CornerPosition cornerPosition)
        {
            SelCorner = cornerPosition;
            DragOffset.X = SelectionCorner[(int)SelCorner].Location.X - startPoint.X;
            DragOffset.Y = SelectionCorner[(int)SelCorner].Location.Y - startPoint.Y;
        }
        void IResizeController.Resize(Point currentPoint)
        {
            var corner = SelectionCorner[(int)SelCorner];
            Point locationPoint;

            var dragPoint = currentPoint;

            dragPoint.Offset(DragOffset.X, DragOffset.Y);

            if (dragPoint.X < 0)
            {
                dragPoint.X = 0;
            }

            if (dragPoint.Y < 0)
            {
                dragPoint.Y = 0;
            }

            switch (SelCorner)
            {
                case CornerPosition.TopLeft:
                    corner.Location = dragPoint;
                    locationPoint = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
                    Element.Size = new Size(Element.Size.Width + (Element.Location.X - locationPoint.X), Element.Size.Height + (Element.Location.Y - locationPoint.Y));
                    Element.Location = locationPoint;
                    break;

                case CornerPosition.TopCenter:
                    corner.Location = new Point(corner.Location.X, dragPoint.Y);
                    locationPoint = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
                    Element.Size = new Size(Element.Size.Width, Element.Size.Height + (Element.Location.Y - locationPoint.Y));
                    Element.Location = new Point(Element.Location.X, locationPoint.Y);
                    break;

                case CornerPosition.TopRight:
                    corner.Location = dragPoint;
                    locationPoint = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
                    Element.Size = new Size(locationPoint.X - Element.Location.X, Element.Size.Height - (locationPoint.Y - Element.Location.Y));
                    Element.Location = new Point(Element.Location.X, locationPoint.Y);
                    break;

                case CornerPosition.MiddleLeft:
                    corner.Location = new Point(dragPoint.X, corner.Location.Y);
                    locationPoint = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
                    Element.Size = new Size(Element.Size.Width + (Element.Location.X - locationPoint.X), Element.Size.Height);
                    Element.Location = new Point(locationPoint.X, Element.Location.Y);
                    break;

                case CornerPosition.MiddleRight:
                    corner.Location = new Point(dragPoint.X, corner.Location.Y);
                    locationPoint = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
                    Element.Size = new Size(locationPoint.X - Element.Location.X, Element.Size.Height);
                    break;

                case CornerPosition.BottomLeft:
                    corner.Location = dragPoint;
                    locationPoint = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
                    Element.Size = new Size(Element.Size.Width - (locationPoint.X - Element.Location.X), locationPoint.Y - Element.Location.Y);
                    Element.Location = new Point(locationPoint.X, Element.Location.Y);
                    break;

                case CornerPosition.BottomCenter:
                    corner.Location = new Point(corner.Location.X, dragPoint.Y);
                    locationPoint = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
                    Element.Size = new Size(Element.Size.Width, locationPoint.Y - Element.Location.Y);
                    break;

                case CornerPosition.BottomRight:
                    corner.Location = dragPoint;
                    locationPoint = new Point(corner.Location.X + corner.Size.Width / 2, corner.Location.Y + corner.Size.Height / 2);
                    Element.Size = new Size(locationPoint.X - Element.Location.X, locationPoint.Y - Element.Location.Y);
                    break;
            }
        }
        void IResizeController.End(Point endPoint)
        {
            if ((Element.Size.Height < 0) || (Element.Size.Width < 0))
            {
                var rectangle = Element.GetUnsignedRectangle();
                Element.Location = rectangle.Location;
                Element.Size = rectangle.Size;
            }
            SelCorner = CornerPosition.Nothing;
            DragOffset = Point.Empty;
        }
        public virtual bool HitTest(Point point)
        {
            var graphicsPath = new GraphicsPath();
            var matrix = new Matrix();

            var elLocation = Element.Location;
            var elSize = Element.Size;

            graphicsPath.AddRectangle(new Rectangle(elLocation.X, elLocation.Y, elSize.Width, elSize.Height));
            graphicsPath.Transform(matrix);

            return graphicsPath.IsVisible(point);
        }
        public virtual bool HitTest(Rectangle rectangle)
        {
            var graphicsPath = new GraphicsPath();
            var matrix = new Matrix();

            var location = Element.Location;
            var size = Element.Size;
            graphicsPath.AddRectangle(new Rectangle(location.X, location.Y, size.Width, size.Height));
            graphicsPath.Transform(matrix);
            var rectangleRounded = Rectangle.Round(graphicsPath.GetBounds());
            return rectangle.Contains(rectangleRounded);
        }
        public virtual void DrawSelection(Graphics graphics)
        {
            const int border = 3;

            var location = Element.Location;
            var size = Element.Size;

            var rectangle = BaseElement.GetUnsignedRectangle(
                new Rectangle(location.X - border, location.Y - border, size.Width + border*2, size.Height + border*2));

            using (var brush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.LightGray, Color.Transparent))
            {
                using (var pen = new Pen(brush, border))
                {
                    graphics.DrawRectangle(pen, rectangle);
                }
            }
        }
        CornerPosition IResizeController.HitTestCorner(Point point)
        {
            for (var i = 0; i < SelectionCorner.Length; i++)
            {
                var controller = ((IControllable) SelectionCorner[i]).GetController();
                if (controller.HitTest(point))
                {
                    return (CornerPosition) i;
                }
            }
            return CornerPosition.Nothing;
        }   
    }
}