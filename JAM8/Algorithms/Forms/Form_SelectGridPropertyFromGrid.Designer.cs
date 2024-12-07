namespace JAM8.Algorithms.Forms
{
    partial class Form_SelectGridPropertyFromGrid
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
            listBox1 = new ListBox();
            scottplot4GridProperty1 = new Geometry.Scottplot4GridProperty();
            button1 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(12, 12);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(221, 508);
            listBox1.TabIndex = 115;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // scottplot4GridProperty1
            // 
            scottplot4GridProperty1.Location = new Point(240, 12);
            scottplot4GridProperty1.Name = "scottplot4GridProperty1";
            scottplot4GridProperty1.Size = new Size(716, 508);
            scottplot4GridProperty1.TabIndex = 114;
            // 
            // button1
            // 
            button1.Location = new Point(185, 538);
            button1.Name = "button1";
            button1.Size = new Size(189, 34);
            button1.TabIndex = 118;
            button1.Text = "选择GridProperty";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button3
            // 
            button3.Location = new Point(592, 538);
            button3.Name = "button3";
            button3.Size = new Size(189, 34);
            button3.TabIndex = 119;
            button3.Text = "取消";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form_SelectGridPropertyFromGrid
            // 
            AcceptButton = button1;
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(968, 584);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Controls.Add(scottplot4GridProperty1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_SelectGridPropertyFromGrid";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_SelectGridPropertyFromGrid";
            ResumeLayout(false);
        }

        #endregion
        private Geometry.Scottplot4GridProperty scottplot4GridProperty1;
        private Button button1;
        private Button button3;
        public ListBox listBox1;
    }
}