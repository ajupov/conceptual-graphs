using System.ComponentModel;
using System.Windows.Forms;
using Elan.UserControls;

namespace Elan.Forms
{
    partial class MainForm
    {
        #region Свойства
        private bool IsDocumentChanged = true;
        private bool IsControlPressed = false;
        private string CurrentFileName;
        private IContainer components;
        private Panel panel;
        private Designer designer;
        private PropertyGrid propertyGrid;
        private ToolBar toolBar;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private ImageList imageList;
        private ToolBarButton buttonAddRectangle;
        private ToolBarButton buttonAddEllipse;
        private ToolBarButton buttonAddComment;
        private ToolBarButton buttonAddLink;
        private ToolBarButton buttonToFront;
        private ToolBarButton buttonToBack;
        private ToolBarButton buttonSize;
        private ToolBarButton buttonZoomOut;
        private ToolBarButton buttonZoomIn;
        private ToolBarButton buttonUndo;
        private ToolBarButton buttonRedo;
        private MainMenu mainMenu;
        private MenuItem openFileMenuItem;
        private MenuItem saveFileMenuItem;
        private MenuItem saveFileAsMenuItem;
        private MenuItem dividerMenuItem1;
        private MenuItem openDbMenuItem;
        private MenuItem saveDbMenuItem;
        private MenuItem dividerMenuItem2;
        private MenuItem exitMenuItem;
        private MenuItem fileMenuItem;
        private MenuItem undoMenuItem;
        private MenuItem redoMenuItem;
        private MenuItem dividerMenuItem3;
        private MenuItem cutMenuItem;
        private MenuItem сopyMenuItem;
        private MenuItem pasteMenuItem;
        private MenuItem deleteMenuItem;
        private MenuItem dividerMenuItem4;
        private MenuItem selectAllMenuItem;
        private MenuItem editMenuItem;
        private MenuItem zoomOutMenuItem;
        private MenuItem zoomInMenuItem;
        private MenuItem viewMenuItem;
        private MenuItem dbConnectionMenuItem;
        private MenuItem propertiesMenuItem;
        #endregion

