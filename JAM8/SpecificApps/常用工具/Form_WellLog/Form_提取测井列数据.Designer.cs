namespace JAM8.SpecificApps.常用工具
{
    partial class Form_提取测井列数据
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
            listBox1 = new ListBox();
            dataGridView1 = new DataGridView();
            listBox2 = new ListBox();
            label3 = new Label();
            label4 = new Label();
            listBox3 = new ListBox();
            label5 = new Label();
            button3 = new Button();
            checkBox1 = new CheckBox();
            button4 = new Button();
            button5 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(11, 12);
            button1.Name = "button1";
            button1.Size = new Size(180, 42);
            button1.TabIndex = 0;
            button1.Text = "打开测井文件(txt)";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(313, 12);
            button2.Name = "button2";
            button2.Size = new Size(210, 42);
            button2.TabIndex = 1;
            button2.Text = "打开测井文件(excel)";
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Blue;
            label1.Location = new Point(197, 30);
            label1.Name = "label1";
            label1.Size = new Size(82, 24);
            label1.TabIndex = 2;
            label1.Text = "示例数据";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Blue;
            label2.Location = new Point(529, 30);
            label2.Name = "label2";
            label2.Size = new Size(82, 24);
            label2.TabIndex = 3;
            label2.Text = "示例数据";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(11, 89);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(245, 148);
            listBox1.TabIndex = 4;
            listBox1.MouseClick += listBox1_MouseClick;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(269, 64);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(938, 591);
            dataGridView1.TabIndex = 5;
            dataGridView1.RowPostPaint += dataGridView1_RowPostPaint;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 24;
            listBox2.Location = new Point(11, 271);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(245, 124);
            listBox2.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 62);
            label3.Name = "label3";
            label3.Size = new Size(82, 24);
            label3.TabIndex = 7;
            label3.Text = "井名列表";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(11, 244);
            label4.Name = "label4";
            label4.Size = new Size(82, 24);
            label4.TabIndex = 8;
            label4.Text = "属性列表";
            // 
            // listBox3
            // 
            listBox3.FormattingEnabled = true;
            listBox3.ItemHeight = 24;
            listBox3.Location = new Point(11, 478);
            listBox3.Name = "listBox3";
            listBox3.Size = new Size(245, 124);
            listBox3.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 451);
            label5.Name = "label5";
            label5.Size = new Size(172, 24);
            label5.TabIndex = 10;
            label5.Text = "导出选择的属性列表";
            // 
            // button3
            // 
            button3.Location = new Point(124, 613);
            button3.Name = "button3";
            button3.Size = new Size(121, 42);
            button3.TabIndex = 11;
            button3.Text = "导出为excel";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 621);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(90, 28);
            checkBox1.TabIndex = 12;
            checkBox1.Text = "所有井";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(12, 401);
            button4.Name = "button4";
            button4.Size = new Size(121, 42);
            button4.TabIndex = 13;
            button4.Text = "选取>>";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(135, 401);
            button5.Name = "button5";
            button5.Size = new Size(121, 42);
            button5.TabIndex = 14;
            button5.Text = "撤销<<";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form_提取测井列数据
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1219, 666);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(checkBox1);
            Controls.Add(button3);
            Controls.Add(label5);
            Controls.Add(listBox3);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(listBox2);
            Controls.Add(dataGridView1);
            Controls.Add(listBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_提取测井列数据";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "提取测井列数据";
            Load += Form_WellLog_txt2excel_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
        private ListBox listBox1;
        private DataGridView dataGridView1;
        private ListBox listBox2;
        private Label label3;
        private Label label4;
        private ListBox listBox3;
        private Label label5;
        private Button button3;
        private CheckBox checkBox1;
        private Button button4;
        private Button button5;
    }
}