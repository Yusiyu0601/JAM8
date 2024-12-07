namespace JAM8.Utilities
{
    partial class Form_DataTable
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
            menuStrip1 = new MenuStrip();
            文件ToolStripMenuItem = new ToolStripMenuItem();
            保存ToolStripMenuItem = new ToolStripMenuItem();
            advancedDataGridView1 = new Zuby.ADGV.AdvancedDataGridView();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)advancedDataGridView1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { 文件ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1313, 32);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            文件ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 保存ToolStripMenuItem });
            文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            文件ToolStripMenuItem.Size = new Size(62, 28);
            文件ToolStripMenuItem.Text = "文件";
            // 
            // 保存ToolStripMenuItem
            // 
            保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            保存ToolStripMenuItem.Size = new Size(270, 34);
            保存ToolStripMenuItem.Text = "保存";
            保存ToolStripMenuItem.Click += 保存ToolStripMenuItem_Click;
            // 
            // advancedDataGridView1
            // 
            advancedDataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            advancedDataGridView1.Dock = DockStyle.Fill;
            advancedDataGridView1.FilterAndSortEnabled = true;
            advancedDataGridView1.FilterStringChangedInvokeBeforeDatasourceUpdate = true;
            advancedDataGridView1.Location = new Point(0, 32);
            advancedDataGridView1.MaxFilterButtonImageHeight = 23;
            advancedDataGridView1.Name = "advancedDataGridView1";
            advancedDataGridView1.RightToLeft = RightToLeft.No;
            advancedDataGridView1.RowHeadersWidth = 62;
            advancedDataGridView1.Size = new Size(1313, 685);
            advancedDataGridView1.SortStringChangedInvokeBeforeDatasourceUpdate = true;
            advancedDataGridView1.TabIndex = 2;
            advancedDataGridView1.RowPostPaint += advancedDataGridView1_RowPostPaint;
            // 
            // Form_DataTable
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1313, 717);
            Controls.Add(advancedDataGridView1);
            Controls.Add(menuStrip1);
            Name = "Form_DataTable";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_DataTable";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)advancedDataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem 文件ToolStripMenuItem;
        private ToolStripMenuItem 保存ToolStripMenuItem;
        private Zuby.ADGV.AdvancedDataGridView advancedDataGridView1;
    }
}