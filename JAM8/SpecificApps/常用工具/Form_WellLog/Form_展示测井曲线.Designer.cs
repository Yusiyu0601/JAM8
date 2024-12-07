namespace JAM8.SpecificApps.常用工具
{
    partial class Form_展示测井曲线
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
            formsPlot1 = new ScottPlot.FormsPlot();
            button1 = new Button();
            formsPlot2 = new ScottPlot.FormsPlot();
            formsPlot3 = new ScottPlot.FormsPlot();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(204, 3);
            formsPlot1.Margin = new Padding(6, 5, 6, 5);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(284, 600);
            formsPlot1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(128, 48);
            button1.TabIndex = 1;
            button1.Text = "打开测井数据";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // formsPlot2
            // 
            formsPlot2.Location = new Point(476, 3);
            formsPlot2.Margin = new Padding(6, 5, 6, 5);
            formsPlot2.Name = "formsPlot2";
            formsPlot2.Size = new Size(284, 600);
            formsPlot2.TabIndex = 2;
            // 
            // formsPlot3
            // 
            formsPlot3.Location = new Point(747, 3);
            formsPlot3.Margin = new Padding(6, 5, 6, 5);
            formsPlot3.Name = "formsPlot3";
            formsPlot3.Size = new Size(284, 600);
            formsPlot3.TabIndex = 3;
            // 
            // Form_测井曲线
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1054, 606);
            Controls.Add(formsPlot3);
            Controls.Add(formsPlot2);
            Controls.Add(button1);
            Controls.Add(formsPlot1);
            Name = "Form_测井曲线";
            Text = "Form_测井曲线";
            ResumeLayout(false);
        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private Button button1;
        private ScottPlot.FormsPlot formsPlot2;
        private ScottPlot.FormsPlot formsPlot3;
    }
}