        #region  Инициализация компонентов
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.openFileMenuItem = new System.Windows.Forms.MenuItem();
            this.saveFileMenuItem = new System.Windows.Forms.MenuItem();
            this.saveFileAsMenuItem = new System.Windows.Forms.MenuItem();
            this.dividerMenuItem1 = new System.Windows.Forms.MenuItem();
            this.openDbMenuItem = new System.Windows.Forms.MenuItem();
            this.saveDbMenuItem = new System.Windows.Forms.MenuItem();
            this.dividerMenuItem2 = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.editMenuItem = new System.Windows.Forms.MenuItem();
            this.undoMenuItem = new System.Windows.Forms.MenuItem();
            this.redoMenuItem = new System.Windows.Forms.MenuItem();
            this.dividerMenuItem3 = new System.Windows.Forms.MenuItem();
            this.cutMenuItem = new System.Windows.Forms.MenuItem();
            this.сopyMenuItem = new System.Windows.Forms.MenuItem();
            this.pasteMenuItem = new System.Windows.Forms.MenuItem();
            this.deleteMenuItem = new System.Windows.Forms.MenuItem();
            this.dividerMenuItem4 = new System.Windows.Forms.MenuItem();
            this.selectAllMenuItem = new System.Windows.Forms.MenuItem();
            this.viewMenuItem = new System.Windows.Forms.MenuItem();
            this.zoomOutMenuItem = new System.Windows.Forms.MenuItem();
            this.zoomInMenuItem = new System.Windows.Forms.MenuItem();
            this.propertiesMenuItem = new System.Windows.Forms.MenuItem();
            this.dbConnectionMenuItem = new System.Windows.Forms.MenuItem();
            this.toolBar = new System.Windows.Forms.ToolBar();
            this.buttonUndo = new System.Windows.Forms.ToolBarButton();
            this.buttonRedo = new System.Windows.Forms.ToolBarButton();
            this.buttonAddRectangle = new System.Windows.Forms.ToolBarButton();
            this.buttonAddEllipse = new System.Windows.Forms.ToolBarButton();
            this.buttonAddComment = new System.Windows.Forms.ToolBarButton();
            this.buttonAddLink = new System.Windows.Forms.ToolBarButton();
            this.buttonSize = new System.Windows.Forms.ToolBarButton();
            this.buttonToBack = new System.Windows.Forms.ToolBarButton();
            this.buttonToFront = new System.Windows.Forms.ToolBarButton();
            this.buttonZoomOut = new System.Windows.Forms.ToolBarButton();
            this.buttonZoomIn = new System.Windows.Forms.ToolBarButton();
            this.panel = new System.Windows.Forms.Panel();
            this.designer = new Elan.UserControls.Designer();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Silver;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            this.imageList.Images.SetKeyName(6, "");
            this.imageList.Images.SetKeyName(7, "");
            this.imageList.Images.SetKeyName(8, "");
            this.imageList.Images.SetKeyName(9, "");
            this.imageList.Images.SetKeyName(10, "");
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.editMenuItem,
            this.viewMenuItem,
            this.propertiesMenuItem});
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openFileMenuItem,
            this.saveFileMenuItem,
            this.saveFileAsMenuItem,
            this.dividerMenuItem1,
            this.openDbMenuItem,
            this.saveDbMenuItem,
            this.dividerMenuItem2,
            this.exitMenuItem});
            this.fileMenuItem.Text = "Файл";
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Index = 0;
            this.openFileMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.openFileMenuItem.Text = "Открыть файл";
            this.openFileMenuItem.Click += new System.EventHandler(this.OpenFileMenuItemClick);
            // 
            // saveFileMenuItem
            // 
            this.saveFileMenuItem.Index = 1;
            this.saveFileMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.saveFileMenuItem.Text = "Сохранить в файл";
            this.saveFileMenuItem.Click += new System.EventHandler(this.SaveFileMenuItemClick);
            // 
            // saveFileAsMenuItem
            // 
            this.saveFileAsMenuItem.Index = 2;
            this.saveFileAsMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
            this.saveFileAsMenuItem.Text = "Сохранить в файл как...";
            this.saveFileAsMenuItem.Click += new System.EventHandler(this.SaveFileAsMenuItemClick);
            // 
            // dividerMenuItem1
            // 
            this.dividerMenuItem1.Index = 3;
            this.dividerMenuItem1.Text = "-";
            // 
            // openDbMenuItem
            // 
            this.openDbMenuItem.Index = 4;
            this.openDbMenuItem.Text = "Открыть файл из БД";
            this.openDbMenuItem.Click += new System.EventHandler(this.OpenDbMenuItemClick);
            // 
            // saveDbMenuItem
            // 
            this.saveDbMenuItem.Index = 5;
            this.saveDbMenuItem.Text = "Сохранить в файл в БД";
            this.saveDbMenuItem.Click += new System.EventHandler(this.SaveDbMenuItemClick);
            // 
            // dividerMenuItem2
            // 
            this.dividerMenuItem2.Index = 6;
            this.dividerMenuItem2.Text = "-";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 7;
            this.exitMenuItem.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.exitMenuItem.Text = "Выход";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItemClick);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Index = 1;
            this.editMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.undoMenuItem,
            this.redoMenuItem,
            this.dividerMenuItem3,
            this.cutMenuItem,
            this.сopyMenuItem,
            this.pasteMenuItem,
            this.deleteMenuItem,
            this.dividerMenuItem4,
            this.selectAllMenuItem});
            this.editMenuItem.Text = "Правка";
            // 
            // undoMenuItem
            // 
            this.undoMenuItem.Index = 0;
            this.undoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
            this.undoMenuItem.Text = "Отменить";
            this.undoMenuItem.Click += new System.EventHandler(this.UndoMenuItemClick);
            // 
            // redoMenuItem
            // 
            this.redoMenuItem.Index = 1;
            this.redoMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
            this.redoMenuItem.Text = "Вернуть";
            this.redoMenuItem.Click += new System.EventHandler(this.RedoMenuItemClick);
            // 
            // dividerMenuItem3
            // 
            this.dividerMenuItem3.Index = 2;
            this.dividerMenuItem3.Text = "-";
            // 
            // cutMenuItem
            // 
            this.cutMenuItem.Index = 3;
            this.cutMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
            this.cutMenuItem.Text = "Вырезать";
            this.cutMenuItem.Click += new System.EventHandler(this.CutMenuItemClick);
            // 
            // сopyMenuItem
            // 
            this.сopyMenuItem.Index = 4;
            this.сopyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.сopyMenuItem.Text = "Копировать";
            this.сopyMenuItem.Click += new System.EventHandler(this.CopyMenuItemClick);
            // 
            // pasteMenuItem
            // 
            this.pasteMenuItem.Index = 5;
            this.pasteMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.pasteMenuItem.Text = "Вставить";
            this.pasteMenuItem.Click += new System.EventHandler(this.PasteMenuItemClick);
            // 
            // deleteMenuItem
            // 
            this.deleteMenuItem.Index = 6;
            this.deleteMenuItem.Shortcut = System.Windows.Forms.Shortcut.Del;
            this.deleteMenuItem.Text = "Удалить";
            this.deleteMenuItem.Click += new System.EventHandler(this.DeleteMenuItemClick);
            // 
            // dividerMenuItem4
            // 
            this.dividerMenuItem4.Index = 7;
            this.dividerMenuItem4.Text = "-";
            // 
            // selectAllMenuItem
            // 
            this.selectAllMenuItem.Index = 8;
            this.selectAllMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.selectAllMenuItem.Text = "Выбрать все";
            this.selectAllMenuItem.Click += new System.EventHandler(this.SelectAllMenuItemClick);
            // 
            // viewMenuItem
            // 
            this.viewMenuItem.Index = 2;
            this.viewMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.zoomOutMenuItem,
            this.zoomInMenuItem});
            this.viewMenuItem.Text = "Вид";
            // 
            // zoomOutMenuItem
            // 
            this.zoomOutMenuItem.Index = 0;
            this.zoomOutMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlF1;
            this.zoomOutMenuItem.Text = "Уменьшить";
            this.zoomOutMenuItem.Click += new System.EventHandler(this.ZoomOutMenuItemClick);
            // 
            // zoomInMenuItem
            // 
            this.zoomInMenuItem.Index = 1;
            this.zoomInMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlF2;
            this.zoomInMenuItem.Text = "Увеличить";
            this.zoomInMenuItem.Click += new System.EventHandler(this.ZoomInMenuItemClick);
            // 
            // propertiesMenuItem
            // 
            this.propertiesMenuItem.Index = 3;
            this.propertiesMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.dbConnectionMenuItem});
            this.propertiesMenuItem.Text = "Сервис";
            // 
            // dbConnectionMenuItem
            // 
            this.dbConnectionMenuItem.Index = 0;
            this.dbConnectionMenuItem.Text = "Задать подключение к БД";
            this.dbConnectionMenuItem.Click += new System.EventHandler(this.DbConnectionMenuItemClick);
            // 
            // toolBar
            // 
            this.toolBar.AllowDrop = true;
            this.toolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.toolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.buttonUndo,
            this.buttonRedo,
            this.buttonAddRectangle,
            this.buttonAddEllipse,
            this.buttonAddComment,
            this.buttonAddLink,
            this.buttonSize,
            this.buttonToBack,
            this.buttonToFront,
            this.buttonZoomOut,
            this.buttonZoomIn});
            this.toolBar.ButtonSize = new System.Drawing.Size(16, 16);
            this.toolBar.Divider = false;
            this.toolBar.DropDownArrows = true;
            this.toolBar.ImageList = this.imageList;
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.ShowToolTips = true;
            this.toolBar.Size = new System.Drawing.Size(784, 26);
            this.toolBar.TabIndex = 1;
            this.toolBar.Wrappable = false;
            this.toolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.ToolBarButtonClick);
            // 
            // buttonUndo
            // 
            this.buttonUndo.ImageIndex = 0;
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Tag = "undo";
            this.buttonUndo.ToolTipText = "Undo";
            // 
            // buttonRedo
            // 
            this.buttonRedo.ImageIndex = 1;
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Tag = "redo";
            this.buttonRedo.ToolTipText = "Redo";
            // 
            // buttonAddRectangle
            // 
            this.buttonAddRectangle.ImageIndex = 2;
            this.buttonAddRectangle.Name = "buttonAddRectangle";
            this.buttonAddRectangle.Tag = "rectangle";
            // 
            // buttonAddEllipse
            // 
            this.buttonAddEllipse.ImageIndex = 3;
            this.buttonAddEllipse.Name = "buttonAddEllipse";
            this.buttonAddEllipse.Tag = "ellipse";
            // 
            // buttonAddComment
            // 
            this.buttonAddComment.ImageIndex = 4;
            this.buttonAddComment.Name = "buttonAddComment";
            this.buttonAddComment.Tag = "comment";
            // 
            // buttonAddLink
            // 
            this.buttonAddLink.ImageIndex = 5;
            this.buttonAddLink.Name = "buttonAddLink";
            this.buttonAddLink.Tag = "link";
            // 
            // buttonSize
            // 
            this.buttonSize.ImageIndex = 10;
            this.buttonSize.Name = "buttonSize";
            this.buttonSize.Tag = "size";
            // 
            // buttonToBack
            // 
            this.buttonToBack.ImageIndex = 7;
            this.buttonToBack.Name = "buttonToBack";
            this.buttonToBack.Tag = "back";
            // 
            // buttonToFront
            // 
            this.buttonToFront.ImageIndex = 6;
            this.buttonToFront.Name = "buttonToFront";
            this.buttonToFront.Tag = "front";
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.ImageIndex = 9;
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Tag = "zoomOut";
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.ImageIndex = 8;
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Tag = "zoomIn";
            // 
            // panel
            // 
            this.panel.Controls.Add(this.designer);
            this.panel.Controls.Add(this.propertyGrid);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 26);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(784, 230);
            this.panel.TabIndex = 2;
            // 
            // designer
            // 
            this.designer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.designer.AutoScroll = true;
            this.designer.AutoScrollMinSize = new System.Drawing.Size(100, 100);
            this.designer.BackColor = System.Drawing.SystemColors.Window;
            this.designer.Location = new System.Drawing.Point(220, 0);
            this.designer.Name = "designer";
            this.designer.Size = new System.Drawing.Size(564, 230);
            this.designer.TabIndex = 6;
            this.designer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DesignerMouseUp);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(220, 230);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Граф Elan (*.json)|*.json|Все файлы (*.*)|*.*";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Граф Elan (*.json)|*.json|Все файлы (*.*)|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(784, 256);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.toolBar);
            this.KeyPreview = true;
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "Редактор концептуальных графов";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainFormKeyUp);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}