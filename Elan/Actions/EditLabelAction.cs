using System.Drawing;
using System.Windows.Forms;
using Elan.Enums;
using Elan.Helpers;
using Elan.Models.Base;
using Elan.Models.Contracts;
using Elan.Models.Implementations.Elements;

namespace Elan.Actions
{
    public class EditLabelAction
    {
        #region Ñגמיסעגא
        private Point _center;
        private LabelEditDirection _direction;
        private LabelElement _labelElement;
        private TextBox _labelTextBox;
        private BaseElement _siteLabelElement;
        #endregion

        #region Ìועמהû
        public void StartEdit(BaseElement element, TextBox textBox)
        {
            if (!(element is ILabelElement) || ((ILabelElement)element).Label.ReadOnly)
            {
                return;
            }

            _siteLabelElement = element;
            _labelElement = ((ILabelElement) _siteLabelElement).Label;
            _labelTextBox = textBox;

            _direction = _siteLabelElement is BaseLinkElement ? LabelEditDirection.Both : LabelEditDirection.UpDown;

            SetTextBoxLocation(_siteLabelElement, _labelTextBox);

            _labelTextBox.AutoSize = true;
            _labelTextBox.Show();
            _labelTextBox.Text = _labelElement.Text;
            _labelTextBox.Font = _labelElement.Font;
            _labelTextBox.WordWrap = _labelElement.Wrap;

            _labelElement.Invalidate();

            switch (_labelElement.Alignment)
            {
                case StringAlignment.Near:
                    _labelTextBox.TextAlign = HorizontalAlignment.Left;
                    break;
                case StringAlignment.Center:
                    _labelTextBox.TextAlign = HorizontalAlignment.Center;
                    break;
                case StringAlignment.Far:
                    _labelTextBox.TextAlign = HorizontalAlignment.Right;
                    break;
            }

            _labelTextBox.KeyPress += LabelTextBoxKeyPress;
            _labelTextBox.Focus();
            _center.X = textBox.Location.X + textBox.Size.Width/2;
            _center.Y = textBox.Location.Y + textBox.Size.Height/2;
        }
        public void EndEdit()
        {
            if (_siteLabelElement == null)
            {
                return;
            }

            _labelTextBox.KeyPress -= LabelTextBoxKeyPress;

            var lblCtrl = ControllerHelper.GetLabelController(_siteLabelElement);
            _labelElement.Size = MeasureTextSize();
            _labelElement.Text = _labelTextBox.Text;
            _labelTextBox.Hide();
            if (lblCtrl != null)
            {
                lblCtrl.SetLabelPosition();
            }
            else
            {
                _labelElement.PositionBySite(_siteLabelElement);
            }
            _labelElement.Invalidate();
            _siteLabelElement = null;
            _labelElement = null;
            _labelTextBox = null;
        }

        public static void SetTextBoxLocation(BaseElement element, TextBox textBox)
        {
            if (!(element is ILabelElement))
            {
                return;
            }

            var lab = ((ILabelElement) element).Label;

            element.Invalidate();
            lab.Invalidate();

            if (lab.Text.Length > 0)
            {
                textBox.Location = lab.Location;
                textBox.Size = lab.Size;
            }
            else
            {
                var sizeTmp = DiagramHelper.MeasureString("XXXXXXX", lab.Font, lab.Size.Width, lab.Format);

                if (element is BaseLinkElement)
                {
                    textBox.Size = sizeTmp;
                    textBox.Location = new Point(
                        element.Location.X + element.Size.Width/2 - sizeTmp.Width/2,
                        element.Location.Y + element.Size.Height/2 - sizeTmp.Height/2);
                }
                else
                {
                    sizeTmp.Width = element.Size.Width;
                    textBox.Size = sizeTmp;
                    textBox.Location = new Point(element.Location.X,
                        element.Location.Y + element.Size.Height/2 - sizeTmp.Height/2);
                }
            }

            var rectangle = new Rectangle(textBox.Location, textBox.Size);
            rectangle.Inflate(3, 3);
            textBox.Location = rectangle.Location;
            textBox.Size = rectangle.Size;
        }

        private Size MeasureTextSize()
        {
            var sizeTmp = Size.Empty;

            switch (_direction)
            {
                case LabelEditDirection.UpDown:
                    sizeTmp = DiagramHelper.MeasureString(_labelTextBox.Text, _labelElement.Font, _labelTextBox.Size.Width,
                        _labelElement.Format);
                    break;
                case LabelEditDirection.Both:
                    sizeTmp = DiagramHelper.MeasureString(_labelTextBox.Text, _labelElement.Font);
                    break;
            }

            sizeTmp.Height += 30;

            return sizeTmp;
        }

        private void LabelTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (_labelTextBox.Text.Length == 0)
            {
                return;
            }

            var size = _labelTextBox.Size;
            var sizeTmp = MeasureTextSize();

            switch (_direction)
            {
                case LabelEditDirection.UpDown:
                    size.Height = sizeTmp.Height;
                    break;
                case LabelEditDirection.Both:
                    size = sizeTmp;
                    break;
            }

            _labelTextBox.Size = size;

            _labelTextBox.Location = new Point(_center.X - size.Width/2, _center.Y - size.Height/2);
        }
        #endregion
    }
}