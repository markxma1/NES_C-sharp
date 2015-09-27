namespace NES
{
    partial class MainForm
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.Stop = new System.Windows.Forms.Button();
            this.Start = new System.Windows.Forms.Button();
            this.Load = new System.Windows.Forms.Button();
            this.Display = new System.Windows.Forms.Button();
            this.NameTable = new System.Windows.Forms.Button();
            this.PatternTable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Stop
            // 
            this.Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Stop.Location = new System.Drawing.Point(197, 221);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 23);
            this.Stop.TabIndex = 4;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // Start
            // 
            this.Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Start.Location = new System.Drawing.Point(197, 192);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(75, 23);
            this.Start.TabIndex = 5;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Load
            // 
            this.Load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Load.Location = new System.Drawing.Point(197, 163);
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(75, 23);
            this.Load.TabIndex = 6;
            this.Load.Text = "Load";
            this.Load.UseVisualStyleBackColor = true;
            this.Load.Click += new System.EventHandler(this.Load_Click);
            // 
            // Display
            // 
            this.Display.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Display.Location = new System.Drawing.Point(12, 163);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(75, 23);
            this.Display.TabIndex = 9;
            this.Display.Text = "Display";
            this.Display.UseVisualStyleBackColor = true;
            this.Display.Click += new System.EventHandler(this.Display_Click);
            // 
            // NameTable
            // 
            this.NameTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTable.Location = new System.Drawing.Point(12, 192);
            this.NameTable.Name = "NameTable";
            this.NameTable.Size = new System.Drawing.Size(75, 23);
            this.NameTable.TabIndex = 8;
            this.NameTable.Text = "NameTable";
            this.NameTable.UseVisualStyleBackColor = true;
            this.NameTable.Click += new System.EventHandler(this.NameTable_Click);
            // 
            // PatternTable
            // 
            this.PatternTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PatternTable.Location = new System.Drawing.Point(12, 221);
            this.PatternTable.Name = "PatternTable";
            this.PatternTable.Size = new System.Drawing.Size(75, 23);
            this.PatternTable.TabIndex = 7;
            this.PatternTable.Text = "PatternTable";
            this.PatternTable.UseVisualStyleBackColor = true;
            this.PatternTable.Click += new System.EventHandler(this.PatternTable_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 256);
            this.Controls.Add(this.Display);
            this.Controls.Add(this.NameTable);
            this.Controls.Add(this.PatternTable);
            this.Controls.Add(this.Load);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.Stop);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.Button Load;
        private System.Windows.Forms.Button Display;
        private System.Windows.Forms.Button NameTable;
        private System.Windows.Forms.Button PatternTable;
    }
}