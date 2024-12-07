namespace JAM8.SpecificApps.常用工具
{
    partial class Form_SVM
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
            label1 = new Label();
            listBox1 = new ListBox();
            button2 = new Button();
            button3 = new Button();
            label2 = new Label();
            listBox2 = new ListBox();
            label3 = new Label();
            listBox3 = new ListBox();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            label4 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label5 = new Label();
            label6 = new Label();
            button7 = new Button();
            label7 = new Label();
            listBox4 = new ListBox();
            button8 = new Button();
            label8 = new Label();
            listBox5 = new ListBox();
            label9 = new Label();
            dataGridView1 = new DataGridView();
            label10 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(10, 10);
            button1.Name = "button1";
            button1.Size = new Size(202, 36);
            button1.TabIndex = 1;
            button1.Text = "打开训练数据";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 55);
            label1.Name = "label1";
            label1.Size = new Size(118, 24);
            label1.TabIndex = 5;
            label1.Text = "选择序列名称";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(10, 85);
            listBox1.Name = "listBox1";
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            listBox1.Size = new Size(202, 268);
            listBox1.TabIndex = 4;
            // 
            // button2
            // 
            button2.Location = new Point(220, 120);
            button2.Name = "button2";
            button2.Size = new Size(80, 46);
            button2.TabIndex = 6;
            button2.Text = "输入=>";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(220, 280);
            button3.Name = "button3";
            button3.Size = new Size(80, 46);
            button3.TabIndex = 7;
            button3.Text = "输出=>";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(307, 55);
            label2.Name = "label2";
            label2.Size = new Size(118, 24);
            label2.TabIndex = 9;
            label2.Text = "输入序列名称";
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 24;
            listBox2.Location = new Point(307, 85);
            listBox2.Name = "listBox2";
            listBox2.SelectionMode = SelectionMode.MultiExtended;
            listBox2.Size = new Size(202, 124);
            listBox2.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(307, 219);
            label3.Name = "label3";
            label3.Size = new Size(118, 24);
            label3.TabIndex = 11;
            label3.Text = "输出序列名称";
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.ItemHeight = 24;
            listBox3.Location = new Point(307, 253);
            listBox3.Name = "listBox3";
            listBox3.SelectionMode = SelectionMode.MultiExtended;
            listBox3.Size = new Size(202, 100);
            listBox3.TabIndex = 10;
            // 
            // button4
            // 
            button4.Location = new Point(528, 184);
            button4.Name = "button4";
            button4.Size = new Size(192, 40);
            button4.TabIndex = 12;
            button4.Text = "训练(回归模式)";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(1059, 11);
            button5.Name = "button5";
            button5.Size = new Size(192, 35);
            button5.TabIndex = 13;
            button5.Text = "预测";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(528, 234);
            button6.Name = "button6";
            button6.Size = new Size(192, 40);
            button6.TabIndex = 14;
            button6.Text = "训练(分类模式)";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(554, 67);
            label4.Name = "label4";
            label4.Size = new Size(22, 24);
            label4.TabIndex = 15;
            label4.Text = "C";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(611, 64);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(109, 30);
            textBox1.TabIndex = 16;
            textBox1.Text = "2";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(611, 116);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(109, 30);
            textBox2.TabIndex = 18;
            textBox2.Text = "0.5";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(527, 119);
            label5.Name = "label5";
            label5.Size = new Size(77, 24);
            label5.TabIndex = 17;
            label5.Text = "Gamma";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(528, 326);
            label6.Name = "label6";
            label6.Size = new Size(112, 24);
            label6.TabIndex = 19;
            label6.Text = "训练模型(无)";
            // 
            // button7
            // 
            button7.Location = new Point(763, 10);
            button7.Name = "button7";
            button7.Size = new Size(202, 36);
            button7.TabIndex = 20;
            button7.Text = "打开待预测数据";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(1060, 55);
            label7.Name = "label7";
            label7.Size = new Size(118, 24);
            label7.TabIndex = 25;
            label7.Text = "输入序列名称";
            // 
            // listBox4
            // 
            listBox4.FormattingEnabled = true;
            listBox4.ItemHeight = 24;
            listBox4.Location = new Point(763, 85);
            listBox4.Name = "listBox4";
            listBox4.SelectionMode = SelectionMode.MultiExtended;
            listBox4.Size = new Size(202, 268);
            listBox4.TabIndex = 24;
            // 
            // button8
            // 
            button8.Location = new Point(973, 197);
            button8.Name = "button8";
            button8.Size = new Size(80, 46);
            button8.TabIndex = 23;
            button8.Text = "输入=>";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(763, 55);
            label8.Name = "label8";
            label8.Size = new Size(118, 24);
            label8.TabIndex = 22;
            label8.Text = "选择序列名称";
            // 
            // listBox5
            // 
            listBox5.FormattingEnabled = true;
            listBox5.ItemHeight = 24;
            listBox5.Location = new Point(1059, 85);
            listBox5.Name = "listBox5";
            listBox5.SelectionMode = SelectionMode.MultiExtended;
            listBox5.Size = new Size(202, 268);
            listBox5.TabIndex = 21;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.ForeColor = Color.Blue;
            label9.Location = new Point(220, 16);
            label9.Name = "label9";
            label9.Size = new Size(82, 24);
            label9.TabIndex = 50;
            label9.Text = "示例数据";
            label9.Click += label9_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(10, 389);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.RowTemplate.Height = 32;
            dataGridView1.Size = new Size(1254, 153);
            dataGridView1.TabIndex = 51;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(10, 362);
            label10.Name = "label10";
            label10.Size = new Size(82, 24);
            label10.TabIndex = 52;
            label10.Text = "性能参数";
            // 
            // Form_SVM
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1276, 550);
            Controls.Add(label10);
            Controls.Add(dataGridView1);
            Controls.Add(label9);
            Controls.Add(label7);
            Controls.Add(listBox4);
            Controls.Add(button8);
            Controls.Add(label8);
            Controls.Add(listBox5);
            Controls.Add(button7);
            Controls.Add(label6);
            Controls.Add(textBox2);
            Controls.Add(label5);
            Controls.Add(textBox1);
            Controls.Add(label4);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(label3);
            Controls.Add(listBox3);
            Controls.Add(label2);
            Controls.Add(listBox2);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label1);
            Controls.Add(listBox1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_SVM";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_SVM";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private ListBox listBox1;
        private Button button2;
        private Button button3;
        private Label label2;
        private ListBox listBox2;
        private Label label3;
        private ListBox listBox3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Label label4;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label5;
        private Label label6;
        private Button button7;
        private Label label7;
        private ListBox listBox4;
        private Button button8;
        private Label label8;
        private ListBox listBox5;
        private Label label9;
        private DataGridView dataGridView1;
        private Label label10;
    }
}