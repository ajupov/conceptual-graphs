using System.Collections.Generic;

namespace Elan.Models.Domain
{
    public class Document
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Node> Nodes { get; set; }

        public List<Link> Links { get; set; }
    }
}