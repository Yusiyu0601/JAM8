using System.Drawing;
using System.Windows.Forms;

namespace JAM8.Algorithms.Geometry
{
    partial class Form_ReadGridFromGSLIB
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
            label13 = new Label();
            txt_ValueOfNull = new TextBox();
            label12 = new Label();
            btn_OpenFile = new Button();
            txt_FileName = new TextBox();
            txt_OriginCellZ = new TextBox();
            txt_OriginCellY = new TextBox();
            txt_OriginCellX = new TextBox();
            txt_KCount = new TextBox();
            txt_JCount = new TextBox();
            txt_ICount = new TextBox();
            txt_KSize = new TextBox();
            txt_JSize = new TextBox();
            txt_ISize = new TextBox();
            txt_GridName = new TextBox();
            btn_Cancel = new Button();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            btn_OK = new Button();
            textBox1 = new TextBox();
            button2 = new Button();
            label11 = new Label();
            comboBox1 = new ComboBox();
            SuspendLayout();
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(475, 19);
            label13.Margin = new Padding(6, 0, 6, 0);
            label13.Name = "label13";
            label13.Size = new Size(148, 24);
            label13.TabIndex = 137;
            label13.Text = "preview content";
            // 
            // txt_ValueOfNull
            // 
            txt_ValueOfNull.Location = new Point(123, 100);
            txt_ValueOfNull.Margin = new Padding(6, 5, 6, 5);
            txt_ValueOfNull.Name = "txt_ValueOfNull";
            txt_ValueOfNull.Size = new Size(340, 30);
            txt_ValueOfNull.TabIndex = 136;
            txt_ValueOfNull.Text = "-99";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(21, 102);
            label12.Margin = new Padding(6, 0, 6, 0);
            label12.Name = "label12";
            label12.Size = new Size(90, 24);
            label12.TabIndex = 135;
            label12.Text = "nullValue";
            // 
            // btn_OpenFile
            // 
            btn_OpenFile.Location = new Point(15, 13);
            btn_OpenFile.Margin = new Padding(6, 5, 6, 5);
            btn_OpenFile.Name = "btn_OpenFile";
            btn_OpenFile.Size = new Size(103, 37);
            btn_OpenFile.TabIndex = 134;
            btn_OpenFile.Text = "Open File";
            btn_OpenFile.UseVisualStyleBackColor = true;
            btn_OpenFile.Click += btn_OpenFile_Click;
            // 
            // txt_FileName
            // 
            txt_FileName.Location = new Point(124, 15);
            txt_FileName.Margin = new Padding(6, 5, 6, 5);
            txt_FileName.Name = "txt_FileName";
            txt_FileName.Size = new Size(340, 30);
            txt_FileName.TabIndex = 133;
            txt_FileName.Text = "file_name";
            // 
            // txt_OriginCellZ
            // 
            txt_OriginCellZ.Location = new Point(124, 528);
            txt_OriginCellZ.Margin = new Padding(6, 5, 6, 5);
            txt_OriginCellZ.Name = "txt_OriginCellZ";
            txt_OriginCellZ.Size = new Size(340, 30);
            txt_OriginCellZ.TabIndex = 132;
            txt_OriginCellZ.Text = "0.5";
            // 
            // txt_OriginCellY
            // 
            txt_OriginCellY.Location = new Point(124, 494);
            txt_OriginCellY.Margin = new Padding(6, 5, 6, 5);
            txt_OriginCellY.Name = "txt_OriginCellY";
            txt_OriginCellY.Size = new Size(340, 30);
            txt_OriginCellY.TabIndex = 131;
            txt_OriginCellY.Text = "0.5";
            // 
            // txt_OriginCellX
            // 
            txt_OriginCellX.Location = new Point(124, 459);
            txt_OriginCellX.Margin = new Padding(6, 5, 6, 5);
            txt_OriginCellX.Name = "txt_OriginCellX";
            txt_OriginCellX.Size = new Size(340, 30);
            txt_OriginCellX.TabIndex = 130;
            txt_OriginCellX.Text = "0.5";
            // 
            // txt_KCount
            // 
            txt_KCount.Location = new Point(124, 276);
            txt_KCount.Margin = new Padding(6, 5, 6, 5);
            txt_KCount.Name = "txt_KCount";
            txt_KCount.Size = new Size(340, 30);
            txt_KCount.TabIndex = 129;
            txt_KCount.Text = "1";
            // 
            // txt_JCount
            // 
            txt_JCount.Location = new Point(124, 240);
            txt_JCount.Margin = new Padding(6, 5, 6, 5);
            txt_JCount.Name = "txt_JCount";
            txt_JCount.Size = new Size(340, 30);
            txt_JCount.TabIndex = 128;
            txt_JCount.Text = "1";
            // 
            // txt_ICount
            // 
            txt_ICount.Location = new Point(124, 206);
            txt_ICount.Margin = new Padding(6, 5, 6, 5);
            txt_ICount.Name = "txt_ICount";
            txt_ICount.Size = new Size(340, 30);
            txt_ICount.TabIndex = 127;
            txt_ICount.Text = "1";
            // 
            // txt_KSize
            // 
            txt_KSize.Location = new Point(124, 399);
            txt_KSize.Margin = new Padding(6, 5, 6, 5);
            txt_KSize.Name = "txt_KSize";
            txt_KSize.Size = new Size(340, 30);
            txt_KSize.TabIndex = 126;
            txt_KSize.Text = "1.0";
            // 
            // txt_JSize
            // 
            txt_JSize.Location = new Point(124, 363);
            txt_JSize.Margin = new Padding(6, 5, 6, 5);
            txt_JSize.Name = "txt_JSize";
            txt_JSize.Size = new Size(340, 30);
            txt_JSize.TabIndex = 125;
            txt_JSize.Text = "1.0";
            // 
            // txt_ISize
            // 
            txt_ISize.Location = new Point(124, 329);
            txt_ISize.Margin = new Padding(6, 5, 6, 5);
            txt_ISize.Name = "txt_ISize";
            txt_ISize.Size = new Size(340, 30);
            txt_ISize.TabIndex = 124;
            txt_ISize.Text = "1.0";
            // 
            // txt_GridName
            // 
            txt_GridName.Location = new Point(124, 60);
            txt_GridName.Margin = new Padding(6, 5, 6, 5);
            txt_GridName.Name = "txt_GridName";
            txt_GridName.Size = new Size(340, 30);
            txt_GridName.TabIndex = 123;
            txt_GridName.Text = "grid_name";
            // 
            // btn_Cancel
            // 
            btn_Cancel.Location = new Point(548, 568);
            btn_Cancel.Margin = new Padding(6, 5, 6, 5);
            btn_Cancel.Name = "btn_Cancel";
            btn_Cancel.Size = new Size(164, 35);
            btn_Cancel.TabIndex = 122;
            btn_Cancel.Text = "Cancel";
            btn_Cancel.UseVisualStyleBackColor = true;
            btn_Cancel.Click += btn_Cancel_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(38, 532);
            label10.Margin = new Padding(6, 0, 6, 0);
            label10.Name = "label10";
            label10.Size = new Size(47, 24);
            label10.TabIndex = 121;
            label10.Text = "zmn";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(37, 496);
            label9.Margin = new Padding(6, 0, 6, 0);
            label9.Name = "label9";
            label9.Size = new Size(48, 24);
            label9.TabIndex = 120;
            label9.Text = "ymn";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(38, 462);
            label8.Margin = new Padding(6, 0, 6, 0);
            label8.Name = "label8";
            label8.Size = new Size(47, 24);
            label8.TabIndex = 119;
            label8.Text = "xmn";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(44, 402);
            label7.Margin = new Padding(6, 0, 6, 0);
            label7.Name = "label7";
            label7.Size = new Size(41, 24);
            label7.TabIndex = 118;
            label7.Text = "zsiz";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(43, 366);
            label6.Margin = new Padding(6, 0, 6, 0);
            label6.Name = "label6";
            label6.Size = new Size(42, 24);
            label6.TabIndex = 117;
            label6.Text = "ysiz";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(44, 331);
            label5.Margin = new Padding(6, 0, 6, 0);
            label5.Name = "label5";
            label5.Size = new Size(41, 24);
            label5.TabIndex = 116;
            label5.Text = "xsiz";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(55, 279);
            label4.Margin = new Padding(6, 0, 6, 0);
            label4.Name = "label4";
            label4.Size = new Size(30, 24);
            label4.TabIndex = 115;
            label4.Text = "nz";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(54, 244);
            label3.Margin = new Padding(6, 0, 6, 0);
            label3.Name = "label3";
            label3.Size = new Size(31, 24);
            label3.TabIndex = 114;
            label3.Text = "ny";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(55, 208);
            label2.Margin = new Padding(6, 0, 6, 0);
            label2.Name = "label2";
            label2.Size = new Size(30, 24);
            label2.TabIndex = 113;
            label2.Text = "nx";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 63);
            label1.Margin = new Padding(6, 0, 6, 0);
            label1.Name = "label1";
            label1.Size = new Size(104, 24);
            label1.TabIndex = 112;
            label1.Text = "Grid Name";
            // 
            // btn_OK
            // 
            btn_OK.Location = new Point(252, 568);
            btn_OK.Margin = new Padding(6, 5, 6, 5);
            btn_OK.Name = "btn_OK";
            btn_OK.Size = new Size(164, 35);
            btn_OK.TabIndex = 111;
            btn_OK.Text = "OK";
            btn_OK.UseVisualStyleBackColor = true;
            btn_OK.Click += btn_OK_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(475, 52);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(502, 506);
            textBox1.TabIndex = 140;
            // 
            // button2
            // 
            button2.ForeColor = Color.Blue;
            button2.Location = new Point(811, 566);
            button2.Margin = new Padding(6, 5, 6, 5);
            button2.Name = "button2";
            button2.Size = new Size(166, 37);
            button2.TabIndex = 141;
            button2.Text = "示例数据";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(22, 155);
            label11.Margin = new Padding(6, 0, 6, 0);
            label11.Name = "label11";
            label11.Size = new Size(89, 24);
            label11.TabIndex = 142;
            label11.Text = "splitType";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "space character", "\\t", ",", ";" });
            comboBox1.Location = new Point(123, 152);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(341, 32);
            comboBox1.TabIndex = 143;
            comboBox1.Text = "space character";
            // 
            // Form_ReadGridFromGSLIB
            // 
            AcceptButton = btn_OK;
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(986, 610);
            Controls.Add(comboBox1);
            Controls.Add(label11);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(label13);
            Controls.Add(txt_ValueOfNull);
            Controls.Add(label12);
            Controls.Add(btn_OpenFile);
            Controls.Add(txt_FileName);
            Controls.Add(txt_OriginCellZ);
            Controls.Add(txt_OriginCellY);
            Controls.Add(txt_OriginCellX);
            Controls.Add(txt_KCount);
            Controls.Add(txt_JCount);
            Controls.Add(txt_ICount);
            Controls.Add(txt_KSize);
            Controls.Add(txt_JSize);
            Controls.Add(txt_ISize);
            Controls.Add(txt_GridName);
            Controls.Add(btn_Cancel);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btn_OK);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(6, 5, 6, 5);
            MaximizeBox = false;
            Name = "Form_ReadGridFromGSLIB";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Read Grid from GSLIB Format";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label13;
        private TextBox txt_ValueOfNull;
        private Label label12;
        private Button btn_OpenFile;
        private TextBox txt_FileName;
        private TextBox txt_OriginCellZ;
        private TextBox txt_OriginCellY;
        private TextBox txt_OriginCellX;
        private TextBox txt_KCount;
        private TextBox txt_JCount;
        private TextBox txt_ICount;
        private TextBox txt_KSize;
        private TextBox txt_JSize;
        private TextBox txt_ISize;
        private TextBox txt_GridName;
        private Button btn_Cancel;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btn_OK;
        private TextBox textBox1;
        private Button button2;
        private Label label11;
        private ComboBox comboBox1;
    }
}