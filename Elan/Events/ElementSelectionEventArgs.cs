using System;
using Elan.Models.Implementations.Collections;

namespace Elan.Events
{
    public class ElementSelectionEventArgs : EventArgs
    {
        public ElementSelectionEventArgs(ElementCollection elements)
        {
            Elements = elements;
        }

        public ElementCollection Elements { get; }

        public override string ToString()
        {
            return $"Количество коллекции элементов: {Elements.Count}";
        }
    }
}