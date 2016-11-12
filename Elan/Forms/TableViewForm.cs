using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Elan.Enums;
using Elan.Models.Domain;
using Elan.Models.Implementations.Tuples;

namespace Elan.Forms
{
    public partial class TableViewForm : Form
    {
        public List<DataGridView> DataGridViews { get; set; } = new List<DataGridView>();

        public TableViewForm(Document document)
        {
            InitializeComponent();

            var tableHeaders = new List<TableItemTuple>();

            foreach (var node in document.Nodes)
            {
                if (node.Type == NodeType.Relation && tableHeaders.All(h => h.RightColumn != node.Label))
                {
                    tableHeaders.Add(new TableItemTuple {LeftColumn = "Факт", RightColumn = node.Label});
                }
            }

            tableHeaders = tableHeaders.OrderBy(h => h.RightColumn).ToList();

            foreach (var tableHeader in tableHeaders)
            {
                var dataGridView = new DataGridView();
                dataGridView.Columns.Add("LeftColumn", tableHeader.LeftColumn);
                dataGridView.Columns.Add("RightColumn", tableHeader.RightColumn);
                dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;

                foreach (var node in document.Nodes)
                {
                    if (node.Label == tableHeader.RightColumn)
                    {
                        var leftArrow = document.Links.FirstOrDefault(l => l.EndNodeId == node.Id);
                        var rightArrow = document.Links.FirstOrDefault(l => l.StartNodeId == node.Id);
                        if (leftArrow == null || rightArrow == null)
                        {
                            continue;
                        }

                        var leftNode = document.Nodes.FirstOrDefault(n => n.Id == leftArrow.StartNodeId);
                        var rightNode = document.Nodes.FirstOrDefault(n => n.Id == rightArrow.EndNodeId);
                        if (leftNode == null || rightNode == null)
                        {
                            continue;
                        }

                        dataGridView.Rows.Add(leftNode.Label, rightNode.Label);
                    }
                }

                DataGridViews.Add(dataGridView);
            }

            var horisontalCounter = 0;
            var verticalCounter = 0;

            foreach (var dataGridView in DataGridViews)
            {
                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToResizeRows = false;
                dataGridView.Width = 220;
                dataGridView.Location = new Point
                {
                    X = horisontalCounter * 220,
                    Y = verticalCounter * 220
                };
                horisontalCounter++;
                if (horisontalCounter > Width - 220)
                {
                    verticalCounter++;
                }
                var collumn = dataGridView.Columns["LeftColumn"];
                if (collumn != null)
                {
                    dataGridView.Sort(collumn, ListSortDirection.Ascending);
                }
                Controls.Add(dataGridView);
            }
        }
    }
}