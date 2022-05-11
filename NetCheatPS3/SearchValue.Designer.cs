namespace NetCheatPS3
{
    partial class SearchValue
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.boolBox = new System.Windows.Forms.CheckBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.valBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // boolBox
            // 
            this.boolBox.AutoSize = true;
            this.boolBox.Location = new System.Drawing.Point(370, 3);
            this.boolBox.Name = "boolBox";
            this.boolBox.Size = new System.Drawing.Size(46, 17);
            this.boolBox.TabIndex = 0;
            this.boolBox.Text = "bool";
            this.boolBox.UseVisualStyleBackColor = true;
            this.boolBox.CheckedChanged += new System.EventHandler(this.boolBox_CheckedChanged);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(3, 4);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(33, 13);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "name";
            // 
            // valBox
            // 
            this.valBox.Location = new System.Drawing.Point(42, 1);
            this.valBox.Name = "valBox";
            this.valBox.Size = new System.Drawing.Size(322, 20);
            this.valBox.TabIndex = 2;
            // 
            // SearchValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.valBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.boolBox);
            this.Name = "SearchValue";
            this.Size = new System.Drawing.Size(419, 24);
            this.Resize += new System.EventHandler(this.SearchValue_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox boolBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox valBox;
    }
}
