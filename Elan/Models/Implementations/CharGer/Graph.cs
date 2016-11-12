using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Elan.Models.Implementations.CharGer
{
    public class Graph : Base
    {
        public Graph(XElement element)
            : base(element)
        {
            Relations = new List<Relation>();
            Concepts = new List<Concept>();
            Arrows = new List<Arrow>();

            foreach (var item in element.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "relation":
                        Relations.Add(new Relation(item));
                        break;
                    case "concept":
                        Concepts.Add(new Concept(item));
                        break;
                    case "arrow":
                        Arrows.Add(new Arrow(item));
                        break;
                    case "type":
                        Type = ParseType(item);
                        break;
                }
            }
        }

        public List<Relation> Relations { get; set; }

        public List<Concept> Concepts { get; set; }

        public List<Arrow> Arrows { get; set; }

        public void FillTables(DataGridView conceptsTable, DataGridView relationsTable, DataGridView arrowsTable)
        {
            foreach (var item in Concepts)
            {
                conceptsTable.Rows.Add(item.ParamsAsArray());
            }

            foreach (var item in Relations)
            {
                relationsTable.Rows.Add(item.ParamsAsArray());
            }

            foreach (var item in Arrows)
            {
                arrowsTable.Rows.Add(item.ParamsAsArray());
            }
        }

        public Type ParseType(XElement element)
        {
            var type = new Type("NoName");
            foreach (var item in element.Elements())
            {
                switch (item.Name.ToString())
                {
                    case "label":
                        type = new Type(element);
                        break;
                }
            }

            return type;
        }
    }
}