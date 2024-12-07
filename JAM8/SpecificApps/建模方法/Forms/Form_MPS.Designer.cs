namespace JAM8.SpecificApps.建模方法.Forms
{
    partial class Form_MPS
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button7 = new Button();
            scottplot4Grid2 = new Algorithms.Geometry.Scottplot4Grid();
            tb_file_name = new TextBox();
            button2 = new Button();
            tabPage3 = new TabPage();
            tb_cd_zmn = new TextBox();
            label16 = new Label();
            tb_cd_ymn = new TextBox();
            label17 = new Label();
            tb_cd_xmn = new TextBox();
            label18 = new Label();
            tb_cd_zsiz = new TextBox();
            label13 = new Label();
            tb_cd_ysiz = new TextBox();
            label14 = new Label();
            tb_cd_xsiz = new TextBox();
            label15 = new Label();
            button4 = new Button();
            tb_cd_nz = new TextBox();
            label10 = new Label();
            tb_cd_ny = new TextBox();
            label11 = new Label();
            tb_cd_nx = new TextBox();
            label12 = new Label();
            scottplot4Grid3 = new Algorithms.Geometry.Scottplot4Grid();
            textBox2 = new TextBox();
            button1 = new Button();
            tabPage2 = new TabPage();
            groupBox2 = new GroupBox();
            mps_N = new TextBox();
            label8 = new Label();
            mps_randomSeed = new TextBox();
            label5 = new Label();
            groupBox1 = new GroupBox();
            mps_nx = new TextBox();
            mps_ny = new TextBox();
            label3 = new Label();
            label1 = new Label();
            label2 = new Label();
            mps_nz = new TextBox();
            scottplot4Grid1 = new Algorithms.Geometry.Scottplot4Grid();
            tabControl2 = new TabControl();
            tabPage4 = new TabPage();
            textBox1 = new TextBox();
            tb_template_rz = new TextBox();
            button3 = new Button();
            label9 = new Label();
            tb_template_ry = new TextBox();
            label7 = new Label();
            tb_multigrid = new TextBox();
            label6 = new Label();
            label4 = new Label();
            tb_template_rx = new TextBox();
            tabPage5 = new TabPage();
            snesim_max_number = new TextBox();
            label23 = new Label();
            button5 = new Button();
            tabPage6 = new TabPage();
            ds_距离阈值 = new TextBox();
            button6 = new Button();
            ds_最大条件数 = new TextBox();
            label19 = new Label();
            label20 = new Label();
            ds_搜索比例 = new TextBox();
            label21 = new Label();
            label22 = new Label();
            ds_搜索半径 = new TextBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            tabControl2.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage6.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1242, 665);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(button7);
            tabPage1.Controls.Add(scottplot4Grid2);
            tabPage1.Controls.Add(tb_file_name);
            tabPage1.Controls.Add(button2);
            tabPage1.Location = new Point(4, 33);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1234, 628);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "训练图像";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(8, 90);
            button7.Name = "button7";
            button7.Size = new Size(230, 51);
            button7.TabIndex = 119;
            button7.Text = "GridCatalog读取";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // scottplot4Grid2
            // 
            scottplot4Grid2.Location = new Point(244, 6);
            scottplot4Grid2.Name = "scottplot4Grid2";
            scottplot4Grid2.Size = new Size(984, 614);
            scottplot4Grid2.TabIndex = 118;
            // 
            // tb_file_name
            // 
            tb_file_name.Location = new Point(8, 160);
            tb_file_name.Multiline = true;
            tb_file_name.Name = "tb_file_name";
            tb_file_name.ReadOnly = true;
            tb_file_name.ScrollBars = ScrollBars.Vertical;
            tb_file_name.Size = new Size(230, 136);
            tb_file_name.TabIndex = 117;
            // 
            // button2
            // 
            button2.Location = new Point(8, 22);
            button2.Name = "button2";
            button2.Size = new Size(230, 51);
            button2.TabIndex = 116;
            button2.Text = "Gslib文件读取";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(tb_cd_zmn);
            tabPage3.Controls.Add(label16);
            tabPage3.Controls.Add(tb_cd_ymn);
            tabPage3.Controls.Add(label17);
            tabPage3.Controls.Add(tb_cd_xmn);
            tabPage3.Controls.Add(label18);
            tabPage3.Controls.Add(tb_cd_zsiz);
            tabPage3.Controls.Add(label13);
            tabPage3.Controls.Add(tb_cd_ysiz);
            tabPage3.Controls.Add(label14);
            tabPage3.Controls.Add(tb_cd_xsiz);
            tabPage3.Controls.Add(label15);
            tabPage3.Controls.Add(button4);
            tabPage3.Controls.Add(tb_cd_nz);
            tabPage3.Controls.Add(label10);
            tabPage3.Controls.Add(tb_cd_ny);
            tabPage3.Controls.Add(label11);
            tabPage3.Controls.Add(tb_cd_nx);
            tabPage3.Controls.Add(label12);
            tabPage3.Controls.Add(scottplot4Grid3);
            tabPage3.Controls.Add(textBox2);
            tabPage3.Controls.Add(button1);
            tabPage3.Location = new Point(4, 33);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1234, 628);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "条件数据";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tb_cd_zmn
            // 
            tb_cd_zmn.Location = new Point(121, 450);
            tb_cd_zmn.Name = "tb_cd_zmn";
            tb_cd_zmn.Size = new Size(77, 30);
            tb_cd_zmn.TabIndex = 140;
            tb_cd_zmn.Text = "0.5";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(42, 453);
            label16.Name = "label16";
            label16.Size = new Size(47, 24);
            label16.TabIndex = 139;
            label16.Text = "zmn";
            // 
            // tb_cd_ymn
            // 
            tb_cd_ymn.Location = new Point(121, 402);
            tb_cd_ymn.Name = "tb_cd_ymn";
            tb_cd_ymn.Size = new Size(77, 30);
            tb_cd_ymn.TabIndex = 138;
            tb_cd_ymn.Text = "0.5";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(42, 405);
            label17.Name = "label17";
            label17.Size = new Size(48, 24);
            label17.TabIndex = 137;
            label17.Text = "ymn";
            // 
            // tb_cd_xmn
            // 
            tb_cd_xmn.Location = new Point(121, 354);
            tb_cd_xmn.Name = "tb_cd_xmn";
            tb_cd_xmn.Size = new Size(77, 30);
            tb_cd_xmn.TabIndex = 136;
            tb_cd_xmn.Text = "0.5";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(42, 357);
            label18.Name = "label18";
            label18.Size = new Size(47, 24);
            label18.TabIndex = 135;
            label18.Text = "xmn";
            // 
            // tb_cd_zsiz
            // 
            tb_cd_zsiz.Location = new Point(121, 307);
            tb_cd_zsiz.Name = "tb_cd_zsiz";
            tb_cd_zsiz.Size = new Size(77, 30);
            tb_cd_zsiz.TabIndex = 134;
            tb_cd_zsiz.Text = "1";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(42, 310);
            label13.Name = "label13";
            label13.Size = new Size(41, 24);
            label13.TabIndex = 133;
            label13.Text = "zsiz";
            // 
            // tb_cd_ysiz
            // 
            tb_cd_ysiz.Location = new Point(121, 259);
            tb_cd_ysiz.Name = "tb_cd_ysiz";
            tb_cd_ysiz.Size = new Size(77, 30);
            tb_cd_ysiz.TabIndex = 132;
            tb_cd_ysiz.Text = "1";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(42, 262);
            label14.Name = "label14";
            label14.Size = new Size(42, 24);
            label14.TabIndex = 131;
            label14.Text = "ysiz";
            // 
            // tb_cd_xsiz
            // 
            tb_cd_xsiz.Location = new Point(121, 211);
            tb_cd_xsiz.Name = "tb_cd_xsiz";
            tb_cd_xsiz.Size = new Size(77, 30);
            tb_cd_xsiz.TabIndex = 130;
            tb_cd_xsiz.Text = "1";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(42, 214);
            label15.Name = "label15";
            label15.Size = new Size(41, 24);
            label15.TabIndex = 129;
            label15.Text = "xsiz";
            // 
            // button4
            // 
            button4.Location = new Point(8, 486);
            button4.Name = "button4";
            button4.Size = new Size(239, 51);
            button4.TabIndex = 128;
            button4.Text = "预览";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // tb_cd_nz
            // 
            tb_cd_nz.Location = new Point(121, 163);
            tb_cd_nz.Name = "tb_cd_nz";
            tb_cd_nz.Size = new Size(77, 30);
            tb_cd_nz.TabIndex = 127;
            tb_cd_nz.Text = "1";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(42, 166);
            label10.Name = "label10";
            label10.Size = new Size(30, 24);
            label10.TabIndex = 126;
            label10.Text = "nz";
            // 
            // tb_cd_ny
            // 
            tb_cd_ny.Location = new Point(121, 115);
            tb_cd_ny.Name = "tb_cd_ny";
            tb_cd_ny.Size = new Size(77, 30);
            tb_cd_ny.TabIndex = 125;
            tb_cd_ny.Text = "100";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(42, 118);
            label11.Name = "label11";
            label11.Size = new Size(31, 24);
            label11.TabIndex = 124;
            label11.Text = "ny";
            // 
            // tb_cd_nx
            // 
            tb_cd_nx.Location = new Point(121, 67);
            tb_cd_nx.Name = "tb_cd_nx";
            tb_cd_nx.Size = new Size(77, 30);
            tb_cd_nx.TabIndex = 123;
            tb_cd_nx.Text = "100";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(42, 70);
            label12.Name = "label12";
            label12.Size = new Size(30, 24);
            label12.TabIndex = 122;
            label12.Text = "nx";
            // 
            // scottplot4Grid3
            // 
            scottplot4Grid3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            scottplot4Grid3.Location = new Point(253, 7);
            scottplot4Grid3.Name = "scottplot4Grid3";
            scottplot4Grid3.Size = new Size(973, 615);
            scottplot4Grid3.TabIndex = 121;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(8, 543);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(239, 77);
            textBox2.TabIndex = 120;
            // 
            // button1
            // 
            button1.Location = new Point(8, 7);
            button1.Name = "button1";
            button1.Size = new Size(239, 51);
            button1.TabIndex = 119;
            button1.Text = "导入条件数据(gslib points)";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox2);
            tabPage2.Controls.Add(groupBox1);
            tabPage2.Controls.Add(scottplot4Grid1);
            tabPage2.Controls.Add(tabControl2);
            tabPage2.Location = new Point(4, 33);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1234, 628);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "建模";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(mps_N);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(mps_randomSeed);
            groupBox2.Controls.Add(label5);
            groupBox2.Location = new Point(10, 498);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(310, 122);
            groupBox2.TabIndex = 141;
            groupBox2.TabStop = false;
            groupBox2.Text = "其他参数";
            // 
            // mps_N
            // 
            mps_N.Location = new Point(151, 37);
            mps_N.Name = "mps_N";
            mps_N.Size = new Size(96, 30);
            mps_N.TabIndex = 128;
            mps_N.Text = "1";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(58, 83);
            label8.Name = "label8";
            label8.Size = new Size(82, 24);
            label8.TabIndex = 122;
            label8.Text = "随机种子";
            // 
            // mps_randomSeed
            // 
            mps_randomSeed.Location = new Point(151, 80);
            mps_randomSeed.Name = "mps_randomSeed";
            mps_randomSeed.Size = new Size(96, 30);
            mps_randomSeed.TabIndex = 123;
            mps_randomSeed.Text = "123123";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(58, 40);
            label5.Name = "label5";
            label5.Size = new Size(82, 24);
            label5.TabIndex = 127;
            label5.Text = "模拟次数";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(mps_nx);
            groupBox1.Controls.Add(mps_ny);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(mps_nz);
            groupBox1.Location = new Point(10, 345);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(310, 147);
            groupBox1.TabIndex = 140;
            groupBox1.TabStop = false;
            groupBox1.Text = "模型尺寸";
            // 
            // mps_nx
            // 
            mps_nx.Location = new Point(130, 29);
            mps_nx.Name = "mps_nx";
            mps_nx.Size = new Size(119, 30);
            mps_nx.TabIndex = 117;
            mps_nx.Text = "100";
            // 
            // mps_ny
            // 
            mps_ny.Location = new Point(130, 67);
            mps_ny.Name = "mps_ny";
            mps_ny.Size = new Size(119, 30);
            mps_ny.TabIndex = 119;
            mps_ny.Text = "100";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(76, 108);
            label3.Name = "label3";
            label3.Size = new Size(30, 24);
            label3.TabIndex = 120;
            label3.Text = "nz";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(76, 32);
            label1.Name = "label1";
            label1.Size = new Size(30, 24);
            label1.TabIndex = 116;
            label1.Text = "nx";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(76, 70);
            label2.Name = "label2";
            label2.Size = new Size(31, 24);
            label2.TabIndex = 118;
            label2.Text = "ny";
            // 
            // mps_nz
            // 
            mps_nz.Location = new Point(130, 105);
            mps_nz.Name = "mps_nz";
            mps_nz.Size = new Size(119, 30);
            mps_nz.TabIndex = 121;
            mps_nz.Text = "1";
            // 
            // scottplot4Grid1
            // 
            scottplot4Grid1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            scottplot4Grid1.Location = new Point(326, 3);
            scottplot4Grid1.Name = "scottplot4Grid1";
            scottplot4Grid1.Size = new Size(905, 622);
            scottplot4Grid1.TabIndex = 139;
            // 
            // tabControl2
            // 
            tabControl2.Controls.Add(tabPage4);
            tabControl2.Controls.Add(tabPage5);
            tabControl2.Controls.Add(tabPage6);
            tabControl2.Location = new Point(10, 6);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(310, 333);
            tabControl2.TabIndex = 138;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(textBox1);
            tabPage4.Controls.Add(tb_template_rz);
            tabPage4.Controls.Add(button3);
            tabPage4.Controls.Add(label9);
            tabPage4.Controls.Add(tb_template_ry);
            tabPage4.Controls.Add(label7);
            tabPage4.Controls.Add(tb_multigrid);
            tabPage4.Controls.Add(label6);
            tabPage4.Controls.Add(label4);
            tabPage4.Controls.Add(tb_template_rx);
            tabPage4.Location = new Point(4, 33);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(302, 296);
            tabPage4.TabIndex = 0;
            tabPage4.Text = "SIMPAT";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(56, 254);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(187, 30);
            textBox1.TabIndex = 136;
            textBox1.Text = "自动生成保存名称";
            textBox1.MouseDoubleClick += textBox1_MouseDoubleClick;
            // 
            // tb_template_rz
            // 
            tb_template_rz.Location = new Point(142, 103);
            tb_template_rz.Name = "tb_template_rz";
            tb_template_rz.Size = new Size(113, 30);
            tb_template_rz.TabIndex = 135;
            tb_template_rz.Text = "1";
            // 
            // button3
            // 
            button3.Location = new Point(56, 205);
            button3.Name = "button3";
            button3.Size = new Size(187, 43);
            button3.TabIndex = 124;
            button3.Text = "运行";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(51, 106);
            label9.Name = "label9";
            label9.Size = new Size(70, 24);
            label9.TabIndex = 134;
            label9.Text = "样板_rz";
            // 
            // tb_template_ry
            // 
            tb_template_ry.Location = new Point(142, 60);
            tb_template_ry.Name = "tb_template_ry";
            tb_template_ry.Size = new Size(113, 30);
            tb_template_ry.TabIndex = 133;
            tb_template_ry.Text = "10";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(50, 63);
            label7.Name = "label7";
            label7.Size = new Size(71, 24);
            label7.TabIndex = 132;
            label7.Text = "样板_ry";
            // 
            // tb_multigrid
            // 
            tb_multigrid.Location = new Point(142, 155);
            tb_multigrid.Name = "tb_multigrid";
            tb_multigrid.Size = new Size(113, 30);
            tb_multigrid.TabIndex = 131;
            tb_multigrid.Text = "3";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(28, 158);
            label6.Name = "label6";
            label6.Size = new Size(93, 24);
            label6.TabIndex = 130;
            label6.Text = "MultiGrid";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(51, 20);
            label4.Name = "label4";
            label4.Size = new Size(70, 24);
            label4.TabIndex = 125;
            label4.Text = "样板_rx";
            // 
            // tb_template_rx
            // 
            tb_template_rx.Location = new Point(142, 17);
            tb_template_rx.Name = "tb_template_rx";
            tb_template_rx.Size = new Size(113, 30);
            tb_template_rx.TabIndex = 126;
            tb_template_rx.Text = "10";
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(snesim_max_number);
            tabPage5.Controls.Add(label23);
            tabPage5.Controls.Add(button5);
            tabPage5.Location = new Point(4, 33);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(302, 296);
            tabPage5.TabIndex = 1;
            tabPage5.Text = "SNESIM";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // snesim_max_number
            // 
            snesim_max_number.Location = new Point(147, 22);
            snesim_max_number.Name = "snesim_max_number";
            snesim_max_number.Size = new Size(103, 30);
            snesim_max_number.TabIndex = 133;
            snesim_max_number.Text = "25";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(33, 25);
            label23.Name = "label23";
            label23.Size = new Size(100, 24);
            label23.TabIndex = 132;
            label23.Text = "最大条件数";
            // 
            // button5
            // 
            button5.Location = new Point(56, 234);
            button5.Name = "button5";
            button5.Size = new Size(187, 43);
            button5.TabIndex = 125;
            button5.Text = "运行";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(ds_距离阈值);
            tabPage6.Controls.Add(button6);
            tabPage6.Controls.Add(ds_最大条件数);
            tabPage6.Controls.Add(label19);
            tabPage6.Controls.Add(label20);
            tabPage6.Controls.Add(ds_搜索比例);
            tabPage6.Controls.Add(label21);
            tabPage6.Controls.Add(label22);
            tabPage6.Controls.Add(ds_搜索半径);
            tabPage6.Location = new Point(4, 33);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(302, 296);
            tabPage6.TabIndex = 2;
            tabPage6.Text = "DS";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // ds_距离阈值
            // 
            ds_距离阈值.Location = new Point(148, 79);
            ds_距离阈值.Name = "ds_距离阈值";
            ds_距离阈值.Size = new Size(102, 30);
            ds_距离阈值.TabIndex = 133;
            ds_距离阈值.Text = "0.001";
            // 
            // button6
            // 
            button6.Location = new Point(54, 244);
            button6.Name = "button6";
            button6.Size = new Size(187, 43);
            button6.TabIndex = 125;
            button6.Text = "运行";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // ds_最大条件数
            // 
            ds_最大条件数.Location = new Point(147, 167);
            ds_最大条件数.Name = "ds_最大条件数";
            ds_最大条件数.Size = new Size(103, 30);
            ds_最大条件数.TabIndex = 131;
            ds_最大条件数.Text = "25";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(51, 82);
            label19.Name = "label19";
            label19.Size = new Size(82, 24);
            label19.TabIndex = 132;
            label19.Text = "距离阈值";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(33, 170);
            label20.Name = "label20";
            label20.Size = new Size(100, 24);
            label20.TabIndex = 130;
            label20.Text = "最大条件数";
            // 
            // ds_搜索比例
            // 
            ds_搜索比例.Location = new Point(147, 35);
            ds_搜索比例.Name = "ds_搜索比例";
            ds_搜索比例.Size = new Size(103, 30);
            ds_搜索比例.TabIndex = 129;
            ds_搜索比例.Text = "0.5";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(51, 38);
            label21.Name = "label21";
            label21.Size = new Size(82, 24);
            label21.TabIndex = 128;
            label21.Text = "搜索比例";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(51, 127);
            label22.Name = "label22";
            label22.Size = new Size(82, 24);
            label22.TabIndex = 126;
            label22.Text = "搜索半径";
            // 
            // ds_搜索半径
            // 
            ds_搜索半径.Location = new Point(147, 124);
            ds_搜索半径.Name = "ds_搜索半径";
            ds_搜索半径.Size = new Size(103, 30);
            ds_搜索半径.TabIndex = 127;
            ds_搜索半径.Text = "10";
            // 
            // Form_MPS
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1242, 665);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "Form_MPS";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form_MPS";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabControl2.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox tb_file_name;
        private Button button2;
        private TextBox mps_nz;
        private Label label3;
        private TextBox mps_ny;
        private Label label2;
        private TextBox mps_nx;
        private Label label1;
        private TextBox mps_randomSeed;
        private Label label8;
        private Button button3;
        private TextBox mps_N;
        private Label label5;
        private TextBox tb_template_rx;
        private Label label4;
        private Algorithms.Geometry.Scottplot4Grid scottplot4Grid2;
        private TextBox tb_multigrid;
        private Label label6;
        private TextBox tb_template_rz;
        private Label label9;
        private TextBox tb_template_ry;
        private Label label7;
        private TextBox textBox1;
        private TabPage tabPage3;
        private Algorithms.Geometry.Scottplot4Grid scottplot4Grid3;
        private TextBox textBox2;
        private Button button1;
        private TextBox tb_cd_zmn;
        private Label label16;
        private TextBox tb_cd_ymn;
        private Label label17;
        private TextBox tb_cd_xmn;
        private Label label18;
        private TextBox tb_cd_zsiz;
        private Label label13;
        private TextBox tb_cd_ysiz;
        private Label label14;
        private TextBox tb_cd_xsiz;
        private Label label15;
        private Button button4;
        private TextBox tb_cd_nz;
        private Label label10;
        private TextBox tb_cd_ny;
        private Label label11;
        private TextBox tb_cd_nx;
        private Label label12;
        private TabControl tabControl2;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private Algorithms.Geometry.Scottplot4Grid scottplot4Grid1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button button5;
        private Button button6;
        private TextBox ds_距离阈值;
        private TextBox ds_最大条件数;
        private Label label19;
        private Label label20;
        private TextBox ds_搜索比例;
        private Label label21;
        private Label label22;
        private TextBox ds_搜索半径;
        private Button button7;
        private TextBox snesim_max_number;
        private Label label23;
    }
}