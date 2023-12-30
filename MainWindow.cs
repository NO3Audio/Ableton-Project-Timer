using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Ableton_Project_Timer
{
    public partial class MainWindow : Form
    {
#if (DEBUG)
        string iconLocation = "../../assets/icon.ico";
#else
        string iconLocation = "./icon.ico";
#endif
        public MainWindow()
        {
            InitializeComponent();
            InitializeParams();
            CreateTray();
        }

        private void InitializeParams()
        {
            this.Resize += this.MainWindow_Resize;
            this.timesGridView.SelectionChanged += this.timesGridView_Selected;

            this.timer = new Timer(this);

            this.Icon = new Icon(iconLocation); ;

            this.Text = "Ableton Project Timer";
        }
            

        private void CreateTray()
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.FormClosing += MainWindow_FormClosing;

            contextMenu =   new ContextMenu();
            menuItemExit =  new MenuItem();
            menuItemOpen =  new MenuItem();

            menuItemOpen.Index = 1;
            menuItemOpen.Text = "Open";
            menuItemOpen.Click += new System.EventHandler(this.menuItemOpen_Click);

            menuItemExit.Index = 0;
            menuItemExit.Text = "Exit";
            menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);

            contextMenu.MenuItems.Add(menuItemOpen);
            contextMenu.MenuItems.Add(menuItemExit);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Ableton Project Timer";
            trayIcon.Visible = true;
            trayIcon.Icon = this.Icon;
            trayIcon.ContextMenu = contextMenu;
            
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            if (!this.fullExit) {
                e.Cancel = true;
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            this.timesGridView.SetBounds(0, 0, this.Bounds.Width, this.Bounds.Height);
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            this.fullExit = true;
            trayIcon.Dispose();
            this.timer.EndThread();
            Application.Exit();
        }

        private void timesGridView_Selected(object sender, EventArgs e)
        {
            this.timesGridView.ClearSelection();
        }

        public void PopulateTableData(DataTable data)
        {
            if (firstUiLoad)
                {
                    if (timesGridView.InvokeRequired)
                    {
                        timesGridView.Invoke(new Action(() => timesGridView.DataSource = data));
                    }
                    else
                    {
                        timesGridView.DataSource = data;
                    }

                DataGridViewColumn projectColumn = this.timesGridView.Columns["Project"];
                DataGridViewColumn timeColumn = this.timesGridView.Columns["Time"];

                projectColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                projectColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                projectColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

                timeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                timeColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                timeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;

                firstUiLoad = false;
                }
                else
                {
                    if (timesGridView.InvokeRequired)
                    {
                        timesGridView.Invoke(new Action(() => timesGridView.DataSource = data));
                    }

                }
        }

        private bool firstUiLoad = true;
        private ContextMenu contextMenu;
        private NotifyIcon trayIcon;
        private MenuItem menuItemExit;
        private MenuItem menuItemOpen;
        private Timer timer;
        private bool fullExit;
        
    }
}
