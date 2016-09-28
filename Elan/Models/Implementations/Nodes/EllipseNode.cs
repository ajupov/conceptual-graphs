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
    public class EllipseNode : NodeElement, IControllable, ILabelElement
    {
        public EllipseNode(Rectangle rectangle)
            : base(rectangle.Location.X, rectangle.Location.Y, rectangle.Size.Width, rectangle.Size.Height)
        {
            Ellipse = new EllipseElement(rectangle.Location.X, rectangle.Location.Y, rectangle.Size.Width, rectangle.Size.Height);
            SyncConstructors();
        }

        protected EllipseElement Ellipse;

        protected LabelElement LabelElement = new LabelElement();

        public override Point Location
        {
            get { return base.Location; }
            set
            {
                Ellipse.Location = value;
                base.Location = value;
            }
        }

        public override Size Size
        {
            get { return base.Size; }
            set
            {
                Ellipse.Size = value;
                base.Size = value;
            }
        }

        public override int BorderWidth
        {
            get { return base.BorderWidth; }
            set
            {
                Ellipse.BorderWidth = value;
                base.BorderWidth = value;
            }
        }

        [Browsable(false)]
        public virtual LabelElement Label
        {
            get { return LabelElement; }
            set
            {
                LabelElement = value;
                OnAppearanceChanged(new EventArgs());
            }
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
            Ellipse.Draw(graphics);
        }

        private void SyncConstructors()
        {
            location = Ellipse.Location;
            size = Ellipse.Size;
            borderWidth = Ellipse.BorderWidth;
        }  
    }
}