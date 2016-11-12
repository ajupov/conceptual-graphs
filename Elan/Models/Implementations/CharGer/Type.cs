using System.Xml.Linq;

namespace Elan.Models.Implementations.CharGer
{
    public class Type
    {
        public Type(string label)
        {
            Label = label;
        }

        public Type(XContainer container)
        {
            var xElement = container.Element("label");
            if (xElement != null)
            {
                Label = xElement.Value;
            }
        }

        public string Label { get; set; }
    }
}