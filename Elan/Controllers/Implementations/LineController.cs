using System.Drawing;
using System.Drawing.Drawing2D;
using Elan.Controllers.Contracts;
using Elan.Models.Base;
using Elan.Models.Implementations.Elements;

namespace Elan.Controllers.Implementations
{
    internal class LineController : IController
    {
        public LineController(LineElement element)
        {
            ParentElement = element;
        }

        public BaseElement OwnerElement => ParentElement;

        protected LineElement ParentElement { get; }

        public void DrawSelection(Graphics graphics)
        {
            var pen = new Pen(Color.FromArgb(80, Color.Yellow), ParentElement.BorderWidth + 2)
            {
                StartCap = LineCap.Round,
                CustomEndCap = new AdjustableArrowCap(5, 5)
            };
            graphics.DrawLine(pen, ParentElement.StartPoint, ParentElement.EndPoint);
        }

        public bool HitTest(Point point)
        {
            var graphicsPath = new GraphicsPath();
            var matrix = new Matrix();
            var pen = new Pen(Color.Black, ParentElement.BorderWidth + 4)
            {
                StartCap = LineCap.Round,
                CustomEndCap = new AdjustableArrowCap(5, 5)
            };
            graphicsPath.AddLine(ParentElement.StartPoint, ParentElement.EndPoint);
            graphicsPath.Transform(matrix);
            return graphicsPath.IsOutlineVisible(point, pen);
        }

        public bool HitTest(Rectangle rectangle)
        {
            var graphicsPath = new GraphicsPath();
            var matrix = new Matrix();

            graphicsPath.AddRectangle(new Rectangle(
                ParentElement.Location.X, ParentElement.Location.Y,
                ParentElement.Size.Width, ParentElement.Size.Height));
            graphicsPath.Transform(matrix);
            var rectangleRounded = Rectangle.Round(graphicsPath.GetBounds());
            return rectangle.Contains(rectangleRounded);
        }
    }
}