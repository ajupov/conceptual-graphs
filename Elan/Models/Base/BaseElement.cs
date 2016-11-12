using System;
using System.ComponentModel;
using System.Drawing;

namespace Elan.Models.Base
{
    [Serializable]
    public abstract class BaseElement
    {
        protected BaseElement()
        {
        }

        protected BaseElement(int top, int left, int width, int height)
        {
            location = new Point(top, left);
            size = new Size(width, height);
        }

        [Browsable(false)]
        public int Id { get; set; }

        protected int borderWidth = 1;

        protected internal Rectangle InvalidateRec = Rectangle.Empty;

        protected internal bool IsInvalidated = true;

        protected Point location;

        public Size size;
        
        [DisplayName("Позиция")]
        public virtual Point Location
        {
            get { return location; }
            set
            {
                location = value;
                OnAppearanceChanged(new EventArgs());
            }
        }

        [DisplayName("Размер")]
        public virtual Size Size
        {
            get { return size; }
            set
            {
                size = value;
                OnAppearanceChanged(new EventArgs());
            }
        }

        [Browsable(false)]
        public virtual int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                borderWidth = value;
                OnAppearanceChanged(new EventArgs());
            }
        }

        public virtual void Draw(Graphics graphics)
        {
            IsInvalidated = false;
        }

        public virtual void Invalidate()
        {
            InvalidateRec = IsInvalidated 
                ? Rectangle.Union(InvalidateRec, GetUnsignedRectangle()) 
                : GetUnsignedRectangle();

            IsInvalidated = true;
        }

        protected virtual void OnAppearanceChanged(EventArgs e)
        {
            AppearanceChanged?.Invoke(this, e);
        }

        public virtual Rectangle GetRectangle()
        {
            return new Rectangle(Location, Size);
        }

        public virtual Rectangle GetUnsignedRectangle()
        {
            return GetUnsignedRectangle(GetRectangle());
        }

        public static Rectangle GetUnsignedRectangle(Rectangle rectangle)
        {
            var unsignedRectangle = rectangle;
            if (rectangle.Width < 0)
            {
                unsignedRectangle.X = rectangle.X + rectangle.Width;
                unsignedRectangle.Width = -rectangle.Width;
            }

            if (rectangle.Height < 0)
            {
                unsignedRectangle.Y = rectangle.Y + rectangle.Height;
                unsignedRectangle.Height = -rectangle.Height;
            }

            return unsignedRectangle;
        }

        [field: NonSerialized]
        public event EventHandler AppearanceChanged;
    }
}