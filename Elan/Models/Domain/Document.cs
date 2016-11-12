using System.Collections.Generic;

namespace Elan.Models.Domain
{
    public class Document
    {
        public Document()
        {
            Nodes = new List<Node>();
            Links = new List<Link>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public List<Node> Nodes { get; set; }

        public List<Link> Links { get; set; }
    }
}