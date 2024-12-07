namespace JAM8.SpecificApps.常用工具
{
    partial class Form_KMeans
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
            listBox1 = new ListBox();
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(241, 46);
            button1.TabIndex = 0;
            button1.Text = "打开记录表";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 407);
            button2.Name = "button2";
            button2.Size = new Size(241, 46);
            button2.TabIndex = 1;
            button2.Text = "聚类计算";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(12, 88);
            listBox1.Name = "listBox1";
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            listBox1.Size = new Size(241, 268);
            listBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 61);
            label1.Name = "label1";
            label1.Size = new Size(118, 24);
            label1.TabIndex = 3;
            label1.Text = "选择序列名称";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 369);
            label2.Name = "label2";
            label2.Size = new Size(118, 24);
            label2.TabIndex = 4;
            label2.Text = "N_clustering";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(136, 366);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(117, 30);
            textBox1.TabIndex = 5;
            textBox1.Text = "2";
            // 
            // Form_KMeans
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(265, 463);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_KMeans";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_KMeans";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private ListBox listBox1;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
    }
}