using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace NetCheatPS3
{

    public partial class Form1 : Form
    {
        #region NetCheat PS3 Global Variables
        public static uint ProcessID;
        public static uint[] processIDs;
        public static PS3TMAPI.SNRESULT snresult;
        public static string usage;
        public static string Info;
        public static PS3TMAPI.ConnectStatus connectStatus;
        public static string Status;
        public static string MemStatus;
        public static bool connected = false;
        public static int CodesCount = 0; /* Number of codes */
        public static int ConstantLoop = 0; /* 0 = loop but don't exec, 1 = loop and exec, 2 = exit */
        public static bool bComment = false; // If true, all code lines will be skipped until a "*/" is reached
        public static int cbListIndex = 0; /* Current Code open */

        public const int MaxCodes = 1000; /* Max number of codes */

        /* Search variables */
        public const int MaxRes = 10000; /* Max number of results */
        public static ulong SchResCnt = 0; /* Number of results */
        public static int NextSAlign = 0; /* Initial Search sets this to the current alignment */
        public static bool ValHex = true; /* Determines whether the value is in hex or not */
        public static bool NewSearch = true; /* When true, Initial Scan will show */
        public static int CancelSearch = 0; /* When 1 the search will cancel, 2 = stop */
        public static ulong GlobAlign = 0; /* Alignment */
        public static string dFileName = "a.txt"; /* Dump file when searching */
        public static int compMode = 0; /* Comparison type */

        /* Pre-parsed code struct */
        public struct CodeData
        {
            public byte[] val;
            public ulong addr;
            public char type;
            public byte[] jbool; /* Joker boolean value */
            public int jsize; /* Numbero of lines to execute with it true; joker */
        }

        /* Code struct */
        public struct CodeDB
        {          /* Structure for a single code */
            public bool state;          /* Determines whether to write constantly or not */
            public string name;         /* Name of Code */
            public string codes;        /* Holds codes string */
            public CodeData[] CData;    /* Holds codes in parsed format */
            public string filename;     /* For use with the 'Save' button */
        };

        /* Search result struct - NOT USED */
        public struct CodeRes
        {    /* Structure for a single search result */
            public bool state;     /* Determines whether to write constantly or not */
            public ulong addr;    /* Address of the code */
            public byte[] val;     /* Value of the code */
            public int align;      /* Alignment of the code */
        };

        /* List result struct */
        public struct ListRes
        {
            public string Addr;
            public string HexVal;
            public string DecVal;
            public string AlignStr;
        }

        /* Codes Array */
        public static CodeDB[] Codes = new CodeDB[MaxCodes];

        /* Search Types */
        public const int compEq = 0;        /* Equal To */
        public const int compNEq = 1;       /* Not Equal To */
        public const int compLT = 2;        /* Less Than */
        public const int compLTE = 3;       /* Less Than Or Equal To */
        public const int compGT = 4;        /* Greater Than */
        public const int compGTE = 5;       /* Greater Than Or Equal To */
        public const int compVBet = 6;      /* Value Between */
        public const int compINC = 7;       /* Increased Value */
        public const int compDEC = 8;       /* Decreased Value */
        public const int compChg = 9;       /* Changed Value */
        public const int compUChg = 10;     /* Unchanged Value */

        public const int compANEq = 20;     /* And Equal (used with E joker type) */

        /* Input Box Argument Structure */
        public struct IBArg
        {
            public string label;
            public string defStr;
            public string retStr;
        };

        /* ForeColor and BackColor */
        public static Color ncBackColor = Color.Black;
        public static Color ncForeColor = Color.FromArgb(0, 130, 210);
        /* Keybind arrays */
        public static Keys[] keyBinds = new Keys[8];
        public static string[] keyNames = new string[] {
            "Connect And Attach", "Disconnect",
            "Initial Scan", "Next Scan", "Stop", "Refresh Results",
            "Toggle Constant Write", "Write"
        };
        /* Settings file path */
        public static string settFile = "";
        /* String array that holds each range import */
        public static string[] rangeImports = new string[0];
        /* Int array that defines the order of the recent ranges */
        public static int[] rangeOrder = new int[0];

        /* Plugin form related arrays */
        static PluginForm[] pluginForm = new PluginForm[0];
        public static bool[] pluginFormActive = new bool[0];
        public static int setplugWindow = -1;

        /* Delete block struct */
        public struct deleteArr
        {
            public int start;
            public int size;
        }

        /* Constant writing thread */
        public static System.Threading.Thread tConstWrite = new System.Threading.Thread(new System.Threading.ThreadStart(codes.BeginConstWriting));
        #endregion

        #region Interface Functions
        /* API related functions */
        public static void apiSetMem(ulong addr, byte[] val) //Set the memory
        {
            if (val != null && snresult == PS3TMAPI.SNRESULT.SN_S_OK)
                PS3TMAPI.ProcessSetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, addr, val);
        }

        public static void apiGetMem(ulong addr, ref byte[] val) //Gets the memory as a byte array
        {
            if (val != null && snresult == PS3TMAPI.SNRESULT.SN_S_OK)
                PS3TMAPI.ProcessGetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, addr, ref val);
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);

            /* Related to the keybindings */
            connectButton.KeyUp += new KeyEventHandler(Form1_KeyUp);
            attachProcessButton.KeyUp += new KeyEventHandler(Form1_KeyUp);
            ps3Disc.KeyUp += new KeyEventHandler(Form1_KeyUp);
            refPlugin.KeyUp += new KeyEventHandler(Form1_KeyUp);
            optButton.KeyUp += new KeyEventHandler(Form1_KeyUp);
        }

        /* Saves the options to the ncps3.ini file */
        public static void SaveOptions()
        {
            using (System.IO.StreamWriter fd = new System.IO.StreamWriter(settFile, false))
            {
                //KeyBinds
                for (int x = 0; x < keyBinds.Length; x++)
                {
                    string key = keyBinds[x].GetHashCode().ToString();
                    fd.WriteLine(key);
                }

                //Colors
                fd.WriteLine(ncBackColor.Name);
                fd.WriteLine(ncForeColor.Name);

                //Recently opened ranges order
                string range = "";
                foreach (int val in rangeOrder)
                    range += val.ToString() + ";";
                fd.WriteLine(range);

                //Recently opened ranges paths
                range = "";
                foreach (string str in rangeImports)
                    range += str + ";";
                fd.WriteLine(range);
            }
        }

        /* Everything else because I have no organization skills... */
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConstantLoop = 2;
            PS3TMAPI.Disconnect(0);
            this.statusLabel1.Text = "Disconnected";
            System.IO.File.Delete(dFileName);

            //Close all plugins
            foreach (PluginForm a in pluginForm)
            {
                a.Dispose();
                a.Close();
            }

            if (refPlugin.Text == "Close Plugins")
                Global.Plugins.ClosePlugins();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            int x = 0;
            //Set the settings file and load the settings
            settFile = Application.StartupPath + "\\ncps3.ini";
            if (System.IO.File.Exists(settFile))
            {
                string[] settLines = System.IO.File.ReadAllLines(settFile);
                try
                {
                    //Read the keybinds from the array
                    for (x = 0; x < keyBinds.Length; x++)
                        keyBinds[x] = (Keys)int.Parse(settLines[x]);

                    //Read the colors and update the form
                    ncBackColor = Color.FromArgb(int.Parse(settLines[x], System.Globalization.NumberStyles.HexNumber)); BackColor = ncBackColor; x++;
                    ncForeColor = Color.FromArgb(int.Parse(settLines[x], System.Globalization.NumberStyles.HexNumber)); ForeColor = ncForeColor; x++;
                    
                    //Read the recently opened ranges
                    string[] strRangeOrder = settLines[x].Split(';');
                    Array.Resize(ref rangeOrder, strRangeOrder.Length - 1);
                    for (int valRO = 0; valRO < rangeOrder.Length; valRO++)
                        if (strRangeOrder[valRO] != "")
                            rangeOrder[valRO] = int.Parse(strRangeOrder[valRO]);

                    x++;
                    rangeImports = settLines[x].Split(';'); 
                    //Get rid of extra "" at the end
                    Array.Resize(ref rangeImports, rangeImports.Length - 1);
                    UpdateRecRangeBox();
                    x++;
                }
                catch { }
            }

            refPlugin_Click(null, null);

            attachProcessButton.Enabled = false;

            //Add the first Code
            cbList.Items.Add("NEW CODE");
            //Set backcolor
            cbList.Items[0].ForeColor = ncForeColor;
            cbList.Items[0].BackColor = ncBackColor;

            Codes[CodesCount].name = "NEW CODE";
            Codes[CodesCount].state = false;
            cbSchAlign.SelectedIndex = 2;
            compBox.SelectedIndex = 0;
            dFileName = misc.DirOf(Application.ExecutablePath) + "\\dump.txt";

            cbList.Items[0].Selected = true;
            cbList.Items[0].Selected = false;

            //Add first range
            string[] a = { "00000000", "FFFFFFFC" };
            ListViewItem b = new ListViewItem(a);
            rangeView.Items.Add(b);

            //Update range array
            UpdateMemArray();

            //Update all the controls on the form
            int ctrl = 0;
            for (ctrl = 0; ctrl < Controls.Count; ctrl++)
            {
                Controls[ctrl].BackColor = ncBackColor;
                Controls[ctrl].ForeColor = ncForeColor;
            }

            //Update all the controls on the tabs
            for (ctrl = 0; ctrl < TabCon.TabPages.Count; ctrl++)
            {
                TabCon.TabPages[ctrl].BackColor = ncBackColor;
                TabCon.TabPages[ctrl].ForeColor = ncForeColor;
                //Color each control in the tab too
                for (int tabCtrl = 0; tabCtrl < TabCon.TabPages[ctrl].Controls.Count; tabCtrl++)
                {
                    TabCon.TabPages[ctrl].Controls[tabCtrl].BackColor = ncBackColor;
                    TabCon.TabPages[ctrl].Controls[tabCtrl].ForeColor = ncForeColor;
                }
            }

            toolStripDropDownButton1.BackColor = Color.Maroon;
        }

        /* Connects to PS3 */
        private void connectButton_Click(object sender, EventArgs e)
        {
            int retryAttempts = 0;

        label_retryConnect:

            if (retryAttempts > 2)
                return;

            retryAttempts++;

            ps3Disc_Click(null, null);
            
            this.statusLabel1.Text = "Connecting...";
            try
            {
                snresult = PS3TMAPI.InitTargetComms();
                if (snresult == PS3TMAPI.SNRESULT.SN_E_TM_NOT_RUNNING)
                {
                    this.statusLabel1.Text = "Failed to connect to PS3";
                    connected = false;
                    return;
                }
                //Debug.WriteLine("".PadRight(30) + snresult.ToString());

                PS3TMAPI.TCPIPConnectProperties connectProperties = new PS3TMAPI.TCPIPConnectProperties();

                snresult = PS3TMAPI.Connect(0, null);
                if (snresult == PS3TMAPI.SNRESULT.SN_S_OK)
                {
                    //Debug.WriteLine("".PadRight(30) + snresult.ToString());

                    PS3TMAPI.ConnectStatus connectStatus;
                    string usage;
                    snresult = PS3TMAPI.GetConnectStatus(0, out connectStatus, out usage);
                    //Debug.WriteLine("".PadRight(30) + snresult.ToString());

                    //PS3TMAPI.TargetInfo targetInfo = new PS3TMAPI.TargetInfo();
                    //snresult = PS3TMAPI.GetTargetInfo(ref targetInfo);
                    //Debug.WriteLine("".PadRight(30) + snresult.ToString());
                    connected = true;
                    this.statusLabel1.Text = "Connected";

                    connectButton.Enabled = false;
                    attachProcessButton.Enabled = true;
                    toolStripDropDownButton1.BackColor = Color.DarkGoldenrod;
                }
                else if (snresult == PS3TMAPI.SNRESULT.SN_S_NO_ACTION && connected == false)
                    goto label_retryConnect;
                else if (snresult == PS3TMAPI.SNRESULT.SN_S_NO_ACTION)
                {
                    this.statusLabel1.Text = "Already connected to PS3";
                }
                else
                {
                    this.statusLabel1.Text = "Failed to connect to PS3";
                    connected = false;
                }
            }
            catch
            {
                this.statusLabel1.Text = "Failed to connect to PS3";
                connected = false;
            }
        }

        /* Attachs to process */
        private void attachProcessButton_Click(object sender, EventArgs e)
        {
            try
            {
                PS3TMAPI.GetProcessList(0, out processIDs);
                ulong uProcess = processIDs[0];
                ProcessID = Convert.ToUInt32(uProcess);
                PS3TMAPI.ProcessAttach(0, PS3TMAPI.UnitType.PPU, ProcessID);

                PS3TMAPI.ProcessContinue(0, ProcessID);
                this.statusLabel1.Text = "Process Attached";
                ConstantLoop = 1;

                attachProcessButton.Enabled = false;
                toolStripDropDownButton1.BackColor = Color.DarkGreen;
            }
            catch (Exception)
            {
                this.statusLabel1.Text = "Error attaching process; no game started?";
                toolStripDropDownButton1.BackColor = Color.DarkGoldenrod;
            }
        }

        /* Disconnects from PS3 */
        private void ps3Disc_Click(object sender, EventArgs e)
        {
            PS3TMAPI.Disconnect(0);
            ConstantLoop = 2;
            this.statusLabel1.Text = "Disconnected";
            processIDs = null;

            attachProcessButton.Enabled = false;
            connectButton.Enabled = true;
            toolStripDropDownButton1.BackColor = Color.Maroon;
        }

        /* Calculates the list index and updates the controls */
        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbListIndex = FindLI();
            if (cbListIndex < 0)
                cbListIndex = 0;

            UpdateCB(cbListIndex);
        }

        /* Finds the listindex of the listview cbList */
        public int FindLI()
        {
            int x = 0;

            //Potentially let the list update the selected Code
            Application.DoEvents();

            //Finds the list index (selected Code)
            for (x = 0; x <= cbList.Items.Count - 1; x++)
            {
                if (cbList.Items[x].Selected == true)
                    return x;
            }

            return -1;
        }

        /* Updates the code name, state, and codes */
        private void UpdateCB(int Index)
        {
            if (Index < 0 || Index >= MaxCodes)
                return;

            //Update the textboxes to the new code
            cbName.Text = Codes[Index].name;
            cbCodes.Text = Codes[Index].codes;
            cbState.Checked = Codes[Index].state;
        }

        /* Adds a new code to the list */
        private void cbAdd_Click(object sender, EventArgs e)
        {
            cbList.Items.Add("NEW CODE");
            CodesCount = cbList.Items.Count - 1;
            cbList.Items[CodesCount].ForeColor = ncForeColor;
            cbList.Items[CodesCount].BackColor = ncBackColor;
            Codes[CodesCount].name = "NEW CODE";
            Codes[CodesCount].state = false;
            Codes[CodesCount].codes = "";
        }

        /* Removes a code from the list */
        private void cbRemove_Click(object sender, EventArgs e)
        {
            int ind = cbListIndex, x = 0;
            if (ind < 0)
                return;

            cbList.Items[ind].Remove();

            for (x = ind; x <= (cbList.Items.Count - 1); x++)
            {
                Codes[x].codes = Codes[x + 1].codes;
                Codes[x].name = Codes[x + 1].name;
                Codes[x].state = Codes[x + 1].state;
            }

            Codes[x].codes = "NEW CODE";
            Codes[x].name = "";
            Codes[x].state = false;

            if (cbListIndex >= (cbList.Items.Count - 1))
                cbListIndex = cbList.Items.Count - 1;

            UpdateCB(cbListIndex);
        }

        /* Imports a ncl file into the the list */
        private void cbImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "NetCheat List files (*.ncl)|*.ncl|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                CodeDB[] ret = fileio.OpenFile(fd.FileName);

                if (ret == null)
                    return;

                int cnt = 0;
                if (cbListIndex >= 0)
                    cbList.Items[cbListIndex].Selected = false;
                for (int x = 0; x < ret.Length; x++)
                {

                    if (ret[x].name == null)
                        break;

                    ret[x].filename = fd.FileName;
                    if (ret[x].state)
                        cbList.Items.Add("+ " + ret[x].name);
                    else
                        cbList.Items.Add(ret[x].name);

                    cnt = cbList.Items.Count - 1;
                    cbList.Items[cnt].ForeColor = Color.FromArgb(0, 130, 210);
                    cbList.Items[cnt].BackColor = Color.Black;
                    Codes[cnt] = ret[x];
                }

                CodesCount = cnt;
                cbList.Items[cnt].Selected = true;
            }
        }

        /* Save the selected code as a ncl file */
        private void cbSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "NetCheat List files (*.ncl)|*.ncl|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                fileio.SaveFile(fd.FileName, Codes[cbListIndex]);
                Codes[cbListIndex].filename = fd.FileName;
            }
        }

        /* Saves all the codes as an ncl file */
        private void cbSaveAll_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "NetCheat List files (*.ncl)|*.ncl|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                fileio.SaveFileAll(fd.FileName);
            }
        }

        /* Quickly saves the code as a ncl */
        private void cbSave_Click(object sender, EventArgs e)
        {
            fileio.SaveFile(Codes[cbListIndex].filename, Codes[cbListIndex]);
        }

        /* Writes the selected code to the PS3 */
        private void cbWrite_Click(object sender, EventArgs e)
        {
            codes.WriteToPS32(Codes[cbListIndex]);
        }

        /* Toggles whether the code is constant writing or not */
        private void cbState_CheckedChanged(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;

            if (cbState.Checked == true)
                cbList.Items[ind].Text = "+ " + Codes[ind].name;
            else
                cbList.Items[ind].Text = Codes[ind].name;
            Codes[ind].state = cbState.Checked;

            ConstantLoop = 1;
        }

        /* Updates the name in cbList */
        private void cbName_TextChanged(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;

            Codes[ind].name = cbName.Text;
            if (Codes[ind].state == true)
            {
                cbList.Items[ind].Text = "+ " + cbName.Text;
            }
            else
            {
                cbList.Items[ind].Text = cbName.Text;
            }
        }

        /* Toggles constant write */
        private void cbList_DoubleClick(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;

            if (Codes[ind].state == true)
            {
                cbList.Items[ind].Text = Codes[ind].name;
                Codes[ind].state = false;
            }
            else
            {
                cbList.Items[ind].Text = "+ " + Codes[ind].name;
                Codes[ind].state = true;
                ConstantLoop = 1;
            }
            Application.DoEvents();
            UpdateCB(ind);
        }

        /* Converts the values in the search textboxes between hex and decimal */
        private void SchHexCheck_CheckedChanged(object sender, EventArgs e)
        {
            ValHex = SchHexCheck.Checked;

            if (schVal.Text.Length == 0)
                return;

            if (ValHex && schVal.Text.Length <= 8)
                schVal.Text = int.Parse(schVal.Text).ToString("X8"); //Dec to Hex
            else if (ValHex && schVal.Text.Length > 8)
                schVal.Text = Int64.Parse(schVal.Text).ToString("X16"); //Dec to Hex
            else if (schVal.Text.Length <= 8)
                schVal.Text = Convert.ToInt32(schVal.Text, 16).ToString(); //Hex to Dec
            else
                schVal.Text = Convert.ToInt64(schVal.Text, 16).ToString(); //Hex to Dec

            if (schVal2.Visible)
            {
                if (ValHex && schVal2.Text.Length <= 8)
                    schVal2.Text = int.Parse(schVal2.Text).ToString("X8"); //Dec to Hex
                else if (ValHex && schVal2.Text.Length > 8)
                    schVal2.Text = Int64.Parse(schVal2.Text).ToString("X16"); //Dec to Hex
                else if (schVal2.Text.Length <= 8)
                    schVal2.Text = Convert.ToInt32(schVal2.Text, 16).ToString(); //Hex to Dec
                else
                    schVal2.Text = Convert.ToInt64(schVal2.Text, 16).ToString(); //Hex to Dec
            }
        }

        private void compBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            compMode = compBox.SelectedIndex;
            if (compMode == 6)
            {
                schVal2.Visible = true;
                schVal2.Size = schVal.Size;
                schVal2.Left = schVal.Size.Width + 20 + schVal.Left;

                if (cbSchAlign.SelectedIndex == 5)
                    cbSchAlign.SelectedIndex = 0;
            }
            else
                schVal2.Visible = false;
        }

        private void cbSchAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchMode(cbSchAlign.SelectedIndex);

            /* Sizes */
            Size size_0 = new Size(30, 20);
            Size size_1 = new Size(60, 20);
            Size size_2 = new Size(120, 20);
            Size size_3 = new Size(240, 20);
            Size size_4 = new Size(180, 20);
            Size size_5 = new Size(394, 20);

            switch (cbSchAlign.SelectedIndex)
            {
                case 0: //1 byte
                    schVal.Size = size_0;
                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_0.Width + 20 + schVal.Left;
                        schVal2.Size = size_0;
                    }
                    break;
                case 1: //2 bytes
                    schVal.Size = size_1;

                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_1.Width + 20 + schVal.Left;
                        schVal2.Size = size_1;
                    }
                    break;
                case 2: //4 bytes
                    schVal.Size = size_2;

                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_2.Width + 20 + schVal.Left;
                        schVal2.Size = size_2;
                    }
                    break;
                case 3: //8 bytes
                    schVal.Size = size_3;

                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_3.Width + 20 + schVal.Left;
                        schVal2.Size = size_3;
                    }
                    break;
                case 4: //X bytes
                    schVal.Size = size_4;

                    if (schVal2.Visible)
                    {
                        schVal2.Left = size_4.Width + 20 + schVal.Left;
                        schVal2.Size = size_4;
                    }
                    break;
                case 5: //Text
                    schVal.Size = size_5;
                    schVal2.Visible = false;
                    if (compMode == 6)
                    {
                        compMode = 0;
                        compBox.SelectedIndex = 0;
                    }
                    break;

            }
        }

        private void DumpMem_Click(object sender, EventArgs e)
        {
            ulong rStart = 0, rStop = 0, cnt = 0, incVal = 0x10000;
            int rDif = 0;
            string file = "";
            bool NextSearch = false;

            if (schRange1.Text.Length != 8 || schRange2.Text.Length != 8)
                return;

            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
                file = fd.FileName;

            if (file == "")
                return;

            if (System.IO.File.Exists(file))
                System.IO.File.Delete(file);

            rStart = ulong.Parse(schRange1.Text, System.Globalization.NumberStyles.HexNumber);
            rStop = ulong.Parse(schRange2.Text, System.Globalization.NumberStyles.HexNumber);
            rDif = (int)misc.ParseRealDif(rStart, rStop, incVal);
            schProg.Maximum = rDif;
            schProg.Value = 0;

            statusLabel1.Text = "No, I am not going to add a cancel button. Just wait through it.";

            schSearch.Enabled = false;
            SchRef.Enabled = false;
            DumpMem.Enabled = false;
            NextSearch = schNSearch.Enabled;
            schNSearch.Enabled = false;

            System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Append,
                System.IO.FileAccess.Write);

            for (cnt = rStart; cnt < rStop; cnt += incVal)
            {
                if (cnt != misc.ParseSchAddr(cnt))
                {
                    int maCnt = 0;
                    uint size = 0;
                    while (maCnt < misc.MemArray.Length && cnt > misc.MemArray[maCnt])
                        maCnt++;

                    if (maCnt < misc.MemArray.Length)
                    {
                        size = (uint)misc.MemArray[maCnt] - (uint)misc.MemArray[maCnt - 1];
                        fs.Write(new byte[size], 0, (int)size);
                        cnt = misc.MemArray[maCnt];
                    }

                    int newVal = misc.ParseRealDif((ulong)rStart, cnt, incVal);
                    if ((int)newVal <= schProg.Maximum)
                        schProg.Value = (int)newVal;
                }
                else
                {
                    if ((cnt + incVal) > rStop)
                        incVal = (rStop - cnt);

                    byte[] retByte = new byte[incVal];

                    PS3TMAPI.ProcessGetMemory(0, PS3TMAPI.UnitType.PPU, ProcessID, 0, cnt, ref retByte);
                    fs.Write(retByte, 0, retByte.Length);

                    if ((schProg.Value + 1) <= schProg.Maximum)
                        schProg.Value++;
                }
                Application.DoEvents();
            }
            fs.Close();

            schSearch.Enabled = true;
            SchRef.Enabled = true;
            DumpMem.Enabled = true;
            schNSearch.Enabled = NextSearch;

            schProg.Value = 0;
            schProg.Maximum = 0;
            statusLabel1.Text = "Dump complete";
        }

        private void schSearch_Click(object sender, EventArgs e)
        {
            ulong rDif = 0, rStart = 0, rStop = 0;
            int align = 4;

            if (schSearch.Text == "Stop")
            {
                /* Setup CompBox */
                compBox.Items.RemoveAt(compBox.Items.Count - 1);
                compBox.Items.RemoveAt(compBox.Items.Count - 1);
                compBox.Items.RemoveAt(compBox.Items.Count - 1);
                compBox.Items.RemoveAt(compBox.Items.Count - 1);
                if (compBox.SelectedIndex < 0)
                    compBox.SelectedIndex = 0;

                CancelSearch = 2;
                if (SchPWS.Checked)
                    PS3TMAPI.ProcessContinue(0, ProcessID);
                return;
            }

            if (schRange1.Text.Length != 8 || schRange2.Text.Length != 8)
            {
                MessageBox.Show("Error: range addresses are not of proper length 8.");
                return;
            }

            if (SchPWS.Checked && schSearch.Text != "New Scan")
                PS3TMAPI.ProcessStop(0, ProcessID);

            switch (cbSchAlign.SelectedItem.ToString())
            {
                case "1 byte":
                    align = 1;
                    break;
                case "2 bytes":
                    align = 2;
                    break;
                case "4 bytes":
                    align = 4;
                    break;
                case "8 bytes":
                    align = 8;
                    break;
                case "X bytes":
                    align = -2;
                    break;
                case "Text":
                    align = -1;
                    break;
            }

            String ValStr = "", ValStr2 = "";
            if (align > 0 || align == -2)
            {
                if (ValHex)
                {
                    if (align > 0)
                    {
                        ValStr = schVal.Text.PadLeft(align * 2, '0');
                        schVal.Text = ValStr;
                        if (schVal2.Visible)
                        {
                            ValStr2 = schVal2.Text.PadLeft(align * 2, '0');
                            schVal2.Text = ValStr2;
                        }
                    }
                    else if (align == -2)
                    {
                        ValStr = schVal.Text;
                        if (schVal2.Visible)
                            ValStr2 = schVal2.Text;
                    }
                }
                else
                {
                    if (align == 8)
                        ValStr = ulong.Parse(schVal.Text).ToString("X16");
                    else
                        ValStr = ulong.Parse(schVal.Text).ToString("X8");

                    if (schVal2.Visible)
                        ValStr2 = ulong.Parse(schVal2.Text).ToString("X");
                }
            }

            byte[] sVal = null;
            byte[] c = null;
            if (align > 0)
                sVal = new byte[align];
            else if (align == -1)
                sVal = new byte[schVal.Text.Length];
            else if (align == -2)
                sVal = new byte[schVal.Text.Length / 2];

            rStart = ulong.Parse(schRange1.Text, System.Globalization.NumberStyles.HexNumber);
            rStop = ulong.Parse(schRange2.Text, System.Globalization.NumberStyles.HexNumber);

            if (align == 8)
                sVal = BitConverter.GetBytes(Int64.Parse(misc.ReverseE(ValStr.PadLeft(align * 2, '0'), (align * 2)), System.Globalization.NumberStyles.HexNumber));
            else if (align == -1)
                sVal = misc.StringToByteArray(schVal.Text);
            else if (align == -2)
                sVal = misc.StringBAToBA(schVal.Text);
            else if (align > 0)
                sVal = misc.StringBAToBA(ValStr); //BitConverter.GetBytes(int.Parse(misc.ReverseE(ValStr, (align * 2)), System.Globalization.NumberStyles.HexNumber));
            rDif = rStop - rStart;

            if (schVal2.Visible)
            {
                if (align == -2)
                    c = misc.StringBAToBA(ValStr2);
                else
                    c = misc.StringBAToBA(ValStr2);
            }

            if ((Int64)rDif <= 0)
            {
                MessageBox.Show("Error: range addresses are incompatible.");
                return;
            }

            if (compMode >= compBox.Items.Count)
            {
                compMode = 0;
                compBox.SelectedIndex = 0;
                return;
            }


            /* Setup the buttons */
            if (NewSearch)
            {
                /* Setup CompBox */
                compBox.Items.Add("Increased By");
                compBox.Items.Add("Decreased By");
                compBox.Items.Add("Changed Value");
                compBox.Items.Add("Unchanged Value");
                NewSearch = false;
                schSearch.Text = "Stop";
                //schNSearch.Enabled = true;
            }
            else
            {
                /* Setup CompBox */
                if (compBox.Items[compBox.Items.Count - 1].ToString() == "Unchanged Value")
                {
                    compBox.Items.RemoveAt(compBox.Items.Count - 1);
                    compBox.Items.RemoveAt(compBox.Items.Count - 1);
                    compBox.Items.RemoveAt(compBox.Items.Count - 1);
                    compBox.Items.RemoveAt(compBox.Items.Count - 1);
                    if (compBox.SelectedIndex < 0)
                        compBox.SelectedIndex = 0;
                }
                NewSearch = true;
                lvSch.Items.Clear();
                schSearch.Text = "Initial Scan";
                schNSearch.Enabled = false;
                return;
            }

            ulong recvCnt = 0, sSize = 0x10000;

            rDif = (ulong)misc.ParseRealDif(rStart, rStop, sSize);
            schProg.Value = 0;
            schProg.Maximum = (int)rDif;
            SchResCnt = 0;
            int len = schVal.Text.Length;
            if (!SchHexCheck.Checked)
                len = sVal.Length;
            NextSAlign = align;

            //Calculate the size of the ret byte
            if ((rStop - rStart) < sSize)
                sSize = (rStop - rStart);

            if (System.IO.File.Exists(dFileName))
                System.IO.File.Delete(dFileName);

            Application.DoEvents();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dFileName, true))
            {

                lvSch.BeginUpdate();
                for (recvCnt = rStart; rDif > 0; recvCnt += sSize)
                {
                    ulong oldRecv = recvCnt;
                    recvCnt = misc.ParseSchAddr(recvCnt);
                    if (recvCnt >= rStop || (recvCnt == 0 && oldRecv != 0))
                        break;

                    ulong ret = 0;
                    //Calculate the size of the ret byte
                    if ((rStop - recvCnt) < sSize)
                        sSize = (rStop - recvCnt);

                    if (align > 0)
                        ret = InitSearch(recvCnt, sVal, c, align, sSize, file);
                    else if (align == -1)
                        ret = InitSearchText(recvCnt, sVal, c, len * 2, sSize, file);
                    else if (align == -2)
                        ret = InitSearchText(recvCnt, sVal, c, len, sSize, file);

                    //Only refresh if there are results to add
                    if (ret > 0)
                    {
                        lvSch.EndUpdate();
                        Application.DoEvents();
                        lvSch.BeginUpdate();
                    }

                    SchResCnt += ret;
                    this.statusLabel1.Text = "Results found: " + SchResCnt.ToString();
                    /*
                    if (SchResCnt >= MaxRes)
                    {
                        SchResCnt = MaxRes; SchResCnt--;
                        schSearch.Text = "New Scan";
                        schProg.Value = 0;
                        schProg.Maximum = 0;
                        statusStrip1.Text = "Last result of search: " + recvCnt.ToString("X8");
                        break;
                    }
                    */

                    if (CancelSearch == 1)
                    {
                        NewSearch = true;
                        schSearch.Text = "Initial Scan";
                        schNSearch.Enabled = false;
                        schProg.Maximum = 0;
                        schProg.Value = 0;
                        SchResCnt = 0;
                        lvSch.Items.Clear();
                        CancelSearch = 0;
                        lvSch.EndUpdate();
                        return;
                    }
                    else if (CancelSearch == 2)
                        goto exitInitSearchLoop;

                    if ((schProg.Value + 1) < schProg.Maximum)
                        schProg.Value++; //+= (int)sSize;
                    rDif--; //-= sSize;
                    Application.DoEvents();
                }
            exitInitSearchLoop: ;
            }
                //RefreshSearchResults(0);

            schSearch.Text = "New Scan";
            schProg.Maximum = 0;
            NewSearch = false;
            schNSearch.Enabled = true;
            schProg.Value = 0;

            GlobAlign = (ulong)align;
            if (SchPWS.Checked)
                PS3TMAPI.ProcessContinue(0, ProcessID);
            CancelSearch = 0;
            lvSch.EndUpdate();
        }

        private void SchRef_Click(object sender, EventArgs e)
        {
            RefreshSearchResults(1);
        }

        private void schNSearch_Click(object sender, EventArgs e)
        {
            byte[] sVal = null;
            byte[] c = null;

            if (schNSearch.Text == "Cancel")
            {
                CancelSearch = 1;
                if (SchPWS.Checked)
                    PS3TMAPI.ProcessContinue(0, ProcessID);
                return;
            }

            if (NewSearch == false)
            {
                NewSearch = true;
                schNSearch.Text = "Cancel";
            }
            else
            {
                NewSearch = false;
                schNSearch.Text = "Next Scan";
                return;
            }

            if (NextSAlign > 0 || NextSAlign == -2)
            {
                int align = Form1.NextSAlign;
                String ValStr = "", ValStr2 = "";
                int oldA = align;
                if (align == -2)
                    align = schVal.Text.Length / 2;

                if (ValHex)
                {
                    ValStr = schVal.Text.PadLeft(align * 2, '0');
                    schVal.Text = ValStr;
                    if (schVal2.Visible)
                    {
                        ValStr2 = schVal2.Text.PadLeft(align * 2, '0');
                        schVal2.Text = ValStr2;
                    }
                }
                else
                {
                    ValStr = int.Parse(schVal.Text).ToString("X8");
                }

                if (ValStr == "")
                {
                    MessageBox.Show("Error: please perform an initial search first.");
                    return;
                }

                sVal = misc.StringBAToBA(ValStr);
                if ((int)NextSAlign == -2)
                    align = ValStr2.Length;
                if (schVal2.Visible)
                    c = misc.StringBAToBA(ValStr2);
                align = oldA;
            }

            if (SchPWS.Checked)
                PS3TMAPI.ProcessStop(0, ProcessID);

            ulong a = 0;
            if (NextSAlign == -1)
            {                                                           
                sVal = misc.StringToByteArray(schVal.Text);                                                                            
                a = NextSearchText(sVal, c, NextSAlign);
            }
            else if (NextSAlign == -2)
            {
                byte[] newB = new byte[schVal.Text.Length / 2];
                Array.Copy(sVal, 0, newB, 0, schVal.Text.Length / 2);
                a = NextSearchText(newB, c, NextSAlign);
            }
            else if (NextSAlign > 0)
                a = NextSearch(sVal, c, NextSAlign);

            if (CancelSearch == 0)
            {
                SchResCnt = a;

                this.statusLabel1.Text = "Results found: " + SchResCnt.ToString();
                RefreshSearchResults(0);
            }

            schNSearch.Text = "Next Scan";
            NewSearch = false;
            schProg.Maximum = 0;
            schProg.Value = 0;

            if (SchPWS.Checked)
                PS3TMAPI.ProcessContinue(0, ProcessID);
            CancelSearch = 0;
            lvSch.EndUpdate();
        }

        private void lvSch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lvSch_KeyUp(object sender, KeyEventArgs e)
        {
            //Refresh
            if (e.KeyCode == Keys.R && e.Control)
            {
                RefreshFromDump();
            }
            //Copy
            else if (e.KeyValue == 67)
            {
                CopyResults();
            }
        }

        private void SearchMode(int mode)
        {

            switch (mode)
            {
                case 0: //1 byte
                    SchHexCheck.Visible = true;
                    break;
                case 1: //2 bytes
                    SchHexCheck.Visible = true;
                    break;
                case 2: //4 bytes
                    SchHexCheck.Visible = true;
                    break;
                case 3: //8 bytes
                    SchHexCheck.Visible = true;
                    break;
                case 4: //X bytes
                    SchHexCheck.Visible = true;
                    break;
                case 5: //Text
                    SchHexCheck.Visible = false;
                    break;
            }
        }

        public ulong InitSearch(ulong sStart, byte[] sVal, byte[] c, int align, ulong sSize, System.IO.StreamWriter fStream)
        {
            ulong ResCnt = 0, sCount = 0;
            byte[] ret = new byte[sSize];

            PS3TMAPI.ProcessGetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, sStart, ref ret);

            while (sCount < sSize)
            {
                //if (Form1.CancelSearch)
                //    return 0;

                byte[] argB = new byte[(int)align];
                Array.Copy(ret, (int)sCount, argB, 0, argB.Length);
                if (misc.ArrayCompare(sVal, argB, c, compMode))
                {
                    ListRes a = misc.GetlvVals(align, ret, (int)sCount);

                    string[] row = { (sStart + sCount).ToString("X8"), a.HexVal, a.DecVal, a.AlignStr };
                    var listViewItem = new ListViewItem(row);
                    lvSch.Items.Add(listViewItem);

                    //fileio.AppendDump(Form1.SchRes[Form1.SchResCnt + ResCnt], dFileName);
                    fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sVal, " ") + " " + align);

                    ResCnt++;
                }
                sCount += (ulong)align;
            }
            return ResCnt;
        }

        public ulong InitSearchText(ulong sStart, byte[] sText, byte[] c, int len, ulong sSize, System.IO.StreamWriter fStream)
        {
            ulong ResCnt = 0, sCount = 0;
            byte[] ret = new byte[sSize];

            PS3TMAPI.ProcessGetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, sStart, ref ret);

            while ((sCount + (ulong)sText.Length) <= sSize)
            {

                if (Form1.CancelSearch == 1)
                    return 0;
                
                //if (misc.ArrayCompare(sTextLong, ret, cLong, len / 2, (int)sCount, compMode))
                byte[] argB = new byte[len / 2];
                Array.Copy(ret, (int)sCount, argB, 0, argB.Length);
                if (misc.ArrayCompare(sText, argB, c, compMode))
                {
                    byte[] newB = new byte[len / 2];
                    Array.Copy(ret, (int)sCount, newB, 0, newB.Length);

                    ListRes a = misc.GetlvVals((int)NextSAlign, newB, 0);
                    string[] row = { (sStart + sCount).ToString("X8"), a.HexVal, a.DecVal, a.AlignStr};

                    var listViewItem = new ListViewItem(row);
                    lvSch.Items.Add(listViewItem);

                    if ((int)NextSAlign == -1)
                        fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sText, " ") + " " + (len / 2) + " -1");
                    else if ((int)NextSAlign == -2)
                        fStream.WriteLine((sStart + sCount) + " " + misc.ByteAToStringInt(sText, " ") + " " + (len / 2) + " -2");

                    ResCnt++;
                }
                sCount++;
            }
            return ResCnt;
        }

        public ulong NextSearch(byte[] sVal, byte[] c, int align)
        {
            ulong cnt = 0, ResCnt = 0;
            byte[] ret = new byte[align];
            schProg.Maximum = (int)SchResCnt + 1;
            schProg.Value = 0;
            ulong maxRes2 = SchResCnt;
            ulong x = 0;
            if (SchResCnt >= MaxRes)
                maxRes2 = MaxCodes;

            Form1.CodeRes[] tempSchRes = new Form1.CodeRes[maxRes2];

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dFileName + "2", true))
            {

                for (cnt = 0; cnt < SchResCnt; cnt++)
                {
                    tempSchRes = fileio.ReadDumpArray(dFileName, (int)cnt, (int)maxRes2 + (int)cnt, align);

                    for (x = 0; x < maxRes2; x++)
                    {

                        if (cnt >= SchResCnt)
                            break;
                        cnt++;

                        if (cnt == (SchResCnt - 1))
                            cnt = SchResCnt - 1;

                        if (CancelSearch != 0)
                        {
                            schNSearch.Text = "Next Scan";
                            schProg.Maximum = 0;
                            schProg.Value = 0;
                            lvSch.Items.Clear();
                        }
                        if (CancelSearch == 1)
                            return 0;
                        if (CancelSearch == 2)
                            return 0;

                        PS3TMAPI.ProcessGetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, tempSchRes[x].addr, ref ret);

                        schProg.Value++;
                        Application.DoEvents();

                        if (compMode == compINC || compMode == compDEC || compMode == compChg || compMode == compUChg)
                            c = tempSchRes[x].val;

                        if (misc.ArrayCompare(sVal, ret, c, compMode))
                        {
                            file.WriteLine(tempSchRes[x].addr + " " + misc.ByteAToStringInt(ret, " ") + " " + align);
                            ResCnt++;
                        }
                    }
                }
            }

            System.IO.File.Delete(dFileName);
            System.IO.File.Copy(dFileName + "2", dFileName);
            System.IO.File.Delete(dFileName + "2");

            return ResCnt;
        }

        public ulong NextSearchText(byte[] sVal, byte[] cVal, int align)
        {
            ulong cnt = 0, ResCnt = 0;
            schProg.Maximum = (int)SchResCnt;
            schProg.Value = 0;
            ulong maxRes2 = SchResCnt;
            ulong x = 0;
            if (SchResCnt >= MaxRes)
                maxRes2 = MaxCodes;

            Form1.CodeRes[] tempSchRes = new Form1.CodeRes[maxRes2];
            for (cnt = 0; cnt < SchResCnt; cnt++)
            {

                tempSchRes = fileio.ReadDumpArray(dFileName, (int)cnt, (int)maxRes2 + (int)cnt, align);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(dFileName + "2", true))
                {

                    for (x = 0; x < maxRes2; x++)
                    {
                        cnt++;
                        byte[] ret = new byte[sVal.Length];

                        if (CancelSearch == 1)
                        {
                            schNSearch.Text = "Next Scan";
                            schProg.Maximum = 0;
                            schProg.Value = 0;

                            return 0;
                        }

                        PS3TMAPI.ProcessGetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, tempSchRes[x].addr, ref ret);

                        schProg.Value++;
                        Application.DoEvents();

                        if (misc.ArrayCompare(sVal, ret, cVal, compMode))
                        {
                            string end = " -1";
                            if (NextSAlign == -2)
                                end = " -2";
                            file.WriteLine(tempSchRes[x].addr + " " + misc.ByteAToStringInt(ret, " ") + " " + sVal.Length + end);
                            ResCnt++;
                        }
                    }
                }

                System.IO.File.Delete(dFileName);
                System.IO.File.Copy(dFileName + "2", dFileName);
                System.IO.File.Delete(dFileName + "2");
            }

            return ResCnt;
        }

        public void RefreshSearchResults(int mode)
        {
            ulong x = 0;
            bool reloadDump = false;
            ListView.ListViewItemCollection items = new ListView.ListViewItemCollection(lvSch);
            //lvSch.Items.

            switch (mode)
            {
                case 0: /* Refresh by reloading everything from the dump */
                    lvSch.Items.Clear();

                    if (Form1.SchResCnt == 0)
                        reloadDump = true;

                    if (reloadDump)
                    {
                        lvSch.BeginUpdate();
                        while (true)
                        {
                            String Addr = "";

                            CodeRes[] retRes = fileio.ReadDumpArray(dFileName, (int)x, (MaxRes - 1) + (int)x, (int)GlobAlign);
                            if (retRes == null)
                                return;

                            GlobAlign = (ulong)retRes[0].align;
                            NextSAlign = (int)retRes[0].align;

                            for (int z = 0; z < retRes.Length; z++)
                            {

                                if (retRes[z].val == null)
                                    goto nextZ;

                                ListRes a = misc.GetlvVals((int)GlobAlign, retRes[z].val, 0);

                                Addr = (retRes[z].addr).ToString("X8");

                                string[] row = { Addr, a.HexVal, a.DecVal, a.AlignStr };
                                var listViewItem = new ListViewItem(row);
                                //lvSch.Items.Add(listViewItem);
                                items.Add(listViewItem);

                            nextZ:
                                if ((x % 1000) == 0)
                                {
                                    statusLabel1.Text = "Results: " + x.ToString();
                                    Application.DoEvents();
                                }
                                if (retRes[z].val == null && reloadDump)
                                {
                                    Form1.SchResCnt = x;
                                    statusLabel1.Text = "Results: " + x.ToString();
                                    lvSch.EndUpdate();
                                    NewSearch = false;
                                    schNSearch.Enabled = true;
                                    return;
                                }
                                x++;
                            }
                        }
                    }
                    else
                    {
                        lvSch.BeginUpdate();
                        //for (x = 0; x < Form1.SchResCnt; x++)
                        while (x < Form1.SchResCnt)
                        {
                            String Addr = "";

                            CodeRes[] retRes = fileio.ReadDumpArray(dFileName, (int)x, (MaxRes - 1) + (int)x, (int)GlobAlign);
                            if (retRes == null)
                                return;

                            if ((int)x < retRes.Length)
                            {
                                GlobAlign = (ulong)retRes[x].align;
                                NextSAlign = (int)retRes[x].align;
                            }

                            int z = 0;
                            for (z = 0; z < retRes.Length; z++)
                            {
                                if (retRes[z].val == null)
                                    goto nextZ;

                                ListRes a = misc.GetlvVals((int)GlobAlign, retRes[z].val, 0);

                                Addr = (retRes[z].addr).ToString("X8");

                                string[] row = { Addr, a.HexVal, a.DecVal, a.AlignStr };
                                var listViewItem = new ListViewItem(row);
                                items.Add(listViewItem);

                            nextZ:
                                if ((z % 1000) == 0)
                                    Application.DoEvents();
                            }
                            x += (ulong)z;
                        }
                        lvSch.EndUpdate();
                    }

                    break;
                case 1: /* Refresh everything by grabbing the values from the PS3 */
                    lvSch.BeginUpdate();
                    for (x = 0; x < Form1.SchResCnt; x++)
                    {
                        String Addr = "";
                        int align = (int)GlobAlign;

                        CodeRes[] retRes = fileio.ReadDumpArray(dFileName, (int)x, MaxRes + (int)x, (int)GlobAlign);
                        if (retRes == null)
                            return;

                        for (int z = 0; z < retRes.Length; z++)
                        {

                            if (x >= Form1.SchResCnt)
                                break;

                            if ((int)GlobAlign == -1 || (int)GlobAlign == -2)
                                align = retRes[z].val.Length;

                            byte[] ret = new byte[align];
                            PS3TMAPI.ProcessGetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, retRes[z].addr, ref ret);

                            ListRes a = new ListRes();
                            a = misc.GetlvVals((int)GlobAlign, ret, 0);

                            Addr = (retRes[z].addr).ToString("X8");

                            if ((int)x < lvSch.Items.Count)
                            {
                                items[(int)x].SubItems[0].Text = Addr;
                                items[(int)x].SubItems[1].Text = a.HexVal;
                                items[(int)x].SubItems[2].Text = a.DecVal;
                                items[(int)x].SubItems[3].Text = a.AlignStr;
                            }
                            else
                            {
                                string[] row = { Addr, a.HexVal, a.DecVal, a.AlignStr };
                                var listViewItem = new ListViewItem(row);
                                items.Add(listViewItem);
                            }


                            x++;
                            if ((x % 500) == 0)
                                Application.DoEvents();
                        }
                    }
                    lvSch.EndUpdate();
                    break;
            }
        }

        private void cbCodes_TextChanged(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;

            CodesCount = cbList.Items.Count - 1;
            Codes[ind].codes = cbCodes.Text;
            codes.UpdateCData(Codes[ind].codes, ind);
        }

        private void cbCodes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Shift == false && e.Control)
            {
                cbCodes.SelectionStart = 0;
                cbCodes.SelectionLength = cbCodes.Text.Length;
            }
        }

        private void cbCodes_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void AddRange_Click(object sender, EventArgs e)
        {
            string[] rV = { "00000000", "00000000" };
            ListViewItem a = new ListViewItem(rV);
            rangeView.Items.Add(a);

            //Update range array
            UpdateMemArray();
        }

        private void RemoveRange_Click(object sender, EventArgs e)
        {
            if (rangeListIndex < 0)
                return;

            rangeView.Items.RemoveAt(rangeListIndex);

            //Update range array
            UpdateMemArray();

            if (rangeListIndex >= rangeView.Items.Count)
                rangeListIndex = (rangeView.Items.Count - 1);

            if (rangeListIndex < 0)
                return;

            rangeView.Items[rangeListIndex].Selected = true;
        }

        public int rangeListIndex = -1;
        private void rangeView_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int x = 0; x < rangeView.Items.Count; x++)
            {
                if (rangeView.Items[x].Selected)
                {
                    rangeListIndex = x;
                    return;
                }
            }
        }

        private void rangeView_DoubleClick(object sender, EventArgs e)
        {
            IBArg[] a = new IBArg[2];

            a[0].defStr = rangeView.Items[rangeListIndex].SubItems[0].Text;
            a[0].label = "Start Address";

            a[1].defStr = rangeView.Items[rangeListIndex].SubItems[1].Text;
            a[1].label = "End Address";

            a = CallIBox(a);

            if (a == null)
                return;

            if (a[0].retStr.Length == 8)
                rangeView.Items[rangeListIndex].SubItems[0].Text = a[0].retStr;
            else
                rangeView.Items[rangeListIndex].SubItems[0].Text = a[0].defStr;

            if (a[1].retStr.Length == 8)
                rangeView.Items[rangeListIndex].SubItems[1].Text = a[1].retStr;
            else
                rangeView.Items[rangeListIndex].SubItems[1].Text = a[1].defStr;

            //Update range array
            UpdateMemArray();
        }

        /* Brings up the Input Box with the arguments of a */
        public IBArg[] CallIBox(IBArg[] a)
        {
            InputBox ib = new InputBox();

            ib.Arg = a;
            ib.fmHeight = this.Height;
            ib.fmWidth = this.Width;
            ib.fmLeft = this.Left;
            ib.fmTop = this.Top;
            ib.TopMost = true;
            ib.fmForeColor = ForeColor;
            ib.fmBackColor = BackColor;
            ib.Show();

            while (ib.ret == 0)
            {
                a = ib.Arg;
                Application.DoEvents();
            }
            a = ib.Arg;

            if (ib.ret == 1)
                return a;
            else if (ib.ret == 2)
                return null;

            return null;
        }

        private void ImportRange_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "NetCheat Memory Range files (*.ncm)|*.ncm|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                //Check if file is already added to the recent range imports
                bool added = false;
                foreach (string rI in rangeImports)
                {
                    if (rI == fd.FileName)
                    {
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    //File path
                    Array.Resize(ref rangeImports, rangeImports.Length + 1);
                    rangeImports[rangeImports.Length - 1] = fd.FileName;

                    //Order
                    Array.Resize(ref rangeOrder, rangeOrder.Length + 1);
                    for (int rO = 0; rO < rangeOrder.Length - 1; rO++)
                        rangeOrder[rO]++;
                    rangeOrder[rangeOrder.Length - 1] = 0;

                    //Add to recRangeBox
                    System.IO.FileInfo fi = new System.IO.FileInfo(fd.FileName);
                    ListViewItem lvi = new ListViewItem(new string[] { fi.Name });
                    lvi.Tag = rangeOrder.Length - 1;
                    lvi.ToolTipText = fi.FullName;
                    recRangeBox.Items.Insert(0, lvi);

                    SaveOptions();
                }


                ListView a = new ListView();
                a = fileio.OpenRangeFile(fd.FileName);

                if (a == null)
                    return;

                rangeView.Items.Clear();
                string[] str = new string[2];
                for (int x = 0; x < a.Items.Count; x++)
                {
                    str[0] = a.Items[x].SubItems[0].Text;
                    str[1] = a.Items[x].SubItems[1].Text;
                    ListViewItem strLV = new ListViewItem(str);
                    rangeView.Items.Add(strLV);
                }

                //Update range array
                UpdateMemArray();

                Text = "NetCheat PS3 4.1 by Dnawrkshp (" + new System.IO.FileInfo(fd.FileName).Name + ")";
            }
        }

        private void SaveRange_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "NetCheat Memory Range files (*.ncm)|*.ncm|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                fileio.SaveRangeFile(fd.FileName, rangeView);

                //Check if file is already added to the recent range imports
                bool added = false;
                foreach (string rI in rangeImports)
                {
                    if (rI == fd.FileName)
                    {
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    //File path
                    Array.Resize(ref rangeImports, rangeImports.Length + 1);
                    rangeImports[rangeImports.Length - 1] = fd.FileName;

                    //Order
                    Array.Resize(ref rangeOrder, rangeOrder.Length + 1);
                    for (int rO = 0; rO < rangeOrder.Length - 1; rO++)
                        rangeOrder[rO]++;
                    rangeOrder[rangeOrder.Length - 1] = 0;

                    //Add to recRangeBox
                    System.IO.FileInfo fi = new System.IO.FileInfo(fd.FileName);
                    ListViewItem lvi = new ListViewItem(new string[] { fi.Name });
                    lvi.Tag = rangeOrder.Length - 1;
                    lvi.ToolTipText = fi.FullName;
                    recRangeBox.Items.Insert(0, lvi);

                    SaveOptions();
                }
            }
        }

        /* Updates the memory array (range) to the array specified by the rangeView listview */
        public void UpdateMemArray()
        {
            misc.MemArray = new uint[rangeView.Items.Count * 2];

            for (int x = 0; x < rangeView.Items.Count; x++)
            {
                misc.MemArray[x * 2] = uint.Parse(rangeView.Items[x].SubItems[0].Text,
                    System.Globalization.NumberStyles.HexNumber);
                misc.MemArray[(x * 2) + 1] = uint.Parse(rangeView.Items[x].SubItems[1].Text,
                    System.Globalization.NumberStyles.HexNumber);
            }
        }

        private void RangeUp_Click(object sender, EventArgs e)
        {
            if (rangeListIndex <= 0)
                return;

            ListViewItem selected = rangeView.Items[rangeListIndex];

            rangeView.Items.RemoveAt(rangeListIndex);
            rangeView.Items.Insert(rangeListIndex - 1, selected);
        }

        private void RangeDown_Click(object sender, EventArgs e)
        {
            if (rangeListIndex < 0 || rangeListIndex >= rangeView.Items.Count)
                return;

            ListViewItem selected = rangeView.Items[rangeListIndex];

            rangeView.Items.RemoveAt(rangeListIndex);
            rangeView.Items.Insert(rangeListIndex + 1, selected);
        }

        public static void HandlePluginControls(Control.ControlCollection plgCtrl)
        {
            foreach (Control ctrl in plgCtrl)
            {
                if (ctrl is GroupBox || ctrl is Panel || ctrl is TabControl || ctrl is TabPage||
                    ctrl is UserControl || ctrl is ListBox || ctrl is ListView)
                    HandlePluginControls(ctrl.Controls);
                
                ctrl.BackColor = ncBackColor;
                ctrl.ForeColor = ncForeColor;
            }
        }

        private void refPlugin_Click(object sender, EventArgs e)
        {
            if (refPlugin.Text == "Close Plugins")
            {
                Global.Plugins.ClosePlugins();
                //TabCon.SelectedIndex = 0;

                pluginList.Items.Clear();

                refPlugin.Text = "Load Plugins";
                codes.ConstCodes = new codes.ConstCode[0];
            }
            else
            {
                int x = 0;

                //Close any open plugins
                Global.Plugins.ClosePlugins();
                pluginList.Items.Clear();

                if (System.IO.Directory.Exists(Application.StartupPath + @"\Plugins") == false)
                    return;

                //Delete any excess PluginInterface.dll's (result of a build and not a copy)
                foreach (string file in System.IO.Directory.GetFiles(Application.StartupPath + @"\Plugins", "PluginInterface.dll", System.IO.SearchOption.AllDirectories))
                    System.IO.File.Delete(file);

                //Call the find plugins routine, to search in our Plugins Folder
                Global.Plugins.FindPlugins(Application.StartupPath + @"\Plugins");

                //Load plugins
                pluginForm = Global.Plugins.GetPlugin(ncBackColor, ncForeColor);
                if (pluginForm != null)
                {
                    Array.Resize(ref pluginFormActive, pluginForm.Length);
                    for (x = 0; x < pluginForm.Length; x++)
                    {
                        pluginForm[x].Tag = x;
                        pluginForm[x].FormClosing += new FormClosingEventHandler(HandlePlugin_Closing);
                        pluginList.Items.Add(pluginForm[x].plugText);
                    }
                }

                //Fixes a bug that causes the BackColor to be white after adding another TabPage
                RangeTab.BackColor = ncForeColor;
                SearchTab.BackColor = ncForeColor;
                CodesTab.BackColor = ncForeColor;
                Application.DoEvents();
                RangeTab.BackColor = ncBackColor;
                SearchTab.BackColor = ncBackColor;
                CodesTab.BackColor = ncBackColor;

                if (pluginForm.Length != 0)
                    pluginList.SelectedIndex = 0;

                refPlugin.Text = "Close Plugins";
            }

            toolStripDropDownButton1.DropDownItems[1].Text = refPlugin.Text;
        }

        private void optButton_Click(object sender, EventArgs e)
        {
            OptionForm oForm = new OptionForm();
            oForm.Show();
        }

        private void TabCon_KeyUp(object sender, KeyEventArgs e)
        {
            if (ProcessKeyBinds(e.KeyData))
                e.SuppressKeyPress = true;
        }

        private bool ProcessKeyBinds(Keys data)
        {
            int match = -1;

            for (int x = 0; x < keyBinds.Length; x++)
                if (keyBinds[x].Equals(data))
                    match = x;
            
            if (match < 0)
                return false;

            switch (match)
            {
                case 0: //Connect And Attach
                    //Connect
                    connectButton_Click(null, null);
                    //Attach if connected
                    if (connected)
                        attachProcessButton_Click(null, null);
                    break;
                case 1: //Disconnect
                    ps3Disc_Click(null, null);
                    break;
                case 2: //Initial Scan
                    if (schSearch.Text == "Stop")
                        schSearch_Click(null, null);
                    if (schNSearch.Text == "Cancel")
                        schNSearch_Click(null, null);

                    schSearch.Text = "Initial Scan";
                    schSearch_Click(null, null);
                    break;
                case 3: //Next Scan
                    if (schSearch.Text == "Stop")
                        schSearch_Click(null, null);
                    if (schNSearch.Text == "Cancel")
                        schNSearch_Click(null, null);

                    schNSearch.Text = "Next Scan";
                    schNSearch_Click(null, null);
                    break;
                case 4: //Stop
                    if (schSearch.Text == "Stop")
                        schSearch_Click(null, null);
                    if (schNSearch.Text == "Cancel")
                        schNSearch_Click(null, null);
                    break;
                case 5: //RefreshResults
                    RefreshSearchResults(0);
                    break;
                case 6: //Toggle Constant Write
                    if (cbListIndex >= 0 && cbListIndex < cbList.Items.Count)
                    {
                        cbList_DoubleClick(null, null);
                    }
                    break;
                case 7: //Write
                    if (cbListIndex >= 0 && cbListIndex < cbList.Items.Count)
                    {
                        cbWrite_Click(null, null);
                    }
                    break;
            }
            return true;
        }

        
        /*
        public void TabCon_DoubleClick(object sender, EventArgs e)
        {
            if (TabCon.SelectedIndex < 3)
                return;

            int ind = 0;
            for (ind = 0; ind < tabs.Length; ind++)
                if (TabCon.TabPages[TabCon.SelectedIndex].Name == tabs[ind].Name)
                    break;
            // Move tab to form and remove the tab
            pluginFormActive[ind] = true;

            //Make new form
            pluginForm[ind] = new PluginForm();
            pluginForm[ind].Controls.Add(TabCon.TabPages[TabCon.SelectedIndex].Controls[0]);
            pluginForm[ind].Size = new Size(470, 410);
            pluginForm[ind].BackColor = ncBackColor;
            pluginForm[ind].ForeColor = ncForeColor;
            pluginForm[ind].tabIndex = ind;
            pluginForm[ind].Dock = DockStyle.Fill;
            pluginForm[ind].tabMax = TabCon.TabPages.Count;
            pluginForm[ind].Text = tabs[ind].Text;
            pluginForm[ind].FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Plugin_Closing);
            TabCon.TabPages.Remove(tabs[ind]);
            pluginForm[ind].Show();
        }
        */

        /* When the plugin closes, tell NetCheat to load the controls back into the tab */
        /*
        private void Plugin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int tInd = ((PluginForm)sender).tabIndex;
            int tMax = ((PluginForm)sender).tabMax;
            tabs[tInd].Controls.Clear();
            tabs[tInd].Controls.Add(pluginForm[tInd].Controls[0]);
            //TabCon.TabPages.Add(tabs[tInd]);
            int insVal = (int)tabs[tInd].Tag;
            int x = insVal;
            while (x > 0)
            {
                x--;
                if (pluginFormActive[x] == true)
                    insVal--;
            }

            insVal += 3;
            pluginFormActive[tInd] = false;
            TabCon.TabPages.Insert(insVal, tabs[tInd]);
            TabCon.SelectedIndex = insVal;
        }
        */

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (ProcessKeyBinds(e.KeyData))
                e.SuppressKeyPress = true;
        }

        private void HandlePlugin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int ind = (int)((PluginForm)sender).Tag;
            if (pluginFormActive[ind])
            {
                e.Cancel = true;
                pluginList.SelectedIndex = ind;
                pluginList_DoubleClick(null, null);
            }
        }

        private void pluginList_DoubleClick(object sender, EventArgs e)
        {
            int ind = pluginList.SelectedIndex;
            if (pluginFormActive[ind]) //Already on
            {
                pluginForm[ind].Visible = false;
                pluginForm[ind].WindowState = FormWindowState.Minimized;
                pluginList.Items[ind] = pluginForm[ind].plugText;
                pluginFormActive[pluginList.SelectedIndex] = false;
            }
            else //Turn on
            {
                pluginFormActive[pluginList.SelectedIndex] = true;
                pluginForm[ind].WindowState = FormWindowState.Normal;
                pluginList.Items[ind] = "+ " + pluginForm[ind].plugText;
                pluginForm[pluginList.SelectedIndex].Show();
                pluginForm[ind].Visible = true;
            }
        }

        private void pluginList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = pluginList.SelectedIndex;
            if (ind >= 0 && pluginForm[ind] != null)
            {
                //Get the selected Plugin
                Types.AvailablePlugin selectedPlugin = Global.Plugins.AvailablePlugins.GetIndex(ind);

                if (selectedPlugin != null && pluginForm[ind].Controls.Count == 0)
                {
                    //Again, if the plugin is found, do some work...

                    //This part adds the plugin's info to the 'Plugin Information:' Frame
                    //this.lblPluginName.Text = selectedPlugin.Instance.Name;
                    //this.lblPluginVersion.Text = "(" + selectedPlugin.Instance.Version + ")";
                    //this.lblPluginAuthor.Text = "By: " + selectedPlugin.Instance.Author;
                    //this.lblPluginDesc.Text = selectedPlugin.Instance.Description;

                    //Clear the current panel of any other plugin controls... 
                    //Note: this only affects visuals.. doesn't close the instance of the plugin
                    //this.pnlPlugin.Controls.Clear();
                    pluginForm[ind].Controls.Clear();

                    //Set the dockstyle of the plugin to fill, to fill up the space provided
                    selectedPlugin.Instance.MainInterface.Dock = DockStyle.Fill;

                    //Finally, add the usercontrol to the tab... Tadah!
                    pluginForm[ind].Controls.Add(selectedPlugin.Instance.MainInterface);
                    pluginForm[ind].Controls[0].Resize += new EventHandler(pluginForm[ind].Plugin_Resize);

                    //TabCon.TabPages[TabCon.SelectedIndex].Controls[0].Dock = DockStyle.None;

                    pluginForm[ind].Controls[0].BackColor = ncBackColor;
                    pluginForm[ind].Controls[0].ForeColor = ncForeColor;
                    //Color each control in the tab too
                    //for (int tabCtrl = 0; tabCtrl < TabCon.TabPages[TabCon.SelectedIndex].Controls[0].Controls.Count; tabCtrl++)
                    HandlePluginControls(pluginForm[ind].Controls[0].Controls);
                }

                if (pluginForm[ind].Controls.Count > 0)
                {
                    descPlugAuth.Text = "by " + selectedPlugin.Instance.Author;
                    descPlugName.Text = selectedPlugin.Instance.Name;
                    descPlugVer.Text = selectedPlugin.Instance.Version;
                    descPlugDesc.Text = selectedPlugin.Instance.Description;
                    if (selectedPlugin.Instance.MainIcon != null)
                        plugIcon.Image = (Bitmap)selectedPlugin.Instance.MainIcon.BackgroundImage.Clone();
                    else
                        plugIcon.Image = (Bitmap)plugIcon.InitialImage.Clone();
                }
            }
        }

        private void lvSch_MouseUp(object sender, MouseEventArgs e)
        {
            //Right click
            if (e.Button == MouseButtons.Right)
            {
                var loc = lvSch.HitTest(e.Location);
                if (loc.Item != null) contextMenuStrip1.Show(lvSch, e.Location);
            }
        }

        private void CopyResults()
        {
            int x = 0, max = lvSch.Items.Count - 1;
            String text = "", res = "";

            for (x = 0; x <= max; x++)
            {

                while (x <= max && lvSch.Items[x].Selected == false)
                    x++;

                if (x > max)
                    break;

                CodeRes a = fileio.ReadDump(dFileName, x, (int)GlobAlign);

                int align = (int)GlobAlign;
                if ((int)GlobAlign == -2)
                    align = a.val.Length;

                if (align > 0 && align <= 8)
                {
                    a.val = BitConverter.GetBytes(uint.Parse(lvSch.Items[x].SubItems[1].Text, System.Globalization.NumberStyles.HexNumber));
                    if (a.val == null)
                        return;
                    Array.Reverse(a.val);
                }

                switch (align)
                {
                    case 1:
                        text = "0 " + a.addr.ToString("X8") + " ";
                        text += a.val[3].ToString("X2") + "000000";
                        break;
                    case 2:
                        text = "1 " + a.addr.ToString("X8") + " ";
                        text = text + a.val[2].ToString("X2") + a.val[3].ToString("X2") + "0000";
                        break;
                    case 4:
                        text = "2 " + a.addr.ToString("X8") + " ";
                        text = text + (a.val[0].ToString("X2") + a.val[1].ToString("X2") + a.val[2].ToString("X2") + a.val[3].ToString("X2")).PadLeft(8, '0');
                        break;
                    case 8:
                        text = "2 " + a.addr.ToString("X8") + " ";
                        text = text + (a.val[0].ToString("X2") + a.val[1].ToString("X2") + a.val[2].ToString("X2") + a.val[3].ToString("X2")).PadLeft(8, '0') + "\n";
                        text = text + "2 " + (a.addr + 4).ToString("X8") + " ";
                        text = text + (a.val[4].ToString("X2") + a.val[5].ToString("X2") + a.val[6].ToString("X2") + a.val[7].ToString("X2")).PadLeft(8, '0');
                        break;
                    default:
                        text = "2 " + a.addr.ToString("X8") + " ";
                        break;
                }
                res = res + text + "\n";
            }
            if (res != null)
                Clipboard.SetDataObject(new DataObject(DataFormats.Text, res));
        }

        private void RefreshFromDump()
        {
            Form1.SchResCnt = 0;
            Form1.GlobAlign = 0;
            bool update = !schNSearch.Enabled;
            RefreshSearchResults(0);
            if (Form1.SchResCnt != 0 && update)
            {
                /* Setup CompBox */
                compBox.Items.Add("Increased By");
                compBox.Items.Add("Decreased By");
                compBox.Items.Add("Changed Value");
                compBox.Items.Add("Unchanged Value");
                NewSearch = false;
                schSearch.Text = "New Scan";
                schNSearch.Enabled = true;
            }
        }

        private void DeleteSearchResult()
        {
            int[] selectedItems = new int[0];
            int x = 0;

            /* Add selected items to int array */
            for (x = 0; x < lvSch.Items.Count; x++)
            {
                if (lvSch.Items[x].Selected)
                {
                    Array.Resize(ref selectedItems, selectedItems.Length + 1);
                    selectedItems[selectedItems.Length - 1] = x;
                }
            }
            
            /* Delete selected items */
            /*
            lvSch.BeginUpdate();
            for (x = 0; x < selectedItems.Length; x++)
                lvSch.Items[selectedItems[x]].Remove();
            lvSch.EndUpdate();
            */

            if (selectedItems.Length == 0)
                return;

            fileio.DeleteDumpBlock(dFileName, selectedItems);

            RefreshSearchResults(0);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyResults();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSearchResult();
        }

        private void refreshFromPS3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshSearchResults(1);
        }

        private void refreshFromDumptxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshFromDump();
        }

        private void recRangeBox_DoubleClick(object sender, EventArgs e)
        {
            
            if (recRangeBox.SelectedIndices.Count >= 0 && recRangeBox.SelectedIndices[0] >= 0)
            {
                int ind = recRangeBox.SelectedIndices[0];
                string path = recRangeBox.Items[ind].ToolTipText;

                if (System.IO.File.Exists(path))
                {
                    ListView a = new ListView();
                    a = fileio.OpenRangeFile(path);

                    if (a == null)
                        return;

                    rangeView.Items.Clear();
                    string[] str = new string[2];
                    for (int x = 0; x < a.Items.Count; x++)
                    {
                        str[0] = a.Items[x].SubItems[0].Text;
                        str[1] = a.Items[x].SubItems[1].Text;
                        ListViewItem strLV = new ListViewItem(str);
                        rangeView.Items.Add(strLV);
                    }

                    //Update range array
                    UpdateMemArray();

                    Text = "NetCheat PS3 4.1 by Dnawrkshp (" + recRangeBox.Items[ind].Text + ")";

                    int roInd = int.Parse(recRangeBox.Items[ind].Tag.ToString());
                    if (ind != 0)
                    {
                        for (int xRO = 0; xRO < ind; xRO++)
                        {
                            int newInd = int.Parse(recRangeBox.Items[xRO].Tag.ToString());
                            rangeOrder[newInd]++;
                        }
                        rangeOrder[roInd] = 0;
                        SaveOptions();
                        UpdateRecRangeBox();
                    }
                }
                else
                {
                    if (MessageBox.Show(this, "Would you like to remove the reference to it?", "File Doesn't Exist!", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //Delete reference to file
                        int index = 0;

                        //Find index of file
                        for (index = 0; index < rangeOrder.Length; index++)
                        {
                            if (rangeImports[index] == path)
                                break;
                        }

                        int[] newRangeOrder = new int[rangeOrder.Length - 1];
                        string[] newRangeImports = new string[newRangeOrder.Length];

                        int y = 0;
                        for (int x = 0; x < rangeOrder.Length; x++)
                        {
                            if (x != index)
                            {
                                newRangeOrder[y] = rangeOrder[x];
                                newRangeImports[y] = rangeImports[x];
                                if (y >= rangeOrder[index])
                                    newRangeOrder[y]--;
                                y++;
                            }
                        }

                        rangeOrder = newRangeOrder;
                        rangeImports = newRangeImports;
                        SaveOptions();
                        recRangeBox.Items.RemoveAt(ind);
                    }
                }
                
            }
        }

        /* Sorts the recRangeBox according to the rangeOrder */
        void UpdateRecRangeBox()
        {
            recRangeBox.Items.Clear();
            foreach (int val in rangeOrder)
                recRangeBox.Items.Add("");

            for (int impRan = 0; impRan < rangeOrder.Length; impRan++)
            {
                string str = rangeImports[impRan];
                if (str != "")
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(str);
                    ListViewItem lvi = new ListViewItem(new string[] { fi.Name });
                    lvi.Text = fi.Name;
                    lvi.Tag = impRan.ToString();
                    lvi.ToolTipText = fi.FullName;
                    lvi.BackColor = ncBackColor;
                    lvi.ForeColor = ncForeColor;
                    recRangeBox.Items[rangeOrder[impRan]] = lvi;
                }
            }
        }

        private void loadSRes_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.FileName != dFileName)
                    System.IO.File.Copy(fd.FileName, dFileName, true);
                RefreshFromDump();
                MessageBox.Show("Results loaded!");
            }
        }

        private void saveSRes_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
                System.IO.File.Copy(dFileName, fd.FileName, true);

            MessageBox.Show("Results saved!");
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectButton_Click(null, null);
        }

        private void attachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            attachProcessButton_Click(null, null);
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ps3Disc_Click(null, null);
        }

        private void shutdownPS3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PS3TMAPI.PowerOff(0, true);
        }

        private void loadPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refPlugin_Click(null, null);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            optButton_Click(null, null);
        }

    }
}
