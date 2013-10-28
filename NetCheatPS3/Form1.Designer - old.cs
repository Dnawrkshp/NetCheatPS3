namespace NetCheatPS3
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.TabCon = new System.Windows.Forms.TabControl();
            this.CodesTab = new System.Windows.Forms.TabPage();
            this.cbCodes = new System.Windows.Forms.RichTextBox();
            this.cbSaveAll = new System.Windows.Forms.Button();
            this.cbSaveAs = new System.Windows.Forms.Button();
            this.cbImport = new System.Windows.Forms.Button();
            this.cbSave = new System.Windows.Forms.Button();
            this.cbRemove = new System.Windows.Forms.Button();
            this.cbAdd = new System.Windows.Forms.Button();
            this.cbWrite = new System.Windows.Forms.Button();
            this.cbState = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbName = new System.Windows.Forms.TextBox();
            this.cbList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SearchTab = new System.Windows.Forms.TabPage();
            this.saveSRes = new System.Windows.Forms.Button();
            this.loadSRes = new System.Windows.Forms.Button();
            this.schVal2 = new System.Windows.Forms.TextBox();
            this.DumpMem = new System.Windows.Forms.Button();
            this.compBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SchPWS = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.schProg = new System.Windows.Forms.ProgressBar();
            this.SchHexCheck = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbSchAlign = new System.Windows.Forms.ComboBox();
            this.SchRef = new System.Windows.Forms.Button();
            this.lvSch = new System.Windows.Forms.ListView();
            this.lvSchAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSchValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSchDec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvSchAlign = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.schNSearch = new System.Windows.Forms.Button();
            this.schVal = new System.Windows.Forms.TextBox();
            this.schRange2 = new System.Windows.Forms.TextBox();
            this.schRange1 = new System.Windows.Forms.TextBox();
            this.schSearch = new System.Windows.Forms.Button();
            this.RangeTab = new System.Windows.Forms.TabPage();
            this.recRangeBox = new System.Windows.Forms.ListView();
            this.colFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.RangeDown = new System.Windows.Forms.Button();
            this.RangeUp = new System.Windows.Forms.Button();
            this.RemoveRange = new System.Windows.Forms.Button();
            this.AddRange = new System.Windows.Forms.Button();
            this.SaveRange = new System.Windows.Forms.Button();
            this.ImportRange = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rangeView = new System.Windows.Forms.ListView();
            this.StartAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EndAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pluginTab = new System.Windows.Forms.TabPage();
            this.plugIcon = new System.Windows.Forms.PictureBox();
            this.descPlugDesc = new System.Windows.Forms.Label();
            this.descPlugVer = new System.Windows.Forms.Label();
            this.descPlugAuth = new System.Windows.Forms.Label();
            this.descPlugName = new System.Windows.Forms.Label();
            this.pluginList = new System.Windows.Forms.ListBox();
            this.ps3Disc = new System.Windows.Forms.Button();
            this.attachProcessButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.refPlugin = new System.Windows.Forms.Button();
            this.optButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownPS3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshFromPS3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshFromDumptxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TabCon.SuspendLayout();
            this.CodesTab.SuspendLayout();
            this.SearchTab.SuspendLayout();
            this.RangeTab.SuspendLayout();
            this.pluginTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plugIcon)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabCon
            // 
            this.TabCon.Controls.Add(this.CodesTab);
            this.TabCon.Controls.Add(this.SearchTab);
            this.TabCon.Controls.Add(this.RangeTab);
            this.TabCon.Controls.Add(this.pluginTab);
            this.TabCon.Location = new System.Drawing.Point(12, 42);
            this.TabCon.Name = "TabCon";
            this.TabCon.SelectedIndex = 0;
            this.TabCon.Size = new System.Drawing.Size(461, 393);
            this.TabCon.TabIndex = 0;
            this.TabCon.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TabCon_KeyUp);
            // 
            // CodesTab
            // 
            this.CodesTab.AutoScroll = true;
            this.CodesTab.BackColor = System.Drawing.Color.Black;
            this.CodesTab.Controls.Add(this.cbCodes);
            this.CodesTab.Controls.Add(this.cbSaveAll);
            this.CodesTab.Controls.Add(this.cbSaveAs);
            this.CodesTab.Controls.Add(this.cbImport);
            this.CodesTab.Controls.Add(this.cbSave);
            this.CodesTab.Controls.Add(this.cbRemove);
            this.CodesTab.Controls.Add(this.cbAdd);
            this.CodesTab.Controls.Add(this.cbWrite);
            this.CodesTab.Controls.Add(this.cbState);
            this.CodesTab.Controls.Add(this.label5);
            this.CodesTab.Controls.Add(this.cbName);
            this.CodesTab.Controls.Add(this.cbList);
            this.CodesTab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.CodesTab.Location = new System.Drawing.Point(4, 22);
            this.CodesTab.Name = "CodesTab";
            this.CodesTab.Padding = new System.Windows.Forms.Padding(3);
            this.CodesTab.Size = new System.Drawing.Size(453, 367);
            this.CodesTab.TabIndex = 0;
            this.CodesTab.Text = "Codes";
            // 
            // cbCodes
            // 
            this.cbCodes.BackColor = System.Drawing.Color.Black;
            this.cbCodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbCodes.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCodes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.cbCodes.Location = new System.Drawing.Point(265, 79);
            this.cbCodes.Name = "cbCodes";
            this.cbCodes.Size = new System.Drawing.Size(179, 225);
            this.cbCodes.TabIndex = 32;
            this.cbCodes.Text = "";
            this.cbCodes.TextChanged += new System.EventHandler(this.cbCodes_TextChanged);
            this.cbCodes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbCodes_KeyUp);
            // 
            // cbSaveAll
            // 
            this.cbSaveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSaveAll.Location = new System.Drawing.Point(77, 337);
            this.cbSaveAll.Name = "cbSaveAll";
            this.cbSaveAll.Size = new System.Drawing.Size(65, 21);
            this.cbSaveAll.TabIndex = 31;
            this.cbSaveAll.Text = "Save All";
            this.cbSaveAll.UseVisualStyleBackColor = true;
            this.cbSaveAll.Click += new System.EventHandler(this.cbSaveAll_Click);
            // 
            // cbSaveAs
            // 
            this.cbSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSaveAs.Location = new System.Drawing.Point(6, 337);
            this.cbSaveAs.Name = "cbSaveAs";
            this.cbSaveAs.Size = new System.Drawing.Size(65, 21);
            this.cbSaveAs.TabIndex = 30;
            this.cbSaveAs.Text = "Save As";
            this.cbSaveAs.UseVisualStyleBackColor = true;
            this.cbSaveAs.Click += new System.EventHandler(this.cbSaveAs_Click);
            // 
            // cbImport
            // 
            this.cbImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbImport.Location = new System.Drawing.Point(148, 310);
            this.cbImport.Name = "cbImport";
            this.cbImport.Size = new System.Drawing.Size(65, 21);
            this.cbImport.TabIndex = 29;
            this.cbImport.Text = "Import";
            this.cbImport.UseVisualStyleBackColor = true;
            this.cbImport.Click += new System.EventHandler(this.cbImport_Click);
            // 
            // cbSave
            // 
            this.cbSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSave.Location = new System.Drawing.Point(148, 337);
            this.cbSave.Name = "cbSave";
            this.cbSave.Size = new System.Drawing.Size(65, 21);
            this.cbSave.TabIndex = 28;
            this.cbSave.Text = "Save";
            this.cbSave.UseVisualStyleBackColor = true;
            this.cbSave.Click += new System.EventHandler(this.cbSave_Click);
            // 
            // cbRemove
            // 
            this.cbRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRemove.Location = new System.Drawing.Point(77, 310);
            this.cbRemove.Name = "cbRemove";
            this.cbRemove.Size = new System.Drawing.Size(65, 21);
            this.cbRemove.TabIndex = 26;
            this.cbRemove.Text = "Remove";
            this.cbRemove.UseVisualStyleBackColor = true;
            this.cbRemove.Click += new System.EventHandler(this.cbRemove_Click);
            // 
            // cbAdd
            // 
            this.cbAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAdd.Location = new System.Drawing.Point(6, 310);
            this.cbAdd.Name = "cbAdd";
            this.cbAdd.Size = new System.Drawing.Size(65, 21);
            this.cbAdd.TabIndex = 25;
            this.cbAdd.Text = "Add";
            this.cbAdd.UseVisualStyleBackColor = true;
            this.cbAdd.Click += new System.EventHandler(this.cbAdd_Click);
            // 
            // cbWrite
            // 
            this.cbWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbWrite.Location = new System.Drawing.Point(265, 319);
            this.cbWrite.Name = "cbWrite";
            this.cbWrite.Size = new System.Drawing.Size(180, 23);
            this.cbWrite.TabIndex = 24;
            this.cbWrite.Text = "Write";
            this.cbWrite.UseVisualStyleBackColor = true;
            this.cbWrite.Click += new System.EventHandler(this.cbWrite_Click);
            // 
            // cbState
            // 
            this.cbState.Location = new System.Drawing.Point(265, 55);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(180, 17);
            this.cbState.TabIndex = 23;
            this.cbState.Text = "Constant Write";
            this.cbState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbState.UseVisualStyleBackColor = true;
            this.cbState.CheckedChanged += new System.EventHandler(this.cbState_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(265, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(181, 20);
            this.label5.TabIndex = 22;
            this.label5.Text = "Name of code";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cbName
            // 
            this.cbName.BackColor = System.Drawing.Color.Black;
            this.cbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbName.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.cbName.Location = new System.Drawing.Point(265, 29);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(181, 20);
            this.cbName.TabIndex = 21;
            this.cbName.TextChanged += new System.EventHandler(this.cbName_TextChanged);
            // 
            // cbList
            // 
            this.cbList.BackColor = System.Drawing.Color.Black;
            this.cbList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.cbList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.cbList.FullRowSelect = true;
            this.cbList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.cbList.HideSelection = false;
            this.cbList.LabelWrap = false;
            this.cbList.Location = new System.Drawing.Point(6, 6);
            this.cbList.Name = "cbList";
            this.cbList.Size = new System.Drawing.Size(207, 298);
            this.cbList.TabIndex = 20;
            this.cbList.UseCompatibleStateImageBehavior = false;
            this.cbList.View = System.Windows.Forms.View.Details;
            this.cbList.SelectedIndexChanged += new System.EventHandler(this.cbList_SelectedIndexChanged);
            this.cbList.DoubleClick += new System.EventHandler(this.cbList_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Codes";
            this.columnHeader1.Width = 150;
            // 
            // SearchTab
            // 
            this.SearchTab.AutoScroll = true;
            this.SearchTab.BackColor = System.Drawing.Color.Black;
            this.SearchTab.Controls.Add(this.saveSRes);
            this.SearchTab.Controls.Add(this.loadSRes);
            this.SearchTab.Controls.Add(this.schVal2);
            this.SearchTab.Controls.Add(this.DumpMem);
            this.SearchTab.Controls.Add(this.compBox);
            this.SearchTab.Controls.Add(this.label8);
            this.SearchTab.Controls.Add(this.SchPWS);
            this.SearchTab.Controls.Add(this.label6);
            this.SearchTab.Controls.Add(this.label7);
            this.SearchTab.Controls.Add(this.schProg);
            this.SearchTab.Controls.Add(this.SchHexCheck);
            this.SearchTab.Controls.Add(this.label9);
            this.SearchTab.Controls.Add(this.cbSchAlign);
            this.SearchTab.Controls.Add(this.SchRef);
            this.SearchTab.Controls.Add(this.lvSch);
            this.SearchTab.Controls.Add(this.schNSearch);
            this.SearchTab.Controls.Add(this.schVal);
            this.SearchTab.Controls.Add(this.schRange2);
            this.SearchTab.Controls.Add(this.schRange1);
            this.SearchTab.Controls.Add(this.schSearch);
            this.SearchTab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.SearchTab.Location = new System.Drawing.Point(4, 22);
            this.SearchTab.Name = "SearchTab";
            this.SearchTab.Padding = new System.Windows.Forms.Padding(3);
            this.SearchTab.Size = new System.Drawing.Size(453, 367);
            this.SearchTab.TabIndex = 1;
            this.SearchTab.Text = "Search";
            // 
            // saveSRes
            // 
            this.saveSRes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveSRes.Location = new System.Drawing.Point(230, 138);
            this.saveSRes.Name = "saveSRes";
            this.saveSRes.Size = new System.Drawing.Size(110, 28);
            this.saveSRes.TabIndex = 46;
            this.saveSRes.Text = "Save Scan Results";
            this.saveSRes.UseVisualStyleBackColor = true;
            this.saveSRes.Click += new System.EventHandler(this.saveSRes_Click);
            // 
            // loadSRes
            // 
            this.loadSRes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadSRes.Location = new System.Drawing.Point(114, 138);
            this.loadSRes.Name = "loadSRes";
            this.loadSRes.Size = new System.Drawing.Size(110, 28);
            this.loadSRes.TabIndex = 45;
            this.loadSRes.Text = "Load Scan Results";
            this.loadSRes.UseVisualStyleBackColor = true;
            this.loadSRes.Click += new System.EventHandler(this.loadSRes_Click);
            // 
            // schVal2
            // 
            this.schVal2.BackColor = System.Drawing.Color.Black;
            this.schVal2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.schVal2.Location = new System.Drawing.Point(181, 3);
            this.schVal2.Name = "schVal2";
            this.schVal2.Size = new System.Drawing.Size(120, 20);
            this.schVal2.TabIndex = 44;
            this.schVal2.Text = "00000001";
            this.schVal2.Visible = false;
            // 
            // DumpMem
            // 
            this.DumpMem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DumpMem.Location = new System.Drawing.Point(9, 138);
            this.DumpMem.Name = "DumpMem";
            this.DumpMem.Size = new System.Drawing.Size(99, 28);
            this.DumpMem.TabIndex = 42;
            this.DumpMem.Text = "Dump Memory";
            this.DumpMem.UseVisualStyleBackColor = true;
            this.DumpMem.Click += new System.EventHandler(this.DumpMem_Click);
            // 
            // compBox
            // 
            this.compBox.BackColor = System.Drawing.Color.Black;
            this.compBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.compBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.compBox.FormattingEnabled = true;
            this.compBox.Items.AddRange(new object[] {
            "Equal",
            "Not Equal",
            "Less Than",
            "Less Than Or Equal",
            "Greater Than",
            "Greater Than Or Equal",
            "Value Between"});
            this.compBox.Location = new System.Drawing.Point(55, 29);
            this.compBox.Name = "compBox";
            this.compBox.Size = new System.Drawing.Size(138, 21);
            this.compBox.TabIndex = 41;
            this.compBox.SelectedIndexChanged += new System.EventHandler(this.compBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(1, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 15);
            this.label8.TabIndex = 40;
            this.label8.Text = "Compare";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SchPWS
            // 
            this.SchPWS.AutoSize = true;
            this.SchPWS.Location = new System.Drawing.Point(311, 29);
            this.SchPWS.Name = "SchPWS";
            this.SchPWS.Size = new System.Drawing.Size(136, 17);
            this.SchPWS.TabIndex = 39;
            this.SchPWS.Text = "Pause When Scanning";
            this.SchPWS.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(21, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 22);
            this.label6.TabIndex = 38;
            this.label6.Text = "Stop";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(21, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 22);
            this.label7.TabIndex = 37;
            this.label7.Text = "Start";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // schProg
            // 
            this.schProg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(210)))));
            this.schProg.Location = new System.Drawing.Point(9, 170);
            this.schProg.Name = "schProg";
            this.schProg.Size = new System.Drawing.Size(440, 26);
            this.schProg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.schProg.TabIndex = 36;
            // 
            // SchHexCheck
            // 
            this.SchHexCheck.AutoSize = true;
            this.SchHexCheck.Checked = true;
            this.SchHexCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SchHexCheck.Location = new System.Drawing.Point(4, 6);
            this.SchHexCheck.Name = "SchHexCheck";
            this.SchHexCheck.Size = new System.Drawing.Size(45, 17);
            this.SchHexCheck.TabIndex = 35;
            this.SchHexCheck.Text = "Hex";
            this.SchHexCheck.UseVisualStyleBackColor = true;
            this.SchHexCheck.CheckedChanged += new System.EventHandler(this.SchHexCheck_CheckedChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(1, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 15);
            this.label9.TabIndex = 34;
            this.label9.Text = "Val Type";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbSchAlign
            // 
            this.cbSchAlign.BackColor = System.Drawing.Color.Black;
            this.cbSchAlign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSchAlign.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.cbSchAlign.FormattingEnabled = true;
            this.cbSchAlign.Items.AddRange(new object[] {
            "1 byte",
            "2 bytes",
            "4 bytes",
            "8 bytes",
            "X bytes",
            "Text"});
            this.cbSchAlign.Location = new System.Drawing.Point(55, 56);
            this.cbSchAlign.Name = "cbSchAlign";
            this.cbSchAlign.Size = new System.Drawing.Size(138, 21);
            this.cbSchAlign.TabIndex = 33;
            this.cbSchAlign.SelectedIndexChanged += new System.EventHandler(this.cbSchAlign_SelectedIndexChanged);
            // 
            // SchRef
            // 
            this.SchRef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SchRef.Location = new System.Drawing.Point(127, 203);
            this.SchRef.Name = "SchRef";
            this.SchRef.Size = new System.Drawing.Size(200, 28);
            this.SchRef.TabIndex = 32;
            this.SchRef.Text = "Refresh Results From PS3";
            this.SchRef.UseVisualStyleBackColor = true;
            this.SchRef.Click += new System.EventHandler(this.SchRef_Click);
            // 
            // lvSch
            // 
            this.lvSch.BackColor = System.Drawing.Color.Black;
            this.lvSch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvSchAddr,
            this.lvSchValue,
            this.lvSchDec,
            this.lvSchAlign});
            this.lvSch.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.lvSch.FullRowSelect = true;
            this.lvSch.HideSelection = false;
            this.lvSch.LabelEdit = true;
            this.lvSch.LabelWrap = false;
            this.lvSch.Location = new System.Drawing.Point(9, 237);
            this.lvSch.Name = "lvSch";
            this.lvSch.Size = new System.Drawing.Size(440, 127);
            this.lvSch.TabIndex = 31;
            this.lvSch.UseCompatibleStateImageBehavior = false;
            this.lvSch.View = System.Windows.Forms.View.Details;
            this.lvSch.SelectedIndexChanged += new System.EventHandler(this.lvSch_SelectedIndexChanged);
            this.lvSch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvSch_KeyUp);
            this.lvSch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvSch_MouseUp);
            // 
            // lvSchAddr
            // 
            this.lvSchAddr.Text = "Address";
            this.lvSchAddr.Width = 80;
            // 
            // lvSchValue
            // 
            this.lvSchValue.Text = "Hex Value";
            this.lvSchValue.Width = 120;
            // 
            // lvSchDec
            // 
            this.lvSchDec.Text = "Dec Value";
            this.lvSchDec.Width = 130;
            // 
            // lvSchAlign
            // 
            this.lvSchAlign.Text = "Alignment";
            this.lvSchAlign.Width = 80;
            // 
            // schNSearch
            // 
            this.schNSearch.Enabled = false;
            this.schNSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schNSearch.Location = new System.Drawing.Point(347, 203);
            this.schNSearch.Name = "schNSearch";
            this.schNSearch.Size = new System.Drawing.Size(100, 28);
            this.schNSearch.TabIndex = 30;
            this.schNSearch.Text = "Next Scan";
            this.schNSearch.UseVisualStyleBackColor = true;
            this.schNSearch.Click += new System.EventHandler(this.schNSearch_Click);
            // 
            // schVal
            // 
            this.schVal.BackColor = System.Drawing.Color.Black;
            this.schVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.schVal.Location = new System.Drawing.Point(55, 4);
            this.schVal.Name = "schVal";
            this.schVal.Size = new System.Drawing.Size(120, 20);
            this.schVal.TabIndex = 29;
            this.schVal.Text = "00000001";
            // 
            // schRange2
            // 
            this.schRange2.BackColor = System.Drawing.Color.Black;
            this.schRange2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.schRange2.Location = new System.Drawing.Point(55, 109);
            this.schRange2.Name = "schRange2";
            this.schRange2.Size = new System.Drawing.Size(59, 20);
            this.schRange2.TabIndex = 28;
            this.schRange2.Text = "00000000";
            // 
            // schRange1
            // 
            this.schRange1.BackColor = System.Drawing.Color.Black;
            this.schRange1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.schRange1.Location = new System.Drawing.Point(55, 83);
            this.schRange1.Name = "schRange1";
            this.schRange1.Size = new System.Drawing.Size(59, 20);
            this.schRange1.TabIndex = 27;
            this.schRange1.Text = "00000000";
            // 
            // schSearch
            // 
            this.schSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.schSearch.Location = new System.Drawing.Point(9, 203);
            this.schSearch.Name = "schSearch";
            this.schSearch.Size = new System.Drawing.Size(99, 28);
            this.schSearch.TabIndex = 26;
            this.schSearch.Text = "Initial Scan";
            this.schSearch.UseVisualStyleBackColor = true;
            this.schSearch.Click += new System.EventHandler(this.schSearch_Click);
            // 
            // RangeTab
            // 
            this.RangeTab.AutoScroll = true;
            this.RangeTab.BackColor = System.Drawing.Color.Black;
            this.RangeTab.Controls.Add(this.recRangeBox);
            this.RangeTab.Controls.Add(this.label3);
            this.RangeTab.Controls.Add(this.RangeDown);
            this.RangeTab.Controls.Add(this.RangeUp);
            this.RangeTab.Controls.Add(this.RemoveRange);
            this.RangeTab.Controls.Add(this.AddRange);
            this.RangeTab.Controls.Add(this.SaveRange);
            this.RangeTab.Controls.Add(this.ImportRange);
            this.RangeTab.Controls.Add(this.label2);
            this.RangeTab.Controls.Add(this.label1);
            this.RangeTab.Controls.Add(this.rangeView);
            this.RangeTab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.RangeTab.Location = new System.Drawing.Point(4, 22);
            this.RangeTab.Name = "RangeTab";
            this.RangeTab.Padding = new System.Windows.Forms.Padding(3);
            this.RangeTab.Size = new System.Drawing.Size(453, 367);
            this.RangeTab.TabIndex = 2;
            this.RangeTab.Text = "Range";
            // 
            // recRangeBox
            // 
            this.recRangeBox.BackColor = System.Drawing.Color.Black;
            this.recRangeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.recRangeBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName});
            this.recRangeBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.recRangeBox.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.recRangeBox.LabelWrap = false;
            this.recRangeBox.Location = new System.Drawing.Point(235, 60);
            this.recRangeBox.MultiSelect = false;
            this.recRangeBox.Name = "recRangeBox";
            this.recRangeBox.Size = new System.Drawing.Size(212, 192);
            this.recRangeBox.TabIndex = 11;
            this.recRangeBox.TabStop = false;
            this.recRangeBox.UseCompatibleStateImageBehavior = false;
            this.recRangeBox.View = System.Windows.Forms.View.Details;
            this.recRangeBox.DoubleClick += new System.EventHandler(this.recRangeBox_DoubleClick);
            // 
            // colFileName
            // 
            this.colFileName.Text = "File";
            this.colFileName.Width = 204;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(235, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(212, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "Recent Ranges";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RangeDown
            // 
            this.RangeDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RangeDown.Location = new System.Drawing.Point(356, 258);
            this.RangeDown.Name = "RangeDown";
            this.RangeDown.Size = new System.Drawing.Size(91, 30);
            this.RangeDown.TabIndex = 8;
            this.RangeDown.Text = "Down";
            this.RangeDown.UseVisualStyleBackColor = true;
            this.RangeDown.Click += new System.EventHandler(this.RangeDown_Click);
            // 
            // RangeUp
            // 
            this.RangeUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RangeUp.Location = new System.Drawing.Point(235, 258);
            this.RangeUp.Name = "RangeUp";
            this.RangeUp.Size = new System.Drawing.Size(91, 30);
            this.RangeUp.TabIndex = 7;
            this.RangeUp.Text = "Up";
            this.RangeUp.UseVisualStyleBackColor = true;
            this.RangeUp.Click += new System.EventHandler(this.RangeUp_Click);
            // 
            // RemoveRange
            // 
            this.RemoveRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveRange.Location = new System.Drawing.Point(356, 330);
            this.RemoveRange.Name = "RemoveRange";
            this.RemoveRange.Size = new System.Drawing.Size(91, 30);
            this.RemoveRange.TabIndex = 6;
            this.RemoveRange.Text = "Remove";
            this.RemoveRange.UseVisualStyleBackColor = true;
            this.RemoveRange.Click += new System.EventHandler(this.RemoveRange_Click);
            // 
            // AddRange
            // 
            this.AddRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddRange.Location = new System.Drawing.Point(235, 330);
            this.AddRange.Name = "AddRange";
            this.AddRange.Size = new System.Drawing.Size(91, 30);
            this.AddRange.TabIndex = 5;
            this.AddRange.Text = "Add";
            this.AddRange.UseVisualStyleBackColor = true;
            this.AddRange.Click += new System.EventHandler(this.AddRange_Click);
            // 
            // SaveRange
            // 
            this.SaveRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveRange.Location = new System.Drawing.Point(356, 294);
            this.SaveRange.Name = "SaveRange";
            this.SaveRange.Size = new System.Drawing.Size(91, 30);
            this.SaveRange.TabIndex = 4;
            this.SaveRange.Text = "Save";
            this.SaveRange.UseVisualStyleBackColor = true;
            this.SaveRange.Click += new System.EventHandler(this.SaveRange_Click);
            // 
            // ImportRange
            // 
            this.ImportRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ImportRange.Location = new System.Drawing.Point(235, 294);
            this.ImportRange.Name = "ImportRange";
            this.ImportRange.Size = new System.Drawing.Size(91, 30);
            this.ImportRange.TabIndex = 3;
            this.ImportRange.Text = "Import";
            this.ImportRange.UseVisualStyleBackColor = true;
            this.ImportRange.Click += new System.EventHandler(this.ImportRange_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(441, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Be Sure To Order From Least To Greatest";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(441, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Memory Range Of Game";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rangeView
            // 
            this.rangeView.BackColor = System.Drawing.Color.Black;
            this.rangeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rangeView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.StartAddr,
            this.EndAddr});
            this.rangeView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.rangeView.FullRowSelect = true;
            this.rangeView.HideSelection = false;
            this.rangeView.LabelWrap = false;
            this.rangeView.Location = new System.Drawing.Point(6, 34);
            this.rangeView.MultiSelect = false;
            this.rangeView.Name = "rangeView";
            this.rangeView.Size = new System.Drawing.Size(223, 327);
            this.rangeView.TabIndex = 0;
            this.rangeView.UseCompatibleStateImageBehavior = false;
            this.rangeView.View = System.Windows.Forms.View.Details;
            this.rangeView.SelectedIndexChanged += new System.EventHandler(this.rangeView_SelectedIndexChanged);
            this.rangeView.DoubleClick += new System.EventHandler(this.rangeView_DoubleClick);
            // 
            // StartAddr
            // 
            this.StartAddr.Text = "Start Address";
            this.StartAddr.Width = 110;
            // 
            // EndAddr
            // 
            this.EndAddr.Text = "End Address";
            this.EndAddr.Width = 110;
            // 
            // pluginTab
            // 
            this.pluginTab.BackColor = System.Drawing.Color.Black;
            this.pluginTab.Controls.Add(this.plugIcon);
            this.pluginTab.Controls.Add(this.descPlugDesc);
            this.pluginTab.Controls.Add(this.descPlugVer);
            this.pluginTab.Controls.Add(this.descPlugAuth);
            this.pluginTab.Controls.Add(this.descPlugName);
            this.pluginTab.Controls.Add(this.pluginList);
            this.pluginTab.Location = new System.Drawing.Point(4, 22);
            this.pluginTab.Name = "pluginTab";
            this.pluginTab.Size = new System.Drawing.Size(453, 367);
            this.pluginTab.TabIndex = 3;
            this.pluginTab.Text = "Plugins";
            // 
            // plugIcon
            // 
            this.plugIcon.InitialImage = ((System.Drawing.Image)(resources.GetObject("plugIcon.InitialImage")));
            this.plugIcon.Location = new System.Drawing.Point(184, 147);
            this.plugIcon.Name = "plugIcon";
            this.plugIcon.Size = new System.Drawing.Size(266, 210);
            this.plugIcon.TabIndex = 11;
            this.plugIcon.TabStop = false;
            // 
            // descPlugDesc
            // 
            this.descPlugDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descPlugDesc.Location = new System.Drawing.Point(187, 43);
            this.descPlugDesc.Name = "descPlugDesc";
            this.descPlugDesc.Size = new System.Drawing.Size(263, 101);
            this.descPlugDesc.TabIndex = 8;
            this.descPlugDesc.Text = "Plugin Description";
            // 
            // descPlugVer
            // 
            this.descPlugVer.AutoSize = true;
            this.descPlugVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descPlugVer.Location = new System.Drawing.Point(206, 30);
            this.descPlugVer.Name = "descPlugVer";
            this.descPlugVer.Size = new System.Drawing.Size(74, 13);
            this.descPlugVer.TabIndex = 6;
            this.descPlugVer.Text = "Plugin Version";
            // 
            // descPlugAuth
            // 
            this.descPlugAuth.AutoSize = true;
            this.descPlugAuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descPlugAuth.Location = new System.Drawing.Point(206, 17);
            this.descPlugAuth.Name = "descPlugAuth";
            this.descPlugAuth.Size = new System.Drawing.Size(100, 13);
            this.descPlugAuth.TabIndex = 4;
            this.descPlugAuth.Text = "by Plugin Author";
            // 
            // descPlugName
            // 
            this.descPlugName.AutoSize = true;
            this.descPlugName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descPlugName.Location = new System.Drawing.Point(184, 4);
            this.descPlugName.Name = "descPlugName";
            this.descPlugName.Size = new System.Drawing.Size(78, 13);
            this.descPlugName.TabIndex = 2;
            this.descPlugName.Text = "Plugin Name";
            // 
            // pluginList
            // 
            this.pluginList.BackColor = System.Drawing.Color.Black;
            this.pluginList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pluginList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.pluginList.FormattingEnabled = true;
            this.pluginList.Location = new System.Drawing.Point(4, 4);
            this.pluginList.Name = "pluginList";
            this.pluginList.Size = new System.Drawing.Size(174, 353);
            this.pluginList.TabIndex = 0;
            this.pluginList.SelectedIndexChanged += new System.EventHandler(this.pluginList_SelectedIndexChanged);
            this.pluginList.DoubleClick += new System.EventHandler(this.pluginList_DoubleClick);
            // 
            // ps3Disc
            // 
            this.ps3Disc.BackColor = System.Drawing.Color.Black;
            this.ps3Disc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ps3Disc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.ps3Disc.Location = new System.Drawing.Point(200, 12);
            this.ps3Disc.Name = "ps3Disc";
            this.ps3Disc.Size = new System.Drawing.Size(87, 23);
            this.ps3Disc.TabIndex = 8;
            this.ps3Disc.Text = "Disconnect";
            this.ps3Disc.UseVisualStyleBackColor = false;
            this.ps3Disc.Click += new System.EventHandler(this.ps3Disc_Click);
            // 
            // attachProcessButton
            // 
            this.attachProcessButton.BackColor = System.Drawing.Color.Black;
            this.attachProcessButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.attachProcessButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.attachProcessButton.Location = new System.Drawing.Point(107, 12);
            this.attachProcessButton.Name = "attachProcessButton";
            this.attachProcessButton.Size = new System.Drawing.Size(87, 23);
            this.attachProcessButton.TabIndex = 7;
            this.attachProcessButton.Text = "Attach Process";
            this.attachProcessButton.UseVisualStyleBackColor = false;
            this.attachProcessButton.Click += new System.EventHandler(this.attachProcessButton_Click);
            // 
            // connectButton
            // 
            this.connectButton.BackColor = System.Drawing.Color.Black;
            this.connectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connectButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.connectButton.Location = new System.Drawing.Point(14, 12);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(87, 23);
            this.connectButton.TabIndex = 6;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = false;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // refPlugin
            // 
            this.refPlugin.BackColor = System.Drawing.Color.Black;
            this.refPlugin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refPlugin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.refPlugin.Location = new System.Drawing.Point(293, 12);
            this.refPlugin.Name = "refPlugin";
            this.refPlugin.Size = new System.Drawing.Size(87, 23);
            this.refPlugin.TabIndex = 10;
            this.refPlugin.Text = "Load Plugins";
            this.refPlugin.UseVisualStyleBackColor = false;
            this.refPlugin.Click += new System.EventHandler(this.refPlugin_Click);
            // 
            // optButton
            // 
            this.optButton.BackColor = System.Drawing.Color.Black;
            this.optButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.optButton.Location = new System.Drawing.Point(386, 12);
            this.optButton.Name = "optButton";
            this.optButton.Size = new System.Drawing.Size(87, 23);
            this.optButton.TabIndex = 11;
            this.optButton.Text = "Options";
            this.optButton.UseVisualStyleBackColor = false;
            this.optButton.Click += new System.EventHandler(this.optButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Black;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel1,
            this.toolStripDropDownButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 441);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(485, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel1
            // 
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // loadPluginsToolStripMenuItem
            // 
            this.loadPluginsToolStripMenuItem.Name = "loadPluginsToolStripMenuItem";
            this.loadPluginsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadPluginsToolStripMenuItem.Text = "Load Plugins";
            this.loadPluginsToolStripMenuItem.Click += new System.EventHandler(this.loadPluginsToolStripMenuItem_Click);
            // 
            // shutdownPS3ToolStripMenuItem
            // 
            this.shutdownPS3ToolStripMenuItem.Name = "shutdownPS3ToolStripMenuItem";
            this.shutdownPS3ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.shutdownPS3ToolStripMenuItem.Text = "Shutdown PS3";
            this.shutdownPS3ToolStripMenuItem.Click += new System.EventHandler(this.shutdownPS3ToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // attachToolStripMenuItem
            // 
            this.attachToolStripMenuItem.Name = "attachToolStripMenuItem";
            this.attachToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.attachToolStripMenuItem.Text = "Attach";
            this.attachToolStripMenuItem.Click += new System.EventHandler(this.attachToolStripMenuItem_Click);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.loadPluginsToolStripMenuItem,
            this.shutdownPS3ToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.attachToolStripMenuItem,
            this.connectToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.BlueViolet;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 20);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // refreshFromPS3ToolStripMenuItem
            // 
            this.refreshFromPS3ToolStripMenuItem.Name = "refreshFromPS3ToolStripMenuItem";
            this.refreshFromPS3ToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.refreshFromPS3ToolStripMenuItem.Text = "Refresh From PS3";
            this.refreshFromPS3ToolStripMenuItem.Click += new System.EventHandler(this.refreshFromPS3ToolStripMenuItem_Click);
            // 
            // refreshFromDumptxtToolStripMenuItem
            // 
            this.refreshFromDumptxtToolStripMenuItem.Name = "refreshFromDumptxtToolStripMenuItem";
            this.refreshFromDumptxtToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.refreshFromDumptxtToolStripMenuItem.Text = "Refresh From dump.txt";
            this.refreshFromDumptxtToolStripMenuItem.Click += new System.EventHandler(this.refreshFromDumptxtToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.refreshFromPS3ToolStripMenuItem,
            this.refreshFromDumptxtToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(196, 92);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(485, 463);
            this.Controls.Add(this.optButton);
            this.Controls.Add(this.refPlugin);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ps3Disc);
            this.Controls.Add(this.attachProcessButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.TabCon);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(501, 501);
            this.MinimumSize = new System.Drawing.Size(501, 501);
            this.Name = "Form1";
            this.Text = "NetCheat PS3 by Dnawrkshp";
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.TabCon.ResumeLayout(false);
            this.CodesTab.ResumeLayout(false);
            this.CodesTab.PerformLayout();
            this.SearchTab.ResumeLayout(false);
            this.SearchTab.PerformLayout();
            this.RangeTab.ResumeLayout(false);
            this.pluginTab.ResumeLayout(false);
            this.pluginTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plugIcon)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl TabCon;
        private System.Windows.Forms.TabPage CodesTab;
        private System.Windows.Forms.TabPage SearchTab;
        private System.Windows.Forms.Button cbSaveAll;
        private System.Windows.Forms.Button cbSaveAs;
        private System.Windows.Forms.Button cbImport;
        private System.Windows.Forms.Button cbSave;
        private System.Windows.Forms.Button cbRemove;
        private System.Windows.Forms.Button cbAdd;
        private System.Windows.Forms.Button cbWrite;
        private System.Windows.Forms.CheckBox cbState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox cbName;
        private System.Windows.Forms.ListView cbList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TextBox schVal2;
        private System.Windows.Forms.Button DumpMem;
        private System.Windows.Forms.ComboBox compBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox SchPWS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar schProg;
        private System.Windows.Forms.CheckBox SchHexCheck;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbSchAlign;
        private System.Windows.Forms.Button SchRef;
        public System.Windows.Forms.ListView lvSch;
        private System.Windows.Forms.ColumnHeader lvSchAddr;
        private System.Windows.Forms.ColumnHeader lvSchValue;
        private System.Windows.Forms.ColumnHeader lvSchDec;
        private System.Windows.Forms.ColumnHeader lvSchAlign;
        private System.Windows.Forms.Button schNSearch;
        private System.Windows.Forms.TextBox schVal;
        private System.Windows.Forms.TextBox schRange2;
        private System.Windows.Forms.TextBox schRange1;
        private System.Windows.Forms.Button ps3Disc;
        private System.Windows.Forms.Button attachProcessButton;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.RichTextBox cbCodes;
        private System.Windows.Forms.TabPage RangeTab;
        private System.Windows.Forms.ListView rangeView;
        private System.Windows.Forms.Button ImportRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader StartAddr;
        private System.Windows.Forms.ColumnHeader EndAddr;
        private System.Windows.Forms.Button SaveRange;
        private System.Windows.Forms.Button RemoveRange;
        private System.Windows.Forms.Button AddRange;
        private System.Windows.Forms.Button RangeDown;
        private System.Windows.Forms.Button RangeUp;
        private System.Windows.Forms.Button refPlugin;
        private System.Windows.Forms.Button schSearch;
        private System.Windows.Forms.Button optButton;
        private System.Windows.Forms.TabPage pluginTab;
        private System.Windows.Forms.ListBox pluginList;
        private System.Windows.Forms.Label descPlugDesc;
        private System.Windows.Forms.Label descPlugVer;
        private System.Windows.Forms.Label descPlugAuth;
        private System.Windows.Forms.Label descPlugName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView recRangeBox;
        private System.Windows.Forms.ColumnHeader colFileName;
        private System.Windows.Forms.Button loadSRes;
        private System.Windows.Forms.Button saveSRes;
        private System.Windows.Forms.PictureBox plugIcon;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shutdownPS3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem attachToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshFromPS3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshFromDumptxtToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}

