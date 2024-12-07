namespace JAM8.SpecificApps.常用工具
{
    partial class Form_TrainTestSplitData
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
            textBox1 = new TextBox();
            label2 = new Label();
            textBox2 = new TextBox();
            checkBox1 = new CheckBox();
            label3 = new Label();
            comboBox1 = new ComboBox();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(59, 12);
            button1.Name = "button1";
            button1.Size = new Size(165, 36);
            button1.TabIndex = 27;
            button1.Text = "打开数据集";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 70);
            label1.Name = "label1";
            label1.Size = new Size(160, 24);
            label1.TabIndex = 28;
            label1.Text = "训练集的占比(0-1)";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(174, 67);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(105, 30);
            textBox1.TabIndex = 29;
            textBox1.Text = "0.8";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 112);
            label2.Name = "label2";
            label2.Size = new Size(160, 24);
            label2.TabIndex = 30;
            label2.Text = "测试集的占比(0-1)";
            // 
            // textBox2
            // 
            textBox2.Enabled = false;
            textBox2.Location = new Point(174, 109);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(105, 30);
            textBox2.TabIndex = 31;
            textBox2.Text = "0.2";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(8, 164);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(216, 28);
            checkBox1.TabIndex = 32;
            checkBox1.Text = "保持输出序列类别比例";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 211);
            label3.Name = "label3";
            label3.Size = new Size(154, 24);
            label3.TabIndex = 33;
            label3.Text = "输出类别序列名称";
            // 
            // comboBox1
            // 
            comboBox1.Enabled = false;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(174, 208);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(105, 32);
            comboBox1.TabIndex = 34;
            // 
            // button2
            // 
            button2.Location = new Point(59, 249);
            button2.Name = "button2";
            button2.Size = new Size(165, 36);
            button2.TabIndex = 35;
            button2.Text = "Split";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form_TrainTestSplitData
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(291, 297);
            Controls.Add(button2);
            Controls.Add(comboBox1);
            Controls.Add(label3);
            Controls.Add(checkBox1);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_TrainTestSplitData";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_TrainTestSplitData";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private CheckBox checkBox1;
        private Label label3;
        private ComboBox comboBox1;
        private Button button2;
    }
}