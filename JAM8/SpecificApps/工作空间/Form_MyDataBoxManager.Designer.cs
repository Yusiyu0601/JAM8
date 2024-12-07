namespace JAM8.SpecificApps.工作空间
{
    partial class Form_MyDataBoxManager
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
            button1 = new Button();
            comboBox1 = new ComboBox();
            listView1 = new ListView();
            header_item_name = new ColumnHeader();
            header_item_typename = new ColumnHeader();
            SuspendLayout();
            // 
            // button2
            // 
            button2.Location = new Point(138, 392);
            button2.Name = "button2";
            button2.Size = new Size(120, 37);
            button2.TabIndex = 2;
            button2.Text = "操作";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 392);
            button1.Name = "button1";
            button1.Size = new Size(120, 37);
            button1.TabIndex = 6;
            button1.Text = "预览";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Grid", "Other" });
            comboBox1.Location = new Point(264, 395);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(111, 32);
            comboBox1.TabIndex = 7;
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { header_item_name, header_item_typename });
            listView1.Dock = DockStyle.Top;
            listView1.FullRowSelect = true;
            listView1.Location = new Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new Size(505, 386);
            listView1.TabIndex = 8;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            // 
            // header_item_name
            // 
            header_item_name.Text = "名称";
            header_item_name.Width = 300;
            // 
            // header_item_typename
            // 
            header_item_typename.Text = "类型";
            header_item_typename.Width = 260;
            // 
            // Form_MyDataBoxManager
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(505, 437);
            Controls.Add(listView1);
            Controls.Add(comboBox1);
            Controls.Add(button1);
            Controls.Add(button2);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_MyDataBoxManager";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MyDataBoxManager";
            Load += Form_MyDataBoxManager_Load;
            ResumeLayout(false);
        }

        #endregion
        private Button button2;
        private Button button1;
        private ComboBox comboBox1;
        private ColumnHeader header_item_name;
        private ColumnHeader header_item_typename;
        public ListView listView1;
    }
}