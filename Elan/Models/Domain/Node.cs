using Elan.Enums;

namespace Elan.Models.Domain
{
    public class Node
    {
        public long Id { get; set; }

        public long DocumentId { get; set; }

        public NodeType Type { get; set; }

        public string Label { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}