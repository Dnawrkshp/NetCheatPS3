using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Speech.Recognition;
using System.Runtime.Serialization.Formatters.Binary;
using Ionic.Zip;

namespace NetCheatPS3
{

    public partial class Form1 : Form
    {

        public static Form1 Instance = null;
        public static FindReplaceManager FRManager = null;

        public static string versionNum = "4.53";
        public static string apiName = "Target Manager API (420.1.14.7)";

        private static Types.AvailableAPI _curapi;
        public static Types.AvailableAPI curAPI
        {
            get { return _curapi; }
            set
            {
                _curapi = value;
                if (_curapi == null)
                {
                    apiName = "None";
                    Form1.Instance.Text = "NetCheat " + versionNum + " by Dnawrkshp" + ((IntPtr.Size == 8) ? " (64 Bit)" : " (32 Bit)");
                    Form1.Instance.statusLabel2.Text = "API: None";

                    // disable parts of the ui
                    foreach (Control tabPage in Form1.Instance.TabCon.TabPages)
                    {
                        if (tabPage.Name == "apiTab")
                        {
                            Form1.Instance.TabCon.SelectedTab = (TabPage)tabPage;
                            tabPage.Enabled = true;
                        }
                        else
                        {
                            tabPage.Enabled = false;
                        }
                    }
                }
                else
                {
                    Form1.Instance.CurrentEndian = _curapi.Instance.isPlatformLittleEndian ? Endian.Little : Endian.Big;
                    apiName = _curapi.Instance.Name + " (" + _curapi.Instance.Version + ")";
                    Form1.Instance.Text = "NetCheat " + _curapi.Instance.Platform + " " + versionNum + " by Dnawrkshp" + ((IntPtr.Size == 8) ? " (64 Bit)" : " (32 Bit)");
                    Form1.Instance.statusLabel2.Text = "API: " + _curapi.Instance.Name;
                }
            }
        }

        public bool allowForce = false;

        #region NetCheat PS3 Global Variables

        //public static AppDomain pluginDomain = AppDomain.CreateDomain("NC Plugin Domain");

        public static bool PluginAllowColoring = false;
        public static bool DefaultPluginAllowColoring = true;

        bool isRecognizing = false;
        public static bool isClosing = false;
        SpeechRecognitionEngine sRecognize = new SpeechRecognitionEngine();
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
        public struct ncCode
        {
            public char codeType;
            public byte[] codeArg2;
            public ulong codeArg1;
            public byte[] codeArg1_BA;
            public uint codeArg0;
        }

        public delegate ncCode ParseCode(string code);
        public delegate int ExecCode(int cnt, ref CodeDB cDB, bool isCWrite);
        public struct ncCodeType
        {
            public ParseCode ParseCode;         //Function that parses the code
            public ExecCode ExecCode;           //Function that executes the code
            public char Command;                //What defines the code type
        }

        /* Code struct */
        public struct CodeDB
        {          /* Structure for a single code */
            public bool state;              /* Determines whether to write constantly or not */
            public string name;             /* Name of Code */
            public string codes;            /* Holds codes string */
            public ncCode[] CData;          /* Holds codes in parsed format */
            public string filename;         /* For use with the 'Save' button */
            public ncCode[] backUp;         /* Holds what the memory originally held before writing */
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
        public static List<CodeDB> Codes = new List<CodeDB>();

        /* Search Types */
        public const int compEq = 0;        /* Equal To */
        public const int compNEq = 1;       /* Not Equal To */
        public const int compLT = 2;        /* Less Than Signed */
        public const int compLTE = 3;       /* Less Than Or Equal To Signed */
        public const int compGT = 4;        /* Greater Than Signed */
        public const int compGTE = 5;       /* Greater Than Or Equal To Signed */
        public const int compVBet = 6;      /* Value Between */
        public const int compINC = 7;       /* Increased Value Signed */
        public const int compDEC = 8;       /* Decreased Value Signed */
        public const int compChg = 9;       /* Changed Value */
        public const int compUChg = 10;     /* Unchanged Value */
        public const int compLTU = 11;      /* Less Than Unsigned */
        public const int compLTEU = 12;     /* Less Than Or Equal To Unsigned */
        public const int compGTU = 13;      /* Greater Than Unsigned */
        public const int compGTEU = 14;     /* Greater Than Or Equal To Unsigned */


        public const int compANEq = 20;     /* And Equal (used with E joker type) */

        /* Input Box Argument Structure */
        public struct IBArg
        {
            public string label;
            public string defStr;
            public string retStr;
        };

        /* Little and Big Endian */
        public enum Endian
        {
            Little,
            Big
        }

        private static Endian _curEndian = Endian.Big;
        public static bool doFlipArray = false;
        public Endian CurrentEndian
        {
            get { return _curEndian; }
            set
            {
                _curEndian = value;

                if (_curEndian == Endian.Big)
                    endianStripMenuItem.Checked = true;
                else
                    endianStripMenuItem.Checked = false;


                doFlipArray = false;
                if (CurrentEndian == Endian.Big && BitConverter.IsLittleEndian)
                    doFlipArray = true;
                else if (CurrentEndian == Endian.Little && !BitConverter.IsLittleEndian)
                    doFlipArray = true;
            }
        }

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
        /* Whether to have the donations form pop up or not */
        public static bool ncDonatePopup = true;

        /* Settings file path */
        public static string settFile = "";
        /* String array that holds each range import */
        public static string[] rangeImports = new string[0];
        /* Int array that defines the order of the recent ranges */
        public static int[] rangeOrder = new int[0];

        /* Plugin form related arrays */
        public static PluginForm[] pluginForm = new PluginForm[0];
        public static bool[] pluginFormActive = new bool[0];
        public static int setplugWindow = -1;

        /* Delete block struct */
        public struct deleteArr
        {
            public int start;
            public int size;
        }

        public struct OnlineCode
        {
            public string id;
            public int ver;
        }

        /* Constant writing thread */
        public static System.Threading.Thread tConstWrite = new System.Threading.Thread(new System.Threading.ThreadStart(codes.BeginConstWriting));
        #endregion

        #region Interface Functions

        /* API related functions */
        public static void apiSetMem(ulong addr, byte[] val) //Set the memory
        {
            if (val != null && connected)
            {

                byte[] newV = new byte[val.Length];
                Array.Copy(val, 0, newV, 0, val.Length);
                newV = misc.notrevif(newV);
                curAPI.Instance.SetBytes(addr, newV);
            }
            //PS3TMAPI.ProcessSetMemory(Target, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, addr, val);
        }

        public static bool apiGetMem(ulong addr, ref byte[] val) //Gets the memory as a byte array
        {
            bool ret = false;
            if (val != null && connected)
            {
                ret = curAPI.Instance.GetBytes(addr, ref val);
            }
            return ret;
        }

