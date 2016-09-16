using System.Drawing;
using Elan.Models.Base;

namespace Elan.Controllers.Implementations
{
    internal class ConnectorController : RectangleController
    {
        public ConnectorController(BaseElement element)
            : base(element)
        {
        }

        public override void DrawSelection(Graphics graphics)
        {
            const int distance = 1;
            const int border = 2;

            var rectangle = BaseElement.GetUnsignedRectangle(new Rectangle(
                Element.Location.X - distance, Element.Location.Y - distance,
                Element.Size.Width + distance*2, Element.Size.Height + distance*2));

            using (var brush = new SolidBrush(Color.FromArgb(150, Color.Green)))
            {
                using (var pen = new Pen(brush, border))
                {
                    graphics.DrawRectangle(pen, rectangle);
                }
            }
        }
    }
}