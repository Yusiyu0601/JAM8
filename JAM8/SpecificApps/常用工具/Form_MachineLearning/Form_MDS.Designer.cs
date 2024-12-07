namespace JAM8.SpecificApps.常用工具
{
    partial class Form_MDS
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
            textBox1 = new TextBox();
            label1 = new Label();
            button4 = new Button();
            listBox1 = new ListBox();
            button5 = new Button();
            label2 = new Label();
            dataGridView1 = new DataGridView();
            label3 = new Label();
            checkBox2 = new CheckBox();
            comboBox1 = new ComboBox();
            label4 = new Label();
            comboBox2 = new ComboBox();
            label9 = new Label();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(16, 10);
            button1.Name = "button1";
            button1.Size = new Size(182, 47);
            button1.TabIndex = 0;
            button1.Text = "读取距离矩阵";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(429, 10);
            button2.Name = "button2";
            button2.Size = new Size(182, 47);
            button2.TabIndex = 2;
            button2.Text = "MDS";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(16, 95);
            button3.Name = "button3";
            button3.Size = new Size(182, 47);
            button3.TabIndex = 3;
            button3.Text = "读取记录表";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(321, 18);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(99, 30);
            textBox1.TabIndex = 4;
            textBox1.Text = "2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(233, 21);
            label1.Name = "label1";
            label1.Size = new Size(82, 24);
            label1.TabIndex = 5;
            label1.Text = "目标维度";
            // 
            // button4
            // 
            button4.Location = new Point(622, 10);
            button4.Name = "button4";
            button4.Size = new Size(182, 47);
            button4.TabIndex = 6;
            button4.Text = "MDS(2d)并显示";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(16, 207);
            listBox1.Name = "listBox1";
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            listBox1.Size = new Size(182, 148);
            listBox1.TabIndex = 7;
            // 
            // button5
            // 
            button5.Location = new Point(16, 463);
            button5.Name = "button5";
            button5.Size = new Size(182, 52);
            button5.TabIndex = 8;
            button5.Text = "计算距离矩阵";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 180);
            label2.Name = "label2";
            label2.Size = new Size(208, 24);
            label2.TabIndex = 9;
            label2.Text = "选择用于距离计算的序列";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(222, 90);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.RowTemplate.Height = 32;
            dataGridView1.Size = new Size(587, 427);
            dataGridView1.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(222, 61);
            label3.Name = "label3";
            label3.Size = new Size(82, 24);
            label3.TabIndex = 12;
            label3.Text = "距离矩阵";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(16, 373);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(108, 28);
            checkBox2.TabIndex = 13;
            checkBox2.Text = "记录名称";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // comboBox1
            // 
            comboBox1.Enabled = false;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(130, 371);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(68, 32);
            comboBox1.TabIndex = 14;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 422);
            label4.Name = "label4";
            label4.Size = new Size(82, 24);
            label4.TabIndex = 15;
            label4.Text = "距离类型";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(130, 419);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(68, 32);
            comboBox2.TabIndex = 16;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Blue;
            label9.Location = new Point(116, 64);
            label9.Name = "label9";
            label9.Size = new Size(82, 24);
            label9.TabIndex = 51;
            label9.Text = "示例数据";
            label9.Click += label9_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Blue;
            label5.Location = new Point(116, 149);
            label5.Name = "label5";
            label5.Size = new Size(82, 24);
            label5.TabIndex = 52;
            label5.Text = "示例数据";
            label5.Click += label5_Click;
            // 
            // Form_MDS
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(827, 530);
            Controls.Add(label5);
            Controls.Add(label9);
            Controls.Add(comboBox2);
            Controls.Add(label4);
            Controls.Add(comboBox1);
            Controls.Add(checkBox2);
            Controls.Add(label3);
            Controls.Add(dataGridView1);
            Controls.Add(label2);
            Controls.Add(button5);
            Controls.Add(listBox1);
            Controls.Add(button4);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_MDS";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_MDS";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private TextBox textBox1;
        private Label label1;
        private Button button4;
        private ListBox listBox1;
        private Button button5;
        private Label label2;
        private DataGridView dataGridView1;
        private Label label3;
        private CheckBox checkBox2;
        private ComboBox comboBox1;
        private Label label4;
        private ComboBox comboBox2;
        private Label label9;
        private Label label5;
    }
}