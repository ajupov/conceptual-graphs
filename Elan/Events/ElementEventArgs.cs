using System;
using Elan.Models.Base;

namespace Elan.Events
{
    public class ElementEventArgs : EventArgs
    {
        public ElementEventArgs(BaseElement element)
        {
            Element = element;
        }

        public BaseElement Element { get; }

        public override string ToString()
        {
            return "Ёлемент родитель: " + Element.GetHashCode();
        }
    }
}