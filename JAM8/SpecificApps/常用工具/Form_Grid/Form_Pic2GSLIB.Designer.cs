
namespace JAM8.SpecificApps.常用工具
{
    partial class Form_Pic2GSLIB
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
            pictureBox1 = new PictureBox();
            button3 = new Button();
            button4 = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            button7 = new Button();
            scottplot4Grid1 = new Algorithms.Geometry.Scottplot4Grid();
            button6 = new Button();
            button5 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(10, 84);
            button1.Margin = new Padding(5, 7, 5, 7);
            button1.Name = "button1";
            button1.Size = new Size(205, 44);
            button1.TabIndex = 6;
            button1.Text = "离散型_转换为GSLIB";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(10, 14);
            button2.Margin = new Padding(5, 7, 5, 7);
            button2.Name = "button2";
            button2.Size = new Size(205, 44);
            button2.TabIndex = 7;
            button2.Text = "打开图像";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(234, 14);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(863, 590);
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // button3
            // 
            button3.Enabled = false;
            button3.Location = new Point(10, 157);
            button3.Margin = new Padding(5, 7, 5, 7);
            button3.Name = "button3";
            button3.Size = new Size(205, 44);
            button3.TabIndex = 9;
            button3.Text = "连续型_转换为GSLIB";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Enabled = false;
            button4.Location = new Point(10, 230);
            button4.Margin = new Padding(5, 7, 5, 7);
            button4.Name = "button4";
            button4.Size = new Size(205, 44);
            button4.TabIndex = 10;
            button4.Text = "RGB通道->MultiGrid";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1114, 648);
            tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(pictureBox1);
            tabPage1.Controls.Add(button4);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(button1);
            tabPage1.Location = new Point(4, 33);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1106, 611);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Image to GSLIB";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button7);
            tabPage2.Controls.Add(scottplot4Grid1);
            tabPage2.Controls.Add(button6);
            tabPage2.Controls.Add(button5);
            tabPage2.Location = new Point(4, 33);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1106, 611);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "GSLIB to Image";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(18, 138);
            button7.Name = "button7";
            button7.Size = new Size(177, 40);
            button7.TabIndex = 5;
            button7.Text = "保存图片";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // scottplot4Grid1
            // 
            scottplot4Grid1.Location = new Point(217, 6);
            scottplot4Grid1.Name = "scottplot4Grid1";
            scottplot4Grid1.Size = new Size(881, 597);
            scottplot4Grid1.TabIndex = 4;
            // 
            // button6
            // 
            button6.Location = new Point(18, 76);
            button6.Name = "button6";
            button6.Size = new Size(177, 40);
            button6.TabIndex = 3;
            button6.Text = "保存图片";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Location = new Point(18, 17);
            button5.Name = "button5";
            button5.Size = new Size(177, 40);
            button5.TabIndex = 0;
            button5.Text = "打开Grid文件";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // Form_Pic2GSLIB
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1114, 648);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "Form_Pic2GSLIB";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_Pic2GSLIB";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button3;
        private Button button4;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button5;
        private Button button6;
        private Algorithms.Geometry.Scottplot4Grid scottplot4Grid1;
        private Button button7;
    }
}