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
using System.Net;
using System.IO;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using PS3Lib;
using System.Reflection;
using System.IO.Compression;

namespace NetCheatPS3
{

    public partial class Form1 : Form
    {

        #region NetCheat PS3 Global Variables

        public static string versionNum = "4.30";

        public static PS3API PS3 = new PS3API();
        public static string IPAddrStr = "";

        bool isRecognizing = false;
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();
        public static uint ProcessID;
        public static uint[] processIDs;
        public static PS3Lib.NET.PS3TMAPI.SNRESULT snresult;
        public static string usage;
        public static string Info;
        public static PS3Lib.NET.PS3TMAPI.ConnectStatus connectStatus;
        public static string Status;
        public static string MemStatus;
        public static bool connected = false;
        public static bool attached = false;
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
        /* API dll */
        public static int apiDLL = 0;
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
            if (val != null && connected)
                PS3.SetMemory((uint)addr, val);
            //PS3TMAPI.ProcessSetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, addr, val);
        }

        public static bool apiGetMem(ulong addr, ref byte[] val) //Gets the memory as a byte array
        {
            bool ret = false;
            if (val != null && connected)
            {
                if (apiDLL == 0)
                    ret = (PS3Lib.NET.PS3TMAPI.ProcessGetMemory(0, PS3Lib.NET.PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID, 0, addr, ref val) ==
                        PS3Lib.NET.PS3TMAPI.SNRESULT.SN_S_OK);
                else
                    ret = (PS3.CCAPI.GetMemory(addr, val) >= 0);
            }
            return ret;
        }
        #endregion

        #region NetCheat Updater

