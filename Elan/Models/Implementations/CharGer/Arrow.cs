using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Elan.Models.Implementations.CharGer
{
    public class Arrow : Base
    {
        public Arrow(XElement element)
            : base(element)
        {
            foreach (var attribute in element.Attributes())
            {
                switch (attribute.Name.ToString())
                {
                    case "from":
                        From = Convert.ToInt64(attribute.Value);
                        break;
                    case "to":
                        To = Convert.ToInt64(attribute.Value);
                        break;
                    case "label":
                        Type = new Type("-");
                        break;
                }

            }
        }

        public long From { get; set; }

        public long To { get; set; }

        public override object[] ParamsAsArray()
        {
            var parameters = new List<string>
            {
                Id.ToString(),
                Owner.ToString(),
                "'" + Type.Label + "'",
                From.ToString(),
                To.ToString(),
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