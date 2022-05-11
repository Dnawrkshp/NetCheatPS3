namespace NCMemBrowser
{
    partial class ctlMain
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
            this.memSizeVal = new System.Windows.Forms.NumericUpDown();
            this.memPanel = new System.Windows.Forms.Panel();
            this.refMSVal = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.refButton = new System.Windows.Forms.Button();
            this.addrGrid1 = new NCMemBrowser.addrGrid();
            this.hexGrid1 = new NCMemBrowser.hexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.memSizeVal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refMSVal)).BeginInit();
            this.SuspendLayout();
            // 
            // memSizeVal
            // 
            this.memSizeVal.Hexadecimal = true;
            this.memSizeVal.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.memSizeVal.Location = new System.Drawing.Point(186, 25);
            this.memSizeVal.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.memSizeVal.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.memSizeVal.Name = "memSizeVal";
            this.memSizeVal.Size = new System.Drawing.Size(67, 20);
            this.memSizeVal.TabIndex = 3;
            this.memSizeVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.memSizeVal.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.memSizeVal.ValueChanged += new System.EventHandler(this.memSizeVal_ValueChanged);
            // 
            // memPanel
            // 
            this.memPanel.Location = new System.Drawing.Point(3, 3);
            this.memPanel.Name = "memPanel";
            this.memPanel.Size = new System.Drawing.Size(173, 311);
            this.memPanel.TabIndex = 4;
            // 
            // refMSVal
            // 
            this.refMSVal.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.refMSVal.Location = new System.Drawing.Point(186, 73);
            this.refMSVal.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.refMSVal.Name = "refMSVal";
            this.refMSVal.Size = new System.Drawing.Size(67, 20);
            this.refMSVal.TabIndex = 5;
            this.refMSVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.refMSVal.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.refMSVal.ValueChanged += new System.EventHandler(this.refMSVal_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(186, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Lines";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(186, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Refresh (ms)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // refButton
            // 
            this.refButton.Location = new System.Drawing.Point(186, 99);
            this.refButton.Name = "refButton";
            this.refButton.Size = new System.Drawing.Size(67, 21);
            this.refButton.TabIndex = 8;
            this.refButton.Text = "Refresh";
            this.refButton.UseVisualStyleBackColor = true;
            this.refButton.Click += new System.EventHandler(this.refButton_Click);
            // 
            // addrGrid1
            // 
            this.addrGrid1.Location = new System.Drawing.Point(186, 298);
            this.addrGrid1.Name = "addrGrid1";
            this.addrGrid1.Size = new System.Drawing.Size(67, 16);
            this.addrGrid1.TabIndex = 2;
            this.addrGrid1.Tag = "0";
            this.addrGrid1.Visible = false;
            // 
            // hexGrid1
            // 
            this.hexGrid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 131070F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.hexGrid1.Location = new System.Drawing.Point(186, 279);
            this.hexGrid1.Margin = new System.Windows.Forms.Padding(0);
            this.hexGrid1.Name = "hexGrid1";
            this.hexGrid1.Size = new System.Drawing.Size(84, 16);
            this.hexGrid1.TabIndex = 1;
            this.hexGrid1.Tag = "0";
            this.hexGrid1.Visible = false;
            // 
            // ctlMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.refButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.refMSVal);
            this.Controls.Add(this.addrGrid1);
            this.Controls.Add(this.memPanel);
            this.Controls.Add(this.hexGrid1);
            this.Controls.Add(this.memSizeVal);
            this.MaximumSize = new System.Drawing.Size(270, 2000);
            this.Name = "ctlMain";
            this.Size = new System.Drawing.Size(270, 320);
            this.Load += new System.EventHandler(this.ctlMain_Load);
            this.ClientSizeChanged += new System.EventHandler(this.ctlMain_ClientSizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.memSizeVal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refMSVal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private hexGrid hexGrid1;
        private addrGrid addrGrid1;
        private System.Windows.Forms.NumericUpDown memSizeVal;
        private System.Windows.Forms.Panel memPanel;
        private System.Windows.Forms.NumericUpDown refMSVal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button refButton;


    }
}
