namespace NCMemBrowser
{
    partial class hexGrid
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
            this.b1 = new System.Windows.Forms.RichTextBox();
            this.b2 = new System.Windows.Forms.RichTextBox();
            this.b3 = new System.Windows.Forms.RichTextBox();
            this.b4 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // b1
            // 
            this.b1.BackColor = System.Drawing.Color.Black;
            this.b1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.b1.DetectUrls = false;
            this.b1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.b1.Location = new System.Drawing.Point(0, 0);
            this.b1.MaxLength = 2;
            this.b1.Name = "b1";
            this.b1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.b1.Size = new System.Drawing.Size(24, 24);
            this.b1.TabIndex = 0;
            this.b1.Tag = "1";
            this.b1.Text = "00";
            this.b1.WordWrap = false;
            this.b1.SelectionChanged += new System.EventHandler(this.byte_SelectionChanged);
            this.b1.TextChanged += new System.EventHandler(this.b1_TextChanged);
            this.b1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.byte_KeyDown);
            // 
            // b2
            // 
            this.b2.BackColor = System.Drawing.Color.Black;
            this.b2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.b2.DetectUrls = false;
            this.b2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.b2.Location = new System.Drawing.Point(24, 0);
            this.b2.MaxLength = 2;
            this.b2.Name = "b2";
            this.b2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.b2.Size = new System.Drawing.Size(24, 24);
            this.b2.TabIndex = 1;
            this.b2.Tag = "2";
            this.b2.Text = "00";
            this.b2.WordWrap = false;
            this.b2.SelectionChanged += new System.EventHandler(this.byte_SelectionChanged);
            this.b2.TextChanged += new System.EventHandler(this.b2_TextChanged);
            this.b2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.byte_KeyDown);
            // 
            // b3
            // 
            this.b3.BackColor = System.Drawing.Color.Black;
            this.b3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.b3.DetectUrls = false;
            this.b3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.b3.Location = new System.Drawing.Point(48, 0);
            this.b3.MaxLength = 2;
            this.b3.Name = "b3";
            this.b3.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.b3.Size = new System.Drawing.Size(24, 24);
            this.b3.TabIndex = 2;
            this.b3.Tag = "3";
            this.b3.Text = "00";
            this.b3.WordWrap = false;
            this.b3.SelectionChanged += new System.EventHandler(this.byte_SelectionChanged);
            this.b3.TextChanged += new System.EventHandler(this.b3_TextChanged);
            this.b3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.byte_KeyDown);
            // 
            // b4
            // 
            this.b4.BackColor = System.Drawing.Color.Black;
            this.b4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.b4.DetectUrls = false;
            this.b4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.b4.Location = new System.Drawing.Point(72, 0);
            this.b4.MaxLength = 2;
            this.b4.Name = "b4";
            this.b4.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.b4.Size = new System.Drawing.Size(24, 24);
            this.b4.TabIndex = 3;
            this.b4.Tag = "4";
            this.b4.Text = "00";
            this.b4.WordWrap = false;
            this.b4.SelectionChanged += new System.EventHandler(this.byte_SelectionChanged);
            this.b4.TextChanged += new System.EventHandler(this.b4_TextChanged);
            this.b4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.byte_KeyDown);
            // 
            // hexGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.b4);
            this.Controls.Add(this.b3);
            this.Controls.Add(this.b2);
            this.Controls.Add(this.b1);
            this.Name = "hexGrid";
            this.Size = new System.Drawing.Size(96, 24);
            this.BackColorChanged += new System.EventHandler(this.hexGrid_BackColorChanged);
            this.ForeColorChanged += new System.EventHandler(this.hexGrid_ForeColorChanged);
            this.Resize += new System.EventHandler(this.hexGrid_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox b1;
        private System.Windows.Forms.RichTextBox b2;
        private System.Windows.Forms.RichTextBox b3;
        private System.Windows.Forms.RichTextBox b4;


    }
}
