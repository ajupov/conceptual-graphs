using System.ComponentModel;
using System.Windows.Forms;

namespace Elan.Forms
{
    partial class TableViewForm
    {
        #region Свойства
        #endregion

        #region Инициализация компонентов
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TableViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Name = "TableViewForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Табличное представление";
            this.ResumeLayout(false);

        }
        #endregion
    }
}