namespace JAM8.Algorithms.Geometry
{
    partial class Form_GridStructure
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
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            tb_ICount = new TextBox();
            tb_JCount = new TextBox();
            tb_KCount = new TextBox();
            tb_ISize = new TextBox();
            tb_JSize = new TextBox();
            tb_KSize = new TextBox();
            tb_OriginCellZ = new TextBox();
            tb_OriginCellY = new TextBox();
            tb_OriginCellX = new TextBox();
            label10 = new Label();
            comboBox2 = new ComboBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(110, 185);
            button1.Name = "button1";
            button1.Size = new Size(151, 33);
            button1.TabIndex = 0;
            button1.Text = "确定";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(460, 185);
            button2.Name = "button2";
            button2.Size = new Size(151, 33);
            button2.TabIndex = 1;
            button2.Text = "取消";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 61);
            label1.Name = "label1";
            label1.Size = new Size(30, 24);
            label1.TabIndex = 2;
            label1.Text = "nx";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 101);
            label2.Name = "label2";
            label2.Size = new Size(31, 24);
            label2.TabIndex = 3;
            label2.Text = "ny";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(23, 139);
            label3.Name = "label3";
            label3.Size = new Size(30, 24);
            label3.TabIndex = 4;
            label3.Text = "nz";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(259, 61);
            label4.Name = "label4";
            label4.Size = new Size(41, 24);
            label4.TabIndex = 5;
            label4.Text = "xsiz";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(259, 101);
            label5.Name = "label5";
            label5.Size = new Size(42, 24);
            label5.TabIndex = 6;
            label5.Text = "ysiz";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(259, 139);
            label6.Name = "label6";
            label6.Size = new Size(41, 24);
            label6.TabIndex = 7;
            label6.Text = "zsiz";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(503, 139);
            label7.Name = "label7";
            label7.Size = new Size(47, 24);
            label7.TabIndex = 10;
            label7.Text = "zmn";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(503, 101);
            label8.Name = "label8";
            label8.Size = new Size(48, 24);
            label8.TabIndex = 9;
            label8.Text = "ymn";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(503, 61);
            label9.Name = "label9";
            label9.Size = new Size(47, 24);
            label9.TabIndex = 8;
            label9.Text = "xmn";
            // 
            // tb_ICount
            // 
            tb_ICount.Location = new Point(61, 55);
            tb_ICount.Name = "tb_ICount";
            tb_ICount.Size = new Size(153, 30);
            tb_ICount.TabIndex = 11;
            tb_ICount.Text = "100";
            // 
            // tb_JCount
            // 
            tb_JCount.Location = new Point(61, 95);
            tb_JCount.Name = "tb_JCount";
            tb_JCount.Size = new Size(153, 30);
            tb_JCount.TabIndex = 12;
            tb_JCount.Text = "100";
            // 
            // tb_KCount
            // 
            tb_KCount.Location = new Point(61, 133);
            tb_KCount.Name = "tb_KCount";
            tb_KCount.Size = new Size(153, 30);
            tb_KCount.TabIndex = 13;
            tb_KCount.Text = "10";
            // 
            // tb_ISize
            // 
            tb_ISize.Location = new Point(308, 55);
            tb_ISize.Name = "tb_ISize";
            tb_ISize.Size = new Size(153, 30);
            tb_ISize.TabIndex = 14;
            tb_ISize.Text = "1";
            // 
            // tb_JSize
            // 
            tb_JSize.Location = new Point(308, 95);
            tb_JSize.Name = "tb_JSize";
            tb_JSize.Size = new Size(153, 30);
            tb_JSize.TabIndex = 15;
            tb_JSize.Text = "1";
            // 
            // tb_KSize
            // 
            tb_KSize.Location = new Point(308, 133);
            tb_KSize.Name = "tb_KSize";
            tb_KSize.Size = new Size(153, 30);
            tb_KSize.TabIndex = 16;
            tb_KSize.Text = "1";
            // 
            // tb_OriginCellZ
            // 
            tb_OriginCellZ.Location = new Point(561, 133);
            tb_OriginCellZ.Name = "tb_OriginCellZ";
            tb_OriginCellZ.Size = new Size(153, 30);
            tb_OriginCellZ.TabIndex = 19;
            tb_OriginCellZ.Text = "0.5";
            // 
            // tb_OriginCellY
            // 
            tb_OriginCellY.Location = new Point(561, 95);
            tb_OriginCellY.Name = "tb_OriginCellY";
            tb_OriginCellY.Size = new Size(153, 30);
            tb_OriginCellY.TabIndex = 18;
            tb_OriginCellY.Text = "0.5";
            // 
            // tb_OriginCellX
            // 
            tb_OriginCellX.Location = new Point(561, 55);
            tb_OriginCellX.Name = "tb_OriginCellX";
            tb_OriginCellX.Size = new Size(153, 30);
            tb_OriginCellX.TabIndex = 17;
            tb_OriginCellX.Text = "0.5";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(10, 12);
            label10.Name = "label10";
            label10.Size = new Size(46, 24);
            label10.TabIndex = 23;
            label10.Text = "维度";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "2D", "3D" });
            comboBox2.Location = new Point(62, 9);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(153, 32);
            comboBox2.TabIndex = 22;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // Form_GridStructure
            // 
            AcceptButton = button1;
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(737, 230);
            Controls.Add(label10);
            Controls.Add(comboBox2);
            Controls.Add(tb_OriginCellZ);
            Controls.Add(tb_OriginCellY);
            Controls.Add(tb_OriginCellX);
            Controls.Add(tb_KSize);
            Controls.Add(tb_JSize);
            Controls.Add(tb_ISize);
            Controls.Add(tb_KCount);
            Controls.Add(tb_JCount);
            Controls.Add(tb_ICount);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Form_GridStructure";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GridStructure设置";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox tb_ICount;
        private TextBox tb_JCount;
        private TextBox tb_KCount;
        private TextBox tb_ISize;
        private TextBox tb_JSize;
        private TextBox tb_KSize;
        private TextBox tb_OriginCellZ;
        private TextBox tb_OriginCellY;
        private TextBox tb_OriginCellX;
        private Label label10;
        private ComboBox comboBox2;
    }
}