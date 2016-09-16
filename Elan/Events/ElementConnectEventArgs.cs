using System;
using Elan.Models.Base;
using Elan.Models.Implementations.Elements;

namespace Elan.Events
{
    public class ElementConnectEventArgs : EventArgs
    {
        public ElementConnectEventArgs(NodeElement node1, NodeElement node2, BaseLinkElement link)
        {
            Node1 = node1;
            Node2 = node2;
            Link = link;
        }

        public NodeElement Node1 { get; }
        public NodeElement Node2 { get; }
        public BaseLinkElement Link { get; }

        public override string ToString()
        {
            var toString = "";

            if (Node1 != null)
            {
                toString += "Узел 1:" + Node1;
            }

            if (Node2 != null)
            {
                toString += "Узел 2:" + Node2;
            }

            if (Link != null)
            {
                toString += "Линия:" + Link;
            }

            return toString;
        }
    }
}