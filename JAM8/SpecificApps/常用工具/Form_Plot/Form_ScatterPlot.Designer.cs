namespace JAM8.SpecificApps.常用工具
{
    partial class Form_ScatterPlot
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
            label1 = new Label();
            comboBox1 = new ComboBox();
            label2 = new Label();
            comboBox2 = new ComboBox();
            label3 = new Label();
            comboBox3 = new ComboBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(24, 19);
            button1.Name = "button1";
            button1.Size = new Size(245, 46);
            button1.TabIndex = 0;
            button1.Text = "读取散点文件";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 79);
            label1.Name = "label1";
            label1.Size = new Size(130, 24);
            label1.TabIndex = 2;
            label1.Text = "X轴的序列名称";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(24, 116);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(245, 32);
            comboBox1.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 165);
            label2.Name = "label2";
            label2.Size = new Size(129, 24);
            label2.TabIndex = 4;
            label2.Text = "Y轴的序列名称";
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(24, 203);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(245, 32);
            comboBox2.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 254);
            label3.Name = "label3";
            label3.Size = new Size(136, 24);
            label3.TabIndex = 6;
            label3.Text = "标签的序列名称";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(24, 293);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(245, 32);
            comboBox3.TabIndex = 7;
            // 
            // button2
            // 
            button2.Location = new Point(24, 341);
            button2.Name = "button2";
            button2.Size = new Size(245, 46);
            button2.TabIndex = 8;
            button2.Text = "绘制";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form_ScatterPlot
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(299, 416);
            Controls.Add(button2);
            Controls.Add(comboBox3);
            Controls.Add(label3);
            Controls.Add(comboBox2);
            Controls.Add(label2);
            Controls.Add(comboBox1);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_ScatterPlot";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_ScatterPlot";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private ComboBox comboBox1;
        private Label label2;
        private ComboBox comboBox2;
        private Label label3;
        private ComboBox comboBox3;
        private Button button2;
    }
}