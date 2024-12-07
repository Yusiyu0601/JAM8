namespace JAM8.Work
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPage_常用工具 = new TabPage();
            listBox_常用工具 = new ListBox();
            tabPage_建模方法 = new TabPage();
            listBox_建模方法 = new ListBox();
            tabPage_研究方法 = new TabPage();
            listBox_研究方法 = new ListBox();
            tabPage_模块测试 = new TabPage();
            listBox_测试 = new ListBox();
            button_run = new Button();
            button2 = new Button();
            textBox1 = new TextBox();
            tabControl1.SuspendLayout();
            tabPage_常用工具.SuspendLayout();
            tabPage_建模方法.SuspendLayout();
            tabPage_研究方法.SuspendLayout();
            tabPage_模块测试.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage_常用工具);
            tabControl1.Controls.Add(tabPage_建模方法);
            tabControl1.Controls.Add(tabPage_研究方法);
            tabControl1.Controls.Add(tabPage_模块测试);
            tabControl1.Dock = DockStyle.Bottom;
            tabControl1.Location = new Point(0, 65);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(686, 300);
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage_常用工具
            // 
            tabPage_常用工具.Controls.Add(listBox_常用工具);
            tabPage_常用工具.Location = new Point(4, 33);
            tabPage_常用工具.Name = "tabPage_常用工具";
            tabPage_常用工具.Padding = new Padding(3);
            tabPage_常用工具.Size = new Size(678, 263);
            tabPage_常用工具.TabIndex = 1;
            tabPage_常用工具.Text = "常用工具";
            tabPage_常用工具.UseVisualStyleBackColor = true;
            // 
            // listBox_常用工具
            // 
            listBox_常用工具.Dock = DockStyle.Fill;
            listBox_常用工具.FormattingEnabled = true;
            listBox_常用工具.ItemHeight = 24;
            listBox_常用工具.Location = new Point(3, 3);
            listBox_常用工具.Name = "listBox_常用工具";
            listBox_常用工具.Size = new Size(672, 257);
            listBox_常用工具.TabIndex = 0;
            listBox_常用工具.SelectedIndexChanged += listBox_常用工具_SelectedIndexChanged;
            // 
            // tabPage_建模方法
            // 
            tabPage_建模方法.Controls.Add(listBox_建模方法);
            tabPage_建模方法.Location = new Point(4, 33);
            tabPage_建模方法.Name = "tabPage_建模方法";
            tabPage_建模方法.Padding = new Padding(3);
            tabPage_建模方法.Size = new Size(678, 263);
            tabPage_建模方法.TabIndex = 0;
            tabPage_建模方法.Text = "建模方法";
            tabPage_建模方法.UseVisualStyleBackColor = true;
            // 
            // listBox_建模方法
            // 
            listBox_建模方法.Dock = DockStyle.Fill;
            listBox_建模方法.FormattingEnabled = true;
            listBox_建模方法.ItemHeight = 24;
            listBox_建模方法.Location = new Point(3, 3);
            listBox_建模方法.Name = "listBox_建模方法";
            listBox_建模方法.Size = new Size(672, 257);
            listBox_建模方法.TabIndex = 0;
            listBox_建模方法.SelectedIndexChanged += listBox_建模方法_SelectedIndexChanged;
            // 
            // tabPage_研究方法
            // 
            tabPage_研究方法.Controls.Add(listBox_研究方法);
            tabPage_研究方法.Location = new Point(4, 33);
            tabPage_研究方法.Name = "tabPage_研究方法";
            tabPage_研究方法.Size = new Size(678, 263);
            tabPage_研究方法.TabIndex = 3;
            tabPage_研究方法.Text = "研究方法";
            tabPage_研究方法.UseVisualStyleBackColor = true;
            // 
            // listBox_研究方法
            // 
            listBox_研究方法.Dock = DockStyle.Fill;
            listBox_研究方法.FormattingEnabled = true;
            listBox_研究方法.ItemHeight = 24;
            listBox_研究方法.Location = new Point(0, 0);
            listBox_研究方法.Name = "listBox_研究方法";
            listBox_研究方法.Size = new Size(678, 263);
            listBox_研究方法.TabIndex = 0;
            listBox_研究方法.SelectedIndexChanged += listBox_研究方法_SelectedIndexChanged;
            // 
            // tabPage_模块测试
            // 
            tabPage_模块测试.Controls.Add(listBox_测试);
            tabPage_模块测试.Location = new Point(4, 33);
            tabPage_模块测试.Name = "tabPage_模块测试";
            tabPage_模块测试.Padding = new Padding(3);
            tabPage_模块测试.Size = new Size(678, 263);
            tabPage_模块测试.TabIndex = 2;
            tabPage_模块测试.Text = "模块测试";
            tabPage_模块测试.UseVisualStyleBackColor = true;
            // 
            // listBox_测试
            // 
            listBox_测试.Dock = DockStyle.Fill;
            listBox_测试.FormattingEnabled = true;
            listBox_测试.ItemHeight = 24;
            listBox_测试.Location = new Point(3, 3);
            listBox_测试.Name = "listBox_测试";
            listBox_测试.Size = new Size(672, 257);
            listBox_测试.TabIndex = 0;
            listBox_测试.SelectedIndexChanged += listBox_测试_SelectedIndexChanged;
            // 
            // button_run
            // 
            button_run.Location = new Point(4, 6);
            button_run.Name = "button_run";
            button_run.Size = new Size(105, 57);
            button_run.TabIndex = 1;
            button_run.Text = "执行";
            button_run.UseVisualStyleBackColor = true;
            button_run.Click += button_run_Click;
            // 
            // button2
            // 
            button2.Location = new Point(497, 13);
            button2.Name = "button2";
            button2.Size = new Size(133, 42);
            button2.TabIndex = 2;
            button2.Text = "调用外部程序";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.ForeColor = Color.Blue;
            textBox1.Location = new Point(115, 6);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(566, 57);
            textBox1.TabIndex = 3;
            textBox1.Text = "选中任务的功能";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(686, 365);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(button_run);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "地质建模工具箱(2024 V2)";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            tabControl1.ResumeLayout(false);
            tabPage_常用工具.ResumeLayout(false);
            tabPage_建模方法.ResumeLayout(false);
            tabPage_研究方法.ResumeLayout(false);
            tabPage_模块测试.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage_建模方法;
        private TabPage tabPage_常用工具;
        private Button button_run;
        private Button button2;
        private TextBox textBox1;
        private ListBox listBox_常用工具;
        private ListBox listBox_建模方法;
        private TabPage tabPage_模块测试;
        private ListBox listBox_测试;
        private TabPage tabPage_研究方法;
        private ListBox listBox_研究方法;
    }
}