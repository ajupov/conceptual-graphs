using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Elan.Models.Implementations.CharGer
{
    public abstract class Base
    {
        protected Base(XElement element)
        {
            foreach (var attribute in element.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "id":
                        Id = Convert.ToInt64(attribute.Value);
                        break;
                    case "owner":
                        Owner = Convert.ToInt64(attribute.Value);
                        break;
                }
            }

            foreach (var item in element.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "type":
                        Type = new Type(item);
                        break;
                    case "layout":
                        Layout = new Layout(item);
                        break;
                }
            }
        }

        public long Id { get; set; }

        public long Owner { get; set; }

        public Type Type { get; set; }

        public Layout Layout { get; set; }

        public virtual object[] ParamsAsArray() 
        {
            var parameters = new List<string>
            {
                Id.ToString(),
                Owner.ToString(),
                "'" + Type.Label + "'",
                Layout.Rectangle.Width.ToString(CultureInfo.InvariantCulture),
                Layout.Rectangle.Height.ToString(CultureInfo.InvariantCulture),
                Convert.ToInt32(Layout.Rectangle.X).ToString(),
                Convert.ToInt32(Layout.Rectangle.Y).ToString(),
                Layout.Color.Foreground.ToString(),
                Layout.Color.Background.ToString()
            };

            return parameters.ToArray();
        }
    }
}