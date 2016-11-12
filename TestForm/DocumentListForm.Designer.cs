namespace TestForm
{
    partial class DocumentListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewDocuments = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDocuments)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewDocuments
            // 
            this.dataGridViewDocuments.AllowUserToAddRows = false;
            this.dataGridViewDocuments.AllowUserToDeleteRows = false;
            this.dataGridViewDocuments.AllowUserToResizeColumns = false;
            this.dataGridViewDocuments.AllowUserToResizeRows = false;
            this.dataGridViewDocuments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDocuments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Name});
            this.dataGridViewDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDocuments.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDocuments.MultiSelect = false;
            this.dataGridViewDocuments.Name = "dataGridViewDocuments";
            this.dataGridViewDocuments.ReadOnly = true;
            this.dataGridViewDocuments.RowHeadersVisible = false;
            this.dataGridViewDocuments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDocuments.Size = new System.Drawing.Size(584, 361);
            this.dataGridViewDocuments.TabIndex = 0;
            this.dataGridViewDocuments.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewDocumentsCellMouseDoubleClick);
            // 
            // Id
            // 
            this.Id.Frozen = true;
            this.Id.HeaderText = "Номер";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Id.Width = 80;
            // 
            // Name
            // 
            this.Name.Frozen = true;
            this.Name.HeaderText = "Название";
            this.Name.Name = "Name";
            this.Name.ReadOnly = true;
            this.Name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Name.Width = 500;
            // 
            // DocumentListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.dataGridViewDocuments);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Документы";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDocuments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewDocuments;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name;
    }
}