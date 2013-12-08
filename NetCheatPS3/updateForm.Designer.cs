namespace NetCheatPS3
{
    partial class updateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(updateForm));
            this.titleLabel = new System.Windows.Forms.Label();
            this.updateBox = new System.Windows.Forms.TextBox();
            this.yesButt = new System.Windows.Forms.Button();
            this.noButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(403, 41);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updateBox
            // 
            this.updateBox.BackColor = System.Drawing.Color.Black;
            this.updateBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.updateBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.updateBox.Location = new System.Drawing.Point(12, 53);
            this.updateBox.Multiline = true;
            this.updateBox.Name = "updateBox";
            this.updateBox.ReadOnly = true;
            this.updateBox.Size = new System.Drawing.Size(403, 172);
            this.updateBox.TabIndex = 1;
            this.updateBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // yesButt
            // 
            this.yesButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.yesButt.Location = new System.Drawing.Point(12, 231);
            this.yesButt.Name = "yesButt";
            this.yesButt.Size = new System.Drawing.Size(125, 23);
            this.yesButt.TabIndex = 2;
            this.yesButt.Text = "Yes";
            this.yesButt.UseVisualStyleBackColor = true;
            this.yesButt.Click += new System.EventHandler(this.yesButt_Click);
            // 
            // noButt
            // 
            this.noButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noButt.Location = new System.Drawing.Point(290, 231);
            this.noButt.Name = "noButt";
            this.noButt.Size = new System.Drawing.Size(125, 23);
            this.noButt.TabIndex = 3;
            this.noButt.Text = "No";
            this.noButt.UseVisualStyleBackColor = true;
            this.noButt.Click += new System.EventHandler(this.noButt_Click);
            // 
            // updateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(427, 262);
            this.ControlBox = false;
            this.Controls.Add(this.noButt);
            this.Controls.Add(this.yesButt);
            this.Controls.Add(this.updateBox);
            this.Controls.Add(this.titleLabel);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "updateForm";
            this.Load += new System.EventHandler(this.updateForm_Load);
            this.Resize += new System.EventHandler(this.updateForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox updateBox;
        private System.Windows.Forms.Button yesButt;
        private System.Windows.Forms.Button noButt;
    }
}