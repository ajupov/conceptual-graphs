using System.Drawing;
using System.Drawing.Drawing2D;
using Elan.Models.Base;

namespace Elan.Controllers.Implementations
{
    internal class EllipseController : RectangleController
    {
        public EllipseController(BaseElement element)
            : base(element)
        {
        }

        public override void DrawSelection(Graphics graphics)
        {
            const int border = 3;

            var rectangle = BaseElement.GetUnsignedRectangle(new Rectangle(
                Element.Location.X - border, Element.Location.Y - border,
                Element.Size.Width + border * 2, Element.Size.Height + border * 2));

            using (var brush = new HatchBrush(HatchStyle.SmallCheckerBoard, Color.LightGray, Color.Transparent))
            {
                using (var pen = new Pen(brush, border))
                {
                    graphics.DrawEllipse(pen, rectangle);
                }
            }
        }

        public override bool HitTest(Point point)
        {
            var graphicsPath = new GraphicsPath();
            var matrix = new Matrix();

            graphicsPath.AddEllipse(new Rectangle(Element.Location.X, Element.Location.Y, Element.Size.Width, Element.Size.Height));
            graphicsPath.Transform(matrix);

            return graphicsPath.IsVisible(point);
        }
    }
}