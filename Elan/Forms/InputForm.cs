using System;
using System.Windows.Forms;

namespace Elan.Forms
{
    public partial class InputForm : Form
    {
        public string DocumentName { get; set; }

        public InputForm()
        {
            InitializeComponent();
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            DocumentName = textBoxName.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
