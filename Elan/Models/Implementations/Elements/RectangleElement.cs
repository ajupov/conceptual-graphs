using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Elan.Controllers.Contracts;
using Elan.Controllers.Implementations;
using Elan.Models.Base;
using Elan.Models.Contracts;

namespace Elan.Models.Implementations.Elements
{
    [Serializable]
    public class RectangleElement : BaseElement, IControllable, ILabelElement
    {
        public RectangleElement()
            : this(0, 0, 100, 100)
        {
        }
        public RectangleElement(Rectangle rectangle)
            : this(rectangle.Location, rectangle.Size)
        {
        }
        public RectangleElement(Point point, Size size)
            : this(point.X, point.Y, size.Width, size.Height)
        {
        }
        public RectangleElement(int top, int left, int width, int height)
        {
            location = new Point(top, left);
            size = new Size(width, height);
        }

        [DisplayName("Цвет")]
        public virtual Color FillColor
        {
            get { return fillColor; }
            set
            {
                fillColor = value;
                OnAppearanceChanged(new EventArgs());
            }
        }
        [DisplayName("Метка")]
        public virtual LabelElement Label
        {
            get { return label; }
            set
            {
                label = value;
                OnAppearanceChanged(new EventArgs());
            }
        }

        protected Color fillColor = Color.DodgerBlue;
        protected LabelElement label = new LabelElement();
        [NonSerialized]
        private RectangleController _controller;

        public override void Draw(Graphics graphics)
        {
            IsInvalidated = false;

            var rectangle = GetUnsignedRectangle();
            using (var brush = GetBrush())
            {
                graphics.FillRectangle(brush, rectangle);
                DrawBorder(graphics, rectangle);
            }
        }
        public void DrawSelection(Graphics graphics)
        {
            IsInvalidated = false;

            var rectangle = GetUnsignedRectangle();
            using (var brush = GetOpacityBrush(40))
            {
                graphics.FillRectangle(brush, rectangle);
                DrawBorder(graphics, rectangle);
            }
        }

        protected virtual void DrawBorder(Graphics graphics, Rectangle rectangle)
        {
            using (var pen = new Pen(Color.Black, borderWidth))
            {
                graphics.DrawRectangle(pen, rectangle);
            }
        }
        protected virtual Brush GetBrush()
        {
            return new SolidBrush(fillColor);
        }

        private Brush GetOpacityBrush(int opacity)
        {
            var color = Color.FromArgb((int)(255.0f * (opacity / 100.0f)), fillColor);
            return new SolidBrush(color);
        }

        IController IControllable.GetController()
        {
            return _controller ?? (_controller = new RectangleController(this));
        }
    }
}