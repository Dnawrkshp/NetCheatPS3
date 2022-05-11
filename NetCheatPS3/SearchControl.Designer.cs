namespace NetCheatPS3
{
    partial class SearchControl
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
            this.searchNameBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.searchTypeBox = new System.Windows.Forms.ComboBox();
            this.startAddrTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.stopAddrTB = new System.Windows.Forms.TextBox();
            this.searchMemory = new System.Windows.Forms.Button();
            this.refreshFromMem = new System.Windows.Forms.Button();
            this.dumpMem = new System.Windows.Forms.Button();
            this.saveScan = new System.Windows.Forms.Button();
            this.loadScan = new System.Windows.Forms.Button();
            this.searchPWS = new System.Windows.Forms.CheckBox();
            this.nextSearchMem = new System.Windows.Forms.Button();
            this.progBar = new NetCheatPS3.ProgressBar();
            this.SuspendLayout();
            // 
            // searchNameBox
            // 
            this.searchNameBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchNameBox.FormattingEnabled = true;
            this.searchNameBox.Location = new System.Drawing.Point(58, 81);
            this.searchNameBox.Name = "searchNameBox";
            this.searchNameBox.Size = new System.Drawing.Size(180, 21);
            this.searchNameBox.TabIndex = 1;
            this.searchNameBox.SelectedIndexChanged += new System.EventHandler(this.searchNameBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Search";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Type";
            // 
            // searchTypeBox
            // 
            this.searchTypeBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchTypeBox.FormattingEnabled = true;
            this.searchTypeBox.Location = new System.Drawing.Point(58, 103);
            this.searchTypeBox.Name = "searchTypeBox";
            this.searchTypeBox.Size = new System.Drawing.Size(180, 21);
            this.searchTypeBox.TabIndex = 4;
            this.searchTypeBox.SelectedIndexChanged += new System.EventHandler(this.searchTypeBox_SelectedIndexChanged);
            // 
            // startAddrTB
            // 
            this.startAddrTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.startAddrTB.Location = new System.Drawing.Point(320, 81);
            this.startAddrTB.Name = "startAddrTB";
            this.startAddrTB.Size = new System.Drawing.Size(117, 20);
            this.startAddrTB.TabIndex = 5;
            this.startAddrTB.Text = "00010000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Start Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(244, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Stop Address";
            // 
            // stopAddrTB
            // 
            this.stopAddrTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stopAddrTB.Location = new System.Drawing.Point(320, 103);
            this.stopAddrTB.Name = "stopAddrTB";
            this.stopAddrTB.Size = new System.Drawing.Size(117, 20);
            this.stopAddrTB.TabIndex = 7;
            this.stopAddrTB.Text = "00020000";
            // 
            // searchMemory
            // 
            this.searchMemory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchMemory.Location = new System.Drawing.Point(3, 155);
            this.searchMemory.Name = "searchMemory";
            this.searchMemory.Size = new System.Drawing.Size(84, 23);
            this.searchMemory.TabIndex = 9;
            this.searchMemory.Text = "Initial Scan";
            this.searchMemory.UseVisualStyleBackColor = true;
            this.searchMemory.Click += new System.EventHandler(this.searchMemory_Click);
            // 
            // refreshFromMem
            // 
            this.refreshFromMem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshFromMem.Location = new System.Drawing.Point(94, 155);
            this.refreshFromMem.Name = "refreshFromMem";
            this.refreshFromMem.Size = new System.Drawing.Size(303, 23);
            this.refreshFromMem.TabIndex = 11;
            this.refreshFromMem.Text = "Refresh From Memory";
            this.refreshFromMem.UseVisualStyleBackColor = true;
            this.refreshFromMem.Click += new System.EventHandler(this.refreshFromMem_Click);
            // 
            // dumpMem
            // 
            this.dumpMem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dumpMem.Location = new System.Drawing.Point(3, 126);
            this.dumpMem.Name = "dumpMem";
            this.dumpMem.Size = new System.Drawing.Size(85, 23);
            this.dumpMem.TabIndex = 12;
            this.dumpMem.Text = "Dump Memory";
            this.dumpMem.UseVisualStyleBackColor = true;
            this.dumpMem.Click += new System.EventHandler(this.dumpMem_Click);
            // 
            // saveScan
            // 
            this.saveScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveScan.Location = new System.Drawing.Point(94, 126);
            this.saveScan.Name = "saveScan";
            this.saveScan.Size = new System.Drawing.Size(85, 23);
            this.saveScan.TabIndex = 13;
            this.saveScan.Text = "Save Scan";
            this.saveScan.UseVisualStyleBackColor = true;
            this.saveScan.Click += new System.EventHandler(this.saveScan_Click);
            // 
            // loadScan
            // 
            this.loadScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadScan.Location = new System.Drawing.Point(185, 126);
            this.loadScan.Name = "loadScan";
            this.loadScan.Size = new System.Drawing.Size(85, 23);
            this.loadScan.TabIndex = 14;
            this.loadScan.Text = "Load Scan";
            this.loadScan.UseVisualStyleBackColor = true;
            this.loadScan.Click += new System.EventHandler(this.loadScan_Click);
            // 
            // searchPWS
            // 
            this.searchPWS.AutoSize = true;
            this.searchPWS.Location = new System.Drawing.Point(276, 130);
            this.searchPWS.Name = "searchPWS";
            this.searchPWS.Size = new System.Drawing.Size(136, 17);
            this.searchPWS.TabIndex = 40;
            this.searchPWS.Text = "Pause When Scanning";
            this.searchPWS.UseVisualStyleBackColor = true;
            // 
            // nextSearchMem
            // 
            this.nextSearchMem.Enabled = false;
            this.nextSearchMem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextSearchMem.Location = new System.Drawing.Point(403, 155);
            this.nextSearchMem.Name = "nextSearchMem";
            this.nextSearchMem.Size = new System.Drawing.Size(84, 23);
            this.nextSearchMem.TabIndex = 42;
            this.nextSearchMem.Text = "Next Scan";
            this.nextSearchMem.UseVisualStyleBackColor = true;
            this.nextSearchMem.Click += new System.EventHandler(this.nextSearchMem_Click);
            // 
            // progBar
            // 
            this.progBar.BackColor = System.Drawing.Color.White;
            this.progBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progBar.Location = new System.Drawing.Point(3, 184);
            this.progBar.Margin = new System.Windows.Forms.Padding(1);
            this.progBar.Maximum = 10;
            this.progBar.Name = "progBar";
            this.progBar.printText = "";
            this.progBar.progressColor = System.Drawing.Color.Black;
            this.progBar.Size = new System.Drawing.Size(484, 23);
            this.progBar.TabIndex = 43;
            this.progBar.Value = 0;
            // 
            // SearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progBar);
            this.Controls.Add(this.nextSearchMem);
            this.Controls.Add(this.searchPWS);
            this.Controls.Add(this.loadScan);
            this.Controls.Add(this.saveScan);
            this.Controls.Add(this.dumpMem);
            this.Controls.Add(this.refreshFromMem);
            this.Controls.Add(this.searchMemory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.stopAddrTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.startAddrTB);
            this.Controls.Add(this.searchTypeBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchNameBox);
            this.Name = "SearchControl";
            this.Size = new System.Drawing.Size(490, 394);
            this.Load += new System.EventHandler(this.SearchControl_Load);
            this.Resize += new System.EventHandler(this.SearchControl_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox startAddrTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox stopAddrTB;
        private System.Windows.Forms.Button refreshFromMem;
        private System.Windows.Forms.Button dumpMem;
        private System.Windows.Forms.Button saveScan;
        private System.Windows.Forms.Button loadScan;
        private System.Windows.Forms.CheckBox searchPWS;
        private ProgressBar progBar;
        public System.Windows.Forms.Button searchMemory;
        public System.Windows.Forms.Button nextSearchMem;
        public System.Windows.Forms.ComboBox searchNameBox;
        public System.Windows.Forms.ComboBox searchTypeBox;
    }
}
