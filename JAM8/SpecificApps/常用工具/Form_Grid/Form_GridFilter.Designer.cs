namespace JAM8.SpecificApps.常用工具
{
    partial class Form_GridFilter
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
            textBox1 = new TextBox();
            label1 = new Label();
            rb_大于等于 = new RadioButton();
            rb_大于 = new RadioButton();
            button2 = new Button();
            button1 = new Button();
            listBox1 = new ListBox();
            scottplot4GridProperty1 = new Algorithms.Geometry.Scottplot4GridProperty();
            rb_小于 = new RadioButton();
            rb_小于等于 = new RadioButton();
            textBox2 = new TextBox();
            label2 = new Label();
            scottplot4GridProperty2 = new Algorithms.Geometry.Scottplot4GridProperty();
            label3 = new Label();
            checkBox1 = new CheckBox();
            button3 = new Button();
            rb_不等于 = new RadioButton();
            rb_等于 = new RadioButton();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(877, 373);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(83, 30);
            textBox1.TabIndex = 39;
            textBox1.Text = "1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(807, 379);
            label1.Name = "label1";
            label1.Size = new Size(46, 24);
            label1.TabIndex = 38;
            label1.Text = "阈值";
            // 
            // rb_大于等于
            // 
            rb_大于等于.AutoSize = true;
            rb_大于等于.Location = new Point(807, 112);
            rb_大于等于.Name = "rb_大于等于";
            rb_大于等于.Size = new Size(107, 28);
            rb_大于等于.TabIndex = 37;
            rb_大于等于.Text = "大于等于";
            rb_大于等于.UseVisualStyleBackColor = true;
            // 
            // rb_大于
            // 
            rb_大于.AutoSize = true;
            rb_大于.Checked = true;
            rb_大于.Location = new Point(807, 70);
            rb_大于.Name = "rb_大于";
            rb_大于.Size = new Size(71, 28);
            rb_大于.TabIndex = 36;
            rb_大于.TabStop = true;
            rb_大于.Text = "大于";
            rb_大于.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(4, 6);
            button2.Name = "button2";
            button2.Size = new Size(202, 34);
            button2.TabIndex = 35;
            button2.Text = "打开Grid";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(1371, 12);
            button1.Name = "button1";
            button1.Size = new Size(165, 34);
            button1.TabIndex = 34;
            button1.Text = "保存过滤后的网格";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(4, 47);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(202, 532);
            listBox1.TabIndex = 33;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // scottplot4GridProperty1
            // 
            scottplot4GridProperty1.Location = new Point(213, 5);
            scottplot4GridProperty1.Name = "scottplot4GridProperty1";
            scottplot4GridProperty1.Size = new Size(588, 574);
            scottplot4GridProperty1.TabIndex = 32;
            // 
            // rb_小于
            // 
            rb_小于.AutoSize = true;
            rb_小于.Location = new Point(807, 154);
            rb_小于.Name = "rb_小于";
            rb_小于.Size = new Size(71, 28);
            rb_小于.TabIndex = 40;
            rb_小于.Text = "小于";
            rb_小于.UseVisualStyleBackColor = true;
            // 
            // rb_小于等于
            // 
            rb_小于等于.AutoSize = true;
            rb_小于等于.Location = new Point(807, 196);
            rb_小于等于.Name = "rb_小于等于";
            rb_小于等于.Size = new Size(107, 28);
            rb_小于等于.TabIndex = 41;
            rb_小于等于.Text = "小于等于";
            rb_小于等于.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(877, 422);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(83, 30);
            textBox2.TabIndex = 43;
            textBox2.Text = "1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(807, 425);
            label2.Name = "label2";
            label2.Size = new Size(64, 24);
            label2.TabIndex = 42;
            label2.Text = "新数值";
            // 
            // scottplot4GridProperty2
            // 
            scottplot4GridProperty2.Location = new Point(966, 5);
            scottplot4GridProperty2.Name = "scottplot4GridProperty2";
            scottplot4GridProperty2.Size = new Size(588, 574);
            scottplot4GridProperty2.TabIndex = 44;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(807, 16);
            label3.Name = "label3";
            label3.Size = new Size(82, 24);
            label3.TabIndex = 45;
            label3.Text = "过滤条件";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(806, 339);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(108, 28);
            checkBox1.TabIndex = 46;
            checkBox1.Text = "持续过滤";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(824, 479);
            button3.Name = "button3";
            button3.Size = new Size(111, 34);
            button3.TabIndex = 47;
            button3.Text = "过滤计算";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // rb_不等于
            // 
            rb_不等于.AutoSize = true;
            rb_不等于.Location = new Point(807, 280);
            rb_不等于.Name = "rb_不等于";
            rb_不等于.Size = new Size(89, 28);
            rb_不等于.TabIndex = 49;
            rb_不等于.Text = "不等于";
            rb_不等于.UseVisualStyleBackColor = true;
            // 
            // rb_等于
            // 
            rb_等于.AutoSize = true;
            rb_等于.Location = new Point(807, 238);
            rb_等于.Name = "rb_等于";
            rb_等于.Size = new Size(71, 28);
            rb_等于.TabIndex = 48;
            rb_等于.Text = "等于";
            rb_等于.UseVisualStyleBackColor = true;
            // 
            // Form_GridFilter
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1548, 582);
            Controls.Add(rb_不等于);
            Controls.Add(rb_等于);
            Controls.Add(button3);
            Controls.Add(checkBox1);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(scottplot4GridProperty2);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(rb_小于等于);
            Controls.Add(rb_小于);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(rb_大于等于);
            Controls.Add(rb_大于);
            Controls.Add(button2);
            Controls.Add(listBox1);
            Controls.Add(scottplot4GridProperty1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_GridFilter";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_GridFilter";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Label label1;
        private RadioButton rb_大于等于;
        private RadioButton rb_大于;
        private Button button2;
        private Button button1;
        private ListBox listBox1;
        private Algorithms.Geometry.Scottplot4GridProperty scottplot4GridProperty1;
        private RadioButton rb_小于;
        private RadioButton rb_小于等于;
        private TextBox textBox2;
        private Label label2;
        private Algorithms.Geometry.Scottplot4GridProperty scottplot4GridProperty2;
        private Label label3;
        private CheckBox checkBox1;
        private Button button3;
        private RadioButton rb_不等于;
        private RadioButton rb_等于;
    }
}