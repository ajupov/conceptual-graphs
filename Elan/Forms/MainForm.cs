using System;
using System.Windows.Forms;
using Elan.Content;
using Elan.Enums;
using Elan.Models.Implementations.Others;
using Elan.Services;
using Microsoft.Data.ConnectionUI;

namespace Elan.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        #region Меню Файл
        //Файл
        private void OpenFileMenuItemClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                designer.OpenFile(openFileDialog.FileName);
            }
        }
        private void SaveFileMenuItemClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CurrentFileName))
            {
                designer.SaveFile(CurrentFileName);
            }
            else
            {
                SaveToFileAs();
            }
        }
        private void SaveFileAsMenuItemClick(object sender, EventArgs e)
        {
            SaveToFileAs();
        }
        //Бд
        private void OpenDbMenuItemClick(object sender, EventArgs e)
        {
            designer.OpenDb();
        }
        private void SaveDbMenuItemClick(object sender, EventArgs e)
        {
            designer.SaveDb();
        }
        //Выход
        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Меню Правка
        //Возврат/Отмена
        private void UndoMenuItemClick(object sender, EventArgs e)
        {
            Undo();
        }
        private void RedoMenuItemClick(object sender, EventArgs e)
        {
            Redo();
        }
        //Вырезать/Копировать/Вставить
        private void CutMenuItemClick(object sender, EventArgs e)
        {
            designer.Cut();
        }
        private void CopyMenuItemClick(object sender, EventArgs e)
        {
            designer.Copy();
        }
        private void PasteMenuItemClick(object sender, EventArgs e)
        {
            designer.Paste();
        }
        private void DeleteMenuItemClick(object sender, EventArgs e)
        {
            SetNoneAction();
            deleteMenuItem.Checked = true;
            if (IsDocumentChanged)
            {
                designer.Document.DeleteSelectedElements();
            }
            SetNoneAction();
        }
        //Выбрать    
        private void SelectAllMenuItemClick(object sender, EventArgs e)
        {
            designer.Document.SelectAllElements();
        }
        #endregion

        #region Меню Вид
        private void ZoomOutMenuItemClick(object sender, EventArgs e)
        {
            ZoomOut();
        }
        private void ZoomInMenuItemClick(object sender, EventArgs e)
        {
            ZoomIn();
        }
        #endregion

        #region Тулбар
        private void ToolBarButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            switch ((string)e.Button.Tag)
            {
                case "undo":
                    Undo();
                    break;
                case "redo":
                    Redo();
                    break;
                case "rectangle":
                    SetRectangleAction();
                    break;
                case "ellipse":
                    SetEllipseAction();
                    break;
                case "comment":
                    SetCommentAction();
                    break;
                case "link":
                    SetConnectAction();
                    break;
                case "size":
                    SetSizeAction();
                    break;
                case "back":
                    MoveDown();
                    break;
                case "front":
                    MoveUp();
                    break;
                case "zoomOut":
                    ZoomOut();
                    break;
                case "zoomIn":
                    ZoomIn();
                    break;
            }
        }
        #endregion

        #region Меню Cервис
        private void DbConnectionMenuItemClick(object sender, EventArgs e)
        {
            try
            {
                var dialog = new DataConnectionDialog();
                DataSource.AddStandardDataSources(dialog);
                if (DataConnectionDialog.Show(dialog) == DialogResult.OK)
                {
                    var settings = new DbSettings
                    {
                        ConnectionString = dialog.ConnectionString
                    };

                    SettingsManager.SaveDbSettings(settings);
                    DataBaseManager.InitDataBase();
                    MessageBox.Show(Strings.SuccessDbConnectionText, Strings.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), Strings.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region События компонентов
        private void MainFormLoad(object sender, EventArgs e)
        {
            UpdateUndoRedoState();
            designer.Document.PropertyChanged += DocumentPropertyChanged;
        }
        private void DocumentPropertyChanged(object sender, EventArgs e)
        {
            IsDocumentChanged = false;
            SetNoneAction();
            UpdateUndoRedoState();
            IsDocumentChanged = true;
        }
        private void DesignerMouseUp(object sender, MouseEventArgs e)
        {
            propertyGrid.SelectedObject = null;

            if (designer.Document.SelectedElements.Count == 1)
            {
                propertyGrid.SelectedObject = designer.Document.SelectedElements[0];
            }
            else if (designer.Document.SelectedElements.Count > 1)
            {
                propertyGrid.SelectedObjects = designer.Document.SelectedElements.GetArray();
            }
            else if (designer.Document.SelectedElements.Count == 0)
            {
                propertyGrid.SelectedObject = designer.Document;
            }
        }
        private void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            IsControlPressed = e.KeyCode == Keys.ControlKey;
        }
        private void MainFormKeyUp(object sender, KeyEventArgs e)
        {
            IsControlPressed = false;
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (IsControlPressed)
            {
                if (e.Delta > 0)
                {
                    ZoomIn();
                }
                else
                {
                    ZoomOut();
                }
            }
        }
        #endregion

        #region Установка действий
        private void SetNoneAction()
        {
            deleteMenuItem.Checked = false;
        }
        private void SetSizeAction()
        {
            SetNoneAction();
            if (IsDocumentChanged)
            {
                designer.Document.Action = DesignerAction.Select;
            }
        }
        private void SetRectangleAction()
        {
            SetNoneAction();
            if (IsDocumentChanged)
            {
                designer.Document.Action = DesignerAction.Add;
                designer.Document.ElementType = ElementType.RectangleNode;
            }
        }
        private void SetEllipseAction()
        {
            SetNoneAction();
            if (IsDocumentChanged)
            {
                designer.Document.Action = DesignerAction.Add;
                designer.Document.ElementType = ElementType.EllipseNode;
            }
        }
        private void SetCommentAction()
        {
            SetNoneAction();
            if (IsDocumentChanged)
            {
                designer.Document.Action = DesignerAction.Add;
                designer.Document.ElementType = ElementType.CommentBox;
            }
        }
        private void SetConnectAction()
        {
            SetNoneAction();
            if (IsDocumentChanged)
            {
                designer.Document.Action = DesignerAction.Connect;
                designer.Document.LinkType = LinkType.Straight;
            }
        }
        #endregion

        #region Отмена/Возврат
        private void Undo()
        {
            if (designer.CanUndo)
            {
                designer.Undo();
            }

            UpdateUndoRedoState();
        }
        private void Redo()
        {
            if (designer.CanRedo)
            {
                designer.Redo();
            }

            UpdateUndoRedoState();
        }
        private void UpdateUndoRedoState()
        {
            undoMenuItem.Enabled = designer.CanUndo;
            buttonUndo.Enabled = designer.CanUndo;
            redoMenuItem.Enabled = designer.CanRedo;
            buttonRedo.Enabled = designer.CanRedo;
        }
        #endregion

        #region Открыть/Сохранить
        private void SaveToFileAs()
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                CurrentFileName = saveFileDialog.FileName;
                designer.SaveFile(CurrentFileName);
            }
        }
        #endregion

        #region Порядок
        private void ToFront()
        {
            if (designer.Document.SelectedElements.Count == 1)
            {
                designer.Document.BringToFrontElement(designer.Document.SelectedElements[0]);
                designer.Refresh();
            }
        }
        private void ToBack()
        {
            if (designer.Document.SelectedElements.Count == 1)
            {
                designer.Document.SendToBackElement(designer.Document.SelectedElements[0]);
                designer.Refresh();
            }
        }
        private void MoveUp()
        {
            if (designer.Document.SelectedElements.Count == 1)
            {
                designer.Document.MoveUpElement(designer.Document.SelectedElements[0]);
                designer.Refresh();
            }
        }
        private void MoveDown()
        {
            if (designer.Document.SelectedElements.Count == 1)
            {
                designer.Document.MoveDownElement(designer.Document.SelectedElements[0]);
                designer.Refresh();
            }
        }
        #endregion

        #region Масштаб
        private void ZoomOut()
        {
            if (designer.Document.Zoom > 0.2)
            {
                designer.Document.Zoom -= 0.1f;
            }
        }
        private void ZoomIn()
        {
            if (designer.Document.Zoom < 10)
            {
                designer.Document.Zoom += 0.1f;
            }
        }
        #endregion
    }
}