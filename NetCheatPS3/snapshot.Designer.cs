namespace NetCheatPS3
{
    partial class snapshot
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
            this.snapPB = new System.Windows.Forms.PictureBox();
            this.saveSnap = new System.Windows.Forms.Button();
            this.findFrame = new System.Windows.Forms.Button();
            this.frameStart = new System.Windows.Forms.TextBox();
            this.snapPauseGame = new System.Windows.Forms.CheckBox();
            this.sdRadButt = new System.Windows.Forms.RadioButton();
            this.hdRadButt = new System.Windows.Forms.RadioButton();
            this.makeSnapshot = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.hdRadButt1080 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.vidFrame = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.snapPB)).BeginInit();
            this.SuspendLayout();
            // 
            // snapPB
            // 
            this.snapPB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.snapPB.Location = new System.Drawing.Point(4, 61);
            this.snapPB.Name = "snapPB";
            this.snapPB.Size = new System.Drawing.Size(480, 320);
            this.snapPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.snapPB.TabIndex = 4;
            this.snapPB.TabStop = false;
            // 
            // saveSnap
            // 
            this.saveSnap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveSnap.Location = new System.Drawing.Point(9, 32);
            this.saveSnap.Name = "saveSnap";
            this.saveSnap.Size = new System.Drawing.Size(99, 23);
            this.saveSnap.TabIndex = 7;
            this.saveSnap.Text = "Save Snapshot";
            this.saveSnap.UseVisualStyleBackColor = true;
            this.saveSnap.Click += new System.EventHandler(this.saveSnap_Click);
            this.saveSnap.KeyUp += new System.Windows.Forms.KeyEventHandler(this.saveSnap_KeyUp);
            // 
            // findFrame
            // 
            this.findFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.findFrame.Location = new System.Drawing.Point(120, 3);
            this.findFrame.Name = "findFrame";
            this.findFrame.Size = new System.Drawing.Size(99, 23);
            this.findFrame.TabIndex = 5;
            this.findFrame.Text = "Find Frame";
            this.findFrame.UseVisualStyleBackColor = true;
            this.findFrame.Click += new System.EventHandler(this.findFrame_Click);
            this.findFrame.KeyUp += new System.Windows.Forms.KeyEventHandler(this.saveSnap_KeyUp);
            // 
            // frameStart
            // 
            this.frameStart.Location = new System.Drawing.Point(120, 34);
            this.frameStart.Name = "frameStart";
            this.frameStart.Size = new System.Drawing.Size(100, 20);
            this.frameStart.TabIndex = 6;
            this.frameStart.Text = "C0000000";
            // 
            // snapPauseGame
            // 
            this.snapPauseGame.AutoSize = true;
            this.snapPauseGame.Location = new System.Drawing.Point(225, 7);
            this.snapPauseGame.Name = "snapPauseGame";
            this.snapPauseGame.Size = new System.Drawing.Size(87, 17);
            this.snapPauseGame.TabIndex = 5;
            this.snapPauseGame.Text = "Pause Game";
            this.snapPauseGame.UseVisualStyleBackColor = true;
            // 
            // sdRadButt
            // 
            this.sdRadButt.AutoSize = true;
            this.sdRadButt.Checked = true;
            this.sdRadButt.Location = new System.Drawing.Point(427, 32);
            this.sdRadButt.Name = "sdRadButt";
            this.sdRadButt.Size = new System.Drawing.Size(49, 17);
            this.sdRadButt.TabIndex = 0;
            this.sdRadButt.TabStop = true;
            this.sdRadButt.Text = "480p";
            this.sdRadButt.UseVisualStyleBackColor = true;
            // 
            // hdRadButt
            // 
            this.hdRadButt.AutoSize = true;
            this.hdRadButt.Location = new System.Drawing.Point(371, 32);
            this.hdRadButt.Name = "hdRadButt";
            this.hdRadButt.Size = new System.Drawing.Size(49, 17);
            this.hdRadButt.TabIndex = 1;
            this.hdRadButt.Text = "720p";
            this.hdRadButt.UseVisualStyleBackColor = true;
            // 
            // makeSnapshot
            // 
            this.makeSnapshot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.makeSnapshot.Location = new System.Drawing.Point(9, 3);
            this.makeSnapshot.Name = "makeSnapshot";
            this.makeSnapshot.Size = new System.Drawing.Size(99, 23);
            this.makeSnapshot.TabIndex = 6;
            this.makeSnapshot.Text = "Take Snapshot";
            this.makeSnapshot.UseVisualStyleBackColor = true;
            this.makeSnapshot.Click += new System.EventHandler(this.makeSnapshot_Click);
            this.makeSnapshot.KeyUp += new System.Windows.Forms.KeyEventHandler(this.saveSnap_KeyUp);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 386);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // hdRadButt1080
            // 
            this.hdRadButt1080.AutoSize = true;
            this.hdRadButt1080.Location = new System.Drawing.Point(310, 32);
            this.hdRadButt1080.Name = "hdRadButt1080";
            this.hdRadButt1080.Size = new System.Drawing.Size(55, 17);
            this.hdRadButt1080.TabIndex = 9;
            this.hdRadButt1080.Text = "1080p";
            this.hdRadButt1080.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(332, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "PS3 Output";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(225, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Find DUMP";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // vidFrame
            // 
            this.vidFrame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.vidFrame.Location = new System.Drawing.Point(400, 3);
            this.vidFrame.Name = "vidFrame";
            this.vidFrame.Size = new System.Drawing.Size(79, 23);
            this.vidFrame.TabIndex = 12;
            this.vidFrame.Text = "Refresh Const";
            this.vidFrame.UseVisualStyleBackColor = false;
            this.vidFrame.Visible = false;
            this.vidFrame.Click += new System.EventHandler(this.vidFrame_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(529, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "DEV 32";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // snapshot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.vidFrame);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hdRadButt1080);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.findFrame);
            this.Controls.Add(this.frameStart);
            this.Controls.Add(this.saveSnap);
            this.Controls.Add(this.makeSnapshot);
            this.Controls.Add(this.snapPauseGame);
            this.Controls.Add(this.snapPB);
            this.Controls.Add(this.sdRadButt);
            this.Controls.Add(this.hdRadButt);
            this.Name = "snapshot";
            this.Size = new System.Drawing.Size(658, 386);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.saveSnap_KeyUp);
            this.Resize += new System.EventHandler(this.snapshot_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.snapPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox snapPB;
        private System.Windows.Forms.Button saveSnap;
        private System.Windows.Forms.Button findFrame;
        private System.Windows.Forms.TextBox frameStart;
        private System.Windows.Forms.CheckBox snapPauseGame;
        private System.Windows.Forms.RadioButton sdRadButt;
        private System.Windows.Forms.RadioButton hdRadButt;
        private System.Windows.Forms.Button makeSnapshot;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.RadioButton hdRadButt1080;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button vidFrame;
        private System.Windows.Forms.Button button2;
    }
}
