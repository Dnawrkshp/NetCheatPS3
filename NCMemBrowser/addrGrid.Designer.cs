namespace NCMemBrowser
{
    partial class addrGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addrBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // addrBox
            // 
            this.addrBox.BackColor = System.Drawing.SystemColors.InfoText;
            this.addrBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.addrBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.addrBox.Location = new System.Drawing.Point(0, 0);
            this.addrBox.MaxLength = 8;
            this.addrBox.Multiline = false;
            this.addrBox.Name = "addrBox";
            this.addrBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.addrBox.Size = new System.Drawing.Size(150, 10);
            this.addrBox.TabIndex = 0;
            this.addrBox.Text = "00000000";
            this.addrBox.WordWrap = false;
            this.addrBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.addrBox_KeyUp);
            // 
            // addrGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addrBox);
            this.Name = "addrGrid";
            this.Size = new System.Drawing.Size(150, 22);
            this.BackColorChanged += new System.EventHandler(this.addrGrid_BackColorChanged);
            this.ForeColorChanged += new System.EventHandler(this.addrGrid_ForeColorChanged);
            this.Resize += new System.EventHandler(this.addrGrid_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox addrBox;
    }
}
