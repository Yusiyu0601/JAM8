namespace JAM8.Algorithms.Forms
{
    partial class Form_GridEditor
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
            button2 = new Button();
            label3 = new Label();
            button1 = new Button();
            label1 = new Label();
            listBox1 = new ListBox();
            scottplot4GridProperty1 = new Geometry.Scottplot4GridProperty();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            label4 = new Label();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Location = new Point(7, 5);
            button2.Name = "button2";
            button2.Size = new Size(202, 34);
            button2.TabIndex = 21;
            button2.Text = "打开Grid";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(306, 10);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(25, 24);
            label3.TabIndex = 20;
            label3.Text = "N";
            // 
            // button1
            // 
            button1.Location = new Point(107, 45);
            button1.Name = "button1";
            button1.Size = new Size(102, 34);
            button1.TabIndex = 19;
            button1.Text = "另存为";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(216, 10);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(82, 24);
            label1.TabIndex = 18;
            label1.Text = "属性列表";
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(7, 94);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(202, 508);
            listBox1.TabIndex = 17;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // scottplot4GridProperty1
            // 
            scottplot4GridProperty1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            scottplot4GridProperty1.Location = new Point(216, 46);
            scottplot4GridProperty1.Name = "scottplot4GridProperty1";
            scottplot4GridProperty1.Size = new Size(640, 561);
            scottplot4GridProperty1.TabIndex = 22;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(419, 10);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(136, 24);
            label2.TabIndex = 23;
            label2.Text = "设置修改的数值";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(562, 8);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(89, 30);
            numericUpDown1.TabIndex = 24;
            numericUpDown1.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Blue;
            label4.Location = new Point(669, 10);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(168, 24);
            label4.TabIndex = 25;
            label4.Text = "操作: ctrl+鼠标按键";
            // 
            // button3
            // 
            button3.Location = new Point(7, 45);
            button3.Name = "button3";
            button3.Size = new Size(94, 34);
            button3.TabIndex = 26;
            button3.Text = "新建Grid";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form_GridEditor
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(859, 619);
            Controls.Add(button3);
            Controls.Add(label4);
            Controls.Add(numericUpDown1);
            Controls.Add(label2);
            Controls.Add(scottplot4GridProperty1);
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(listBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Form_GridEditor";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_GridEditor";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button2;
        private Label label3;
        private Button button1;
        private Label label1;
        private ListBox listBox1;
        private Geometry.Scottplot4GridProperty scottplot4GridProperty1;
        private Label label2;
        private NumericUpDown numericUpDown1;
        private Label label4;
        private Button button3;
    }
}