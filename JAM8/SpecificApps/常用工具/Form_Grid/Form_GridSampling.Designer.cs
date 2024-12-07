namespace JAM8.SpecificApps.常用工具
{
    partial class Form_GridSampling
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
            button3 = new Button();
            button1 = new Button();
            scottplot4GridProperty2 = new Algorithms.Geometry.Scottplot4GridProperty();
            textBox1 = new TextBox();
            label1 = new Label();
            button2 = new Button();
            listBox1 = new ListBox();
            scottplot4GridProperty1 = new Algorithms.Geometry.Scottplot4GridProperty();
            checkBox1 = new CheckBox();
            button4 = new Button();
            listBox2 = new ListBox();
            textBox2 = new TextBox();
            label2 = new Label();
            textBox3 = new TextBox();
            label3 = new Label();
            checkBox2 = new CheckBox();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Location = new Point(973, 147);
            button3.Name = "button3";
            button3.Size = new Size(84, 34);
            button3.TabIndex = 80;
            button3.Text = "抽样";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button1
            // 
            button1.Location = new Point(844, 546);
            button1.Name = "button1";
            button1.Size = new Size(119, 34);
            button1.TabIndex = 72;
            button1.Text = "保存(Grid)";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // scottplot4GridProperty2
            // 
            scottplot4GridProperty2.Location = new Point(1072, 7);
            scottplot4GridProperty2.Name = "scottplot4GridProperty2";
            scottplot4GridProperty2.Size = new Size(643, 618);
            scottplot4GridProperty2.TabIndex = 78;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(957, 21);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 30);
            textBox1.TabIndex = 75;
            textBox1.Text = "100";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(850, 24);
            label1.Name = "label1";
            label1.Size = new Size(100, 24);
            label1.TabIndex = 74;
            label1.Text = "每次抽样数";
            // 
            // button2
            // 
            button2.Location = new Point(6, 4);
            button2.Name = "button2";
            button2.Size = new Size(202, 34);
            button2.TabIndex = 73;
            button2.Text = "打开Grid";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(6, 45);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(202, 556);
            listBox1.TabIndex = 71;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // scottplot4GridProperty1
            // 
            scottplot4GridProperty1.Location = new Point(215, 7);
            scottplot4GridProperty1.Name = "scottplot4GridProperty1";
            scottplot4GridProperty1.Size = new Size(633, 620);
            scottplot4GridProperty1.TabIndex = 70;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(855, 151);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(108, 28);
            checkBox1.TabIndex = 81;
            checkBox1.Text = "井柱模式";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(969, 546);
            button4.Name = "button4";
            button4.Size = new Size(119, 34);
            button4.TabIndex = 82;
            button4.Text = "保存(cdata)";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // listBox2
            // 
            listBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox2.FormattingEnabled = true;
            listBox2.ItemHeight = 24;
            listBox2.Location = new Point(855, 189);
            listBox2.Margin = new Padding(4);
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(202, 316);
            listBox2.TabIndex = 83;
            listBox2.SelectedIndexChanged += listBox2_SelectedIndexChanged;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(957, 64);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(100, 30);
            textBox2.TabIndex = 85;
            textBox2.Text = "20";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(850, 67);
            label2.Name = "label2";
            label2.Size = new Size(82, 24);
            label2.TabIndex = 84;
            label2.Text = "抽样次数";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(957, 107);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 30);
            textBox3.TabIndex = 87;
            textBox3.Text = "123123";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(850, 110);
            label3.Name = "label3";
            label3.Size = new Size(82, 24);
            label3.TabIndex = 86;
            label3.Text = "随机种子";
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(855, 512);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(126, 28);
            checkBox2.TabIndex = 88;
            checkBox2.Text = "仅保存选中";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // Form_GridSampling
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1727, 632);
            Controls.Add(checkBox2);
            Controls.Add(textBox3);
            Controls.Add(label3);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(listBox2);
            Controls.Add(button4);
            Controls.Add(checkBox1);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(scottplot4GridProperty2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(listBox1);
            Controls.Add(scottplot4GridProperty1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_GridSampling";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_GridSampling";
            Load += Form_GridSampling_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button3;
        private Button button1;
        private Algorithms.Geometry.Scottplot4GridProperty scottplot4GridProperty2;
        private TextBox textBox1;
        private Label label1;
        private Button button2;
        private ListBox listBox1;
        private Algorithms.Geometry.Scottplot4GridProperty scottplot4GridProperty1;
        private CheckBox checkBox1;
        private Button button4;
        private ListBox listBox2;
        private TextBox textBox2;
        private Label label2;
        private TextBox textBox3;
        private Label label3;
        private CheckBox checkBox2;
    }
}