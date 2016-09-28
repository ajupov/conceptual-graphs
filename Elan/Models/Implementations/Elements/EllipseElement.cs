using System;
using System.Drawing;
using Elan.Controllers.Contracts;
using Elan.Controllers.Implementations;

namespace Elan.Models.Implementations.Elements
{
    [Serializable]
    public class EllipseElement : RectangleElement, IControllable
    {
        public EllipseElement(Rectangle rectangle)
            : base(rectangle)
        {
        }

        public EllipseElement(int top, int left, int width, int height)
            : base(top, left, width, height)
        {
        }

        [NonSerialized]
        private EllipseController _controller;

        IController IControllable.GetController()
        {
            return _controller ?? (_controller = new EllipseController(this));
        }

        public override void Draw(Graphics graphics)
        {
            IsInvalidated = false;

            var unsignedRectangle = GetUnsignedRectangle(new Rectangle(location.X, location.Y, size.Width, size.Height));

            using (var brush = new SolidBrush(Color.DarkOrange))
            {
                graphics.FillEllipse(brush, unsignedRectangle);

                using (var pen = new Pen(Color.Black, borderWidth))
                {
                    graphics.DrawEllipse(pen, unsignedRectangle);
                }
            }
        }
    }
}