namespace JAM8.Algorithms.Geometry
{
    partial class Form_ReadConditionData
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
            button3 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            t_NoDataValue = new TextBox();
            t_Check空值 = new CheckBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            t_ColX = new MaskedTextBox();
            t_ColY = new MaskedTextBox();
            t_ColZ = new MaskedTextBox();
            groupBox1 = new GroupBox();
            label6 = new Label();
            label5 = new Label();
            comboBox2 = new ComboBox();
            button4 = new Button();
            textBox2 = new TextBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(183, 484);
            button1.Name = "button1";
            button1.Size = new Size(131, 33);
            button1.TabIndex = 0;
            button1.Text = "确定";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(461, 484);
            button2.Name = "button2";
            button2.Size = new Size(131, 33);
            button2.TabIndex = 1;
            button2.Text = "取消";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(12, 7);
            button3.Name = "button3";
            button3.Size = new Size(124, 62);
            button3.TabIndex = 5;
            button3.Text = "打开文件";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 77);
            label1.Name = "label1";
            label1.Size = new Size(46, 24);
            label1.TabIndex = 7;
            label1.Text = "预览";
            // 
            // textBox1
            // 
            textBox1.AllowDrop = true;
            textBox1.Location = new Point(142, 7);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(646, 62);
            textBox1.TabIndex = 8;
            textBox1.Text = "file path";
            textBox1.DragEnter += textBox1_DragEnter;
            // 
            // t_NoDataValue
            // 
            t_NoDataValue.Location = new Point(545, 27);
            t_NoDataValue.Name = "t_NoDataValue";
            t_NoDataValue.ReadOnly = true;
            t_NoDataValue.Size = new Size(195, 30);
            t_NoDataValue.TabIndex = 10;
            t_NoDataValue.Text = "-99";
            // 
            // t_Check空值
            // 
            t_Check空值.AutoSize = true;
            t_Check空值.Location = new Point(366, 28);
            t_Check空值.Name = "t_Check空值";
            t_Check空值.Size = new Size(161, 28);
            t_Check空值.TabIndex = 11;
            t_Check空值.Text = "No Data Value";
            t_Check空值.UseVisualStyleBackColor = true;
            t_Check空值.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 73);
            label2.Name = "label2";
            label2.Size = new Size(94, 24);
            label2.TabIndex = 12;
            label2.Text = "X - 表列序";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 117);
            label3.Name = "label3";
            label3.Size = new Size(93, 24);
            label3.TabIndex = 13;
            label3.Text = "Y - 表列序";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(27, 162);
            label4.Name = "label4";
            label4.Size = new Size(93, 24);
            label4.TabIndex = 14;
            label4.Text = "Z - 表列序";
            // 
            // t_ColX
            // 
            t_ColX.Location = new Point(139, 70);
            t_ColX.Mask = "99";
            t_ColX.Name = "t_ColX";
            t_ColX.Size = new Size(162, 30);
            t_ColX.TabIndex = 15;
            t_ColX.Text = "0";
            t_ColX.ValidatingType = typeof(int);
            // 
            // t_ColY
            // 
            t_ColY.Location = new Point(139, 114);
            t_ColY.Mask = "99";
            t_ColY.Name = "t_ColY";
            t_ColY.Size = new Size(162, 30);
            t_ColY.TabIndex = 16;
            t_ColY.Text = "1";
            t_ColY.ValidatingType = typeof(int);
            // 
            // t_ColZ
            // 
            t_ColZ.Location = new Point(139, 159);
            t_ColZ.Mask = "99";
            t_ColZ.Name = "t_ColZ";
            t_ColZ.Size = new Size(162, 30);
            t_ColZ.TabIndex = 17;
            t_ColZ.Text = "2";
            t_ColZ.ValidatingType = typeof(int);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(comboBox2);
            groupBox1.Controls.Add(t_ColZ);
            groupBox1.Controls.Add(t_ColY);
            groupBox1.Controls.Add(t_ColX);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(t_Check空值);
            groupBox1.Controls.Add(t_NoDataValue);
            groupBox1.Location = new Point(11, 274);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(772, 204);
            groupBox1.TabIndex = 18;
            groupBox1.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.Red;
            label6.Location = new Point(366, 95);
            label6.Name = "label6";
            label6.Size = new Size(205, 72);
            label6.TabIndex = 22;
            label6.Text = "注意:\r\n1.列序从0开始;\r\n2.每行分隔符只能是空格";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 29);
            label5.Name = "label5";
            label5.Size = new Size(46, 24);
            label5.TabIndex = 19;
            label5.Text = "维度";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "2D", "3D" });
            comboBox2.Location = new Point(139, 24);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(162, 32);
            comboBox2.TabIndex = 18;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // button4
            // 
            button4.ForeColor = Color.Blue;
            button4.Location = new Point(621, 73);
            button4.Name = "button4";
            button4.Size = new Size(167, 33);
            button4.TabIndex = 21;
            button4.Text = "示例数据";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click_1;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(11, 109);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(777, 168);
            textBox2.TabIndex = 20;
            // 
            // Form_ReadConditionData
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 528);
            Controls.Add(textBox2);
            Controls.Add(button4);
            Controls.Add(groupBox1);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Form_ReadConditionData";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Read Condition Data from GSLIB";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private Label label1;
        private TextBox textBox1;
        private TextBox t_NoDataValue;
        private CheckBox t_Check空值;
        private Label label2;
        private Label label3;
        private Label label4;
        private MaskedTextBox t_ColX;
        private MaskedTextBox t_ColY;
        private MaskedTextBox t_ColZ;
        private GroupBox groupBox1;
        private TextBox textBox2;
        private Button button4;
        private Label label5;
        private ComboBox comboBox2;
        private Label label6;
    }
}