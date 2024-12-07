namespace JAM8.Algorithms.Geometry
{
    partial class Scottplot4GridProperty
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
            panel1 = new Panel();
            comboBox2 = new ComboBox();
            trackBar1 = new TrackBar();
            numericUpDown1 = new NumericUpDown();
            checkBox1 = new CheckBox();
            comboBox1 = new ComboBox();
            label4 = new Label();
            button1 = new Button();
            panel2 = new Panel();
            label5 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            formsPlot1 = new ScottPlot.FormsPlot();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(comboBox2);
            panel1.Controls.Add(trackBar1);
            panel1.Controls.Add(numericUpDown1);
            panel1.Controls.Add(checkBox1);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(label4);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(783, 39);
            panel1.TabIndex = 2;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "XY", "YZ", "XZ" });
            comboBox2.Location = new Point(276, 5);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(86, 32);
            comboBox2.TabIndex = 13;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(434, 5);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(106, 69);
            trackBar1.TabIndex = 12;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(366, 6);
            numericUpDown1.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(64, 30);
            numericUpDown1.TabIndex = 11;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(153, 7);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(126, 28);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "纵横比相等";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(54, 5);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(94, 32);
            comboBox1.TabIndex = 5;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 9);
            label4.Name = "label4";
            label4.Size = new Size(50, 24);
            label4.TabIndex = 3;
            label4.Text = "配色:";
            // 
            // button1
            // 
            button1.Location = new Point(6, 4);
            button1.Name = "button1";
            button1.Size = new Size(58, 32);
            button1.TabIndex = 6;
            button1.Text = "统计";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 537);
            panel2.Name = "panel2";
            panel2.Size = new Size(783, 39);
            panel2.TabIndex = 3;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(334, 7);
            label5.Name = "label5";
            label5.Size = new Size(48, 24);
            label5.TabIndex = 3;
            label5.Text = "Size:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(210, 7);
            label3.Name = "label3";
            label3.Size = new Size(62, 24);
            label3.TabIndex = 2;
            label3.Text = "Value:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(138, 7);
            label2.Name = "label2";
            label2.Size = new Size(21, 24);
            label2.TabIndex = 1;
            label2.Text = "J:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(73, 7);
            label1.Name = "label1";
            label1.Size = new Size(19, 24);
            label1.TabIndex = 0;
            label1.Text = "I:";
            // 
            // formsPlot1
            // 
            formsPlot1.Dock = DockStyle.Fill;
            formsPlot1.Location = new Point(0, 39);
            formsPlot1.Margin = new Padding(6, 5, 6, 5);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(783, 498);
            formsPlot1.TabIndex = 4;
            formsPlot1.MouseDown += formsPlot1_MouseDown;
            formsPlot1.MouseMove += formsPlot1_MouseMove;
            // 
            // Scottplot4GridProperty
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(formsPlot1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Scottplot4GridProperty";
            Size = new Size(783, 576);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Panel panel2;
        private Label label3;
        private Label label2;
        private Label label1;
        private ScottPlot.FormsPlot formsPlot1;
        private Label label4;
        private ComboBox comboBox1;
        private Label label5;
        private Button button1;
        private CheckBox checkBox1;
        private NumericUpDown numericUpDown1;
        private TrackBar trackBar1;
        private ComboBox comboBox2;
    }
}
