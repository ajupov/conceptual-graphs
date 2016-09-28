using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Elan.Models.Implementations.Elements;

namespace Elan.Models.Base
{
    [Serializable]
    public abstract class BaseLinkElement : BaseElement
    {
        protected ConnectorElement connector1;

        protected ConnectorElement connector2;

        protected LineCap startCap;

        protected LineCap endCap;

        protected bool needCalcLink = true;

        internal BaseLinkElement(ConnectorElement connectorElement1, ConnectorElement connectorElement2)
        {
            borderWidth = 1;

            connector1 = connectorElement1;
            connector2 = connectorElement2;

            connector1.AddLink(this);
            connector2.AddLink(this);
        }

        [Browsable(false)]
        public ConnectorElement Connector1
        {
            get { return connector1; }
            set
            {
                if (value == null)
                {
                    return;
                }

                connector1.RemoveLink(this);
                connector1 = value;
                needCalcLink = true;
                connector1.AddLink(this);
                OnConnectorChanged(new EventArgs());
            }
        }

        [Browsable(false)]
        public ConnectorElement Connector2
        {
            get { return connector2; }
            set
            {
                if (value == null)
                {
                    return;
                }

                connector2.RemoveLink(this);
                connector2 = value;
                needCalcLink = true;
                connector2.AddLink(this);
                OnConnectorChanged(new EventArgs());
            }
        }

        [Browsable(false)]
        internal bool NeedCalcLink
        {
            get { return needCalcLink; }
            set { needCalcLink = value; }
        }

        [Browsable(false)]
        public abstract override Point Location { get; }

        [Browsable(false)]
        public abstract override Size Size { get; }

        [Browsable(false)]
        public abstract LineElement[] Lines { get; }

        [Browsable(false)]
        public abstract Point Point1 { get; }

        [Browsable(false)]
        public abstract Point Point2 { get; }

        [Browsable(false)]
        public virtual LineCap StartCap
        {
            get { return startCap; }
            set { startCap = value; }
        }

        [Browsable(false)]
        public virtual LineCap EndCap
        {
            get { return endCap; }
            set { endCap = value; }
        }

        internal abstract void CalcLink();

        protected virtual void OnConnectorChanged(EventArgs e)
        {
            ConnectorChanged?.Invoke(this, e);
        }

        [field: NonSerialized]
        public event EventHandler ConnectorChanged;
    }
}