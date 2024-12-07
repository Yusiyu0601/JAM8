namespace JAM8.SpecificApps.常用工具
{
    partial class Form_层位测井均值
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
            label1 = new Label();
            button1 = new Button();
            dataGridView1 = new DataGridView();
            label3 = new Label();
            listBox1 = new ListBox();
            label2 = new Label();
            button2 = new Button();
            dataGridView2 = new DataGridView();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            button3 = new Button();
            label6 = new Label();
            comboBox3 = new ComboBox();
            label7 = new Label();
            listBox2 = new ListBox();
            button4 = new Button();
            label8 = new Label();
            comboBox4 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Blue;
            label1.Location = new Point(205, 30);
            label1.Name = "label1";
            label1.Size = new Size(82, 24);
            label1.TabIndex = 4;
            label1.Text = "示例数据";
            label1.Click += label1_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(187, 42);
            button1.TabIndex = 3;
            button1.Text = "打开层位文件(excel)";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 60);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(575, 495);
            dataGridView1.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(593, 64);
            label3.Name = "label3";
            label3.Size = new Size(82, 24);
            label3.TabIndex = 11;
            label3.Text = "井名列表";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(593, 95);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(245, 196);
            listBox1.TabIndex = 10;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Blue;
            label2.Location = new Point(789, 29);
            label2.Name = "label2";
            label2.Size = new Size(82, 24);
            label2.TabIndex = 9;
            label2.Text = "示例数据";
            label2.Click += label2_Click;
            // 
            // button2
            // 
            button2.Location = new Point(593, 11);
            button2.Name = "button2";
            button2.Size = new Size(190, 42);
            button2.TabIndex = 8;
            button2.Text = "打开测井文件(excel)";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(844, 60);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.RowHeadersWidth = 62;
            dataGridView2.Size = new Size(575, 495);
            dataGridView2.TabIndex = 12;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(456, 576);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(118, 32);
            comboBox1.TabIndex = 13;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(257, 576);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(128, 32);
            comboBox2.TabIndex = 14;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(398, 579);
            label4.Name = "label4";
            label4.Size = new Size(46, 24);
            label4.TabIndex = 15;
            label4.Text = "顶深";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(199, 579);
            label5.Name = "label5";
            label5.Size = new Size(46, 24);
            label5.TabIndex = 16;
            label5.Text = "底深";
            // 
            // button3
            // 
            button3.Location = new Point(1174, 570);
            button3.Name = "button3";
            button3.Size = new Size(245, 42);
            button3.TabIndex = 17;
            button3.Text = "计算层位的测井均值";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 579);
            label6.Name = "label6";
            label6.Size = new Size(46, 24);
            label6.TabIndex = 19;
            label6.Text = "井名";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(72, 576);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(110, 32);
            comboBox3.TabIndex = 18;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(593, 308);
            label7.Name = "label7";
            label7.Size = new Size(82, 24);
            label7.TabIndex = 20;
            label7.Text = "属性列表";
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 24;
            listBox2.Location = new Point(593, 335);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(245, 172);
            listBox2.TabIndex = 21;
            // 
            // button4
            // 
            button4.Location = new Point(593, 513);
            button4.Name = "button4";
            button4.Size = new Size(245, 42);
            button4.TabIndex = 22;
            button4.Text = "检查文件属性相同";
            button4.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(881, 579);
            label8.Name = "label8";
            label8.Size = new Size(130, 24);
            label8.TabIndex = 24;
            label8.Text = "深度[测井文件]";
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(1017, 576);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(118, 32);
            comboBox4.TabIndex = 23;
            // 
            // Form_层位测井均值
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1431, 625);
            Controls.Add(label8);
            Controls.Add(comboBox4);
            Controls.Add(button4);
            Controls.Add(listBox2);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(comboBox3);
            Controls.Add(button3);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(dataGridView2);
            Controls.Add(label3);
            Controls.Add(listBox1);
            Controls.Add(label2);
            Controls.Add(button2);
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_层位测井均值";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_计算层位内的测井均值";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button button1;
        private DataGridView dataGridView1;
        private Label label3;
        private ListBox listBox1;
        private Label label2;
        private Button button2;
        private DataGridView dataGridView2;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Label label4;
        private Label label5;
        private Button button3;
        private Label label6;
        private ComboBox comboBox3;
        private Label label7;
        private ListBox listBox2;
        private Button button4;
        private Label label8;
        private ComboBox comboBox4;
    }
}