using System.ComponentModel;
using System.Windows.Forms;

namespace Elan.Forms
{
    partial class DocumentListForm
    {
        #region Свойства
        private DataGridView dataGridViewDocuments;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Name;
        #endregion

        #region Инициализация компонентов
        private void InitializeComponent()
        {
            dataGridViewDocuments = new DataGridView();
            Id = new DataGridViewTextBoxColumn();
            Name = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)(dataGridViewDocuments)).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewDocuments
            // 
            dataGridViewDocuments.AllowUserToAddRows = false;
            dataGridViewDocuments.AllowUserToDeleteRows = false;
            dataGridViewDocuments.AllowUserToResizeColumns = false;
            dataGridViewDocuments.AllowUserToResizeRows = false;
            dataGridViewDocuments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewDocuments.Columns.AddRange(new DataGridViewColumn[] {Id, Name});
            dataGridViewDocuments.Dock = DockStyle.Fill;
            dataGridViewDocuments.Location = new System.Drawing.Point(0, 0);
            dataGridViewDocuments.MultiSelect = false;
            dataGridViewDocuments.Name = "dataGridViewDocuments";
            dataGridViewDocuments.ReadOnly = true;
            dataGridViewDocuments.RowHeadersVisible = false;
            dataGridViewDocuments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewDocuments.Size = new System.Drawing.Size(584, 361);
            dataGridViewDocuments.TabIndex = 0;
            dataGridViewDocuments.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(DataGridViewDocumentsCellMouseDoubleClick);
            // 
            // Id
            // 
            Id.Frozen = true;
            Id.HeaderText = "Номер";
            Id.Name = "Id";
            Id.ReadOnly = true;
            Id.Resizable = DataGridViewTriState.False;
            Id.Width = 80;
            // 
            // Name
            // 
            Name.Frozen = true;
            Name.HeaderText = "Название";
            Name.Name = "Name";
            Name.ReadOnly = true;
            Name.Resizable = DataGridViewTriState.False;
            Name.Width = 500;
            // 
            // DocumentListForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(584, 361);
            Controls.Add(dataGridViewDocuments);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Документы";
            ((ISupportInitialize)(dataGridViewDocuments)).EndInit();
            ResumeLayout(false);

        }
        #endregion
    }
}