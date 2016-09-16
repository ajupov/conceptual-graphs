using Elan.Models.Base;

namespace Elan.Events
{
    public class ElementMouseEventArgs : ElementEventArgs
    {
        public ElementMouseEventArgs(BaseElement element, int x, int y)
            : base(element)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return base.ToString() + " X:" + X + " Y:" + Y;
        }
    }
}