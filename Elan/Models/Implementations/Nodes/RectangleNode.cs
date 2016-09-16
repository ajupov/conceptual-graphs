using System;
using System.ComponentModel;
using System.Drawing;
using Elan.Controllers.Contracts;
using Elan.Controllers.Implementations;
using Elan.Models.Contracts;
using Elan.Models.Implementations.Elements;

namespace Elan.Models.Implementations.Nodes
{
    [Serializable]
    public class RectangleNode : NodeElement, IControllable, ILabelElement
    {
        public RectangleNode(Rectangle rectangle)
            : base(rectangle.Location.X, rectangle.Location.Y, rectangle.Size.Width, rectangle.Size.Height)
        {
            _rectangle = new RectangleElement(rectangle.Location.X, rectangle.Location.Y, rectangle.Size.Width, rectangle.Size.Height);
            SyncConstructors();
        }

        public override Point Location
        {
            get { return base.Location; }
            set
            {
                _rectangle.Location = value;
                base.Location = value;
            }
        }
        public override Size Size
        {
            get { return base.Size; }
            set
            {
                _rectangle.Size = value;
                base.Size = value;
            }
        }

        [Browsable(false)]
        public virtual LabelElement Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnAppearanceChanged(new EventArgs());
            }
        }

        private LabelElement _label = new LabelElement();
        private readonly RectangleElement _rectangle;

        [NonSerialized]
        private RectangleController _controller;

        public override void Draw(Graphics graphics)
        {
            IsInvalidated = false;
            _rectangle.Draw(graphics);
        }
        private void SyncConstructors()
        {
            location = _rectangle.Location;
            size = _rectangle.Size;
            borderWidth = _rectangle.BorderWidth;
        }
        IController IControllable.GetController()
        {
            return _controller ?? (_controller = new RectangleController(this));
        }
    }
}