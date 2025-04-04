namespace JAM8.Algorithms.Geometry
{
    partial class Scottplot4Grid
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            label6 = new Label();
            cb_gridProperty = new ComboBox();
            cb_xy_yz_zx = new ComboBox();
            trackBar1 = new TrackBar();
            numericUpDown1 = new NumericUpDown();
            checkBox1 = new CheckBox();
            cb_colorMap = new ComboBox();
            label4 = new Label();
            panel2 = new Panel();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            统计ToolStripMenuItem = new ToolStripMenuItem();
            缩放ToolStripMenuItem = new ToolStripMenuItem();
            保存ToolStripMenuItem = new ToolStripMenuItem();
            保存GridToolStripMenuItem = new ToolStripMenuItem();
            保存ToolStripMenuItem1 = new ToolStripMenuItem();
            保存View2dGridToolStripMenuItem = new ToolStripMenuItem();
            转换ToolStripMenuItem = new ToolStripMenuItem();
            二维图片ToolStripMenuItem = new ToolStripMenuItem();
            二维图片当前显示属性ToolStripMenuItem = new ToolStripMenuItem();
            eTypeToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripLabel3 = new ToolStripLabel();
            toolStripTextBox1 = new ToolStripTextBox();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripLabel2 = new ToolStripLabel();
            formsPlot1 = new ScottPlot.FormsPlot();
            toolTip1 = new ToolTip(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            panel2.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label6);
            panel1.Controls.Add(cb_gridProperty);
            panel1.Controls.Add(cb_xy_yz_zx);
            panel1.Controls.Add(trackBar1);
            panel1.Controls.Add(numericUpDown1);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(cb_colorMap);
            panel1.Controls.Add(label4);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(994, 39);
            panel1.TabIndex = 5;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(7, 7);
            label6.Name = "label6";
            label6.Size = new Size(46, 24);
            label6.TabIndex = 15;
            label6.Text = "属性";
            // 
            // cb_gridProperty
            // 
            cb_gridProperty.DrawMode = DrawMode.OwnerDrawFixed;
            cb_gridProperty.FormattingEnabled = true;
            cb_gridProperty.Location = new Point(65, 5);
            cb_gridProperty.Name = "cb_gridProperty";
            cb_gridProperty.Size = new Size(107, 31);
            cb_gridProperty.TabIndex = 0;
            cb_gridProperty.DrawItem += cb_gridProperty_DrawItem;
            cb_gridProperty.SelectedIndexChanged += cb_gridProperty_SelectedIndexChanged;
            cb_gridProperty.DropDownClosed += cb_gridProperty_DropDownClosed;
            // 
            // cb_xy_yz_zx
            // 
            cb_xy_yz_zx.FormattingEnabled = true;
            cb_xy_yz_zx.Location = new Point(530, 5);
            cb_xy_yz_zx.Name = "cb_xy_yz_zx";
            cb_xy_yz_zx.Size = new Size(92, 32);
            cb_xy_yz_zx.TabIndex = 2;
            cb_xy_yz_zx.SelectedIndexChanged += cb_xy_yz_zx_SelectedIndexChanged;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(709, 5);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(106, 69);
            trackBar1.TabIndex = 4;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(640, 6);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(64, 30);
            numericUpDown1.TabIndex = 3;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(407, 7);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(119, 28);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "AxisEqual";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // cb_colorMap
            // 
            cb_colorMap.DrawMode = DrawMode.OwnerDrawFixed;
            cb_colorMap.FormattingEnabled = true;
            cb_colorMap.Location = new Point(273, 5);
            cb_colorMap.Name = "cb_colorMap";
            cb_colorMap.Size = new Size(107, 31);
            cb_colorMap.TabIndex = 1;
            cb_colorMap.DrawItem += cb_colorMap_DrawItem;
            cb_colorMap.SelectedIndexChanged += cb_ColorMap_SelectedIndexChanged;
            cb_colorMap.DropDownClosed += cb_colorMap_DropDownClosed;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(205, 9);
            label4.Name = "label4";
            label4.Size = new Size(46, 24);
            label4.TabIndex = 3;
            label4.Text = "配色";
            // 
            // panel2
            // 
            panel2.Controls.Add(toolStrip1);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 381);
            panel2.Name = "panel2";
            panel2.Size = new Size(994, 39);
            panel2.TabIndex = 6;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripSeparator2, toolStripLabel3, toolStripTextBox1, toolStripSeparator1, toolStripLabel1, toolStripSeparator3, toolStripLabel2 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(994, 33);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { 统计ToolStripMenuItem, 缩放ToolStripMenuItem, 保存ToolStripMenuItem, 转换ToolStripMenuItem, eTypeToolStripMenuItem });
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(64, 28);
            toolStripDropDownButton1.Text = "操作";
            // 
            // 统计ToolStripMenuItem
            // 
            统计ToolStripMenuItem.Name = "统计ToolStripMenuItem";
            统计ToolStripMenuItem.Size = new Size(170, 34);
            统计ToolStripMenuItem.Text = "统计";
            统计ToolStripMenuItem.Click += 统计ToolStripMenuItem_Click;
            // 
            // 缩放ToolStripMenuItem
            // 
            缩放ToolStripMenuItem.Name = "缩放ToolStripMenuItem";
            缩放ToolStripMenuItem.Size = new Size(170, 34);
            缩放ToolStripMenuItem.Text = "缩放";
            缩放ToolStripMenuItem.Click += 缩放ToolStripMenuItem_Click;
            // 
            // 保存ToolStripMenuItem
            // 
            保存ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 保存GridToolStripMenuItem, 保存ToolStripMenuItem1, 保存View2dGridToolStripMenuItem });
            保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            保存ToolStripMenuItem.Size = new Size(170, 34);
            保存ToolStripMenuItem.Text = "保存";
            保存ToolStripMenuItem.Click += 保存ToolStripMenuItem_Click;
            // 
            // 保存GridToolStripMenuItem
            // 
            保存GridToolStripMenuItem.Name = "保存GridToolStripMenuItem";
            保存GridToolStripMenuItem.Size = new Size(268, 34);
            保存GridToolStripMenuItem.Text = "Grid";
            保存GridToolStripMenuItem.Click += 保存GridToolStripMenuItem_Click;
            // 
            // 保存ToolStripMenuItem1
            // 
            保存ToolStripMenuItem1.Name = "保存ToolStripMenuItem1";
            保存ToolStripMenuItem1.Size = new Size(268, 34);
            保存ToolStripMenuItem1.Text = "GridProperty";
            保存ToolStripMenuItem1.Click += 保存ToolStripMenuItem1_Click;
            // 
            // 保存View2dGridToolStripMenuItem
            // 
            保存View2dGridToolStripMenuItem.Name = "保存View2dGridToolStripMenuItem";
            保存View2dGridToolStripMenuItem.Size = new Size(268, 34);
            保存View2dGridToolStripMenuItem.Text = "GridProperty View";
            保存View2dGridToolStripMenuItem.Click += 保存View2dGridToolStripMenuItem_Click;
            // 
            // 转换ToolStripMenuItem
            // 
            转换ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 二维图片ToolStripMenuItem, 二维图片当前显示属性ToolStripMenuItem });
            转换ToolStripMenuItem.Name = "转换ToolStripMenuItem";
            转换ToolStripMenuItem.Size = new Size(170, 34);
            转换ToolStripMenuItem.Text = "转换";
            // 
            // 二维图片ToolStripMenuItem
            // 
            二维图片ToolStripMenuItem.Name = "二维图片ToolStripMenuItem";
            二维图片ToolStripMenuItem.Size = new Size(316, 34);
            二维图片ToolStripMenuItem.Text = "图片(2d GridProperty)";
            二维图片ToolStripMenuItem.Click += 二维图片ToolStripMenuItem_Click;
            // 
            // 二维图片当前显示属性ToolStripMenuItem
            // 
            二维图片当前显示属性ToolStripMenuItem.Name = "二维图片当前显示属性ToolStripMenuItem";
            二维图片当前显示属性ToolStripMenuItem.Size = new Size(316, 34);
            二维图片当前显示属性ToolStripMenuItem.Text = "图片(GridProperty View)";
            二维图片当前显示属性ToolStripMenuItem.Click += 二维图片当前显示属性ToolStripMenuItem_Click;
            // 
            // eTypeToolStripMenuItem
            // 
            eTypeToolStripMenuItem.Name = "eTypeToolStripMenuItem";
            eTypeToolStripMenuItem.Size = new Size(170, 34);
            eTypeToolStripMenuItem.Text = "E-Type";
            eTypeToolStripMenuItem.Click += eTypeToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 33);
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(46, 28);
            toolStripLabel3.Text = "过滤";
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.Name = "toolStripTextBox1";
            toolStripTextBox1.Size = new Size(100, 33);
            toolStripTextBox1.KeyUp += toolStripTextBox1_KeyUp;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 33);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(70, 28);
            toolStripLabel1.Text = "gp信息";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 33);
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(92, 28);
            toolStripLabel2.Text = "point信息";
            // 
            // formsPlot1
            // 
            formsPlot1.Dock = DockStyle.Fill;
            formsPlot1.Location = new Point(0, 39);
            formsPlot1.Margin = new Padding(6, 5, 6, 5);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(994, 342);
            formsPlot1.TabIndex = 8;
            formsPlot1.MouseDown += formsPlot1_MouseDown;
            formsPlot1.MouseMove += formsPlot1_MouseMove;
            // 
            // Scottplot4Grid
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(formsPlot1);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "Scottplot4Grid";
            Size = new Size(994, 420);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox cb_xy_yz_zx;
        private TrackBar trackBar1;
        private NumericUpDown numericUpDown1;
        private CheckBox checkBox1;
        private ComboBox cb_colorMap;
        private Label label4;
        private Panel panel2;
        private Label label6;
        private ComboBox cb_gridProperty;
        private ScottPlot.FormsPlot formsPlot1;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem 统计ToolStripMenuItem;
        private ToolStripMenuItem 保存ToolStripMenuItem;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripMenuItem 转换ToolStripMenuItem;
        private ToolStripMenuItem 二维图片ToolStripMenuItem;
        private ToolStripMenuItem eTypeToolStripMenuItem;
        private ToolStripMenuItem 缩放ToolStripMenuItem;
        private ToolTip toolTip1;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel toolStripLabel3;
        private ToolStripMenuItem 保存GridToolStripMenuItem;
        private ToolStripMenuItem 保存View2dGridToolStripMenuItem;
        private ToolStripMenuItem 保存ToolStripMenuItem1;
        private ToolStripMenuItem 二维图片当前显示属性ToolStripMenuItem;
    }
}
