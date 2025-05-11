namespace JAM8.Algorithms.Forms
{
    partial class Form_VariogramFit4Grid
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
            button1 = new Button();
            numericUpDown3 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            numericUpDown1 = new NumericUpDown();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            label7 = new Label();
            label6 = new Label();
            textBox3 = new TextBox();
            label5 = new Label();
            textBox2 = new TextBox();
            label3 = new Label();
            textBox1 = new TextBox();
            label4 = new Label();
            label2 = new Label();
            label1 = new Label();
            formsPlot1 = new ScottPlot.FormsPlot();
            dataGridView1 = new DataGridView();
            scottplot4GridProperty1 = new JAM8.Algorithms.Geometry.Scottplot4GridProperty();
            listBox1 = new ListBox();
            label11 = new Label();
            label12 = new Label();
            numericUpDown4 = new NumericUpDown();
            numericUpDown5 = new NumericUpDown();
            trackBar1 = new TrackBar();
            trackBar2 = new TrackBar();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            comboBox1 = new ComboBox();
            label13 = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(71, 12);
            button1.Name = "button1";
            button1.Size = new Size(151, 36);
            button1.TabIndex = 56;
            button1.Text = "打开模型";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(1102, 882);
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(114, 30);
            numericUpDown3.TabIndex = 55;
            numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(1102, 842);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(114, 30);
            numericUpDown2.TabIndex = 54;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(1102, 803);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(114, 30);
            numericUpDown1.TabIndex = 53;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(895, 884);
            label8.Name = "label8";
            label8.Size = new Size(46, 24);
            label8.TabIndex = 52;
            label8.Text = "块金";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(895, 844);
            label9.Name = "label9";
            label9.Size = new Size(46, 24);
            label9.TabIndex = 51;
            label9.Text = "基台";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(895, 806);
            label10.Name = "label10";
            label10.Size = new Size(46, 24);
            label10.TabIndex = 50;
            label10.Text = "变程";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(966, 803);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(111, 30);
            textBox4.TabIndex = 49;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(966, 841);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(111, 30);
            textBox5.TabIndex = 48;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(966, 881);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(111, 30);
            textBox6.TabIndex = 47;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(1024, 762);
            label7.Name = "label7";
            label7.Size = new Size(46, 24);
            label7.TabIndex = 46;
            label7.Text = "调节";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(722, 762);
            label6.Name = "label6";
            label6.Size = new Size(82, 24);
            label6.TabIndex = 45;
            label6.Text = "自动拟合";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(710, 878);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(133, 30);
            textBox3.TabIndex = 44;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(641, 881);
            label5.Name = "label5";
            label5.Size = new Size(46, 24);
            label5.TabIndex = 43;
            label5.Text = "块金";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(710, 838);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(133, 30);
            textBox2.TabIndex = 42;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(641, 841);
            label3.Name = "label3";
            label3.Size = new Size(46, 24);
            label3.TabIndex = 41;
            label3.Text = "基台";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(710, 800);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(133, 30);
            textBox1.TabIndex = 40;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(641, 803);
            label4.Name = "label4";
            label4.Size = new Size(46, 24);
            label4.TabIndex = 39;
            label4.Text = "变程";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1015, 250);
            label2.Name = "label2";
            label2.Size = new Size(118, 24);
            label2.TabIndex = 38;
            label2.Text = "变差函数拟合";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1018, 76);
            label1.Name = "label1";
            label1.Size = new Size(118, 24);
            label1.TabIndex = 37;
            label1.Text = "实验变差函数";
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(603, 279);
            formsPlot1.Margin = new Padding(6, 5, 6, 5);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(931, 477);
            formsPlot1.TabIndex = 36;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(603, 107);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(931, 125);
            dataGridView1.TabIndex = 35;
            // 
            // scottplot4GridProperty1
            // 
            scottplot4GridProperty1.Location = new Point(1, 236);
            scottplot4GridProperty1.Name = "scottplot4GridProperty1";
            scottplot4GridProperty1.Size = new Size(590, 679);
            scottplot4GridProperty1.TabIndex = 58;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(13, 60);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(559, 172);
            listBox1.TabIndex = 59;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(1232, 18);
            label11.Name = "label11";
            label11.Size = new Size(64, 24);
            label11.TabIndex = 61;
            label11.Text = "方位角";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(869, 18);
            label12.Name = "label12";
            label12.Size = new Size(100, 24);
            label12.TabIndex = 60;
            label12.Text = "滞后距数量";
            // 
            // numericUpDown4
            // 
            numericUpDown4.Location = new Point(1129, 15);
            numericUpDown4.Name = "numericUpDown4";
            numericUpDown4.Size = new Size(79, 30);
            numericUpDown4.TabIndex = 62;
            numericUpDown4.ValueChanged += numericUpDown4_ValueChanged;
            // 
            // numericUpDown5
            // 
            numericUpDown5.Location = new Point(1455, 15);
            numericUpDown5.Name = "numericUpDown5";
            numericUpDown5.Size = new Size(79, 30);
            numericUpDown5.TabIndex = 63;
            numericUpDown5.ValueChanged += numericUpDown5_ValueChanged;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(971, 18);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(152, 69);
            trackBar1.TabIndex = 64;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // trackBar2
            // 
            trackBar2.Location = new Point(1296, 18);
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(152, 69);
            trackBar2.TabIndex = 65;
            trackBar2.Scroll += trackBar2_Scroll;
            // 
            // button3
            // 
            button3.Location = new Point(1346, 238);
            button3.Name = "button3";
            button3.Size = new Size(188, 36);
            button3.TabIndex = 66;
            button3.Text = "保存实验变差函数";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(1346, 878);
            button4.Name = "button4";
            button4.Size = new Size(188, 36);
            button4.TabIndex = 67;
            button4.Text = "保存拟合变差函数";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Enabled = false;
            button5.Location = new Point(324, 12);
            button5.Name = "button5";
            button5.Size = new Size(146, 36);
            button5.TabIndex = 68;
            button5.Text = "垂向变差函数";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "球状模型", "指数模型", "高斯模型" });
            comboBox1.Location = new Point(727, 14);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(130, 32);
            comboBox1.TabIndex = 69;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(603, 18);
            label13.Name = "label13";
            label13.Size = new Size(118, 24);
            label13.TabIndex = 70;
            label13.Text = "变差函数模型";
            // 
            // Form_VariogramFit4Grid
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1558, 927);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(label13);
            Controls.Add(comboBox1);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(numericUpDown5);
            Controls.Add(label11);
            Controls.Add(trackBar2);
            Controls.Add(trackBar1);
            Controls.Add(numericUpDown4);
            Controls.Add(label12);
            Controls.Add(listBox1);
            Controls.Add(scottplot4GridProperty1);
            Controls.Add(button1);
            Controls.Add(numericUpDown3);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(label10);
            Controls.Add(textBox4);
            Controls.Add(textBox5);
            Controls.Add(textBox6);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(textBox3);
            Controls.Add(label5);
            Controls.Add(textBox2);
            Controls.Add(label3);
            Controls.Add(textBox1);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(formsPlot1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_VariogramFit4Grid";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_VariogramFit4Grid";
            Load += Form_VariogramFit4Grid_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown4).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown5).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown1;
        private Label label8;
        private Label label9;
        private Label label10;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label label7;
        private Label label6;
        private TextBox textBox3;
        private Label label5;
        private TextBox textBox2;
        private Label label3;
        private TextBox textBox1;
        private Label label4;
        private Label label2;
        private Label label1;
        private ScottPlot.FormsPlot formsPlot1;
        private DataGridView dataGridView1;
        private Geometry.Scottplot4GridProperty scottplot4GridProperty1;
        private ListBox listBox1;
        private Label label11;
        private Label label12;
        private NumericUpDown numericUpDown4;
        private NumericUpDown numericUpDown5;
        private TrackBar trackBar1;
        private TrackBar trackBar2;
        private Button button3;
        private Button button4;
        private Button button5;
        private ComboBox comboBox1;
        private Label label13;
    }
}