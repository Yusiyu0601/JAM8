namespace JAM8.Algorithms.Forms
{
    partial class Form_VariogramFit
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
            dataGridView1 = new DataGridView();
            formsPlot1 = new ScottPlot.FormsPlot();
            label1 = new Label();
            label2 = new Label();
            label4 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label3 = new Label();
            textBox3 = new TextBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            numericUpDown3 = new NumericUpDown();
            button1 = new Button();
            button2 = new Button();
            label13 = new Label();
            comboBox1 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(15, 75);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.Size = new Size(487, 249);
            dataGridView1.TabIndex = 0;
            dataGridView1.RowPostPaint += dataGridView1_RowPostPaint;
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(508, 39);
            formsPlot1.Margin = new Padding(6, 5, 6, 5);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(680, 522);
            formsPlot1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 48);
            label1.Name = "label1";
            label1.Size = new Size(118, 24);
            label1.TabIndex = 2;
            label1.Text = "实验变差函数";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(785, 31);
            label2.Name = "label2";
            label2.Size = new Size(118, 24);
            label2.TabIndex = 3;
            label2.Text = "变差函数拟合";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(19, 375);
            label4.Name = "label4";
            label4.Size = new Size(46, 24);
            label4.TabIndex = 11;
            label4.Text = "变程";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(82, 372);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(111, 30);
            textBox1.TabIndex = 14;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(82, 421);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(111, 30);
            textBox2.TabIndex = 16;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(19, 424);
            label3.Name = "label3";
            label3.Size = new Size(46, 24);
            label3.TabIndex = 15;
            label3.Text = "基台";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(82, 471);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(111, 30);
            textBox3.TabIndex = 18;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(19, 474);
            label5.Name = "label5";
            label5.Size = new Size(46, 24);
            label5.TabIndex = 17;
            label5.Text = "块金";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(98, 337);
            label6.Name = "label6";
            label6.Size = new Size(82, 24);
            label6.TabIndex = 19;
            label6.Text = "自动拟合";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(345, 337);
            label7.Name = "label7";
            label7.Size = new Size(46, 24);
            label7.TabIndex = 20;
            label7.Text = "调节";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(268, 372);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(111, 30);
            textBox4.TabIndex = 23;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(268, 421);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(111, 30);
            textBox5.TabIndex = 22;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(268, 471);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(111, 30);
            textBox6.TabIndex = 21;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(216, 474);
            label8.Name = "label8";
            label8.Size = new Size(46, 24);
            label8.TabIndex = 29;
            label8.Text = "块金";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(216, 424);
            label9.Name = "label9";
            label9.Size = new Size(46, 24);
            label9.TabIndex = 28;
            label9.Text = "基台";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(216, 375);
            label10.Name = "label10";
            label10.Size = new Size(46, 24);
            label10.TabIndex = 27;
            label10.Text = "变程";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(385, 372);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(114, 30);
            numericUpDown1.TabIndex = 30;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(385, 422);
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(114, 30);
            numericUpDown2.TabIndex = 31;
            numericUpDown2.ValueChanged += numericUpDown2_ValueChanged;
            // 
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(385, 472);
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(114, 30);
            numericUpDown3.TabIndex = 32;
            numericUpDown3.ValueChanged += numericUpDown3_ValueChanged;
            // 
            // button1
            // 
            button1.Location = new Point(157, 12);
            button1.Name = "button1";
            button1.Size = new Size(174, 36);
            button1.TabIndex = 33;
            button1.Text = "打开(实验变差函数)";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.ForeColor = Color.Blue;
            button2.Location = new Point(314, 517);
            button2.Name = "button2";
            button2.Size = new Size(188, 36);
            button2.TabIndex = 34;
            button2.Text = "Demo数据模板";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(0, 520);
            label13.Name = "label13";
            label13.Size = new Size(82, 24);
            label13.TabIndex = 72;
            label13.Text = "模型类型";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "球状模型", "指数模型", "高斯模型" });
            comboBox1.Location = new Point(82, 517);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(111, 32);
            comboBox1.TabIndex = 71;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // Form_VariogramFit
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1192, 561);
            Controls.Add(label13);
            Controls.Add(comboBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(numericUpDown3);
            Controls.Add(numericUpDown2);
            Controls.Add(numericUpDown1);
            Controls.Add(label8);
            Controls.Add(label9);
            Controls.Add(label10);
            Controls.Add(textBox4);
            Controls.Add(textBox5);
            Controls.Add(textBox6);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(textBox3);
            Controls.Add(label5);
            Controls.Add(textBox2);
            Controls.Add(label3);
            Controls.Add(textBox1);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(formsPlot1);
            Controls.Add(dataGridView1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form_VariogramFit";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "变差函数拟合";
            Load += Form_VariogramFit_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private ScottPlot.FormsPlot formsPlot1;
        private Label label1;
        private Label label2;
        private Label label4;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label3;
        private TextBox textBox3;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label label8;
        private Label label9;
        private Label label10;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private Button button1;
        private Button button2;
        private Label label13;
        private ComboBox comboBox1;
    }
}