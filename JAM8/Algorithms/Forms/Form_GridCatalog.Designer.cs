namespace JAM8.Algorithms.Forms
{
    partial class Form_GridCatalog
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
            scottplot4Grid1 = new JAM8.Algorithms.Geometry.Scottplot4Grid();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            btnSelectGrid = new Button();
            btnSelectGridProperty = new Button();
            listView1 = new ListView();
            btnSelectGrids = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // scottplot4Grid1
            // 
            scottplot4Grid1.Location = new Point(457, 12);
            scottplot4Grid1.Name = "scottplot4Grid1";
            scottplot4Grid1.Size = new Size(1230, 790);
            scottplot4Grid1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(21, 12);
            button1.Name = "button1";
            button1.Size = new Size(198, 46);
            button1.TabIndex = 1;
            button1.Text = "打开GridCatalog文件";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(238, 12);
            button2.Name = "button2";
            button2.Size = new Size(196, 46);
            button2.TabIndex = 2;
            button2.Text = "新建GridCatalog文件";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(35, 812);
            button3.Name = "button3";
            button3.Size = new Size(161, 45);
            button3.TabIndex = 5;
            button3.Text = "添加Grid";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(242, 812);
            button4.Name = "button4";
            button4.Size = new Size(161, 45);
            button4.TabIndex = 6;
            button4.Text = "删除Grid";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // btnSelectGrid
            // 
            btnSelectGrid.Location = new Point(732, 812);
            btnSelectGrid.Name = "btnSelectGrid";
            btnSelectGrid.Size = new Size(167, 45);
            btnSelectGrid.TabIndex = 7;
            btnSelectGrid.Text = "选择Grid";
            btnSelectGrid.UseVisualStyleBackColor = true;
            btnSelectGrid.Click += button5_Click;
            // 
            // btnSelectGridProperty
            // 
            btnSelectGridProperty.Location = new Point(1229, 812);
            btnSelectGridProperty.Name = "btnSelectGridProperty";
            btnSelectGridProperty.Size = new Size(167, 45);
            btnSelectGridProperty.TabIndex = 8;
            btnSelectGridProperty.Text = "选择GridProperty";
            btnSelectGridProperty.UseVisualStyleBackColor = true;
            btnSelectGridProperty.Click += button6_Click;
            // 
            // listView1
            // 
            listView1.Location = new Point(7, 71);
            listView1.Name = "listView1";
            listView1.Size = new Size(444, 731);
            listView1.TabIndex = 10;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            // 
            // btnSelectGrids
            // 
            btnSelectGrids.Location = new Point(506, 812);
            btnSelectGrids.Name = "btnSelectGrids";
            btnSelectGrids.Size = new Size(167, 45);
            btnSelectGrids.TabIndex = 11;
            btnSelectGrids.Text = "选择Grids";
            btnSelectGrids.UseVisualStyleBackColor = true;
            btnSelectGrids.Click += button7_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 866);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1699, 31);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.DoubleClickEnabled = true;
            toolStripStatusLabel1.IsLink = true;
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(214, 24);
            toolStripStatusLabel1.Text = "the file path of gslib file";
            toolStripStatusLabel1.DoubleClick += toolStripStatusLabel1_DoubleClick;
            // 
            // Form_GridCatalog
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1699, 897);
            Controls.Add(statusStrip1);
            Controls.Add(btnSelectGrids);
            Controls.Add(listView1);
            Controls.Add(btnSelectGridProperty);
            Controls.Add(btnSelectGrid);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(scottplot4Grid1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_GridCatalog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_GridCatalog";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Geometry.Scottplot4Grid scottplot4Grid1;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button btnSelectGrid;
        private Button btnSelectGridProperty;
        private ListView listView1;
        private Button btnSelectGrids;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}