        public enum ValueType
        {
            CHAR,
            SHORT,
            INT,
            LONG,
            USHORT,
            UINT,
            ULONG,
            STRING,
            FLOAT,
            DOUBLE
        }

        public static object getVal(uint addr, ValueType type)
        {
            return getVal((ulong)addr, type);
        }

        public static object getVal(ulong addr, ValueType type)
        {
            byte[] b;

            switch (type)
            {
                case ValueType.CHAR:
                    b = new byte[1];
                    apiGetMem(addr, ref b);
                    return (char)b[0];
                case ValueType.DOUBLE:
                    b = new byte[8];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    return BitConverter.ToDouble(b, 0);
                case ValueType.FLOAT:
                    b = new byte[4];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    return BitConverter.ToSingle(b, 0);
                case ValueType.INT:
                    b = new byte[4];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    return BitConverter.ToInt32(b, 0);
                case ValueType.LONG:
                    b = new byte[4];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    return BitConverter.ToInt64(b, 0);
                case ValueType.SHORT:
                    b = new byte[4];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    return BitConverter.ToInt16(b, 0);
                case ValueType.STRING:
                    b = new byte[256];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    string valStringRet = "";
                    for (int str = 0; str < 256; str++)
                    {
                        if (b[str] == 0)
                            break;
                        valStringRet += ((char)b[str]).ToString();
                    }
                    return valStringRet;
                case ValueType.UINT:
                    b = new byte[4];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    return BitConverter.ToUInt32(b, 0);
                case ValueType.ULONG:
                    b = new byte[4];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    return BitConverter.ToUInt64(b, 0);
                case ValueType.USHORT:
                    b = new byte[4];
                    apiGetMem(addr, ref b);
                    if (doFlipArray)
                        Array.Reverse(b);
                    return BitConverter.ToUInt16(b, 0);
            }

            return 0;
        }

        #endregion

        #region NetCheat Updater

        public static void RunUpdateChecker()
        {
            bool allowForce = Form1.Instance.allowForce;
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
                mBox.ForeColor = Form1.Instance.ForeColor;
                mBox.BackColor = Form1.Instance.BackColor;
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
                lbl.Font = new System.Drawing.Font(Form1.Instance.Font.FontFamily, 15.0f);
                loadingFrm.Controls.Add(lbl);
                loadingFrm.BackColor = Form1.Instance.BackColor;
                loadingFrm.ForeColor = Form1.Instance.ForeColor;
                loadingFrm.Show();
                loadingFrm.TopLevel = true;
                loadingFrm.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - (loadingFrm.Width / 2),
                    Screen.PrimaryScreen.WorkingArea.Height / 2 - (loadingFrm.Height / 2));
                Application.DoEvents();

                UpdateNetCheatPS3();
            }
        }

        public static string[] CheckForUpdate()
        {
            string webpath = "http://netcheat.gamehacking.org/ncUpdater/NetCheatUpdate.txt";
            string store = Path.GetTempFileName();

            try
            {
                WebClient Client = new WebClient();
                Client.DownloadFile(webpath, store);

                string[] ver = File.ReadAllLines(store);
                File.Delete(store);

                return ver;
            }
            catch (Exception)
            {
                return new string[] { "0" };
            }
        }

        public static void UpdateNetCheatPS3()
        {
            string webpath = "http://netcheat.gamehacking.org/ncUpdater/ncUpdateDir.zip";
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


            Process.GetCurrentProcess().Kill();
            //Form1.Instance.Close();
        }

