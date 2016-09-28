using System;
using System.ComponentModel;
using System.Drawing;
using Elan.Controllers.Contracts;
using Elan.Controllers.Implementations;
using Elan.Helpers;
using Elan.Models.Base;
using Elan.Models.Contracts;

namespace Elan.Models.Implementations.Elements
{
    [Serializable]
    public class StraightLinkElement : BaseLinkElement, IControllable, ILabelElement
    {
        public StraightLinkElement(ConnectorElement connectorElement1, ConnectorElement connectorElement2)
            : base(connectorElement1, connectorElement2)
        {
            Id = FictitiousIdHelper.NextId;
            label.PositionBySite(line1);
        }

        [Browsable(false)]
        public override Point Point1 => line1.StartPoint;

        [Browsable(false)]
        public override Point Point2 => line1.EndPoint;

        public override int BorderWidth
        {
            get { return line1.BorderWidth; }
            set { line1.BorderWidth = value; }
        }

        public override Point Location
        {
            get
            {
                CalcLink();
                return line1.Location;
            }
        }

        public override Size Size
        {
            get
            {
                CalcLink();
                return line1.Size;
            }
        }

        public override LineElement[] Lines => new[] { line1 };

        protected LabelElement label = new LabelElement();

        protected LineElement line1 = new LineElement(0, 0, 0, 0);

        [NonSerialized]
        private LineController _controller;
        
        public virtual LabelElement Label
        {
            get { return label; }
            set
            {
                label = value;
                OnAppearanceChanged(new EventArgs());
            }
        }

        public override void Draw(Graphics graphics)
        {
            IsInvalidated = false;
            line1.Draw(graphics);
        }

        internal override void CalcLink()
        {
            if (needCalcLink == false)
            {
                return;
            }

            if (line1 != null)
            {
                var connector1Location = connector1.Location;
                var connector2Location = connector2.Location;
                var connector1Size = connector1.Size;
                var connector2Size = connector2.Size;

                line1.StartPoint = new Point(connector1Location.X + connector1Size.Width/2,
                    connector1Location.Y + connector1Size.Height/2);
                line1.EndPoint = new Point(connector2Location.X + connector2Size.Width/2,
                    connector2Location.Y + connector2Size.Height/2);
                line1.Calc();
            }

            needCalcLink = false;
        }
        IController IControllable.GetController()
        {
            return _controller ?? (_controller = new LineController(line1));
        }
    }
}