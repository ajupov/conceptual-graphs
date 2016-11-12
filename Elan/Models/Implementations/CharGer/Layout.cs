using System.Xml.Linq;
using Elan.Models.Base.Charger;

namespace Elan.Models.Implementations.CharGer
{
    public class Layout
    {
        public Layout(XContainer container)
        {
            foreach (var item in container.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "rectangle":
                        Rectangle = new Rectangle(item.Attributes());
                        break;
                    case "color":
                        Color = new Color(item.Attributes());
                        break;
                }
            }
        }

        public Rectangle Rectangle { get; set; }

        public Color Color { get; set; }  
    }
}