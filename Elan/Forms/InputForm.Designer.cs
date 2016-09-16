using System.Windows.Forms;

namespace Elan.Forms
{
    partial class InputForm
    {
        #region Свойства
        private Label labelMessage;
        private TextBox textBoxName;
        private Button buttonOk;
        #endregion

        #region Инициализация компонентов
        private void InitializeComponent()
        {
            labelMessage = new Label();
            textBoxName = new TextBox();
            buttonOk = new Button();
            SuspendLayout();
            // 
            // labelMessage
            // 
            labelMessage.AutoSize = true;
            labelMessage.Location = new System.Drawing.Point(13, 13);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new System.Drawing.Size(57, 13);
            labelMessage.TabIndex = 0;
            labelMessage.Text = "Название";
            // 
            // textBoxName
            // 
            textBoxName.Location = new System.Drawing.Point(76, 10);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new System.Drawing.Size(196, 20);
            textBoxName.TabIndex = 1;
            // 
            // buttonOk
            // 
            buttonOk.Location = new System.Drawing.Point(103, 36);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new System.Drawing.Size(75, 23);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "Сохранить";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += new System.EventHandler(ButtonOkClick);
            // 
            // InputForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(284, 66);
            Controls.Add(buttonOk);
            Controls.Add(textBoxName);
            Controls.Add(labelMessage);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Сохранение в БД";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}