        public void RunUpdateChecker(bool allowForce)
        {
            string[] updateStr = CheckForUpdate();
            string newVer = updateStr[0].Replace("\r", "").Replace("\n", "");
            bool update = int.Parse(newVer.Replace(".", "")) > int.Parse(versionNum.Replace(".", ""));
            string title = update ?
                "NetCheat PS3 Version " + newVer + " is available for download.\nWould you like to update and restart NetCheat?" :
                "NetCheat is up-to-date! Would you like to Force Update?";
            string updateArg = "";
            if (updateStr.Length > 1)
                updateArg = String.Join(Environment.NewLine, updateStr);
            else
                updateArg = "";

            //string title = update ? "Update Available" : "Force Update?";

            bool allow = false;
            if (allowForce || update)
            {
                updateForm mBox = new updateForm();
                mBox.Title = title;
                mBox.UpdateStr = updateArg;
                mBox.ForeColor = ForeColor;
                mBox.BackColor = BackColor;
                mBox.Show();

                while (mBox.Return < 0)
                    Application.DoEvents();
                allow = (mBox.Return == 0) ? false : true;
                mBox.Close();
            }

            if (allow)
            {

                Form loadingFrm = new Form();
                loadingFrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                loadingFrm.ControlBox = false;
                loadingFrm.Size = new System.Drawing.Size(200, 75);
                Label lbl = new Label();
                lbl.Text = "Updating NetCheat PS3...";
                lbl.AutoSize = false;
                lbl.Size = loadingFrm.Size;
                lbl.Location = new Point(0, 0);
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Font = new System.Drawing.Font(this.Font.FontFamily, 15.0f);
                loadingFrm.Controls.Add(lbl);
                loadingFrm.BackColor = BackColor;
                loadingFrm.ForeColor = ForeColor;
                loadingFrm.Show();
                loadingFrm.TopLevel = true;
                loadingFrm.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - (loadingFrm.Width / 2),
                    Screen.PrimaryScreen.WorkingArea.Height / 2 - (loadingFrm.Height / 2));
                Application.DoEvents();

                UpdateNetCheatPS3();
            }
        }

        public string[] CheckForUpdate()
        {
            string webpath = "http://www.cod-orc.com/NetCheatUpdate.txt";
            string store = Path.GetTempFileName();

            WebClient Client = new WebClient();
            Client.DownloadFile(webpath, store);

            string[] ver = File.ReadAllLines(store);
            File.Delete(store);

            return ver;
        }

        public void UpdateNetCheatPS3()
        {
            string webpath = "http://www.cod-orc.com/ncUpdateDir.zip";
            //FileInfo ncFI = new FileInfo(Application.ExecutablePath);
            string store = Application.StartupPath + "\\" + "ncUpdateDir.zip";

            WebClient Client = new WebClient();
            Client.DownloadFile(webpath, store);

            //Decompress rar
            DecompressFile(store, Application.StartupPath + "\\ncUpdateDir\\");
            File.Delete(store);

            //If there is a new updater use that and delete it from the extracted directory
            if (File.Exists(Application.StartupPath + "\\ncUpdateDir\\NetCheatPS3Updater.exe"))
            {
                File.Copy(Application.StartupPath + "\\ncUpdateDir\\NetCheatPS3Updater.exe",
                    Application.StartupPath + "\\NetCheatPS3Updater.exe", true);
                File.Delete(Application.StartupPath + "\\ncUpdateDir\\NetCheatPS3Updater.exe");
            }

            System.Threading.Thread.Sleep(1000);
            Process.Start("NetCheatPS3Updater.exe", Process.GetCurrentProcess().Id.ToString() + 
                " \"" + Application.StartupPath + "\\ncUpdateDir\"" +
                " \"" + Application.StartupPath + "\"" +
                " \"" + Application.ExecutablePath + "\"");
            Close();
        }

        public static void DecompressFile(string file, string directory)
        {
            using (ZipArchive archive = ZipFile.OpenRead(file))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    try
                    {
                        if (entry.Length > 0)
                        {
                            string mergedPath = Path.Combine(directory, entry.FullName);
                            FileInfo fi = new FileInfo(mergedPath);
                            if (!Directory.Exists(fi.Directory.FullName))
                                Directory.CreateDirectory(fi.Directory.FullName);
                            entry.ExtractToFile(mergedPath, true);
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("Exception: \n" + error.Message);
                    }
                }
            }
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
            TabCon.KeyUp += new KeyEventHandler(Form1_KeyUp);

            connectButton.KeyDown += new KeyEventHandler(Form1_KeyDown);
            attachProcessButton.KeyDown += new KeyEventHandler(Form1_KeyDown);
            ps3Disc.KeyDown += new KeyEventHandler(Form1_KeyDown);
            refPlugin.KeyDown += new KeyEventHandler(Form1_KeyDown);
            optButton.KeyDown += new KeyEventHandler(Form1_KeyDown);
            TabCon.KeyDown += new KeyEventHandler(Form1_KeyDown);
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
                if (range == "")
                    range = ";";
                fd.WriteLine(range);

                //Recently opened ranges paths
                range = "";
                foreach (string str in rangeImports)
                    range += str + ";";
                if (range == "")
                    range = ";";
                fd.WriteLine(range);

                //API
                fd.WriteLine(apiDLL.ToString());
            }
        }

        /* Everything else because I have no organization skills... */
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConstantLoop = 2;
            PS3.DisconnectTarget();
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
            RunUpdateChecker(false);
            //if (File.Exists(Application.ExecutablePath + ".bak"))
            //    File.Delete(Application.ExecutablePath + ".bak");
            //if (File.Exists(Application.StartupPath + "\\updateNC.bat"))
            //    File.Delete(Application.StartupPath + "\\updateNC.bat");
            
            this.Text = "NetCheat PS3 " + versionNum + " by Dnawrkshp";

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

                    apiDLL = int.Parse(settLines[x]);
                    x++;
                }
                catch
                {
                }
            }

            PS3.ChangeAPI((apiDLL == 0) ? SelectAPI.TargetManager : SelectAPI.ControlConsole);
            if (apiDLL == 0)
                PS3.PS3TMAPI_NET();
            else
            {
                SchPWS.Visible = false; //Can't stop/continue process with CCAPI
                pauseGameButt.Visible = false;
                startGameButt.Visible = false;
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
            dFileName = Application.StartupPath + "\\dump.txt";

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

            try
            {
                sRecognize.RequestRecognizerUpdate();
                DictationGrammar _dictationGrammar = new DictationGrammar();
                sRecognize.LoadGrammar(_dictationGrammar);
                sRecognize.SpeechRecognized += sr_SpeechRecognized;
                sRecognize.SetInputToDefaultAudioDevice();
            }
            catch
            {
                return;
            }
        }

        /* Connects to PS3 */
        private void connectButton_Click(object sender, EventArgs e)
        {   
            this.statusLabel1.Text = "Connecting...";
            try
            {
                if (apiDLL == 0) //TMAPI
                {
                    if (PS3.ConnectTarget())
                    {
                        connected = true;
                        this.statusLabel1.Text = "Connected";

                        connectButton.Enabled = false;
                        attachProcessButton.Enabled = true;
                        toolStripDropDownButton1.BackColor = Color.DarkGoldenrod;
                    }
                }
                else //CCAPI
                {
                    IBArg[] ibArg = new IBArg[1];
                    ibArg[0].defStr = (IPAddrStr == "") ? "0.0.0.0" : IPAddrStr;
                    ibArg[0].label = "PS3 IP Address";
                    ibArg = CallIBox(ibArg);

                    if (ibArg == null)
                    {
                        this.statusLabel1.Text = "Cancelled Connecting";
                        return;
                    }

                    IPAddrStr = ibArg[0].retStr;
                    if (ibArg[0].retStr != "")
                    {
                        if (PS3.ConnectTarget(ibArg[0].retStr))
                        {
                            connected = true;
                            this.statusLabel1.Text = "Connected";
                            connectButton.Enabled = false;
                            attachProcessButton.Enabled = true;
                            toolStripDropDownButton1.BackColor = Color.DarkGoldenrod;
                        }
                    }
                }
                if (connected == false)
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
                if (PS3.AttachProcess())
                {
                    this.statusLabel1.Text = "Process Attached";
                    ConstantLoop = 1;
                    attachProcessButton.Enabled = false;
                    toolStripDropDownButton1.BackColor = Color.DarkGreen;
                    attached = true;
                }
                else
                {
                    this.statusLabel1.Text = "Error attaching process; no game started?";
                    toolStripDropDownButton1.BackColor = Color.DarkGoldenrod;
                }
                
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
            PS3.DisconnectTarget();
            ConstantLoop = 2;
            this.statusLabel1.Text = "Disconnected";
            processIDs = null;
            attached = false;
            connected = false;

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
            cbCodes.BackColor = BackColor;
            cbCodes.ForeColor = ForeColor;
            cbCodes.SelectionStart = 0;
            cbCodes.SelectionLength = cbCodes.Text.Length;
            cbCodes.SelectionColor = ForeColor;
            cbCodes.SelectionLength = 0;
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
            if (SchHexCheck.Text == "Hex")
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

                    apiGetMem(cnt, ref retByte);
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
            Stopwatch watch = Stopwatch.StartNew();
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
                if (SchPWS.Checked && SchPWS.Visible)
                    PS3Lib.NET.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);
                return;
            }

            if (schRange1.Text.Length != 8 || schRange2.Text.Length != 8)
            {
                MessageBox.Show("Error: range addresses are not of proper length 8.");
                return;
            }

            if (SchPWS.Checked && SchPWS.Visible && schSearch.Text != "New Scan")
                PS3Lib.NET.PS3TMAPI.ProcessAttach(0, PS3Lib.NET.PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID);

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
                    ValStr = ulong.Parse(schVal.Text).ToString("X");
                    ValStr = ValStr.PadLeft(align * 2, '0');

                    if (schVal2.Visible)
                    {
                        ValStr2 = ulong.Parse(schVal2.Text).ToString("X");
                        ValStr2 = ValStr2.PadLeft(align * 2, '0');
                    }
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
                        ret = InitSearchText(SchHexCheck.Checked, recvCnt, sVal, c, len * 2, sSize, file);
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

            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            statusLabel1.Text += ", Search time: " + ((Single)elapsedMs / (Single)1000).ToString("F") + " seconds";

            schSearch.Text = "New Scan";
            schProg.Maximum = 0;
            NewSearch = false;
            schNSearch.Enabled = true;
            schProg.Value = 0;

            GlobAlign = (ulong)align;
            if (SchPWS.Checked && SchPWS.Visible)
                PS3Lib.NET.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);
            CancelSearch = 0;
            lvSch.EndUpdate();
        }

        private void SchRef_Click(object sender, EventArgs e)
        {
            RefreshSearchResults(1);
        }

        private void schNSearch_Click(object sender, EventArgs e)
        {
            Stopwatch watch = Stopwatch.StartNew();
            byte[] sVal = null;
            byte[] c = null;

            if (schNSearch.Text == "Cancel")
            {
                CancelSearch = 1;
                if (SchPWS.Checked)
                    PS3Lib.NET.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);
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
                PS3Lib.NET.PS3TMAPI.ProcessAttach(0, PS3Lib.NET.PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID);

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

            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            statusLabel1.Text += ", Search time: " + ((Single)elapsedMs / (Single)1000).ToString("F") + " seconds";

            schNSearch.Text = "Next Scan";
            NewSearch = false;
            schProg.Maximum = 0;
            schProg.Value = 0;

            if (SchPWS.Checked)
                PS3Lib.NET.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);
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

        bool hexSchHexVal = true;
        bool textSchHexVal = false;
        private void SearchMode(int mode)
        {
            if (mode != 5 && SchHexCheck.Text == "MCase")
            {
                SchHexCheck.Text = "Hex";
                textSchHexVal = SchHexCheck.Checked;
                SchHexCheck.Checked = hexSchHexVal;
                SchHexCheck.Font = Font;
            }
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
                    hexSchHexVal = SchHexCheck.Checked;
                    SchHexCheck.Checked = textSchHexVal;
                    SchHexCheck.Visible = true;
                    SchHexCheck.Text = "MCase";
                    SchHexCheck.Font = new Font(Font.FontFamily, 6.75f, FontStyle.Regular);
                    break;
            }
        }

        public ulong InitSearch(ulong sStart, byte[] sVal, byte[] c, int align, ulong sSize, System.IO.StreamWriter fStream)
        {
            ulong ResCnt = 0, sCount = 0;
            byte[] ret = new byte[sSize];

            apiGetMem(sStart, ref ret);

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

            apiGetMem(sStart, ref ret);

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

        public ulong InitSearchText(bool matchCase, ulong sStart, byte[] sText, byte[] c, int len, ulong sSize, System.IO.StreamWriter fStream)
        {
            ulong ResCnt = 0, sCount = 0;
            byte[] ret = new byte[sSize];
            apiGetMem(sStart, ref ret);

            while ((sCount + (ulong)sText.Length) <= sSize)
            {
                if (Form1.CancelSearch == 1)
                    return 0;

                byte[] argB = new byte[len / 2];
                Array.Copy(ret, (int)sCount, argB, 0, argB.Length);
                if (misc.ArrayCompare(sText, argB, c, compMode, matchCase))
                {
                    byte[] newB = new byte[len / 2];
                    Array.Copy(ret, (int)sCount, newB, 0, newB.Length);

                    ListRes a = misc.GetlvVals((int)NextSAlign, newB, 0);
                    string[] row = { (sStart + sCount).ToString("X8"), a.HexVal, a.DecVal, a.AlignStr };

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

                        apiGetMem(tempSchRes[x].addr, ref ret);

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

                        apiGetMem(tempSchRes[x].addr, ref ret);

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
                            apiGetMem(retRes[z].addr, ref ret);

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

                Text = "NetCheat PS3 " + versionNum + " by Dnawrkshp (" + new System.IO.FileInfo(fd.FileName).Name + ")";
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
                foreach (PluginForm pF in pluginForm)
                {
                    pF.Close();
                    //pF = null;
                }

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

            //loadPluginsToolStripMenuItem
            int index = toolStripDropDownButton1.DropDownItems.IndexOfKey("loadPluginsToolStripMenuItem");
            toolStripDropDownButton1.DropDownItems[index].Text = refPlugin.Text;
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
            if (e.KeyCode == Keys.Oemtilde)
            {
                try
                {
                    sRecognize.RecognizeAsyncStop();
                    isRecognizing = false;
                }
                catch { }
            }
            if (ProcessKeyBinds(e.KeyData))
                e.SuppressKeyPress = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Oemtilde && !isRecognizing)
            {
                try
                {
                    sRecognize.RecognizeAsync(RecognizeMode.Multiple);
                    //sRecognize.Recognize();
                    isRecognizing = true;
                }
                catch { }
            }
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
                    if (BitConverter.IsLittleEndian) Array.Reverse(a.val);
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

                    Text = "NetCheat PS3 " + versionNum + " by Dnawrkshp (" + recRangeBox.Items[ind].Text + ")";

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
            if (apiDLL == 0)
            {
                if (MessageBox.Show("PS3Lib doesn't support TMAPI shutdown.\nWould you like to reset to the XMB?", "Error - Unsupported", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    PS3.TMAPI.ResetToXMB(TMAPI.ResetTarget.Soft);
            }
            else
                PS3.CCAPI.ShutDown(CCAPI.RebootFlags.ShutDown);
        }

        private void loadPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refPlugin_Click(null, null);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            optButton_Click(null, null);
        }

        private void updateStripMenuItem1_Click(object sender, EventArgs e)
        {
            RunUpdateChecker(true);
        }

        private void gameStatusStripMenuItem1_Click(object sender, EventArgs e)
        {
            /*
            if (connected)
            {
                PS3TMAPI.UnitStatus ret;
                PS3TMAPI.GetStatus(0, PS3TMAPI.UnitType.PPU, out ret);
                if (ret == PS3TMAPI.UnitStatus.Stopped)
                {
                    PS3TMAPI.ProcessContinue(0, ProcessID);
                    gameStatusStripMenuItem1.Text = "Pause Game";
                }
                else
                {
                    PS3TMAPI.ProcessStop(0, ProcessID);
                    gameStatusStripMenuItem1.Text = "Continue Game";
                }
            }
            else
                MessageBox.Show("Not yet connected!");
            */
        }

        bool findRangesCancel = false;
        private void findRanges_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                MessageBox.Show("Not connected to the PS3!");
                return;
            }

            if (findRanges.Text == "Stop")
            {
                findRangesCancel = true;
                return;
            }

            rangeView.Items.Clear();

            ulong findAddr = 0;
            ulong blockSize = 0x10000;
            findRanges.Text = "Stop";
            bool inMemRange = false;
            findRangeProgBar.Maximum = 4096;

            ulong blockStart = 0;

            for (findAddr = 0; findAddr < 0xFFFFFFFC; findAddr += blockSize)
            {
                if (findRangesCancel)
                    break;

                if ((findAddr % 0x100000) == 0)
                {
                    statusLabel1.Text = "Scanning memory at 0x" + findAddr.ToString("X8");
                    findRangeProgBar.Increment(1);
                    Application.DoEvents();
                }

                byte[] ret = new byte[1];
                bool validRegion = apiGetMem(findAddr, ref ret);

                //Start new mem block
                if (validRegion && !inMemRange)
                {
                    blockStart = findAddr;
                    inMemRange = true;
                }
                //Add block
                else if (!validRegion && inMemRange)
                {
                    string[] str = new string[2];
                    str[0] = blockStart.ToString("X8");
                    str[1] = findAddr.ToString("X8");
                    ListViewItem strLV = new ListViewItem(str);
                    rangeView.Items.Add(strLV);
                    inMemRange = false;
                }
            }

            findRangesCancel = false;
            findRanges.Text = "Find Ranges";
            findRangeProgBar.Value = 0;

            //Update range array
            UpdateMemArray();
            if (misc.MemArray != null || misc.MemArray.Length > 0)
                MessageBox.Show("Find Ranges Completed!\nUnderstand that the range finder searches in blocks of 0x10000.\nThis may cause the ranges to be off by a value from 1 to 0xFFFF.");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //MessageBox.Show(e.Result.Text);
            switch (e.Result.Words[0].Text.ToLower())
            {
                case "connect":
                    connectButton_Click(null, null);
                    break;
                case "attach":
                    attachProcessButton_Click(null, null);
                    break;
                case "shutdown":
                    shutdownPS3ToolStripMenuItem_Click(null, null);
                    break;
                case "value":
                    if (e.Result.Words.Count > 2 && e.Result.Words[1].Text.ToLower() == "type")
                    {
                        switch (e.Result.Words[2].Text.ToLower())
                        {
                            case "one":
                                cbSchAlign.SelectedIndex = 0;
                                break;
                            case "two":
                                cbSchAlign.SelectedIndex = 1;
                                break;
                            case "four":
                                cbSchAlign.SelectedIndex = 2;
                                break;
                            case "eight":
                                cbSchAlign.SelectedIndex = 3;
                                break;
                            case "ex":
                            case "x":
                                cbSchAlign.SelectedIndex = 4;
                                break;
                            case "text":
                                cbSchAlign.SelectedIndex = 5;
                                break;
                        }
                    }
                    break;
                case "scan":
                    if (e.Result.Words.Count > 1)
                    {
                        string cmp = e.Result.Words[1].Text.ToLower();
                        if (cmp == "a" && e.Result.Words.Count > 2)
                            cmp = e.Result.Words[2].Text.ToLower();
                        if (cmp == "style")
                            cmp = "stop";

                        if (cmp == "new" || cmp == "initial" || cmp == "next")
                        {
                            if (schSearch.Text == "Stop")
                                schSearch_Click(null, null);
                            else if (schNSearch.Text == "Cancel")
                                schNSearch_Click(null, null);
                        }

                        switch (cmp)
                        {
                            case "new":
                                if (schSearch.Text == "New Scan")
                                    schSearch_Click(null, null);
                                break;
                            case "initial":
                                if (schSearch.Text == "Initial Scan")
                                    schSearch_Click(null, null);
                                break;
                            case "next":
                                if (schNSearch.Text == "Next Scan")
                                    schNSearch_Click(null, null);
                                break;
                        }
                    }
                    break;
            }
            if (e.Result.Words[0].Text.ToLower() == "compare")
            {
                //Equal
                if (e.Result.Words.Count == 2)
                {
                    if (e.Result.Words[1].Text.ToLower() == "equal")
                        compBox.SelectedIndex = 0;
                }
                //Not Equal, Less Than, Greater Than, Value Between
                //Increased By, Decreased By, Changed Value, Unchanged Value
                if (e.Result.Words.Count == 3)
                {

                    if (e.Result.Words[1].Text.ToLower() == "not" &&
                            e.Result.Words[2].Text.ToLower() == "equal")
                        compBox.SelectedIndex = 1;
                    else if (e.Result.Words[1].Text.ToLower() == "less" &&
                            e.Result.Words[2].Text.ToLower() == "than")
                        compBox.SelectedIndex = 2;
                    else if (e.Result.Words[1].Text.ToLower() == "greater" &&
                            e.Result.Words[2].Text.ToLower() == "than")
                        compBox.SelectedIndex = 4;
                    else if (e.Result.Words[1].Text.ToLower() == "value" &&
                            e.Result.Words[2].Text.ToLower() == "between")
                        compBox.SelectedIndex = 6;
                    else if (compBox.Items.Count > 7)
                    {
                        if (e.Result.Words[1].Text.ToLower() == "increased" &&
                            e.Result.Words[2].Text.ToLower() == "by")
                        compBox.SelectedIndex = 7;
                        else if (e.Result.Words[1].Text.ToLower() == "decreased" &&
                            e.Result.Words[2].Text.ToLower() == "by")
                            compBox.SelectedIndex = 8;
                        else if (e.Result.Words[1].Text.ToLower() == "changed" &&
                            e.Result.Words[2].Text.ToLower() == "value")
                            compBox.SelectedIndex = 9;
                        else if (e.Result.Words[1].Text.ToLower() == "unchanged" &&
                            e.Result.Words[2].Text.ToLower() == "value")
                            compBox.SelectedIndex = 10;
                    }
                }
                //Less Than or Equal, Greater Than or Equal
                if (e.Result.Words.Count == 5)
                {
                    if (e.Result.Words[1].Text.ToLower() == "less" &&
                            e.Result.Words[2].Text.ToLower() == "than" &&
                            e.Result.Words[3].Text.ToLower() == "or" &&
                            e.Result.Words[4].Text.ToLower() == "equal")
                        compBox.SelectedIndex = 3;
                    else if (e.Result.Words[1].Text.ToLower() == "greater" &&
                            e.Result.Words[2].Text.ToLower() == "than" &&
                            e.Result.Words[3].Text.ToLower() == "or" &&
                            e.Result.Words[4].Text.ToLower() == "equal")
                        compBox.SelectedIndex = 5;
                }
            }
        }

        string ParseValFromStr(string val)
        {
            val = val.ToLower();
            val = val.Replace(" ", "");
            val = val.Replace("zero", "0");
            val = val.Replace("one", "1");
            val = val.Replace("two", "2");
            val = val.Replace("three", "3");
            val = val.Replace("four", "4");
            val = val.Replace("five", "5");
            val = val.Replace("six", "6");
            val = val.Replace("seven", "7");
            val = val.Replace("eight", "8");
            val = val.Replace("nine", "9");
            val = val.Replace("see", "C");
            val = val.Replace("be", "B");
            return val;
        }

        private void TabCon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (apiDLL == 0)
            {
                SchPWS.Visible = true;
                pauseGameButt.Visible = true;
                startGameButt.Visible = true;
            }
            else
            {
                SchPWS.Visible = false;
                pauseGameButt.Visible = false;
                startGameButt.Visible = false;
            }
        }

        private void startGameButt_Click(object sender, EventArgs e)
        {
            if (apiDLL == 0)
                PS3Lib.NET.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);
        }

        private void pauseGameButt_Click(object sender, EventArgs e)
        {
            if (apiDLL == 0)
                PS3Lib.NET.PS3TMAPI.ProcessAttach(0, PS3Lib.NET.PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID);
        }

        private void pauseGameButt_BackColorChanged(object sender, EventArgs e)
        {
            if (pauseGameButt.BackColor != Color.White)
                pauseGameButt.BackColor = Color.White;
        }

        private void pauseGameButt_ForeColorChanged(object sender, EventArgs e)
        {
            if (pauseGameButt.ForeColor != Color.FromArgb(0, 130, 210))
                pauseGameButt.ForeColor = Color.FromArgb(0, 130, 210);
        }

        private void startGameButt_BackColorChanged(object sender, EventArgs e)
        {
            if (startGameButt.BackColor != Color.White)
                startGameButt.BackColor = Color.White;
        }

        private void startGameButt_ForeColorChanged(object sender, EventArgs e)
        {
            if (startGameButt.ForeColor != Color.FromArgb(0, 130, 210))
                startGameButt.ForeColor = Color.FromArgb(0, 130, 210);
        }

    }
}
