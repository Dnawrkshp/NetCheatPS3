namespace NetCheatPS3
{
    partial class ConditionalEditor
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
            this.tbAddr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbCodes = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.buttOkay = new System.Windows.Forms.Button();
            this.buttCancel = new System.Windows.Forms.Button();
            this.cbComp = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbAddr
            // 
            this.tbAddr.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAddr.Location = new System.Drawing.Point(80, 13);
            this.tbAddr.Name = "tbAddr";
            this.tbAddr.Size = new System.Drawing.Size(63, 20);
            this.tbAddr.TabIndex = 2;
            this.tbAddr.Text = "00000000";
            this.tbAddr.TextChanged += new System.EventHandler(this.tbAddr_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "if the value at";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbComp);
            this.groupBox1.Controls.Add(this.tbCodes);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Controls.Add(this.tbValue);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbAddr);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 365);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conditional Settings";
            // 
            // tbCodes
            // 
            this.tbCodes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCodes.HideSelection = false;
            this.tbCodes.Location = new System.Drawing.Point(6, 52);
            this.tbCodes.Multiline = true;
            this.tbCodes.Name = "tbCodes";
            this.tbCodes.Size = new System.Drawing.Size(453, 307);
            this.tbCodes.TabIndex = 9;
            this.tbCodes.WordWrap = false;
            this.tbCodes.TextChanged += new System.EventHandler(this.tbCodes_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(201, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "then the following codes will be executed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(362, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "of type";
            // 
            // cbType
            // 
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Hex",
            "Dec",
            "Float",
            "Double",
            "Text"});
            this.cbType.Location = new System.Drawing.Point(407, 13);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(52, 21);
            this.cbType.TabIndex = 6;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // tbValue
            // 
            this.tbValue.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValue.Location = new System.Drawing.Point(246, 13);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(110, 20);
            this.tbValue.TabIndex = 5;
            this.tbValue.Text = "00000000";
            this.tbValue.TextChanged += new System.EventHandler(this.tbValue_TextChanged);
            // 
            // buttOkay
            // 
            this.buttOkay.Location = new System.Drawing.Point(12, 383);
            this.buttOkay.Name = "buttOkay";
            this.buttOkay.Size = new System.Drawing.Size(75, 23);
            this.buttOkay.TabIndex = 10;
            this.buttOkay.Text = "Okay";
            this.buttOkay.UseVisualStyleBackColor = true;
            this.buttOkay.Click += new System.EventHandler(this.buttOkay_Click);
            // 
            // buttCancel
            // 
            this.buttCancel.Location = new System.Drawing.Point(92, 383);
            this.buttCancel.Name = "buttCancel";
            this.buttCancel.Size = new System.Drawing.Size(75, 23);
            this.buttCancel.TabIndex = 11;
            this.buttCancel.Text = "Cancel";
            this.buttCancel.UseVisualStyleBackColor = true;
            this.buttCancel.Click += new System.EventHandler(this.buttCancel_Click);
            // 
            // cbComp
            // 
            this.cbComp.FormattingEnabled = true;
            this.cbComp.Items.AddRange(new object[] {
            "is equal to",
            "is AND equal to"});
            this.cbComp.Location = new System.Drawing.Point(149, 13);
            this.cbComp.Name = "cbComp";
            this.cbComp.Size = new System.Drawing.Size(91, 21);
            this.cbComp.TabIndex = 10;
            this.cbComp.Text = "is and equal to";
            // 
            // ConditionalEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 410);
            this.Controls.Add(this.buttCancel);
            this.Controls.Add(this.buttOkay);
            this.Controls.Add(this.groupBox1);
            this.Name = "ConditionalEditor";
            this.Text = "Conditional Editor";
            this.Shown += new System.EventHandler(this.ConditionalEditor_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbAddr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCodes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttOkay;
        private System.Windows.Forms.Button buttCancel;
        private System.Windows.Forms.ComboBox cbComp;
    }
}