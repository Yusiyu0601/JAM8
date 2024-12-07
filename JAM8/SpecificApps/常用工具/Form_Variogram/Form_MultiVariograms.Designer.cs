namespace JAM8.SpecificApps.常用工具
{
    partial class Form_MultiVariograms
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
            label2 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            dataGridView1 = new DataGridView();
            listBox1 = new ListBox();
            button2 = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(157, 63);
            button1.TabIndex = 0;
            button1.Text = "打开文件";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(175, 12);
            label1.Name = "label1";
            label1.Size = new Size(78, 24);
            label1.TabIndex = 1;
            label1.Text = "Xs(排序)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(175, 51);
            label2.Name = "label2";
            label2.Size = new Size(77, 24);
            label2.TabIndex = 2;
            label2.Text = "Ys(排序)";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(259, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(478, 30);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(259, 48);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(478, 30);
            textBox2.TabIndex = 4;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(743, 10);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(73, 30);
            numericUpDown1.TabIndex = 5;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(743, 49);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(73, 30);
            numericUpDown2.TabIndex = 6;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(822, 12);
            label3.Name = "label3";
            label3.Size = new Size(46, 24);
            label3.TabIndex = 7;
            label3.Text = "奇数";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(822, 51);
            label4.Name = "label4";
            label4.Size = new Size(46, 24);
            label4.TabIndex = 8;
            label4.Text = "偶数";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 84);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.RowTemplate.Height = 32;
            dataGridView1.Size = new Size(566, 388);
            dataGridView1.TabIndex = 9;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(584, 85);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(292, 340);
            listBox1.TabIndex = 10;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button2
            // 
            button2.Location = new Point(584, 431);
            button2.Name = "button2";
            button2.Size = new Size(292, 41);
            button2.TabIndex = 11;
            button2.Text = "批量计算并保存";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form_MultiVariograms
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(888, 486);
            Controls.Add(button2);
            Controls.Add(listBox1);
            Controls.Add(dataGridView1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_MultiVariograms";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_MultiVariograms";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private TextBox textBox2;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private Label label3;
        private Label label4;
        private DataGridView dataGridView1;
        private ListBox listBox1;
        private Button button2;
    }
}