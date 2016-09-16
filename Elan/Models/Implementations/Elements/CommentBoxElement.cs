using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Elan.Controllers.Contracts;
using Elan.Controllers.Implementations;

namespace Elan.Models.Implementations.Elements
{
    [Serializable]
    public class CommentBoxElement : RectangleElement, IControllable
    {
        public CommentBoxElement() : this(0, 0, 100, 100)
        {
        }
        public CommentBoxElement(Rectangle rectangle)
            : this(rectangle.Location, rectangle.Size)
        {
        }
        public CommentBoxElement(Point point, Size size)
            : this(point.X, point.Y, size.Width, size.Height)
        {
        }
        public CommentBoxElement(int top, int left, int width, int height)
            : base(top, left, width, height)
        {
            fillColor = Color.LemonChiffon;
        }

        protected Size FoldSize = new Size(10, 15);
        [NonSerialized]
        private RectangleController _controller;

        public override void Draw(Graphics graphics)
        {
            IsInvalidated = false;

            var rectangle = GetUnsignedRectangle(new Rectangle(location, size));

            var points = new[]
            {
                new Point(rectangle.X + 0, rectangle.Y + 0),
                new Point(rectangle.X + 0, rectangle.Y + rectangle.Height),
                new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),
                new Point(rectangle.X + rectangle.Width, rectangle.Y + FoldSize.Height),
                new Point(rectangle.X + rectangle.Width - FoldSize.Width, rectangle.Y + 0),
            };

            graphics.FillPolygon(GetBrush(), points, FillMode.Alternate);
            graphics.DrawPolygon(new Pen(Color.Black, borderWidth), points);

            graphics.DrawLine(new Pen(Color.Black, borderWidth),
                new Point(rectangle.X + rectangle.Width - FoldSize.Width, rectangle.Y + FoldSize.Height),
                new Point(rectangle.X + rectangle.Width, rectangle.Y + FoldSize.Height));

            graphics.DrawLine(new Pen(Color.Black, borderWidth),
                new Point(rectangle.X + rectangle.Width - FoldSize.Width, rectangle.Y + 0),
                new Point(rectangle.X + rectangle.Width - FoldSize.Width, rectangle.Y + 0 + FoldSize.Height));
        }
        IController IControllable.GetController()
        {
            return _controller ?? (_controller = new CommentBoxController(this));
        }
    }
}