        public static void DecompressFile(string file, string directory)
        {
            using (ZipFile archive = ZipFile.Read(file))
            {
                foreach (ZipEntry entry in archive.Entries)
                {
                    try
                    {
                        if (entry.UncompressedSize > 0)
                        {
                            string mergedPath = Path.Combine(directory, entry.FileName);
                            FileInfo fi = new FileInfo(directory);
                            if (!Directory.Exists(fi.Directory.FullName))
                                Directory.CreateDirectory(fi.Directory.FullName);
                            if (File.Exists(mergedPath))
                                File.Delete(mergedPath);
                            entry.Extract(directory, ExtractExistingFileAction.OverwriteSilently);
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

            Instance = this;
            compare.LoadSearch();

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.GotFocus += new EventHandler(Form1_Focused);
            HandleFocusControls(this.Controls);

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

            CurrentEndian = Endian.Little;
            CurrentEndian = Endian.Big;
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
                fd.WriteLine(ncBackColor.A.ToString("X2") + ncBackColor.R.ToString("X2") + ncBackColor.G.ToString("X2") + ncBackColor.B.ToString("X2"));
                fd.WriteLine(ncForeColor.A.ToString("X2") + ncForeColor.R.ToString("X2") + ncForeColor.G.ToString("X2") + ncForeColor.B.ToString("X2"));

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
                    if (str != "")
                        range += str + ";";
                if (range == "")
                    range = ";";
                fd.WriteLine(range);

                //API
                if (curAPI != null)
                    fd.WriteLine(curAPI.Instance.Name + " (" + curAPI.Instance.Version + ")");
                else
                    fd.WriteLine("0");

                //Donation Popup
                fd.WriteLine(ncDonatePopup.ToString());
            }
        }

        private void Form1_Focused(object sender, EventArgs e)
        {
            if (Form1.Instance.ContainsFocus)
            {
                if (ProgressBar.progTaskBarError || (searchControl1.searchMemory.Text != "Stop" && searchControl1.nextSearchMem.Text != "Stop"))
                {
                    TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.NoProgress);
                    ProgressBar.progTaskBarError = false;
                }
            }
        }

        /* Everything else because I have no organization skills... */
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isClosing = true;
            ConstantLoop = 2;
            this.statusLabel1.Text = "Disconnected";
            System.IO.File.Delete(dFileName);

            //Close all plugins
            foreach (PluginForm a in pluginForm)
            {
                try
                {
                    a.Dispose();
                    a.Close();
                }
                catch { }
            }

            if (refPlugin.Text == "Close Plugins")
                Global.Plugins.ClosePlugins();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!Debugger.IsAttached)
                TabCon.TabPages.Remove(DumpCompTab);

            if (IntPtr.Size == 8)
            {
                //MessageBox.Show("This is the 64 bit version of NetCheatPS3.\nThis version DOES NOT work with CCAPI 2.5! It is not my fault, if you want CCAPI to support 64 bit applications then please bug Enstone about it.\nThanks.");
            }

            System.Threading.Thread updateCheckThread = new System.Threading.Thread(new System.Threading.ThreadStart(RunUpdateChecker));
            updateCheckThread.IsBackground = true;
            updateCheckThread.Start();
            //RunUpdateChecker(false);

            //if (File.Exists(Application.ExecutablePath + ".bak"))
            //    File.Delete(Application.ExecutablePath + ".bak");
            //if (File.Exists(Application.StartupPath + "\\updateNC.bat"))
            //    File.Delete(Application.StartupPath + "\\updateNC.bat");

            codes.ncCodeTypes = new ncCodeType[10];
            //Byte Write
            codes.ncCodeTypes[0].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[0].ExecCode = new ExecCode(codes.execByteWrite);
            codes.ncCodeTypes[0].Command = '0';
            //Text Write
            codes.ncCodeTypes[1].ParseCode = new ParseCode(codes.parseTextCode);
            codes.ncCodeTypes[1].ExecCode = new ExecCode(codes.execByteWrite);
            codes.ncCodeTypes[1].Command = '1';
            //Pointer Execute
            codes.ncCodeTypes[2].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[2].ExecCode = new ExecCode(codes.execPointerExecute);
            codes.ncCodeTypes[2].Command = '6';
            //Conditional Equal To Execute
            codes.ncCodeTypes[3].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[3].ExecCode = new ExecCode(codes.execEQConditionalExecute);
            codes.ncCodeTypes[3].Command = 'D';
            //Conditional Mask Unset Execute
            codes.ncCodeTypes[4].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[4].ExecCode = new ExecCode(codes.execMUConditionalExecute);
            codes.ncCodeTypes[4].Command = 'E';
            //Copy to address
            codes.ncCodeTypes[5].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[5].ExecCode = new ExecCode(codes.execCopyBytes);
            codes.ncCodeTypes[5].Command = 'F';
            //Float Write
            codes.ncCodeTypes[6].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[6].ExecCode = new ExecCode(codes.execByteWrite);
            codes.ncCodeTypes[6].Command = '2';
            //Find Replace
            codes.ncCodeTypes[7].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[7].ExecCode = new ExecCode(codes.execFindReplace);
            codes.ncCodeTypes[7].Command = 'B';
            //Condensed Multiline Code
            codes.ncCodeTypes[8].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[8].ExecCode = new ExecCode(codes.execMultilineCondensed);
            codes.ncCodeTypes[8].Command = '4';
            //Copy Paste
            codes.ncCodeTypes[9].ParseCode = new ParseCode(codes.parseCode);
            codes.ncCodeTypes[9].ExecCode = new ExecCode(codes.execCopyPasteBytes);
            codes.ncCodeTypes[9].Command = 'A';

            LoadAPIs();

            int x = 0;
            //Set the settings file and load the settings
            settFile = Application.StartupPath + ((IntPtr.Size == 8) ? "\\ncps364.ini" : "\\ncps3.ini");
            if (System.IO.File.Exists(settFile))
            {
                string[] settLines = System.IO.File.ReadAllLines(settFile);
                try
                {
                    //Read the keybinds from the array
                    for (x = 0; x < keyBinds.Length; x++)
                        keyBinds[x] = (Keys)int.Parse(settLines[x]);
                }
                catch { }
                x = keyBinds.Length;

                try
                {
                    //Read the colors and update the form
                    ncBackColor = Color.FromArgb(int.Parse(settLines[x], System.Globalization.NumberStyles.HexNumber)); BackColor = ncBackColor;
                    ncForeColor = Color.FromArgb(int.Parse(settLines[x + 1], System.Globalization.NumberStyles.HexNumber)); ForeColor = ncForeColor;
                }
                catch { }
                x += 2;

                try
                {
                    //Read the recently opened ranges
                    string[] strRangeOrder = settLines[x].Split(';');
                    int size = 0;
                    foreach (string strTMP in strRangeOrder)
                        if (strTMP != "")
                            size++;
                    Array.Resize(ref rangeOrder, size);
                    for (int valRO = 0; valRO < rangeOrder.Length; valRO++)
                        if (strRangeOrder[valRO] != "")
                            rangeOrder[valRO] = int.Parse(strRangeOrder[valRO]);

                    size = 0;
                    rangeImports = settLines[x + 1].Split(';');
                    foreach (string strTMP in strRangeOrder)
                        if (strTMP != "")
                            size++;
                    Array.Resize(ref rangeImports, size);
                    UpdateRecRangeBox();
                }
                catch { }
                x += 2;

                try
                {
                    if (settLines[x] == "0")
                    {
                        int apiDLL = Global.APIs.AvailableAPIs.GetIndex("Target Manager API", "420.1.14.7");
                        curAPI = Global.APIs.AvailableAPIs.GetIndex(apiDLL);
                        curAPI.Instance.Initialize();
                    }
                    else
                    {
                        apiName = settLines[x];
                        int apiC = 0;
                        foreach (Types.AvailableAPI api in Global.APIs.AvailableAPIs)
                        {
                            if ((api.Instance.Name + " (" + api.Instance.Version + ")") == apiName)
                            {
                                //apiDLL = apiC;
                                curAPI = api;
                                curAPI.Instance.Initialize();
                                break;
                            }
                            apiC++;
                        }

                        if (apiC == Global.APIs.AvailableAPIs.Count)
                        {
                            apiC = 0;
                            apiName = "Control Console API (2.60)";
                            foreach (Types.AvailableAPI api in Global.APIs.AvailableAPIs)
                            {
                                if ((api.Instance.Name + " (" + api.Instance.Version + ")") == apiName)
                                {
                                    //apiDLL = apiC;
                                    curAPI = api;
                                    break;
                                }
                                apiC++;
                            }
                        }
                    }
                }
                catch { }
                x++;

                try
                {
                    ncDonatePopup = bool.Parse(settLines[x]);
                }
                catch { }
                x++;
            }
            else
            {
                //apiDLL = 1;
                curAPI = Global.APIs.AvailableAPIs.GetIndex(0);
                curAPI?.Instance?.Initialize();
            }

            refPlugin_Click(null, null);

            attachProcessButton.Enabled = false;

            //Add the first Code
            cbList.Items.Add("NEW CODE");
            //Set backcolor
            cbList.Items[0].ForeColor = ncForeColor;
            cbList.Items[0].BackColor = ncBackColor;

            CodeDB cdb = new CodeDB();
            cdb.name = "NEW CODE";
            cdb.state = false;
            //Codes[CodesCount].name = "NEW CODE";
            //Codes[CodesCount].state = false;
            Codes.Add(cdb);
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

            FRManager = new FindReplaceManager();
            FRManager.BackColor = ncBackColor;
            FRManager.ForeColor = ncForeColor;

            HandlePluginControls(searchControl1.Controls);
            HandlePluginControls(FRManager.Controls);
        }

