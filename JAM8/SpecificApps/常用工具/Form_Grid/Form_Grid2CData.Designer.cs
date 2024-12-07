namespace JAM8.SpecificApps.常用工具
{
    partial class Form_Grid2CData
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
            scottplot4GridProperty1 = new Algorithms.Geometry.Scottplot4GridProperty();
            button2 = new Button();
            button1 = new Button();
            listBox1 = new ListBox();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            label1 = new Label();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // scottplot4GridProperty1
            // 
            scottplot4GridProperty1.Location = new Point(215, 5);
            scottplot4GridProperty1.Name = "scottplot4GridProperty1";
            scottplot4GridProperty1.Size = new Size(670, 574);
            scottplot4GridProperty1.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new Point(6, 6);
            button2.Name = "button2";
            button2.Size = new Size(202, 34);
            button2.TabIndex = 24;
            button2.Text = "打开Grid";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(764, 10);
            button1.Name = "button1";
            button1.Size = new Size(102, 34);
            button1.TabIndex = 23;
            button1.Text = "保存cdata";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(6, 47);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(202, 532);
            listBox1.TabIndex = 22;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(526, 5);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(71, 28);
            radioButton1.TabIndex = 28;
            radioButton1.TabStop = true;
            radioButton1.Text = "等于";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(526, 33);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(89, 28);
            radioButton2.TabIndex = 29;
            radioButton2.Text = "不等于";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(623, 15);
            label1.Name = "label1";
            label1.Size = new Size(46, 24);
            label1.TabIndex = 30;
            label1.Text = "数值";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(675, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(83, 30);
            textBox1.TabIndex = 31;
            textBox1.Text = "1";
            // 
            // Form_Grid2CData
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(878, 582);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Controls.Add(scottplot4GridProperty1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_Grid2CData";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_ConvertGridToCData";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Algorithms.Geometry.Scottplot4GridProperty scottplot4GridProperty1;
        private Button button2;
        private Button button1;
        private ListBox listBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private Label label1;
        private TextBox textBox1;
    }
}