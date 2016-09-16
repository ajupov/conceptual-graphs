using System;
using System.Drawing;
using Elan.Controllers.Contracts;
using Elan.Controllers.Implementations;
using Elan.Enums;
using Elan.Helpers;
using Elan.Models.Base;
using Elan.Models.Implementations.Collections;

namespace Elan.Models.Implementations.Elements
{
    [Serializable]
    public class ConnectorElement : RectangleElement, IControllable
    {
        internal ConnectorElement(NodeElement parent)
            : base(new Rectangle(0, 0, 0, 0))
        {
            ParentElement = parent;
            fillColor = Color.LightGray;
        }

        public NodeElement ParentElement { get; }
        public ElementCollection Links { get; } = new ElementCollection();
        public override Point Location
        {
            get { return base.Location; }
            set
            {
                if (value == base.Location) return;

                foreach (BaseLinkElement baseLinkElement in Links)
                {
                    baseLinkElement.NeedCalcLink = true;
                }
                base.Location = value;
            }
        }
        public override Size Size
        {
            get { return base.Size; }
            set
            {
                if (value == base.Size) return;

                foreach (BaseLinkElement baseLinkElement in Links)
                {
                    baseLinkElement.NeedCalcLink = true;
                }
                base.Size = value;
            }
        }

        [NonSerialized]
        private ConnectorController _controller;

        internal void AddLink(BaseLinkElement baseLinkElement)
        {
            Links.Add(baseLinkElement);
        }
        internal void RemoveLink(BaseLinkElement baseLinkElement)
        {
            Links.Remove(baseLinkElement);
        }
        internal CardinalDirection GetDirection()
        {
            var rectangle = new Rectangle(ParentElement.Location, ParentElement.Size);
            var point = new Point(location.X - ParentElement.Location.X + size.Width/2,
                location.Y - ParentElement.Location.Y + size.Height/2);

            return DiagramHelper.GetDirection(rectangle, point);
        }
        IController IControllable.GetController()
        {
            return _controller ?? (_controller = new ConnectorController(this));
        }
    }
}