        /* Connects to PS3 */
        private void connectButton_Click(object sender, EventArgs e)
        {
            if (curAPI == null)
                return;

            this.statusLabel1.Text = "Connecting...";
            try
            {
                if (curAPI.Instance.Connect())
                {
                    connected = true;
                    this.statusLabel1.Text = "Connected";


                    connectButton.Enabled = false;
                    attachProcessButton.Enabled = true;
                    toolStripDropDownButton1.BackColor = Color.DarkGoldenrod;
                }
                else
                {
                    this.statusLabel1.Text = "Failed to connect";
                    connected = false;
                }
            }
            catch
            {
                this.statusLabel1.Text = "Failed to connect";
                connected = false;
            }
        }

        /* Attachs to process */
        private void attachProcessButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (curAPI.Instance.Attach())
                {
                    this.statusLabel1.Text = "Process Attached";

                    ConstantLoop = 1;
                    attachProcessButton.Enabled = false;
                    toolStripDropDownButton1.BackColor = Color.DarkGreen;
                    attached = true;
                }
                else
                {
                    this.statusLabel1.Text = "Error attaching process";
                    toolStripDropDownButton1.BackColor = Color.DarkGoldenrod;
                }
                
            }
            catch (Exception)
            {
                this.statusLabel1.Text = "Error attaching process";
                toolStripDropDownButton1.BackColor = Color.DarkGoldenrod;
            }
        }

        /* Disconnects from PS3 */
        private void ps3Disc_Click(object sender, EventArgs e)
        {
            if (curAPI == null)
                return;

            try
            {
                curAPI.Instance.Disconnect();
                ConstantLoop = 2;
                this.statusLabel1.Text = "Disconnected";
                attached = false;
                connected = false;

                attachProcessButton.Enabled = false;
                connectButton.Enabled = true;
                toolStripDropDownButton1.BackColor = Color.Maroon;
            }
            catch (Exception)
            {

            }
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
            {
                cbName.Text = "";
                cbCodes.Text = "";
                cbState.Checked = false;
                return;
            }

            //Update the textboxes to the new code
            if (Index >= Codes.Count)
                return;
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

            CodeDB cdb = new CodeDB();
            cdb.name = "NEW CODE";
            cdb.state = false;
            cdb.codes = "";
            //Codes[CodesCount].name = "NEW CODE";
            //Codes[CodesCount].state = false;
            //Codes[CodesCount].codes = "";
            Codes.Add(cdb);
        }

        /* Removes a code from the list */
        private void cbRemove_Click(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;

            cbList.Items[ind].Remove();

            if (ind < Codes.Count)
                Codes.RemoveAt(ind);

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
                    cbList.Items[cnt].ForeColor = ncForeColor;
                    cbList.Items[cnt].BackColor = ncBackColor;
                    Codes.Add(ret[x]);
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

                CodeDB c = Codes[cbListIndex];
                c.filename = fd.FileName;
                //Codes[cbListIndex].filename = fd.FileName;
                Codes[cbListIndex] = c;
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
            if (Codes[cbListIndex].backUp == null || Codes[cbListIndex].backUp.Length == 0)
            {
                CodeDB c = Codes[cbListIndex];
                c.backUp = codes.CreateBackupPS3(Codes[cbListIndex]);
                //Codes[cbListIndex].backUp = codes.CreateBackupPS3(Codes[cbListIndex]);
                Codes[cbListIndex] = c;
            }
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

            CodeDB c = Codes[ind];
            c.state = cbState.Checked;
            Codes[ind] = c;

            ConstantLoop = 1;
        }

        /* Updates the name in cbList */
        private void cbName_TextChanged(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;

            CodeDB c = Codes[ind];
            c.name = cbName.Text;
            //Codes[ind].name = cbName.Text;
            Codes[ind] = c;
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

                CodeDB c = Codes[ind];
                c.state = false;
                Codes[ind] = c;
            }
            else
            {
                cbList.Items[ind].Text = "+ " + Codes[ind].name;
                CodeDB c = Codes[ind];
                c.state = true;
                Codes[ind] = c;
                ConstantLoop = 1;
            }
            Application.DoEvents();
            UpdateCB(ind);
        }

        private void cbCodes_TextChanged(object sender, EventArgs e)
        {
            int ind = cbListIndex;
            if (ind < 0)
                return;

            

            CodesCount = cbList.Items.Count - 1;
            CodeDB c = Codes[ind];
            c.codes = cbCodes.Text.Replace("{", "").Replace("}", "").Replace("#", "");
            Codes[ind] = c;
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

        private void cbCodes_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                bool hasValidCode = false;
                bool startsAsCond = false;
                ncCode[] cds = null;

                try
                {
                    cds = codes.ParseCodeStringFull(cbCodes.SelectedText);

                    for (int x = 0; x < cds.Length; x++)
                    {
                        if (codes.isCodeValid(cds[x]))
                        {
                            hasValidCode = true;

                            if (x == 0 && (cds[x].codeType.ToString().ToUpper() == "D" || cds[x].codeType.ToString().ToUpper() == "E"))
                            {
                                if (cbCodes.SelectedText.StartsWith(cds[x].codeType.ToString() + cds[x].codeArg0.ToString("X") + " " + cds[x].codeArg1.ToString("X8") + " " + misc.ByteAToStringHex(cds[x].codeArg2, "")))
                                    startsAsCond = true;
                            }

                            break;
                        }
                    }
                }
                catch { }

                editConditionalToolStripMenuItem.Visible = startsAsCond;

                if (hasValidCode)
                {
                    bwStripMenuItem1.Visible = true;
                    twStripMenuItem1.Visible = true;
                    fwStripMenuItem1.Visible = true;
                    codesToolMenuStrip.Show(sender as Control, e.Location);
                }
                else
                {
                    bwStripMenuItem1.Visible = false;
                    twStripMenuItem1.Visible = false;
                    fwStripMenuItem1.Visible = false;
                    codesToolMenuStrip.Show(sender as Control, e.Location);
                }
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
                //if (ctrl is GroupBox || ctrl is Panel || ctrl is TabControl || ctrl is TabPage||
                //    ctrl is UserControl || ctrl is ListBox || ctrl is ListView)
                if (ctrl.Controls != null && ctrl.Controls.Count > 0)
                {
                    HandlePluginControls(ctrl.Controls);
                }
                if (ctrl is ListView)
                {
                    foreach (ListViewItem ctrlLVI in (ctrl as ListView).Items)
                    {
                        ctrlLVI.BackColor = ncBackColor;
                        ctrlLVI.ForeColor = ncForeColor;
                    }
                }
                
                ctrl.BackColor = ncBackColor;
                ctrl.ForeColor = ncForeColor;
            }
        }

        public void HandleFocusControls(Control.ControlCollection focCtrl)
        {
            foreach (Control ctrl in focCtrl)
            {
                if (ctrl.Controls != null && ctrl.Controls.Count > 0)
                    HandleFocusControls(ctrl.Controls);

                ctrl.GotFocus += new EventHandler(Form1_Focused);
            }
        }

        private void LoadAPIs()
        {
            apiList.Items.Clear();

            if (System.IO.Directory.Exists(Application.StartupPath + @"\APIs") == false)
                return;

            //Delete any excess NCAppInterface.dll's (result of a build and not a copy)
            foreach (string file in System.IO.Directory.GetFiles(Application.StartupPath + @"\APIs", "NCAppInterface.dll", System.IO.SearchOption.AllDirectories))
                System.IO.File.Delete(file);

            //Call the find apis routine, to search in our APIs Folder
            Global.APIs.FindAPIs(Application.StartupPath + @"\APIs");

            //Load apis
            foreach (Types.AvailableAPI apiOn in Global.APIs.AvailableAPIs)
            {
                apiList.Items.Add(apiOn.Instance.Name + " (" + apiOn.Instance.Version + ")");
            }

            if (apiList.Items.Count > 0)
                apiList.SelectedIndex = 0;
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
                codes.ConstCodes = new List<codes.ConstCode>();
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
                Array.Resize(ref pluginForm, pluginForm.Length + 1);
                pluginForm[pluginForm.Length - 1] = new PluginForm();
                pluginForm[pluginForm.Length - 1].plugAuth = snapshot.author;
                pluginForm[pluginForm.Length - 1].plugDesc = snapshot.desc;
                pluginForm[pluginForm.Length - 1].plugName = snapshot.name;
                pluginForm[pluginForm.Length - 1].plugText = snapshot.tabName;
                pluginForm[pluginForm.Length - 1].plugVers = snapshot.version;

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
                pluginForm[ind].WindowState = FormWindowState.Normal;
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

        snapshot snapShotPlugin = new snapshot();
        private void pluginList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = pluginList.SelectedIndex;
            if (ind < 0)
                return;

            if (pluginList.Items[ind].ToString().IndexOf(snapshot.tabName) >= 0)
            {
                descPlugAuth.Text = "by " + pluginForm[ind].plugAuth;
                descPlugName.Text = pluginForm[ind].plugName;
                descPlugVer.Text = pluginForm[ind].plugVers;
                descPlugDesc.Text = pluginForm[ind].plugDesc;
                pluginForm[ind].Text = pluginForm[ind].plugName + " by " + pluginForm[ind].plugAuth;
                plugIcon.Image = (Bitmap)plugIcon.InitialImage.Clone();

                pluginForm[ind].Controls.Clear();
                pluginForm[ind].Controls.Add(snapShotPlugin);
                pluginForm[ind].Controls[0].Resize += new EventHandler(pluginForm[ind].Plugin_Resize);
                pluginForm[ind].Resize += new EventHandler(snapShotPlugin.snapshot_Resize);

                if (pluginForm[ind].allowColoring)
                {
                    HandlePluginControls(pluginForm[ind].Controls[0].Controls);
                    pluginForm[ind].Controls[0].BackColor = ncBackColor;
                    pluginForm[ind].Controls[0].ForeColor = ncForeColor;
                }
            }
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

                    //Color each control in the tab too
                    //for (int tabCtrl = 0; tabCtrl < TabCon.TabPages[TabCon.SelectedIndex].Controls[0].Controls.Count; tabCtrl++)

                    if (pluginForm[ind].allowColoring)
                    {
                        pluginForm[ind].Controls[0].BackColor = ncBackColor;
                        pluginForm[ind].Controls[0].ForeColor = ncForeColor;
                        HandlePluginControls(pluginForm[ind].Controls[0].Controls);
                    }
                }

                if (pluginForm[ind].Controls.Count > 0 && pluginList.Items[ind].ToString().IndexOf(snapshot.tabName) < 0)
                {
                    descPlugAuth.Text = "by " + selectedPlugin.Instance.Author;
                    descPlugName.Text = selectedPlugin.Instance.Name;
                    descPlugVer.Text = selectedPlugin.Instance.Version;
                    descPlugDesc.Text = selectedPlugin.Instance.Description;
                    if (selectedPlugin.Instance.MainIcon != null && selectedPlugin.Instance.MainIcon.BackgroundImage != null)
                        plugIcon.Image = (Bitmap)selectedPlugin.Instance.MainIcon.BackgroundImage.Clone();
                    else
                        plugIcon.Image = (Bitmap)plugIcon.InitialImage.Clone();
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CopyResults();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DeleteSearchResult();
        }

        private void refreshFromPS3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //RefreshSearchResults(1);
        }

        private void refreshFromDumptxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //RefreshFromDump();
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

                    Text = "NetCheat PS3 " + versionNum + " by Dnawrkshp" + ((IntPtr.Size == 8) ? " (64 Bit)" : " (32 Bit)") + " (" + recRangeBox.Items[ind].Text + ")";

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
            curAPI.Instance.Shutdown();
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
            allowForce = true;
            RunUpdateChecker();
            allowForce = false;
        }

        private void gameStatusStripMenuItem1_Click(object sender, EventArgs e)
        {
            /*
            if (connected)
            {
                PS3TMAPI.UnitStatus ret;
                PS3TMAPI.GetStatus(Target, PS3TMAPI.UnitType.PPU, out ret);
                if (ret == PS3TMAPI.UnitStatus.Stopped)
                {
                    PS3TMAPI.ProcessContinue(Target, ProcessID);
                    gameStatusStripMenuItem1.Text = "Pause Game";
                }
                else
                {
                    PS3TMAPI.ProcessStop(Target, ProcessID);
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

                byte[] ret = new byte[blockSize];
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

            if (TabCon.SelectedTab != null)
            {
                if (TabCon.SelectedTab.Text == "Dump Compare")
                {
                    foreach (SearchValue sv in compare.SearchArgs)
                    {
                        sv.Fore = ForeColor;
                        sv.Back = BackColor;
                    }
                }
            }
        }

        private void startGameButt_Click(object sender, EventArgs e)
        {
            if (!curAPI.Instance.ContinueProcess())
                MessageBox.Show("Feature not supported with this API!");
        }

        private void pauseGameButt_Click(object sender, EventArgs e)
        {
            if (!curAPI.Instance.PauseProcess())
                MessageBox.Show("Feature not supported with this API!");
        }

        public static void PauseProcess()
        {
            curAPI.Instance.PauseProcess();
        }

        public static void ContinueProcess()
        {
            curAPI.Instance.ContinueProcess();
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

        private void cbResetWrite_Click(object sender, EventArgs e)
        {
            //foreach (ncCode nCode in Codes[cbListIndex].backUp)
            for (int x = Codes[cbListIndex].backUp.Length - 1; x >= 0; x--)
            {
                if (Codes[cbListIndex].backUp[x].codeType == '0' || Codes[cbListIndex].backUp[x].codeType == '1')
                    apiSetMem(Codes[cbListIndex].backUp[x].codeArg1, Codes[cbListIndex].backUp[x].codeArg2);
            }
            if (Codes[cbListIndex].backUp.Length == 0)
                MessageBox.Show("Please write before you reset.\nKeep in mind constant writing doesn't save a backup and editing the text box erases the backup.");
        }

        private void cbBackupWrite_Click(object sender, EventArgs e)
        {
            CodeDB c = Codes[cbListIndex];
            c.backUp = codes.CreateBackupPS3(Codes[cbListIndex]);
            Codes[cbListIndex] = c;
        }

        #region Dump Compare Tab

        int lastSearchIndex = -1;
        int lastTypeIndex = -1;
        Comparator compare = new Comparator();
        public static string[] inputBins = new string[2];

        string BrowseForFile(string filter)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (filter != null)
                fd.Filter = filter;
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                return fd.FileName;
            }

            return "";
        }

        private void browseDump1_Click(object sender, EventArgs e)
        {
            dumpTB1.Text = BrowseForFile("Binary files (*.bin)|*.bin|All files (*.*)|*.*");
            inputBins[0] = dumpTB1.Text;
        }

        private void browseDump2_Click(object sender, EventArgs e)
        {
            dumpTB2.Text = BrowseForFile("Binary files (*.bin)|*.bin|All files (*.*)|*.*");
            inputBins[1] = dumpTB2.Text;
        }

        private void searchNameBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lastSearchIndex == searchNameBox.SelectedIndex && compare.SearchArgs.Count > 0)
                return;

            string[] searchArgsOldValue = new string[compare.SearchArgs.Count];
            for (int sAOV = 0; sAOV < searchArgsOldValue.Length; sAOV++)
                searchArgsOldValue[sAOV] = compare.SearchArgs[sAOV].getValue();

            Comparator.ncSearcher searcher = compare.SearchComparisons.Where(ns => ns.Name == searchNameBox.Items[searchNameBox.SelectedIndex].ToString()).FirstOrDefault();
            SearchControl.ncSearchType type = compare.SearchTypes[searchTypeBox.SelectedIndex];
            //searchNameBox.Items.Clear();

            int yOff = 5;

            foreach (SearchValue sv in compare.SearchArgs)
                DumpCompTab.Controls.Remove(sv);
            compare.SearchArgs.Clear();
            if (searcher.Args != null)
            {
                int cnt = 0;
                foreach (string str in searcher.Args)
                {
                    SearchValue a = new SearchValue();

                    string def = type.DefaultValue;
                    if (cnt < searchArgsOldValue.Length && searchArgsOldValue[cnt] != null)
                        def = searchArgsOldValue[cnt];

                    a.SetSValue(str, def, type.CheckboxName, true, true, type.CheckboxConvert);
                    a.Location = new Point(5, yOff);
                    a.Width = Width - 5;
                    a.Back = BackColor;
                    a.Fore = ForeColor;

                    compare.SearchArgs.Add(a);
                    DumpCompTab.Controls.Add(a);
                    yOff += a.Height + 5;
                    cnt++;
                }
            }

            lastSearchIndex = searchNameBox.SelectedIndex;
            UpdateSize();
        }

        private void searchTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (searchTypeBox.SelectedIndex < 0 || searchNameBox.SelectedIndex < 0 || lastTypeIndex == searchTypeBox.SelectedIndex)
                return;

            foreach (SearchValue sv in compare.SearchArgs)
                DumpCompTab.Controls.Remove(sv);
            compare.SearchArgs.Clear();

            Comparator.ncSearcher searcher = compare.SearchComparisons.Where(ns => ns.Name == searchNameBox.Items[searchNameBox.SelectedIndex].ToString()).FirstOrDefault();
            SearchControl.ncSearchType type = compare.SearchTypes[searchTypeBox.SelectedIndex];
            int yOff = 5;

            if (searcher.Args != null)
            {
                foreach (string str in searcher.Args)
                {
                    SearchValue a = new SearchValue();
                    a.SetSValue(str, type.DefaultValue, type.CheckboxName, true, true, type.CheckboxConvert);
                    a.Location = new Point(5, yOff);
                    a.Width = Width - 5;
                    a.Back = BackColor;
                    a.Fore = ForeColor;

                    compare.SearchArgs.Add(a);
                    DumpCompTab.Controls.Add(a);
                    yOff += a.Height + 5;
                }
            }

            lastTypeIndex = searchTypeBox.SelectedIndex;
            UpdateSize();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            progBar.BackColor = BackColor;
            progBar.ForeColor = ForeColor;

            if (searchButton.Text == "Stop")
            {
                compare._shouldStopSearch = true;
                return;
            }
            else if (searchButton.Text == "New Search")
            {
                compare.Items.Clear();
                searchButton.Text = "Search";
            }
            else if (searchButton.Text == "Search")
            {
                try
                {
                    compare._shouldStopSearch = false;
                    uint start = Convert.ToUInt32(startTB.Text, 16);
                    uint stop = Convert.ToUInt32(stopTB.Text, 16);
                    if (stop == 0)
                    {
                        stop = (uint)new FileInfo(inputBins[0]).Length;
                    }

                    ulong addrFrom = Convert.ToUInt64(addrFromTB.Text, 16);
                    string[] args = new string[compare.SearchArgs.Count];
                    for (int x = 0; x < args.Length; x++)
                        args[x] = compare.SearchArgs[x].GetDefValue();
                    int typeIndex = searchTypeBox.SelectedIndex;

                    if (searchNameBox.SelectedIndex < 0)
                        searchNameBox.SelectedIndex = lastSearchIndex;
                    Comparator.ncSearcher searcher = compare.SearchComparisons.Where(sc => sc.Name == searchNameBox.Items[searchNameBox.SelectedIndex].ToString()).FirstOrDefault();

                    //searchThread = new Thread(searcher.InitialSearch(start, stop, searchTypeBox.SelectedIndex, args));
                    //searchThread = new Thread(ThreadInitSearch(start, stop, searchTypeBox.SelectedIndex, args));
                    searchButton.Text = "Stop";
                    compare.ThreadInitSearch(new object[] { searcher, start, stop, addrFrom, typeIndex, args, false });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void saveScan_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(fd.FileName, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, compare.Items.ToArray());
                stream.Close();

                MessageBox.Show("Saved!");
            }
        }

        public void SetSearchStr(string text)
        {
            Invoke((MethodInvoker)delegate
            {
                searchButton.Text = text;
            });
        }

        public void ResetSearchCompBox()
        {
            lastSearchIndex = searchNameBox.SelectedIndex;
            searchNameBox.Items.Clear();
            foreach (Comparator.ncSearcher nS in compare.SearchComparisons)
            {
                searchNameBox.Items.Add(nS.Name);
            }

            if (searchTypeBox.Items.Count == 0)
            {
                foreach (SearchControl.ncSearchType nT in compare.SearchTypes)
                {
                    searchTypeBox.Items.Add(nT.Name);
                    searchTypeBox.SelectedIndex = 0;
                }
            }

            if (lastSearchIndex < 0)
                lastSearchIndex = 0;

            searchNameBox.SelectedIndex = lastSearchIndex;
        }

        void UpdateSize()
        {
            int yOff = 20 + dumpTB1.Height + dumpTB2.Height;
            foreach (SearchValue sb in compare.SearchArgs)
            {
                sb.Location = new Point(5, yOff);
                sb.Width = DumpCompTab.Width - 10;
                yOff += 5 + sb.Height;
            }
        }

        #endregion

        private void cbBManager_Click(object sender, EventArgs e)
        {
            if (FRManager == null || FRManager.IsDisposed)
            {
                FRManager = new FindReplaceManager();
                FRManager.ForeColor = ncForeColor;
                FRManager.BackColor = ncBackColor;
                HandlePluginControls(FRManager.Controls);
            }

            FRManager.Show();
            FRManager.Focus();
        }

        /*
         * Holy
         * Fucking
         * Shit
         */
        private void Form1_Resize(object sender, EventArgs e)
        {
            int ratio = 0;

            //Tab Control
            {
                TabCon.Size = new Size(this.Width - 40, this.Height - (501 - 393));
                int tHeight = TabCon.SelectedTab.Height, tWidth = TabCon.SelectedTab.Width;
                if (tHeight == 0 || tWidth == 0)
                    return;
                //Tab Page: Codes
                {
                    ratio = (tHeight - (367 - 298)) - cbList.Height;
                    cbList.Height = (tHeight - (367 - 298));
                    cbAdd.Top += ratio;
                    cbRemove.Top += ratio;
                    cbImport.Top += ratio;
                    cbSave.Top += ratio;
                    cbSaveAll.Top += ratio;
                    cbSaveAs.Top += ratio;

                    label5.Width = (tWidth - (453 - 225));
                    cbName.Width = (tWidth - (453 - 225));
                    cbState.Width = (tWidth - (453 - 225));
                    ratio = (tHeight - (367 - 225)) - cbCodes.Height;
                    cbCodes.Height = (tHeight - (367 - 225));
                    cbCodes.Width = (tWidth - (453 - 225));
                    cbWrite.Top += ratio;
                    cbWrite.Width = (tWidth - (453 - 225));
                    cbBManager.Top += ratio;
                    cbResetWrite.Top += ratio;
                    cbBackupWrite.Top += ratio;

                    ratio = (tWidth - cbBManager.Left - 18) / 3;
                    cbBManager.Width = ratio;
                    cbResetWrite.Width = ratio;
                    cbBackupWrite.Width = ratio;
                    cbResetWrite.Left = cbBManager.Left + cbBManager.Width + 5;
                    cbBackupWrite.Left = cbResetWrite.Left + cbResetWrite.Width + 5;
                }
                //Tab Page: Search
                {
                    searchControl1.Size = new Size(tWidth - (461 - 444), tHeight - (393 - 383));
                }
                //Tab Page: Range
                {
                    label1.Width = tWidth - 20;
                    label2.Width = tWidth - 20;
                    rangeView.Height = tHeight - 40;

                    ratio = tWidth - 235 - 8;
                    label3.Width = ratio;
                    recRangeBox.Width = ratio;
                    findRangeProgBar.Width = ratio - 91 - 6;
                    findRanges.Left = findRangeProgBar.Left + findRangeProgBar.Width + 6;

                    ratio -= 30;
                    ratio /= 2;

                    RangeUp.Width = ratio;
                    RangeDown.Width = ratio;
                    RangeDown.Left = RangeUp.Left + ratio + 30;
                    ImportRange.Width = ratio;
                    SaveRange.Width = ratio;
                    SaveRange.Left = ImportRange.Left + ratio + 30;
                    AddRange.Width = ratio;
                    RemoveRange.Width = ratio;
                    RemoveRange.Left = AddRange.Left + ratio + 30;

                    ratio = tHeight - 200 - recRangeBox.Height;
                    recRangeBox.Height = tHeight - 200;
                    RangeUp.Top += ratio;
                    RangeDown.Top += ratio;
                    ImportRange.Top += ratio;
                    SaveRange.Top += ratio;
                    AddRange.Top += ratio;
                    RemoveRange.Top += ratio;
                }
                //Tab Page: Plugins
                {
                    pluginList.Height = tHeight - 40;
                    int pWidth = tWidth - (461 - 174);
                    if (pWidth > 250)
                        pWidth = 250;
                    pluginList.Width = pWidth;

                    descPlugName.Left = pluginList.Left + pluginList.Width + 6;
                    descPlugAuth.Left = descPlugName.Left + 22;
                    descPlugVer.Left = descPlugAuth.Left;
                    descPlugDesc.Left = descPlugName.Left + 3;
                    plugIcon.Left = descPlugName.Left;

                    int plugIconHeight = tHeight - (393 - 210);
                    plugIcon.Height = (int)(210f * ((float)plugIconHeight / 210f));
                    plugIcon.Width = (int)(266f * ((float)plugIconHeight / 210f));

                    descPlugDesc.Width = tWidth - descPlugDesc.Left;
                }
                //Tab Page: APIs
                {
                    apiList.Height = tHeight - 40;
                    int pWidth = tWidth - (461 - 174);
                    if (pWidth > 250)
                        pWidth = 250;
                    apiList.Width = pWidth;

                    descAPIName.Left = apiList.Left + apiList.Width + 6;
                    descAPIAuth.Left = descAPIName.Left + 22;
                    descAPIVer.Left = descAPIAuth.Left;
                    descAPIDesc.Left = descAPIName.Left + 3;
                    apiIcon.Left = descAPIName.Left;

                    int APIIconHeight = tHeight - (393 - 210);
                    apiIcon.Height = (int)(210f * ((float)APIIconHeight / 210f));
                    apiIcon.Width = (int)(266f * ((float)APIIconHeight / 210f));

                    descAPIDesc.Width = tWidth - descAPIDesc.Left;
                }
            }

            //Buttons on main form
            {

            }

            this.HScroll = false;
            this.VScroll = false;
            this.AutoScroll = false;
        }

        public bool isProcessStopped()
        {
            if (curAPI == null)
                return true;
            return curAPI.Instance.isProcessStopped();
        }

        private void endianStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentEndian == Endian.Big)
                CurrentEndian = Endian.Little;
            else
                CurrentEndian = Endian.Big;
        }

        private void bwStripMenuItem1_Click(object sender, EventArgs e)
        {
            string text = "";
            string[] lines = cbCodes.SelectedText.Split(new char[] { '\r', '\n' });
            //ncCode[] cds = codes.ParseCodeStringFull(cbCodes.SelectedText);
            char[] supported = new char[] { '1', '2' };

            for (int x = 0; x < lines.Length; x++)
            {
                ncCode c = codes.ParseCodeStringFull(lines[x])[0];
                if (codes.isCodeValid(c) && supported.Contains(c.codeType))
                {
                    text += "0 " + c.codeArg1.ToString("X8") + " " + misc.ByteAToStringHex(c.codeArg2, "") + "\r\n";
                }
                else
                {
                    text += lines[x] + "\r\n";
                }

            }

            if (text.EndsWith("\r\n"))
                text = text.Remove(text.Length - 2, 2);

            cbCodes.SelectedText = text;
        }

        private void twStripMenuItem1_Click(object sender, EventArgs e)
        {
            string text = "";
            string[] lines = cbCodes.SelectedText.Split(new char[] { '\r', '\n' });
            //ncCode[] cds = codes.ParseCodeStringFull(cbCodes.SelectedText);
            char[] supported = new char[] { '0' };

            for (int x = 0; x < lines.Length; x++)
            {
                ncCode c = codes.ParseCodeStringFull(lines[x])[0];
                if (codes.isCodeValid(c) && supported.Contains(c.codeType))
                {
                    text += "1 " + c.codeArg1.ToString("X8") + " " + misc.ByteAToString(c.codeArg2, "").Replace("\0", "") + "\r\n";
                }
                else
                {
                    text += lines[x] + "\r\n";
                }

            }

            if (text.EndsWith("\r\n"))
                text = text.Remove(text.Length - 2, 2);

            cbCodes.SelectedText = text;
        }

        private void fwStripMenuItem1_Click(object sender, EventArgs e)
        {
            string text = "";
            string[] lines = cbCodes.SelectedText.Split(new char[] { '\r', '\n' });
            //ncCode[] cds = codes.ParseCodeStringFull(cbCodes.SelectedText);
            char[] supported = new char[] { '0' };

            for (int x = 0; x < lines.Length; x++)
            {
                ncCode c = codes.ParseCodeStringFull(lines[x])[0];
                if (codes.isCodeValid(c) && supported.Contains(c.codeType) && c.codeArg2.Length <= 4)
                {
                    byte[] newBA = c.codeArg2;
                    if (c.codeArg2.Length < 4)
                    {
                        newBA = new byte[4];
                        int mod = (int)c.codeArg1 % 4;
                        Array.Copy(c.codeArg2, 0, newBA, mod, c.codeArg2.Length);
                    }

                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(newBA);
                    text += "2 " + c.codeArg1.ToString("X8") + " " + BitConverter.ToSingle(newBA, 0) + "\r\n";
                }
                else
                {
                    text += lines[x] + "\r\n";
                }

            }

            if (text.EndsWith("\r\n"))
                text = text.Remove(text.Length - 2, 2);

            cbCodes.SelectedText = text;
        }

        private void createCondStripMenuItem1_Click(object sender, EventArgs e)
        {
            ConditionalEditor ce = new ConditionalEditor();
            ce.rtb = cbCodes;
            ce.isEditing = false;
            ce.code = cbCodes.SelectedText;
            ce.ShowDialog();

        }

        private void editConditionalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConditionalEditor ce = new ConditionalEditor();
            ce.rtb = cbCodes;
            ce.isEditing = true;
            ce.code = cbCodes.SelectedText;
            ce.ShowDialog();
        }

        Types.AvailableAPI selAPI;
        private void apiList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = apiList.SelectedIndex;
            if (ind < 0)
                return;

            Types.AvailableAPI api = Global.APIs.AvailableAPIs.GetIndex(ind);
            descAPIAuth.Text = "by " + api.Instance.Author;
            descAPIName.Text = api.Instance.Name;
            descAPIVer.Text = api.Instance.Version;
            descAPIDesc.Text = api.Instance.Description;
            if (api.Instance.Icon != null)
                apiIcon.Image = (Bitmap)api.Instance.Icon.Clone();
            else
                apiIcon.Image = (Bitmap)apiIcon.InitialImage.Clone();

            string[] parts = apiList.Items[ind].ToString().Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            int apiIndex = Global.APIs.AvailableAPIs.GetIndex(parts[0].Trim(), parts[1]);
            selAPI = Global.APIs.AvailableAPIs.GetIndex(apiIndex);
        }

        private void apiList_DoubleClick(object sender, EventArgs e)
        {
            int ind = apiList.SelectedIndex;
            if (ind < 0)
                return;

            if (apiName == apiList.Items[ind].ToString())
                return;
            else
            {
                if (MessageBox.Show("Are you sure you'd like to switch the API to " + apiList.Items[ind].ToString() + "?", "Current API: " + apiName, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    //curAPI.Instance.Disconnect();
                    ps3Disc_Click(null, null);
                    string[] parts = apiList.Items[ind].ToString().Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    int apiDLL = Global.APIs.AvailableAPIs.GetIndex(parts[0].Trim(), parts[1]);
                    curAPI = Global.APIs.AvailableAPIs.GetIndex(apiDLL);
                    curAPI.Instance.Initialize();

                    SaveOptions();
                }
            }
        }

        private void apiIcon_MouseLeave(object sender, EventArgs e)
        {
            int ind = apiList.SelectedIndex;
            if (ind < 0)
                return;

            if (selAPI == null)
                return;

            if (selAPI.Instance.ContactLink != null && selAPI.Instance.ContactLink != "")
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void apiIcon_Click(object sender, EventArgs e)
        {
            int ind = apiList.SelectedIndex;
            if (ind < 0)
                return;

            if (selAPI == null)
                return;

            if (selAPI.Instance.ContactLink != null && selAPI.Instance.ContactLink != "")
            {
                System.Diagnostics.Process.Start(selAPI.Instance.ContactLink);
            }
        }

        private void apiIcon_MouseEnter(object sender, EventArgs e)
        {
            int ind = apiList.SelectedIndex;
            if (ind < 0)
                return;

            if (selAPI == null)
                return;

            if (selAPI.Instance.ContactLink != null && selAPI.Instance.ContactLink != "")
            {
                Cursor.Current = Cursors.Hand;
            }
        }

        private void apiIcon_MouseMove(object sender, MouseEventArgs e)
        {
            if (selAPI == null)
                return;

            if (selAPI.Instance.ContactLink != null && selAPI.Instance.ContactLink != "")
            {
                Cursor.Current = Cursors.Hand;
            }
        }

        private void apiIcon_MouseHover(object sender, EventArgs e)
        {
            if (selAPI == null)
                return;

            if (selAPI.Instance.ContactLink != null && selAPI.Instance.ContactLink != "")
            {
                Cursor.Current = Cursors.Hand;
            }
        }

        private void configureAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            curAPI.Instance.Configure();
        }

    }
}
