namespace JAM8.Algorithms.Forms
{
    partial class Form_SelectPropertyFromCData
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
            listBox1 = new ListBox();
            scottplot4GridProperty1 = new Geometry.Scottplot4GridProperty();
            SuspendLayout();
            // 
            // button3
            // 
            button3.Location = new Point(593, 539);
            button3.Name = "button3";
            button3.Size = new Size(189, 34);
            button3.TabIndex = 123;
            button3.Text = "取消";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button1
            // 
            button1.Location = new Point(209, 539);
            button1.Name = "button1";
            button1.Size = new Size(189, 34);
            button1.TabIndex = 122;
            button1.Text = "选择cdata property";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 24;
            listBox1.Location = new Point(13, 13);
            listBox1.Margin = new Padding(4);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(221, 508);
            listBox1.TabIndex = 121;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // scottplot4GridProperty1
            // 
            scottplot4GridProperty1.Location = new Point(241, 13);
            scottplot4GridProperty1.Name = "scottplot4GridProperty1";
            scottplot4GridProperty1.Size = new Size(716, 508);
            scottplot4GridProperty1.TabIndex = 120;
            // 
            // Form_SelectPropertyFromCData
            // 
            AcceptButton = button1;
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(969, 582);
            Controls.Add(button3);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Controls.Add(scottplot4GridProperty1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_SelectPropertyFromCData";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_SelectPropertyFromCData";
            ResumeLayout(false);
        }

        #endregion

        private Button button3;
        private Button button1;
        public ListBox listBox1;
        private Geometry.Scottplot4GridProperty scottplot4GridProperty1;
    }
}