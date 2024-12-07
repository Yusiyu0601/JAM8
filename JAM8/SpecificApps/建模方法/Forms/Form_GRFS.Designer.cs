namespace JAM8.SpecificApps.建模方法
{
    partial class Form_GRFS
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
            label5 = new Label();
            comboBox2 = new ComboBox();
            scottplot4Grid1 = new Algorithms.Geometry.Scottplot4Grid();
            label10 = new Label();
            tb_cd最小数量 = new TextBox();
            tb_nugget = new TextBox();
            label4 = new Label();
            tb_sill = new TextBox();
            label6 = new Label();
            tb_range_max = new TextBox();
            label9 = new Label();
            label13 = new Label();
            comboBox1 = new ComboBox();
            textBox10 = new TextBox();
            tb_seed = new TextBox();
            label8 = new Label();
            button3 = new Button();
            tb_nz = new TextBox();
            label3 = new Label();
            tb_ny = new TextBox();
            label2 = new Label();
            button5 = new Button();
            tb_nx = new TextBox();
            label1 = new Label();
            tb_range_med = new TextBox();
            label7 = new Label();
            tb_range_min = new TextBox();
            label11 = new Label();
            tb_Rake = new TextBox();
            label12 = new Label();
            tb_Dip = new TextBox();
            label14 = new Label();
            tb_Azimuth = new TextBox();
            label15 = new Label();
            groupBox1 = new GroupBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(229, 72);
            button1.Name = "button1";
            button1.Size = new Size(185, 32);
            button1.TabIndex = 211;
            button1.Text = "cdata预览";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 76);
            label5.Name = "label5";
            label5.Size = new Size(82, 24);
            label5.TabIndex = 210;
            label5.Text = "属性选取";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(106, 73);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(105, 32);
            comboBox2.TabIndex = 209;
            // 
            // scottplot4Grid1
            // 
            scottplot4Grid1.Location = new Point(427, 12);
            scottplot4Grid1.Name = "scottplot4Grid1";
            scottplot4Grid1.Size = new Size(714, 619);
            scottplot4Grid1.TabIndex = 208;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(228, 172);
            label10.Name = "label10";
            label10.Size = new Size(67, 24);
            label10.TabIndex = 206;
            label10.Text = "cd数量";
            // 
            // tb_cd最小数量
            // 
            tb_cd最小数量.Location = new Point(311, 172);
            tb_cd最小数量.Name = "tb_cd最小数量";
            tb_cd最小数量.Size = new Size(103, 30);
            tb_cd最小数量.TabIndex = 207;
            tb_cd最小数量.Text = "20";
            // 
            // tb_nugget
            // 
            tb_nugget.Location = new Point(307, 90);
            tb_nugget.Name = "tb_nugget";
            tb_nugget.Size = new Size(103, 30);
            tb_nugget.TabIndex = 205;
            tb_nugget.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(233, 93);
            label4.Name = "label4";
            label4.Size = new Size(46, 24);
            label4.TabIndex = 204;
            label4.Text = "块金";
            // 
            // tb_sill
            // 
            tb_sill.Location = new Point(105, 90);
            tb_sill.Name = "tb_sill";
            tb_sill.Size = new Size(105, 30);
            tb_sill.TabIndex = 203;
            tb_sill.Text = "1";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(15, 93);
            label6.Name = "label6";
            label6.Size = new Size(64, 24);
            label6.TabIndex = 202;
            label6.Text = "基台值";
            // 
            // tb_range_max
            // 
            tb_range_max.Location = new Point(105, 139);
            tb_range_max.Name = "tb_range_max";
            tb_range_max.Size = new Size(105, 30);
            tb_range_max.TabIndex = 201;
            tb_range_max.Text = "20";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(15, 142);
            label9.Name = "label9";
            label9.Size = new Size(83, 24);
            label9.TabIndex = 200;
            label9.Text = "Max变程";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(15, 42);
            label13.Name = "label13";
            label13.Size = new Size(82, 24);
            label13.TabIndex = 199;
            label13.Text = "理论模型";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "球状模型", "指数模型", "高斯模型" });
            comboBox1.Location = new Point(105, 39);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(304, 32);
            comboBox1.TabIndex = 198;
            // 
            // textBox10
            // 
            textBox10.Location = new Point(240, 14);
            textBox10.Multiline = true;
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.ScrollBars = ScrollBars.Vertical;
            textBox10.Size = new Size(174, 32);
            textBox10.TabIndex = 197;
            // 
            // tb_seed
            // 
            tb_seed.Location = new Point(311, 128);
            tb_seed.Name = "tb_seed";
            tb_seed.Size = new Size(103, 30);
            tb_seed.TabIndex = 196;
            tb_seed.Text = "123123";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(220, 131);
            label8.Name = "label8";
            label8.Size = new Size(82, 24);
            label8.TabIndex = 195;
            label8.Text = "随机种子";
            // 
            // button3
            // 
            button3.Location = new Point(12, 590);
            button3.Name = "button3";
            button3.Size = new Size(199, 34);
            button3.TabIndex = 189;
            button3.Text = "估计";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // tb_nz
            // 
            tb_nz.Location = new Point(106, 219);
            tb_nz.Name = "tb_nz";
            tb_nz.Size = new Size(105, 30);
            tb_nz.TabIndex = 194;
            tb_nz.Text = "1";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(44, 225);
            label3.Name = "label3";
            label3.Size = new Size(30, 24);
            label3.TabIndex = 193;
            label3.Text = "nz";
            // 
            // tb_ny
            // 
            tb_ny.Location = new Point(106, 172);
            tb_ny.Name = "tb_ny";
            tb_ny.Size = new Size(105, 30);
            tb_ny.TabIndex = 192;
            tb_ny.Text = "100";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(44, 178);
            label2.Name = "label2";
            label2.Size = new Size(31, 24);
            label2.TabIndex = 191;
            label2.Text = "ny";
            // 
            // button5
            // 
            button5.Location = new Point(12, 12);
            button5.Name = "button5";
            button5.Size = new Size(199, 34);
            button5.TabIndex = 190;
            button5.Text = "打开cdata";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // tb_nx
            // 
            tb_nx.Location = new Point(106, 128);
            tb_nx.Name = "tb_nx";
            tb_nx.Size = new Size(105, 30);
            tb_nx.TabIndex = 188;
            tb_nx.Text = "100";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 134);
            label1.Name = "label1";
            label1.Size = new Size(30, 24);
            label1.TabIndex = 187;
            label1.Text = "nx";
            // 
            // tb_range_med
            // 
            tb_range_med.Location = new Point(105, 194);
            tb_range_med.Name = "tb_range_med";
            tb_range_med.Size = new Size(105, 30);
            tb_range_med.TabIndex = 213;
            tb_range_med.Text = "20";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(15, 197);
            label7.Name = "label7";
            label7.Size = new Size(86, 24);
            label7.TabIndex = 212;
            label7.Text = "Med变程";
            // 
            // tb_range_min
            // 
            tb_range_min.Location = new Point(105, 247);
            tb_range_min.Name = "tb_range_min";
            tb_range_min.Size = new Size(105, 30);
            tb_range_min.TabIndex = 215;
            tb_range_min.Text = "2";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(15, 250);
            label11.Name = "label11";
            label11.Size = new Size(80, 24);
            label11.TabIndex = 214;
            label11.Text = "Min变程";
            // 
            // tb_Rake
            // 
            tb_Rake.Location = new Point(307, 244);
            tb_Rake.Name = "tb_Rake";
            tb_Rake.Size = new Size(103, 30);
            tb_Rake.TabIndex = 221;
            tb_Rake.Text = "0";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(236, 247);
            label12.Name = "label12";
            label12.Size = new Size(52, 24);
            label12.TabIndex = 220;
            label12.Text = "Rake";
            // 
            // tb_Dip
            // 
            tb_Dip.Location = new Point(307, 197);
            tb_Dip.Name = "tb_Dip";
            tb_Dip.Size = new Size(103, 30);
            tb_Dip.TabIndex = 219;
            tb_Dip.Text = "0";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(242, 200);
            label14.Name = "label14";
            label14.Size = new Size(41, 24);
            label14.TabIndex = 218;
            label14.Text = "Dip";
            // 
            // tb_Azimuth
            // 
            tb_Azimuth.Location = new Point(307, 142);
            tb_Azimuth.Name = "tb_Azimuth";
            tb_Azimuth.Size = new Size(103, 30);
            tb_Azimuth.TabIndex = 217;
            tb_Azimuth.Text = "0";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(221, 145);
            label15.Name = "label15";
            label15.Size = new Size(83, 24);
            label15.TabIndex = 216;
            label15.Text = "Azimuth";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tb_nugget);
            groupBox1.Controls.Add(tb_Rake);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(tb_range_max);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(tb_Dip);
            groupBox1.Controls.Add(tb_range_med);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(tb_sill);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(tb_Azimuth);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(tb_range_min);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(label15);
            groupBox1.Location = new Point(12, 279);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(430, 294);
            groupBox1.TabIndex = 222;
            groupBox1.TabStop = false;
            groupBox1.Text = "变差函数";
            // 
            // Form_GRFS
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1141, 641);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(label5);
            Controls.Add(comboBox2);
            Controls.Add(scottplot4Grid1);
            Controls.Add(label10);
            Controls.Add(tb_cd最小数量);
            Controls.Add(textBox10);
            Controls.Add(tb_seed);
            Controls.Add(label8);
            Controls.Add(button3);
            Controls.Add(tb_nz);
            Controls.Add(label3);
            Controls.Add(tb_ny);
            Controls.Add(label2);
            Controls.Add(button5);
            Controls.Add(tb_nx);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_GRFS";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_GRFS";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label5;
        private ComboBox comboBox2;
        private Algorithms.Geometry.Scottplot4Grid scottplot4Grid1;
        private Label label10;
        private TextBox tb_cd最小数量;
        private TextBox tb_nugget;
        private Label label4;
        private TextBox tb_sill;
        private Label label6;
        private TextBox tb_range_max;
        private Label label9;
        private Label label13;
        private ComboBox comboBox1;
        private TextBox textBox10;
        private TextBox tb_seed;
        private Label label8;
        private Button button3;
        private TextBox tb_nz;
        private Label label3;
        private TextBox tb_ny;
        private Label label2;
        private Button button5;
        private TextBox tb_nx;
        private Label label1;
        private TextBox tb_range_med;
        private Label label7;
        private TextBox tb_range_min;
        private Label label11;
        private TextBox tb_Rake;
        private Label label12;
        private TextBox tb_Dip;
        private Label label14;
        private TextBox tb_Azimuth;
        private Label label15;
        private GroupBox groupBox1;
    }
}