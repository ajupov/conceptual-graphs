using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Elan.Models.Base;

namespace Elan.Models.Implementations.Elements
{
    [Serializable]
    public abstract class NodeElement : BaseElement
    {
        protected NodeElement(int top, int left, int width, int height)
            : base(top, left, width, height)
        {
            InitConnectors();
        }

        protected const int ConnectSize = 3;

        [Browsable(false)]
        public virtual ConnectorElement[] Connectors => Connects;

        public override Point Location
        {
            get { return location; }
            set
            {
                location = value;
                UpdateConnectorsPosition();
                OnAppearanceChanged(new EventArgs());
            }
        }

        public override Size Size
        {
            get { return size; }
            set
            {
                size = value;
                UpdateConnectorsPosition();
                OnAppearanceChanged(new EventArgs());
            }
        }

        [Browsable(false)]
        public virtual bool IsConnected => Connects.Any(c => c.Links.Count > 0);

        protected ConnectorElement[] Connects = new ConnectorElement[4];

        protected void InitConnectors()
        {
            Connects[0] = new ConnectorElement(this);
            Connects[1] = new ConnectorElement(this);
            Connects[2] = new ConnectorElement(this);
            Connects[3] = new ConnectorElement(this);
            UpdateConnectorsPosition();
        }
        protected void UpdateConnectorsPosition()
        {
            //Top
            var point = new Point(location.X + size.Width/2, location.Y);
            var connect = Connects[0];
            connect.Location = new Point(point.X - ConnectSize, point.Y - ConnectSize);
            connect.Size = new Size(ConnectSize*2, ConnectSize*2);

            //Botton
            point = new Point(location.X + size.Width/2, location.Y + size.Height);
            connect = Connects[1];
            connect.Location = new Point(point.X - ConnectSize, point.Y - ConnectSize);
            connect.Size = new Size(ConnectSize*2, ConnectSize*2);

            //Left
            point = new Point(location.X, location.Y + size.Height/2);
            connect = Connects[2];
            connect.Location = new Point(point.X - ConnectSize, point.Y - ConnectSize);
            connect.Size = new Size(ConnectSize*2, ConnectSize*2);

            //Right
            point = new Point(location.X + size.Width, location.Y + size.Height/2);
            connect = Connects[3];
            connect.Location = new Point(point.X - ConnectSize, point.Y - ConnectSize);
            connect.Size = new Size(ConnectSize*2, ConnectSize*2);
        }

        public override void Invalidate()
        {
            base.Invalidate();

            for (var i = Connects.Length - 1; i >= 0; i--)
            {
                for (var ii = Connects[i].Links.Count - 1; ii >= 0; ii--)
                {
                    Connects[i].Links[ii].Invalidate();
                }
            }
        }

        internal virtual void Draw(Graphics graphics, bool drawConnector)
        {
            Draw(graphics);
            if (drawConnector)
            {
                DrawConnectors(graphics);
            }
        }

        protected void DrawConnectors(Graphics graphics)
        {
            foreach (var c in Connects)
            {
                c.Draw(graphics);
            }
        }
    }
}