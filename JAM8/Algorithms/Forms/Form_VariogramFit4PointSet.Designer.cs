namespace JAM8.Algorithms.Forms
{
    partial class Form_VariogramFit4PointSet
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
            button5 = new Button();
            listBox1 = new ListBox();
            scottplot4GridProperty1 = new JAM8.Algorithms.Geometry.Scottplot4GridProperty();
            button1 = new Button();
            label1 = new Label();
            dataGridView1 = new DataGridView();
            label13 = new Label();
            comboBox1 = new ComboBox();
            numericUpDown_azimuth = new NumericUpDown();
            label11 = new Label();
            trackBar_azimuth = new TrackBar();
            trackBar_N_lag = new TrackBar();
            numericUpDown_N_lag = new NumericUpDown();
            label12 = new Label();
            numericUpDown_bandwidth = new NumericUpDown();
            label2 = new Label();
            trackBar_bandwidth = new TrackBar();
            trackBar_lag_unit = new TrackBar();
            numericUpDown_lag_unit = new NumericUpDown();
            label3 = new Label();
            numericUpDown_azimuth_tolerance = new NumericUpDown();
            label4 = new Label();
            trackBar_azimuth_tolerance = new TrackBar();
            button4 = new Button();
            button3 = new Button();
            numericUpDown6 = new NumericUpDown();
            numericUpDown7 = new NumericUpDown();
            numericUpDown8 = new NumericUpDown();
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
            label14 = new Label();
            textBox1 = new TextBox();
            label15 = new Label();
            label16 = new Label();
            formsPlot1 = new ScottPlot.FormsPlot();
            formsPlot2 = new ScottPlot.FormsPlot();
            label17 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_azimuth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_azimuth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_N_lag).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_N_lag).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_bandwidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_bandwidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_lag_unit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lag_unit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_azimuth_tolerance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_azimuth_tolerance).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown8).BeginInit();
            SuspendLayout();
            // 
            // button5
            // 
            button5.Enabled = false;
            button5.Location = new Point(324, 14);
            button5.Name = "button5";
            button5.Size = new Size(146, 36);
            button5.TabIndex = 72;
            button5.Text = "垂向变差函数";
            button5.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(13, 62);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(559, 220);
            listBox1.TabIndex = 71;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // scottplot4GridProperty1
            // 
            scottplot4GridProperty1.Location = new Point(1, 289);
            scottplot4GridProperty1.Name = "scottplot4GridProperty1";
            scottplot4GridProperty1.Size = new Size(590, 628);
            scottplot4GridProperty1.TabIndex = 70;
            // 
            // button1
            // 
            button1.Location = new Point(71, 14);
            button1.Name = "button1";
            button1.Size = new Size(151, 36);
            button1.TabIndex = 69;
            button1.Text = "打开模型";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1240, 14);
            label1.Name = "label1";
            label1.Size = new Size(118, 24);
            label1.TabIndex = 74;
            label1.Text = "实验变差函数";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(1008, 48);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(583, 265);
            dataGridView1.TabIndex = 73;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(723, 304);
            label13.Name = "label13";
            label13.Size = new Size(118, 24);
            label13.TabIndex = 82;
            label13.Text = "变差函数模型";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "球状模型", "指数模型", "高斯模型" });
            comboBox1.Location = new Point(847, 300);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(130, 32);
            comboBox1.TabIndex = 81;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // numericUpDown_azimuth
            // 
            numericUpDown_azimuth.Location = new Point(898, 75);
            numericUpDown_azimuth.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            numericUpDown_azimuth.Name = "numericUpDown_azimuth";
            numericUpDown_azimuth.Size = new Size(79, 30);
            numericUpDown_azimuth.TabIndex = 78;
            numericUpDown_azimuth.ValueChanged += numericUpDown_azimuth_ValueChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(667, 77);
            label11.Name = "label11";
            label11.Size = new Size(80, 24);
            label11.TabIndex = 76;
            label11.Text = "azimuth";
            // 
            // trackBar_azimuth
            // 
            trackBar_azimuth.Location = new Point(740, 75);
            trackBar_azimuth.Maximum = 360;
            trackBar_azimuth.Name = "trackBar_azimuth";
            trackBar_azimuth.Size = new Size(152, 69);
            trackBar_azimuth.TabIndex = 80;
            trackBar_azimuth.Scroll += trackBar_azimuth_Scroll;
            // 
            // trackBar_N_lag
            // 
            trackBar_N_lag.Location = new Point(741, 20);
            trackBar_N_lag.Maximum = 100;
            trackBar_N_lag.Minimum = 5;
            trackBar_N_lag.Name = "trackBar_N_lag";
            trackBar_N_lag.Size = new Size(152, 69);
            trackBar_N_lag.TabIndex = 79;
            trackBar_N_lag.Value = 5;
            trackBar_N_lag.Scroll += trackBar_N_lag_Scroll;
            // 
            // numericUpDown_N_lag
            // 
            numericUpDown_N_lag.Location = new Point(898, 18);
            numericUpDown_N_lag.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            numericUpDown_N_lag.Name = "numericUpDown_N_lag";
            numericUpDown_N_lag.Size = new Size(79, 30);
            numericUpDown_N_lag.TabIndex = 77;
            numericUpDown_N_lag.Value = new decimal(new int[] { 5, 0, 0, 0 });
            numericUpDown_N_lag.ValueChanged += numericUpDown_N_lag_ValueChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(687, 20);
            label12.Name = "label12";
            label12.Size = new Size(60, 24);
            label12.TabIndex = 75;
            label12.Text = "N_lag";
            // 
            // numericUpDown_bandwidth
            // 
            numericUpDown_bandwidth.Location = new Point(898, 189);
            numericUpDown_bandwidth.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numericUpDown_bandwidth.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_bandwidth.Name = "numericUpDown_bandwidth";
            numericUpDown_bandwidth.Size = new Size(79, 30);
            numericUpDown_bandwidth.TabIndex = 86;
            numericUpDown_bandwidth.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_bandwidth.ValueChanged += numericUpDown_bandwidth_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(643, 191);
            label2.Name = "label2";
            label2.Size = new Size(104, 24);
            label2.TabIndex = 84;
            label2.Text = "bandwidth";
            // 
            // trackBar_bandwidth
            // 
            trackBar_bandwidth.Location = new Point(740, 191);
            trackBar_bandwidth.Maximum = 50;
            trackBar_bandwidth.Minimum = 1;
            trackBar_bandwidth.Name = "trackBar_bandwidth";
            trackBar_bandwidth.Size = new Size(152, 69);
            trackBar_bandwidth.TabIndex = 88;
            trackBar_bandwidth.Value = 1;
            trackBar_bandwidth.Scroll += trackBar_bandwidth_Scroll;
            // 
            // trackBar_lag_unit
            // 
            trackBar_lag_unit.Location = new Point(741, 134);
            trackBar_lag_unit.Minimum = 1;
            trackBar_lag_unit.Name = "trackBar_lag_unit";
            trackBar_lag_unit.Size = new Size(152, 69);
            trackBar_lag_unit.TabIndex = 87;
            trackBar_lag_unit.Value = 1;
            trackBar_lag_unit.Scroll += trackBar_lag_unit_Scroll;
            // 
            // numericUpDown_lag_unit
            // 
            numericUpDown_lag_unit.Location = new Point(898, 132);
            numericUpDown_lag_unit.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numericUpDown_lag_unit.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_lag_unit.Name = "numericUpDown_lag_unit";
            numericUpDown_lag_unit.Size = new Size(79, 30);
            numericUpDown_lag_unit.TabIndex = 85;
            numericUpDown_lag_unit.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDown_lag_unit.ValueChanged += numericUpDown_lag_unit_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(668, 134);
            label3.Name = "label3";
            label3.Size = new Size(79, 24);
            label3.TabIndex = 83;
            label3.Text = "lag_unit";
            // 
            // numericUpDown_azimuth_tolerance
            // 
            numericUpDown_azimuth_tolerance.Location = new Point(899, 246);
            numericUpDown_azimuth_tolerance.Maximum = new decimal(new int[] { 45, 0, 0, 0 });
            numericUpDown_azimuth_tolerance.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            numericUpDown_azimuth_tolerance.Name = "numericUpDown_azimuth_tolerance";
            numericUpDown_azimuth_tolerance.Size = new Size(79, 30);
            numericUpDown_azimuth_tolerance.TabIndex = 90;
            numericUpDown_azimuth_tolerance.Value = new decimal(new int[] { 15, 0, 0, 0 });
            numericUpDown_azimuth_tolerance.ValueChanged += numericUpDown_azimuth_tolerance_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(579, 248);
            label4.Name = "label4";
            label4.Size = new Size(168, 24);
            label4.TabIndex = 89;
            label4.Text = "azimuth_tolerance";
            // 
            // trackBar_azimuth_tolerance
            // 
            trackBar_azimuth_tolerance.Location = new Point(740, 244);
            trackBar_azimuth_tolerance.Maximum = 45;
            trackBar_azimuth_tolerance.Minimum = 15;
            trackBar_azimuth_tolerance.Name = "trackBar_azimuth_tolerance";
            trackBar_azimuth_tolerance.Size = new Size(152, 69);
            trackBar_azimuth_tolerance.TabIndex = 91;
            trackBar_azimuth_tolerance.Value = 15;
            trackBar_azimuth_tolerance.Scroll += trackBar_azimuth_tolerance_Scroll;
            // 
            // button4
            // 
            button4.Location = new Point(1403, 909);
            button4.Name = "button4";
            button4.Size = new Size(188, 36);
            button4.TabIndex = 112;
            button4.Text = "保存拟合变差函数";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(1403, 319);
            button3.Name = "button3";
            button3.Size = new Size(188, 36);
            button3.TabIndex = 111;
            button3.Text = "保存实验变差函数";
            button3.UseVisualStyleBackColor = true;
            // 
            // numericUpDown6
            // 
            numericUpDown6.Location = new Point(1471, 873);
            numericUpDown6.Name = "numericUpDown6";
            numericUpDown6.Size = new Size(114, 30);
            numericUpDown6.TabIndex = 110;
            // 
            // numericUpDown7
            // 
            numericUpDown7.Location = new Point(1471, 833);
            numericUpDown7.Name = "numericUpDown7";
            numericUpDown7.Size = new Size(114, 30);
            numericUpDown7.TabIndex = 109;
            // 
            // numericUpDown8
            // 
            numericUpDown8.Location = new Point(1471, 794);
            numericUpDown8.Name = "numericUpDown8";
            numericUpDown8.Size = new Size(114, 30);
            numericUpDown8.TabIndex = 108;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(1264, 875);
            label8.Name = "label8";
            label8.Size = new Size(46, 24);
            label8.TabIndex = 107;
            label8.Text = "块金";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(1264, 835);
            label9.Name = "label9";
            label9.Size = new Size(46, 24);
            label9.TabIndex = 106;
            label9.Text = "基台";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(1264, 797);
            label10.Name = "label10";
            label10.Size = new Size(46, 24);
            label10.TabIndex = 105;
            label10.Text = "变程";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(1335, 794);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(111, 30);
            textBox4.TabIndex = 104;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(1335, 832);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(111, 30);
            textBox5.TabIndex = 103;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(1335, 872);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(111, 30);
            textBox6.TabIndex = 102;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(1393, 753);
            label7.Name = "label7";
            label7.Size = new Size(46, 24);
            label7.TabIndex = 101;
            label7.Text = "调节";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(1091, 753);
            label6.Name = "label6";
            label6.Size = new Size(82, 24);
            label6.TabIndex = 100;
            label6.Text = "自动拟合";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(1079, 869);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(133, 30);
            textBox3.TabIndex = 99;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(1010, 872);
            label5.Name = "label5";
            label5.Size = new Size(46, 24);
            label5.TabIndex = 98;
            label5.Text = "块金";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(1079, 829);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(133, 30);
            textBox2.TabIndex = 97;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(1010, 832);
            label14.Name = "label14";
            label14.Size = new Size(46, 24);
            label14.TabIndex = 96;
            label14.Text = "基台";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(1079, 791);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(133, 30);
            textBox1.TabIndex = 95;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(1010, 794);
            label15.Name = "label15";
            label15.Size = new Size(46, 24);
            label15.TabIndex = 94;
            label15.Text = "变程";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(1240, 364);
            label16.Name = "label16";
            label16.Size = new Size(118, 24);
            label16.TabIndex = 93;
            label16.Text = "变差函数拟合";
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(1008, 379);
            formsPlot1.Margin = new Padding(6, 5, 6, 5);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(583, 369);
            formsPlot1.TabIndex = 92;
            // 
            // formsPlot2
            // 
            formsPlot2.Location = new Point(588, 379);
            formsPlot2.Margin = new Padding(6, 5, 6, 5);
            formsPlot2.Name = "formsPlot2";
            formsPlot2.Size = new Size(429, 369);
            formsPlot2.TabIndex = 113;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(762, 364);
            label17.Name = "label17";
            label17.Size = new Size(100, 24);
            label17.TabIndex = 114;
            label17.Text = "点对搜索窗";
            // 
            // Form_VariogramFit4PointSet
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1612, 968);
            Controls.Add(label17);
            Controls.Add(formsPlot2);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(numericUpDown6);
            Controls.Add(numericUpDown7);
            Controls.Add(numericUpDown8);
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
            Controls.Add(label14);
            Controls.Add(textBox1);
            Controls.Add(label15);
            Controls.Add(label16);
            Controls.Add(formsPlot1);
            Controls.Add(label13);
            Controls.Add(comboBox1);
            Controls.Add(numericUpDown_azimuth_tolerance);
            Controls.Add(label4);
            Controls.Add(trackBar_azimuth_tolerance);
            Controls.Add(numericUpDown_bandwidth);
            Controls.Add(label2);
            Controls.Add(numericUpDown_lag_unit);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(numericUpDown_azimuth);
            Controls.Add(label11);
            Controls.Add(numericUpDown_N_lag);
            Controls.Add(label12);
            Controls.Add(button5);
            Controls.Add(listBox1);
            Controls.Add(scottplot4GridProperty1);
            Controls.Add(button1);
            Controls.Add(trackBar_bandwidth);
            Controls.Add(trackBar_lag_unit);
            Controls.Add(trackBar_azimuth);
            Controls.Add(trackBar_N_lag);
            Name = "Form_VariogramFit4PointSet";
            Text = "Form_VariogramFit4PointSet";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_azimuth).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_azimuth).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_N_lag).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_N_lag).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_bandwidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_bandwidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_lag_unit).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_lag_unit).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown_azimuth_tolerance).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar_azimuth_tolerance).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown6).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown7).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown8).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button5;
        private ListBox listBox1;
        private Geometry.Scottplot4GridProperty scottplot4GridProperty1;
        private Button button1;
        private Label label1;
        private DataGridView dataGridView1;
        private Label label13;
        private ComboBox comboBox1;
        private NumericUpDown numericUpDown_azimuth;
        private Label label11;
        private TrackBar trackBar_azimuth;
        private TrackBar trackBar_N_lag;
        private NumericUpDown numericUpDown_N_lag;
        private Label label12;
        private NumericUpDown numericUpDown_bandwidth;
        private Label label2;
        private TrackBar trackBar_bandwidth;
        private TrackBar trackBar_lag_unit;
        private NumericUpDown numericUpDown_lag_unit;
        private Label label3;
        private NumericUpDown numericUpDown_azimuth_tolerance;
        private Label label4;
        private TrackBar trackBar_azimuth_tolerance;
        private Button button4;
        private Button button3;
        private NumericUpDown numericUpDown6;
        private NumericUpDown numericUpDown7;
        private NumericUpDown numericUpDown8;
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
        private Label label14;
        private TextBox textBox1;
        private Label label15;
        private Label label16;
        private ScottPlot.FormsPlot formsPlot1;
        private ScottPlot.FormsPlot formsPlot2;
        private Label label17;
    }
}