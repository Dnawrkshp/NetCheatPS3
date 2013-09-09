namespace NetCheatPS3
{
    partial class InputBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputBox));
            this.ibOkay = new System.Windows.Forms.Button();
            this.ibCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ibOkay
            // 
            this.ibOkay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ibOkay.Location = new System.Drawing.Point(12, 96);
            this.ibOkay.Name = "ibOkay";
            this.ibOkay.Size = new System.Drawing.Size(83, 29);
            this.ibOkay.TabIndex = 0;
            this.ibOkay.Text = "Okay";
            this.ibOkay.UseVisualStyleBackColor = true;
            this.ibOkay.Click += new System.EventHandler(this.ibOkay_Click);
            // 
            // ibCancel
            // 
            this.ibCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ibCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ibCancel.Location = new System.Drawing.Point(117, 96);
            this.ibCancel.Name = "ibCancel";
            this.ibCancel.Size = new System.Drawing.Size(83, 29);
            this.ibCancel.TabIndex = 1;
            this.ibCancel.Text = "Cancel";
            this.ibCancel.UseVisualStyleBackColor = true;
            this.ibCancel.Click += new System.EventHandler(this.ibCancel_Click);
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.CancelButton = this.ibCancel;
            this.ClientSize = new System.Drawing.Size(212, 137);
            this.ControlBox = false;
            this.Controls.Add(this.ibCancel);
            this.Controls.Add(this.ibOkay);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InputBox";
            this.Load += new System.EventHandler(this.InputBox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ibOkay;
        private System.Windows.Forms.Button ibCancel;
    }
}