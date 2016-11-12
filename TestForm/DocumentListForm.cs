using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elan.Models.Domain;

namespace TestForm
{
    public partial class DocumentListForm : Form
    {
        public int SelectedDocumentId { get; set; }

        public DocumentListForm(List<Document> documents)
        {
            InitializeComponent();
            documents.ForEach(d => dataGridViewDocuments.Rows.Add(d.Id, d.Name));
        }

        private void DataGridViewDocumentsCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var cellValue = dataGridViewDocuments.Rows[e.RowIndex].Cells["Id"].Value;
            SelectedDocumentId = Convert.ToInt32(cellValue);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
