namespace JAM8.Algorithms.Forms
{
    partial class Form_GridCatalog
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
            scottplot4Grid1 = new Geometry.Scottplot4Grid();
            button1 = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            listView1 = new ListView();
            SuspendLayout();
            // 
            // scottplot4Grid1
            // 
            scottplot4Grid1.Location = new Point(342, 71);
            scottplot4Grid1.Name = "scottplot4Grid1";
            scottplot4Grid1.Size = new Size(964, 564);
            scottplot4Grid1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(7, 5);
            button1.Name = "button1";
            button1.Size = new Size(165, 60);
            button1.TabIndex = 1;
            button1.Text = "打开GridCatalog文件";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(171, 5);
            button2.Name = "button2";
            button2.Size = new Size(165, 60);
            button2.TabIndex = 2;
            button2.Text = "新建GridCatalog文件";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(342, 5);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(964, 60);
            textBox1.TabIndex = 4;
            // 
            // button3
            // 
            button3.Location = new Point(6, 641);
            button3.Name = "button3";
            button3.Size = new Size(161, 60);
            button3.TabIndex = 5;
            button3.Text = "添加Grid";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(171, 641);
            button4.Name = "button4";
            button4.Size = new Size(161, 60);
            button4.TabIndex = 6;
            button4.Text = "删除Grid";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button5
            // 
            button5.Location = new Point(576, 641);
            button5.Name = "button5";
            button5.Size = new Size(167, 60);
            button5.TabIndex = 7;
            button5.Text = "选择Grid";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(898, 641);
            button6.Name = "button6";
            button6.Size = new Size(167, 60);
            button6.TabIndex = 8;
            button6.Text = "选择GridProperty";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // listView1
            // 
            listView1.Location = new Point(7, 71);
            listView1.Name = "listView1";
            listView1.Size = new Size(329, 564);
            listView1.TabIndex = 10;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            // 
            // Form_GridCatalog
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1318, 708);
            Controls.Add(listView1);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(scottplot4Grid1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_GridCatalog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_GridCatalog";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Geometry.Scottplot4Grid scottplot4Grid1;
        private Button button1;
        private Button button2;
        private TextBox textBox1;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private ListView listView1;
    }
}