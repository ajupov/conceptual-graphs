namespace Elan.Models.Domain
{
    public class Link
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int StartNodeId { get; set; }
        public int EndNodeId { get; set; }
        public string Label { get; set; }
        public int StartPointX { get; set; }
        public int StartPointY { get; set; }
        public int EndPointX { get; set; }
        public int EndPointY { get; set; }
    }
}