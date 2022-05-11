namespace NetCheatPS3
{
    partial class SearchListView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.addrLabel = new System.Windows.Forms.Label();
            this.hexValLabel = new System.Windows.Forms.Label();
            this.decValLabel = new System.Windows.Forms.Label();
            this.alignLabel = new System.Windows.Forms.Label();
            this.vertSBar = new System.Windows.Forms.VScrollBar();
            this.printBox = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshFromPS3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.printBox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // addrLabel
            // 
            this.addrLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.addrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addrLabel.Location = new System.Drawing.Point(3, 2);
            this.addrLabel.Name = "addrLabel";
            this.addrLabel.Size = new System.Drawing.Size(117, 19);
            this.addrLabel.TabIndex = 1;
            this.addrLabel.Text = "Address";
            this.addrLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hexValLabel
            // 
            this.hexValLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hexValLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexValLabel.Location = new System.Drawing.Point(127, 2);
            this.hexValLabel.Name = "hexValLabel";
            this.hexValLabel.Size = new System.Drawing.Size(117, 19);
            this.hexValLabel.TabIndex = 2;
            this.hexValLabel.Text = "Value";
            this.hexValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // decValLabel
            // 
            this.decValLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.decValLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.decValLabel.Location = new System.Drawing.Point(251, 2);
            this.decValLabel.Name = "decValLabel";
            this.decValLabel.Size = new System.Drawing.Size(117, 19);
            this.decValLabel.TabIndex = 3;
            this.decValLabel.Text = "Dec";
            this.decValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // alignLabel
            // 
            this.alignLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.alignLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alignLabel.Location = new System.Drawing.Point(374, 2);
            this.alignLabel.Name = "alignLabel";
            this.alignLabel.Size = new System.Drawing.Size(117, 19);
            this.alignLabel.TabIndex = 4;
            this.alignLabel.Text = "Type";
            this.alignLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // vertSBar
            // 
            this.vertSBar.Location = new System.Drawing.Point(471, 25);
            this.vertSBar.Name = "vertSBar";
            this.vertSBar.Size = new System.Drawing.Size(17, 209);
            this.vertSBar.TabIndex = 0;
            this.vertSBar.Visible = false;
            this.vertSBar.ValueChanged += new System.EventHandler(this.vertSBar_ValueChanged);
            this.vertSBar.VisibleChanged += new System.EventHandler(this.vertSBar_VisibleChanged);
            // 
            // printBox
            // 
            this.printBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.printBox.Location = new System.Drawing.Point(3, 25);
            this.printBox.Name = "printBox";
            this.printBox.Size = new System.Drawing.Size(465, 209);
            this.printBox.TabIndex = 6;
            this.printBox.TabStop = false;
            this.printBox.Click += new System.EventHandler(this.printBox_Click);
            this.printBox.Paint += new System.Windows.Forms.PaintEventHandler(this.printBox_Paint);
            this.printBox.DoubleClick += new System.EventHandler(this.printBox_DoubleClick);
            this.printBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.printBox_MouseClick);
            this.printBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.printBox_MouseDown);
            this.printBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.printBox_MouseUp);
            this.printBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.printBox_PreviewKeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.refreshFromPS3ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(142, 92);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // refreshFromPS3ToolStripMenuItem
            // 
            this.refreshFromPS3ToolStripMenuItem.Name = "refreshFromPS3ToolStripMenuItem";
            this.refreshFromPS3ToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.refreshFromPS3ToolStripMenuItem.Text = "Refresh From PS3";
            this.refreshFromPS3ToolStripMenuItem.Click += new System.EventHandler(this.refreshFromPS3ToolStripMenuItem_Click);
            // 
            // SearchListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.printBox);
            this.Controls.Add(this.vertSBar);
            this.Controls.Add(this.decValLabel);
            this.Controls.Add(this.alignLabel);
            this.Controls.Add(this.hexValLabel);
            this.Controls.Add(this.addrLabel);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SearchListView";
            this.Size = new System.Drawing.Size(492, 234);
            this.Load += new System.EventHandler(this.SearchListView_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SearchListView_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchListView_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchListView_KeyUp);
            this.Resize += new System.EventHandler(this.SearchListView_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.printBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label addrLabel;
        private System.Windows.Forms.Label hexValLabel;
        private System.Windows.Forms.Label decValLabel;
        private System.Windows.Forms.Label alignLabel;
        private System.Windows.Forms.VScrollBar vertSBar;
        private System.Windows.Forms.PictureBox printBox;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshFromPS3ToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
