namespace JAM8.Algorithms.Geometry
{
    partial class Form_Scottplot4Grid
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
            scottplot4Grid1 = new Scottplot4Grid();
            SuspendLayout();
            // 
            // scottplot4Grid1
            // 
            scottplot4Grid1.Dock = DockStyle.Fill;
            scottplot4Grid1.Location = new Point(0, 0);
            scottplot4Grid1.Name = "scottplot4Grid1";
            scottplot4Grid1.Size = new Size(976, 604);
            scottplot4Grid1.TabIndex = 0;
            // 
            // Form_Scottplot4Grid2
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(976, 604);
            Controls.Add(scottplot4Grid1);
            Name = "Form_Scottplot4Grid2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "show grid";
            ResumeLayout(false);
        }

        #endregion

        private Scottplot4Grid scottplot4Grid1;
    }
}