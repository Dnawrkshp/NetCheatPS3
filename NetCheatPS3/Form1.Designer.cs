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
            this.cbBManager = new System.Windows.Forms.Button();
            this.cbBackupWrite = new System.Windows.Forms.Button();
            this.cbResetWrite = new System.Windows.Forms.Button();
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
            this.searchControl1 = new NetCheatPS3.SearchControl();
            this.RangeTab = new System.Windows.Forms.TabPage();
            this.findRangeProgBar = new System.Windows.Forms.ProgressBar();
            this.findRanges = new System.Windows.Forms.Button();
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
            this.apiTab = new System.Windows.Forms.TabPage();
            this.apiIcon = new System.Windows.Forms.PictureBox();
            this.descAPIDesc = new System.Windows.Forms.Label();
            this.descAPIVer = new System.Windows.Forms.Label();
            this.descAPIAuth = new System.Windows.Forms.Label();
            this.descAPIName = new System.Windows.Forms.Label();
            this.apiList = new System.Windows.Forms.ListBox();
            this.DumpCompTab = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.addrFLab = new System.Windows.Forms.Label();
            this.addrFromTB = new System.Windows.Forms.TextBox();
            this.stopLab = new System.Windows.Forms.Label();
            this.stopTB = new System.Windows.Forms.TextBox();
            this.readLab = new System.Windows.Forms.Label();
            this.startTB = new System.Windows.Forms.TextBox();
            this.saveScan = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.typeLabel = new System.Windows.Forms.Label();
            this.searchTypeBox = new System.Windows.Forms.ComboBox();
            this.searchNameBox = new System.Windows.Forms.ComboBox();
            this.dumpTB2 = new System.Windows.Forms.TextBox();
            this.dumpTB1 = new System.Windows.Forms.TextBox();
            this.browseDump2 = new System.Windows.Forms.Button();
            this.browseDump1 = new System.Windows.Forms.Button();
            this.progBar = new NetCheatPS3.ProgressBar();
            this.ps3Disc = new System.Windows.Forms.Button();
            this.attachProcessButton = new System.Windows.Forms.Button();
            this.connectButton = new System.Windows.Forms.Button();
            this.refPlugin = new System.Windows.Forms.Button();
            this.optButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureAPIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shutdownPS3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.attachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.endianStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshFromPS3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshFromDumptxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.codesToolMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bwStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.twStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fwStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.createCondStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editConditionalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startGameButt = new System.Windows.Forms.Button();
            this.pauseGameButt = new System.Windows.Forms.Button();
            this.TabCon.SuspendLayout();
            this.CodesTab.SuspendLayout();
            this.SearchTab.SuspendLayout();
            this.RangeTab.SuspendLayout();
            this.pluginTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plugIcon)).BeginInit();
            this.apiTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apiIcon)).BeginInit();
            this.DumpCompTab.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.codesToolMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabCon
            // 
            this.TabCon.Controls.Add(this.CodesTab);
            this.TabCon.Controls.Add(this.SearchTab);
            this.TabCon.Controls.Add(this.RangeTab);
            this.TabCon.Controls.Add(this.pluginTab);
            this.TabCon.Controls.Add(this.apiTab);
            this.TabCon.Controls.Add(this.DumpCompTab);
            this.TabCon.Location = new System.Drawing.Point(12, 42);
            this.TabCon.Name = "TabCon";
            this.TabCon.SelectedIndex = 0;
            this.TabCon.Size = new System.Drawing.Size(461, 393);
            this.TabCon.TabIndex = 0;
            this.TabCon.SelectedIndexChanged += new System.EventHandler(this.TabCon_SelectedIndexChanged);
            this.TabCon.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TabCon_KeyUp);
            // 
            // CodesTab
            // 
            this.CodesTab.AutoScroll = true;
            this.CodesTab.BackColor = System.Drawing.Color.Black;
            this.CodesTab.Controls.Add(this.cbBManager);
            this.CodesTab.Controls.Add(this.cbBackupWrite);
            this.CodesTab.Controls.Add(this.cbResetWrite);
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
            // cbBManager
            // 
            this.cbBManager.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBManager.Location = new System.Drawing.Point(220, 336);
            this.cbBManager.Name = "cbBManager";
            this.cbBManager.Size = new System.Drawing.Size(72, 23);
            this.cbBManager.TabIndex = 35;
            this.cbBManager.Text = "B Manager";
            this.toolTip1.SetToolTip(this.cbBManager, "Manages Find/Replace (OGP/COP) writes");
            this.cbBManager.UseVisualStyleBackColor = true;
            this.cbBManager.Click += new System.EventHandler(this.cbBManager_Click);
            // 
            // cbBackupWrite
            // 
            this.cbBackupWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBackupWrite.Location = new System.Drawing.Point(373, 336);
            this.cbBackupWrite.Name = "cbBackupWrite";
            this.cbBackupWrite.Size = new System.Drawing.Size(72, 23);
            this.cbBackupWrite.TabIndex = 34;
            this.cbBackupWrite.Text = "Backup Memory";
            this.toolTip1.SetToolTip(this.cbBackupWrite, "Backs up the memory that the code accesses.\r\nSupports 0, 1, 2, and 6 codetypes.");
            this.cbBackupWrite.UseVisualStyleBackColor = true;
            this.cbBackupWrite.Click += new System.EventHandler(this.cbBackupWrite_Click);
            // 
            // cbResetWrite
            // 
            this.cbResetWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbResetWrite.Location = new System.Drawing.Point(297, 336);
            this.cbResetWrite.Name = "cbResetWrite";
            this.cbResetWrite.Size = new System.Drawing.Size(72, 23);
            this.cbResetWrite.TabIndex = 33;
            this.cbResetWrite.Text = "Reset Memory";
            this.toolTip1.SetToolTip(this.cbResetWrite, "Resets the memory back to what the backup holds");
            this.cbResetWrite.UseVisualStyleBackColor = true;
            this.cbResetWrite.Click += new System.EventHandler(this.cbResetWrite_Click);
            // 
            // cbCodes
            // 
            this.cbCodes.BackColor = System.Drawing.Color.Black;
            this.cbCodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbCodes.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCodes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.cbCodes.Location = new System.Drawing.Point(219, 79);
            this.cbCodes.Name = "cbCodes";
            this.cbCodes.Size = new System.Drawing.Size(225, 225);
            this.cbCodes.TabIndex = 32;
            this.cbCodes.Text = "";
            this.cbCodes.WordWrap = false;
            this.cbCodes.TextChanged += new System.EventHandler(this.cbCodes_TextChanged);
            this.cbCodes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbCodes_KeyUp);
            this.cbCodes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cbCodes_MouseUp);
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
            this.cbWrite.Location = new System.Drawing.Point(219, 310);
            this.cbWrite.Name = "cbWrite";
            this.cbWrite.Size = new System.Drawing.Size(226, 23);
            this.cbWrite.TabIndex = 24;
            this.cbWrite.Text = "Write";
            this.cbWrite.UseVisualStyleBackColor = true;
            this.cbWrite.Click += new System.EventHandler(this.cbWrite_Click);
            // 
            // cbState
            // 
            this.cbState.Location = new System.Drawing.Point(219, 55);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(225, 17);
            this.cbState.TabIndex = 23;
            this.cbState.Text = "Constant Write";
            this.cbState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbState.UseVisualStyleBackColor = true;
            this.cbState.CheckedChanged += new System.EventHandler(this.cbState_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(219, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(225, 20);
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
            this.cbName.Location = new System.Drawing.Point(219, 29);
            this.cbName.Name = "cbName";
            this.cbName.Size = new System.Drawing.Size(225, 20);
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
            this.SearchTab.Controls.Add(this.searchControl1);
            this.SearchTab.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.SearchTab.Location = new System.Drawing.Point(4, 22);
            this.SearchTab.Name = "SearchTab";
            this.SearchTab.Padding = new System.Windows.Forms.Padding(3);
            this.SearchTab.Size = new System.Drawing.Size(453, 367);
            this.SearchTab.TabIndex = 1;
            this.SearchTab.Text = "Search";
            // 
            // searchControl1
            // 
            this.searchControl1.isInitialScan = true;
            this.searchControl1.isPWSVisible = true;
            this.searchControl1.Location = new System.Drawing.Point(6, 6);
            this.searchControl1.Name = "searchControl1";
            this.searchControl1.Size = new System.Drawing.Size(444, 358);
            this.searchControl1.TabIndex = 0;
            // 
            // RangeTab
            // 
            this.RangeTab.AutoScroll = true;
            this.RangeTab.BackColor = System.Drawing.Color.Black;
            this.RangeTab.Controls.Add(this.findRangeProgBar);
            this.RangeTab.Controls.Add(this.findRanges);
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
            // findRangeProgBar
            // 
            this.findRangeProgBar.Location = new System.Drawing.Point(235, 34);
            this.findRangeProgBar.Name = "findRangeProgBar";
            this.findRangeProgBar.Size = new System.Drawing.Size(115, 30);
            this.findRangeProgBar.TabIndex = 13;
            // 
            // findRanges
            // 
            this.findRanges.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.findRanges.Location = new System.Drawing.Point(356, 34);
            this.findRanges.Name = "findRanges";
            this.findRanges.Size = new System.Drawing.Size(91, 30);
            this.findRanges.TabIndex = 12;
            this.findRanges.Text = "Find Ranges";
            this.findRanges.UseVisualStyleBackColor = true;
            this.findRanges.Click += new System.EventHandler(this.findRanges_Click);
            // 
            // recRangeBox
            // 
            this.recRangeBox.BackColor = System.Drawing.Color.Black;
            this.recRangeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.recRangeBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName});
            this.recRangeBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.recRangeBox.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.recRangeBox.HideSelection = false;
            this.recRangeBox.LabelWrap = false;
            this.recRangeBox.Location = new System.Drawing.Point(235, 85);
            this.recRangeBox.MultiSelect = false;
            this.recRangeBox.Name = "recRangeBox";
            this.recRangeBox.Size = new System.Drawing.Size(212, 167);
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
            this.label3.Location = new System.Drawing.Point(235, 67);
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
            this.plugIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
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
            // apiTab
            // 
            this.apiTab.BackColor = System.Drawing.Color.Black;
            this.apiTab.Controls.Add(this.apiIcon);
            this.apiTab.Controls.Add(this.descAPIDesc);
            this.apiTab.Controls.Add(this.descAPIVer);
            this.apiTab.Controls.Add(this.descAPIAuth);
            this.apiTab.Controls.Add(this.descAPIName);
            this.apiTab.Controls.Add(this.apiList);
            this.apiTab.Location = new System.Drawing.Point(4, 22);
            this.apiTab.Name = "apiTab";
            this.apiTab.Size = new System.Drawing.Size(453, 367);
            this.apiTab.TabIndex = 5;
            this.apiTab.Text = "APIs";
            // 
            // apiIcon
            // 
            this.apiIcon.InitialImage = ((System.Drawing.Image)(resources.GetObject("apiIcon.InitialImage")));
            this.apiIcon.Location = new System.Drawing.Point(183, 146);
            this.apiIcon.Name = "apiIcon";
            this.apiIcon.Size = new System.Drawing.Size(266, 210);
            this.apiIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.apiIcon.TabIndex = 14;
            this.apiIcon.TabStop = false;
            this.apiIcon.Click += new System.EventHandler(this.apiIcon_Click);
            this.apiIcon.MouseEnter += new System.EventHandler(this.apiIcon_MouseEnter);
            this.apiIcon.MouseLeave += new System.EventHandler(this.apiIcon_MouseLeave);
            this.apiIcon.MouseHover += new System.EventHandler(this.apiIcon_MouseHover);
            this.apiIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.apiIcon_MouseMove);
            // 
            // descAPIDesc
            // 
            this.descAPIDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descAPIDesc.Location = new System.Drawing.Point(186, 42);
            this.descAPIDesc.Name = "descAPIDesc";
            this.descAPIDesc.Size = new System.Drawing.Size(263, 101);
            this.descAPIDesc.TabIndex = 13;
            this.descAPIDesc.Text = "API Description";
            // 
            // descAPIVer
            // 
            this.descAPIVer.AutoSize = true;
            this.descAPIVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descAPIVer.Location = new System.Drawing.Point(205, 29);
            this.descAPIVer.Name = "descAPIVer";
            this.descAPIVer.Size = new System.Drawing.Size(62, 13);
            this.descAPIVer.TabIndex = 12;
            this.descAPIVer.Text = "API Version";
            // 
            // descAPIAuth
            // 
            this.descAPIAuth.AutoSize = true;
            this.descAPIAuth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descAPIAuth.Location = new System.Drawing.Point(205, 16);
            this.descAPIAuth.Name = "descAPIAuth";
            this.descAPIAuth.Size = new System.Drawing.Size(85, 13);
            this.descAPIAuth.TabIndex = 11;
            this.descAPIAuth.Text = "by API Author";
            // 
            // descAPIName
            // 
            this.descAPIName.AutoSize = true;
            this.descAPIName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descAPIName.Location = new System.Drawing.Point(183, 3);
            this.descAPIName.Name = "descAPIName";
            this.descAPIName.Size = new System.Drawing.Size(63, 13);
            this.descAPIName.TabIndex = 10;
            this.descAPIName.Text = "API Name";
            // 
            // apiList
            // 
            this.apiList.BackColor = System.Drawing.Color.Black;
            this.apiList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.apiList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.apiList.FormattingEnabled = true;
            this.apiList.Location = new System.Drawing.Point(3, 3);
            this.apiList.Name = "apiList";
            this.apiList.Size = new System.Drawing.Size(174, 353);
            this.apiList.TabIndex = 9;
            this.apiList.SelectedIndexChanged += new System.EventHandler(this.apiList_SelectedIndexChanged);
            this.apiList.DoubleClick += new System.EventHandler(this.apiList_DoubleClick);
            // 
            // DumpCompTab
            // 
            this.DumpCompTab.BackColor = System.Drawing.Color.Black;
            this.DumpCompTab.Controls.Add(this.label4);
            this.DumpCompTab.Controls.Add(this.addrFLab);
            this.DumpCompTab.Controls.Add(this.addrFromTB);
            this.DumpCompTab.Controls.Add(this.stopLab);
            this.DumpCompTab.Controls.Add(this.stopTB);
            this.DumpCompTab.Controls.Add(this.readLab);
            this.DumpCompTab.Controls.Add(this.startTB);
            this.DumpCompTab.Controls.Add(this.saveScan);
            this.DumpCompTab.Controls.Add(this.searchButton);
            this.DumpCompTab.Controls.Add(this.typeLabel);
            this.DumpCompTab.Controls.Add(this.searchTypeBox);
            this.DumpCompTab.Controls.Add(this.searchNameBox);
            this.DumpCompTab.Controls.Add(this.dumpTB2);
            this.DumpCompTab.Controls.Add(this.dumpTB1);
            this.DumpCompTab.Controls.Add(this.browseDump2);
            this.DumpCompTab.Controls.Add(this.browseDump1);
            this.DumpCompTab.Controls.Add(this.progBar);
            this.DumpCompTab.Location = new System.Drawing.Point(4, 22);
            this.DumpCompTab.Name = "DumpCompTab";
            this.DumpCompTab.Size = new System.Drawing.Size(453, 367);
            this.DumpCompTab.TabIndex = 4;
            this.DumpCompTab.Text = "Dump Compare";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Search";
            // 
            // addrFLab
            // 
            this.addrFLab.AutoSize = true;
            this.addrFLab.Location = new System.Drawing.Point(3, 251);
            this.addrFLab.Name = "addrFLab";
            this.addrFLab.Size = new System.Drawing.Size(71, 13);
            this.addrFLab.TabIndex = 38;
            this.addrFLab.Text = "Address From";
            // 
            // addrFromTB
            // 
            this.addrFromTB.Location = new System.Drawing.Point(80, 248);
            this.addrFromTB.Name = "addrFromTB";
            this.addrFromTB.Size = new System.Drawing.Size(68, 20);
            this.addrFromTB.TabIndex = 37;
            this.addrFromTB.Text = "00000000";
            // 
            // stopLab
            // 
            this.stopLab.AutoSize = true;
            this.stopLab.Location = new System.Drawing.Point(45, 224);
            this.stopLab.Name = "stopLab";
            this.stopLab.Size = new System.Drawing.Size(29, 13);
            this.stopLab.TabIndex = 36;
            this.stopLab.Text = "Stop";
            // 
            // stopTB
            // 
            this.stopTB.Location = new System.Drawing.Point(80, 221);
            this.stopTB.Name = "stopTB";
            this.stopTB.Size = new System.Drawing.Size(68, 20);
            this.stopTB.TabIndex = 35;
            this.stopTB.Text = "00000000";
            // 
            // readLab
            // 
            this.readLab.AutoSize = true;
            this.readLab.Location = new System.Drawing.Point(45, 195);
            this.readLab.Name = "readLab";
            this.readLab.Size = new System.Drawing.Size(29, 13);
            this.readLab.TabIndex = 34;
            this.readLab.Text = "Start";
            // 
            // startTB
            // 
            this.startTB.Location = new System.Drawing.Point(80, 192);
            this.startTB.Name = "startTB";
            this.startTB.Size = new System.Drawing.Size(68, 20);
            this.startTB.TabIndex = 33;
            this.startTB.Text = "00000000";
            // 
            // saveScan
            // 
            this.saveScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveScan.Location = new System.Drawing.Point(365, 304);
            this.saveScan.Name = "saveScan";
            this.saveScan.Size = new System.Drawing.Size(85, 23);
            this.saveScan.TabIndex = 32;
            this.saveScan.Text = "Save Scan";
            this.saveScan.UseVisualStyleBackColor = true;
            this.saveScan.Click += new System.EventHandler(this.saveScan_Click);
            // 
            // searchButton
            // 
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Location = new System.Drawing.Point(274, 304);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(85, 23);
            this.searchButton.TabIndex = 31;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(43, 307);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(31, 13);
            this.typeLabel.TabIndex = 30;
            this.typeLabel.Text = "Type";
            // 
            // searchTypeBox
            // 
            this.searchTypeBox.FormattingEnabled = true;
            this.searchTypeBox.Location = new System.Drawing.Point(80, 304);
            this.searchTypeBox.Name = "searchTypeBox";
            this.searchTypeBox.Size = new System.Drawing.Size(121, 21);
            this.searchTypeBox.TabIndex = 28;
            this.searchTypeBox.SelectedIndexChanged += new System.EventHandler(this.searchTypeBox_SelectedIndexChanged);
            // 
            // searchNameBox
            // 
            this.searchNameBox.FormattingEnabled = true;
            this.searchNameBox.Location = new System.Drawing.Point(80, 274);
            this.searchNameBox.Name = "searchNameBox";
            this.searchNameBox.Size = new System.Drawing.Size(121, 21);
            this.searchNameBox.TabIndex = 27;
            this.searchNameBox.SelectedIndexChanged += new System.EventHandler(this.searchNameBox_SelectedIndexChanged);
            // 
            // dumpTB2
            // 
            this.dumpTB2.Location = new System.Drawing.Point(3, 32);
            this.dumpTB2.Name = "dumpTB2";
            this.dumpTB2.Size = new System.Drawing.Size(361, 20);
            this.dumpTB2.TabIndex = 26;
            // 
            // dumpTB1
            // 
            this.dumpTB1.Location = new System.Drawing.Point(3, 3);
            this.dumpTB1.Name = "dumpTB1";
            this.dumpTB1.Size = new System.Drawing.Size(361, 20);
            this.dumpTB1.TabIndex = 25;
            // 
            // browseDump2
            // 
            this.browseDump2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseDump2.Location = new System.Drawing.Point(375, 30);
            this.browseDump2.Name = "browseDump2";
            this.browseDump2.Size = new System.Drawing.Size(75, 23);
            this.browseDump2.TabIndex = 24;
            this.browseDump2.Text = "Browse";
            this.browseDump2.UseVisualStyleBackColor = true;
            this.browseDump2.Click += new System.EventHandler(this.browseDump2_Click);
            // 
            // browseDump1
            // 
            this.browseDump1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseDump1.Location = new System.Drawing.Point(375, 1);
            this.browseDump1.Name = "browseDump1";
            this.browseDump1.Size = new System.Drawing.Size(75, 23);
            this.browseDump1.TabIndex = 23;
            this.browseDump1.Text = "Browse";
            this.browseDump1.UseVisualStyleBackColor = true;
            this.browseDump1.Click += new System.EventHandler(this.browseDump1_Click);
            // 
            // progBar
            // 
            this.progBar.BackColor = System.Drawing.Color.White;
            this.progBar.Location = new System.Drawing.Point(3, 333);
            this.progBar.Maximum = 0;
            this.progBar.Name = "progBar";
            this.progBar.printText = "";
            this.progBar.progressColor = System.Drawing.SystemColors.ControlText;
            this.progBar.Size = new System.Drawing.Size(447, 31);
            this.progBar.TabIndex = 39;
            this.progBar.Value = 0;
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
            this.toolStripDropDownButton1,
            this.statusLabel1,
            this.statusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 439);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(485, 24);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.configureAPIToolStripMenuItem,
            this.loadPluginsToolStripMenuItem,
            this.shutdownPS3ToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.attachToolStripMenuItem,
            this.connectToolStripMenuItem,
            this.updateStripMenuItem1,
            this.endianStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.BlueViolet;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // configureAPIToolStripMenuItem
            // 
            this.configureAPIToolStripMenuItem.Name = "configureAPIToolStripMenuItem";
            this.configureAPIToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.configureAPIToolStripMenuItem.Text = "Configure API";
            this.configureAPIToolStripMenuItem.ToolTipText = "Configure API.\r\nIf nothing happens then the API doesn\'t support this feature.";
            this.configureAPIToolStripMenuItem.Click += new System.EventHandler(this.configureAPIToolStripMenuItem_Click);
            // 
            // loadPluginsToolStripMenuItem
            // 
            this.loadPluginsToolStripMenuItem.Name = "loadPluginsToolStripMenuItem";
            this.loadPluginsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadPluginsToolStripMenuItem.Text = "Load Plugins";
            this.loadPluginsToolStripMenuItem.Click += new System.EventHandler(this.loadPluginsToolStripMenuItem_Click);
            // 
            // shutdownPS3ToolStripMenuItem
            // 
            this.shutdownPS3ToolStripMenuItem.Name = "shutdownPS3ToolStripMenuItem";
            this.shutdownPS3ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.shutdownPS3ToolStripMenuItem.Text = "Shutdown";
            this.shutdownPS3ToolStripMenuItem.Click += new System.EventHandler(this.shutdownPS3ToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // attachToolStripMenuItem
            // 
            this.attachToolStripMenuItem.Name = "attachToolStripMenuItem";
            this.attachToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.attachToolStripMenuItem.Text = "Attach";
            this.attachToolStripMenuItem.Click += new System.EventHandler(this.attachToolStripMenuItem_Click);
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // updateStripMenuItem1
            // 
            this.updateStripMenuItem1.Name = "updateStripMenuItem1";
            this.updateStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.updateStripMenuItem1.Text = "Update NetCheat";
            this.updateStripMenuItem1.Click += new System.EventHandler(this.updateStripMenuItem1_Click);
            // 
            // endianStripMenuItem
            // 
            this.endianStripMenuItem.Checked = true;
            this.endianStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.endianStripMenuItem.Name = "endianStripMenuItem";
            this.endianStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.endianStripMenuItem.Text = "Big Endian";
            this.endianStripMenuItem.Click += new System.EventHandler(this.endianStripMenuItem_Click);
            // 
            // statusLabel1
            // 
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(88, 19);
            this.statusLabel1.Text = "Not Connected";
            // 
            // statusLabel2
            // 
            this.statusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.statusLabel2.Name = "statusLabel2";
            this.statusLabel2.Size = new System.Drawing.Size(117, 19);
            this.statusLabel2.Text = "API: Target Manager";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // refreshFromPS3ToolStripMenuItem
            // 
            this.refreshFromPS3ToolStripMenuItem.Name = "refreshFromPS3ToolStripMenuItem";
            this.refreshFromPS3ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.refreshFromPS3ToolStripMenuItem.Text = "Refresh From PS3";
            this.refreshFromPS3ToolStripMenuItem.Click += new System.EventHandler(this.refreshFromPS3ToolStripMenuItem_Click);
            // 
            // refreshFromDumptxtToolStripMenuItem
            // 
            this.refreshFromDumptxtToolStripMenuItem.Name = "refreshFromDumptxtToolStripMenuItem";
            this.refreshFromDumptxtToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
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
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 92);
            // 
            // codesToolMenuStrip
            // 
            this.codesToolMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bwStripMenuItem1,
            this.twStripMenuItem1,
            this.fwStripMenuItem1,
            this.createCondStripMenuItem1,
            this.editConditionalToolStripMenuItem});
            this.codesToolMenuStrip.Name = "codesToolMenuStrip";
            this.codesToolMenuStrip.Size = new System.Drawing.Size(191, 114);
            // 
            // bwStripMenuItem1
            // 
            this.bwStripMenuItem1.Name = "bwStripMenuItem1";
            this.bwStripMenuItem1.Size = new System.Drawing.Size(190, 22);
            this.bwStripMenuItem1.Text = "Convert to Byte Write";
            this.bwStripMenuItem1.Click += new System.EventHandler(this.bwStripMenuItem1_Click);
            // 
            // twStripMenuItem1
            // 
            this.twStripMenuItem1.Name = "twStripMenuItem1";
            this.twStripMenuItem1.Size = new System.Drawing.Size(190, 22);
            this.twStripMenuItem1.Text = "Convert to Text Write";
            this.twStripMenuItem1.Click += new System.EventHandler(this.twStripMenuItem1_Click);
            // 
            // fwStripMenuItem1
            // 
            this.fwStripMenuItem1.Name = "fwStripMenuItem1";
            this.fwStripMenuItem1.Size = new System.Drawing.Size(190, 22);
            this.fwStripMenuItem1.Text = "Convert to Float Write";
            this.fwStripMenuItem1.Click += new System.EventHandler(this.fwStripMenuItem1_Click);
            // 
            // createCondStripMenuItem1
            // 
            this.createCondStripMenuItem1.Name = "createCondStripMenuItem1";
            this.createCondStripMenuItem1.Size = new System.Drawing.Size(190, 22);
            this.createCondStripMenuItem1.Text = "Create Conditional";
            this.createCondStripMenuItem1.Click += new System.EventHandler(this.createCondStripMenuItem1_Click);
            // 
            // editConditionalToolStripMenuItem
            // 
            this.editConditionalToolStripMenuItem.Name = "editConditionalToolStripMenuItem";
            this.editConditionalToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.editConditionalToolStripMenuItem.Text = "Edit Conditional";
            this.editConditionalToolStripMenuItem.Click += new System.EventHandler(this.editConditionalToolStripMenuItem_Click);
            // 
            // startGameButt
            // 
            this.startGameButt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("startGameButt.BackgroundImage")));
            this.startGameButt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.startGameButt.Location = new System.Drawing.Point(420, 37);
            this.startGameButt.Name = "startGameButt";
            this.startGameButt.Size = new System.Drawing.Size(24, 24);
            this.startGameButt.TabIndex = 13;
            this.startGameButt.UseVisualStyleBackColor = true;
            this.startGameButt.BackColorChanged += new System.EventHandler(this.startGameButt_BackColorChanged);
            this.startGameButt.ForeColorChanged += new System.EventHandler(this.startGameButt_ForeColorChanged);
            this.startGameButt.Click += new System.EventHandler(this.startGameButt_Click);
            // 
            // pauseGameButt
            // 
            this.pauseGameButt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pauseGameButt.BackgroundImage")));
            this.pauseGameButt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pauseGameButt.Location = new System.Drawing.Point(450, 37);
            this.pauseGameButt.Name = "pauseGameButt";
            this.pauseGameButt.Size = new System.Drawing.Size(24, 24);
            this.pauseGameButt.TabIndex = 12;
            this.pauseGameButt.UseVisualStyleBackColor = true;
            this.pauseGameButt.BackColorChanged += new System.EventHandler(this.pauseGameButt_BackColorChanged);
            this.pauseGameButt.ForeColorChanged += new System.EventHandler(this.pauseGameButt_ForeColorChanged);
            this.pauseGameButt.Click += new System.EventHandler(this.pauseGameButt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(485, 463);
            this.Controls.Add(this.startGameButt);
            this.Controls.Add(this.pauseGameButt);
            this.Controls.Add(this.optButton);
            this.Controls.Add(this.refPlugin);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ps3Disc);
            this.Controls.Add(this.attachProcessButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.TabCon);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(210)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(501, 501);
            this.Name = "Form1";
            this.Text = "NetCheat PS3 by Dnawrkshp";
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.TabCon.ResumeLayout(false);
            this.CodesTab.ResumeLayout(false);
            this.CodesTab.PerformLayout();
            this.SearchTab.ResumeLayout(false);
            this.RangeTab.ResumeLayout(false);
            this.pluginTab.ResumeLayout(false);
            this.pluginTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plugIcon)).EndInit();
            this.apiTab.ResumeLayout(false);
            this.apiTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apiIcon)).EndInit();
            this.DumpCompTab.ResumeLayout(false);
            this.DumpCompTab.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.codesToolMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button ps3Disc;
        private System.Windows.Forms.Button attachProcessButton;
        private System.Windows.Forms.Button connectButton;
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
        private System.Windows.Forms.PictureBox plugIcon;
        private System.Windows.Forms.StatusStrip statusStrip1;
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
        private System.Windows.Forms.ToolStripMenuItem updateStripMenuItem1;
        private System.Windows.Forms.ProgressBar findRangeProgBar;
        private System.Windows.Forms.Button findRanges;
        private System.Windows.Forms.Button pauseGameButt;
        private System.Windows.Forms.Button startGameButt;
        private System.Windows.Forms.Button cbResetWrite;
        private System.Windows.Forms.Button cbBackupWrite;
        private System.Windows.Forms.ToolTip toolTip1;
        private SearchControl searchControl1;
        private System.Windows.Forms.TabPage DumpCompTab;
        private System.Windows.Forms.Label addrFLab;
        private System.Windows.Forms.TextBox addrFromTB;
        private System.Windows.Forms.Label stopLab;
        private System.Windows.Forms.TextBox stopTB;
        private System.Windows.Forms.Label readLab;
        private System.Windows.Forms.TextBox startTB;
        private System.Windows.Forms.Button saveScan;
        public System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox searchTypeBox;
        private System.Windows.Forms.ComboBox searchNameBox;
        private System.Windows.Forms.TextBox dumpTB2;
        private System.Windows.Forms.TextBox dumpTB1;
        private System.Windows.Forms.Button browseDump2;
        private System.Windows.Forms.Button browseDump1;
        public ProgressBar progBar;
        public System.Windows.Forms.ToolStripStatusLabel statusLabel1;
        public System.Windows.Forms.ListView cbList;
        public System.Windows.Forms.RichTextBox cbCodes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cbBManager;
        private System.Windows.Forms.ToolStripMenuItem endianStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip codesToolMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem bwStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem twStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fwStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem createCondStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem editConditionalToolStripMenuItem;
        private System.Windows.Forms.TabPage apiTab;
        private System.Windows.Forms.PictureBox apiIcon;
        private System.Windows.Forms.Label descAPIDesc;
        private System.Windows.Forms.Label descAPIVer;
        private System.Windows.Forms.Label descAPIAuth;
        private System.Windows.Forms.Label descAPIName;
        private System.Windows.Forms.ListBox apiList;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel2;
        private System.Windows.Forms.ToolStripMenuItem configureAPIToolStripMenuItem;
    }
}

