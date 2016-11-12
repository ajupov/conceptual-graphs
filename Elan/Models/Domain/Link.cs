namespace Elan.Models.Domain
{
    public class Link
    {
        public long Id { get; set; }

        public long DocumentId { get; set; }

        public long StartNodeId { get; set; }

        public long EndNodeId { get; set; }

        public string Label { get; set; }

        public int StartPointX { get; set; }

        public int StartPointY { get; set; }

        public int EndPointX { get; set; }

        public int EndPointY { get; set; }
    }
}