using System;
using System.Windows.Forms;

namespace Ableton_Project_Timer
{
    partial class MainWindow
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
            this.ClientSize = new System.Drawing.Size(509, 496);
            this.timesGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.timesGridView)).BeginInit();
            this.SuspendLayout();
            this.VScroll = true;
            
            // 
            // timesGridView
            // 
            this.timesGridView.Location = new System.Drawing.Point(0, 0);
            this.timesGridView.Name = "timesGridView";
            this.timesGridView.ScrollBars = ScrollBars.Vertical;
            this.timesGridView.GridColor = System.Drawing.Color.FromArgb(255, 237, 237, 237);
            this.timesGridView.Font = new System.Drawing.Font("Gadugi", 11);
            this.timesGridView.SetBounds(this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
            this.timesGridView.ReadOnly = true;
            this.timesGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.timesGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.AliceBlue;
            this.timesGridView.RowHeadersVisible = false;
            this.timesGridView.AllowUserToResizeRows = false;
            this.timesGridView.AllowUserToResizeColumns = false;
            this.timesGridView.AllowUserToOrderColumns = false;

            // 
            // MainWindow
            // 
            this.Controls.Add(this.timesGridView);
            this.Name = "Ableton Project Timer";
            ((System.ComponentModel.ISupportInitialize)(this.timesGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView timesGridView;
    }
}

