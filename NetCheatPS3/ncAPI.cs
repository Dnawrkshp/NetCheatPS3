using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace NetCheatPS3
{
    class ncAPI
    {
        public static Control[] retControlArr;
        public static PluginNCP[] funcCtrArray;

        public struct PluginNCP
        {
            public PluginScript sTrue;
            public PluginScript sFalse;
            public int tabIndex;
        };

        public struct PluginScript
        {
            public PluginByteA[] pbA;
            public PluginULongA[] pulA;
            public PluginCall[] funcCalls;
        };

        public struct PluginByteA
        {
            public byte[] byteA;
            public string bName;
        };

        public struct PluginULongA
        {
            public ulong ulongA;
            public string ulName;
        };

        public struct PluginCall
        {
            public string fName;
            public string[] argNames;
        };

        public static TabPage[] LoadTabs(string PluginDir)
        {
            if (Directory.Exists(PluginDir) == false)
                Directory.CreateDirectory(PluginDir);
            if (File.Exists(PluginDir + "\\default.txt") == false)
                return null;

            RichTextBox rtb = new RichTextBox();
            /* Remove Comments */
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            rtb.Text = System.Text.RegularExpressions.Regex.Replace(File.ReadAllText(PluginDir + "\\default.txt"), re, "$1");

            TabPage[] ret = new TabPage[0];

            int x = 0;
            while (x < rtb.Lines.Length && rtb.Lines[x] == "")
                x++;
            while (x < rtb.Lines.Length)
            {
                string[] text = rtb.Lines[x].Split(' ');
                switch (text[0])
                {
                    case "NEWTAB": //Adds a new tab
                        Array.Resize(ref ret, ret.Length + 1);
                        int ind = ret.Length - 1;
                        string tabName = text[1].Replace(",", "");
                        string tabText = text[2].Replace(",", "");
                        TabPage tempTab = new TabPage();
                        tempTab.Name = tabName;
                        tempTab.Text = tabText;
                        tempTab.BackColor = System.Drawing.Color.Black;
                        tempTab.ForeColor = System.Drawing.Color.FromArgb(0, 130, 210);
                        tempTab.Tag = ind;
                        ret[ind] = tempTab;
                        break;
                }
                x++;
            }
            return ret;
        }

        public static Control[] LoadPlugins(string PluginDir)
        {
            if (Directory.Exists(PluginDir) == false)
                Directory.CreateDirectory(PluginDir);
            string[] files = Directory.GetFiles(PluginDir, "*.ncp", SearchOption.AllDirectories);
            if (files == null)
                return null;


            //Set defaults
            retControlArr = new Control[files.Length];
            funcCtrArray = new PluginNCP[files.Length];
            int x = 0;
            for (x = 0; x < files.Length; x++)
            {
                //True
                funcCtrArray[x].sTrue.pbA = new PluginByteA[0];
                funcCtrArray[x].sTrue.pulA = new PluginULongA[0];
                funcCtrArray[x].sTrue.funcCalls = new PluginCall[0];
                //False
                funcCtrArray[x].sFalse.pbA = new PluginByteA[0];
                funcCtrArray[x].sFalse.pulA = new PluginULongA[0];
                funcCtrArray[x].sFalse.funcCalls = new PluginCall[0];
                ProcessPlugin(files[x], x);
            }

            return retControlArr;
        }

        private static void ProcessPlugin(string file, int index)
        {
            //Whether to add to sTrue or sFalse
            bool stateTrue = true;

            if (File.Exists(file) == false)
                return;

            RichTextBox rtb = new RichTextBox();
            /* Remove Comments */
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            rtb.Text = System.Text.RegularExpressions.Regex.Replace(File.ReadAllText(file), re, "$1");

            //Process lines
            
            //Type
            int x = 0;
            while (rtb.Lines[x] == "\r\n" || rtb.Lines[x] == "")
                x++;
            switch (rtb.Lines[x])
            {
                case "Checkbox":
                    CheckBox tempCB = new CheckBox();
                    tempCB.CheckedChanged += new EventHandler(DoEventCheckBox);
                    tempCB.Tag = index;
                    tempCB.FlatStyle = FlatStyle.Standard;
                    tempCB.BackColor = System.Drawing.Color.Black;
                    tempCB.ForeColor = System.Drawing.Color.FromArgb(0, 130, 210);
                    retControlArr[index] = tempCB;
                    break;
                case "Button":
                    Button tempB = new Button();
                    tempB.Click += new EventHandler(DoEventButton);
                    tempB.Tag = index;
                    tempB.FlatStyle = FlatStyle.Standard;
                    tempB.AutoSize = true;
                    tempB.BackColor = System.Drawing.Color.Black;
                    tempB.ForeColor = System.Drawing.Color.FromArgb(0, 130, 210);
                    retControlArr[index] = tempB;
                    break;
            }
            x++;

            //Tab owner
            while (rtb.Lines[x] == "\r\n" || rtb.Lines[x] == "")
                x++;
            string tabName = rtb.Lines[x];
            
            //Default
            funcCtrArray[index].tabIndex = -1;
            for (int tabCnt = 0; tabCnt < Form1.tabs.Length; tabCnt++)
            {
                if (Form1.tabs[tabCnt].Name == tabName)
                    funcCtrArray[index].tabIndex = (int)Form1.tabs[tabCnt].Tag;
            }
            x++;

            //Properties
            while (rtb.Lines[x].IndexOf("True") < 0 && rtb.Lines[x].IndexOf("False") < 0 && rtb.Lines[x].IndexOf("{") < 0)
            {
                if (rtb.Lines[x] != "")
                {
                    string[] tempArr = rtb.Lines[x].Split(' ');
                    switch (tempArr[0])
                    {
                        case ".Text":
                            string propText = "";
                            for (int y = 1; y < tempArr.Length; y++)
                                propText += tempArr[y] + " ";
                            propText = misc.sLeft(propText, propText.Length - 1);
                            retControlArr[index].Text = propText;
                            break;
                        case ".Name":
                            retControlArr[index].Name = tempArr[1];
                            break;
                        case ".Location":
                            int pntX = int.Parse(tempArr[1].Replace(",", ""));
                            int pntY = int.Parse(tempArr[2].Replace(",", ""));
                            System.Drawing.Point pnt = new System.Drawing.Point(pntX, pntY);
                            retControlArr[index].Location = pnt;
                            break;
                    }
                }
                x++;
            }


            //Function
            if (rtb.Lines[x].IndexOf("True") >= 0)
                stateTrue = true;
            else if (rtb.Lines[x].IndexOf("False") >= 0)
                stateTrue = false;
            else
                stateTrue = true;

                int cnt = 0;
                while (cnt < rtb.Lines.Length)
                {
                    if (cnt != 0)
                        x = cnt;
                    //Parse each line
                    for (cnt = x; cnt < rtb.Lines.Length; cnt++)
                    {
                        if (rtb.Lines[cnt] == "" || rtb.Lines[cnt][0] == '}')
                            goto CheckState;

                        string[] strArr = rtb.Lines[cnt].Split(' ');
                        if (strArr != null && strArr.Length > 0)
                        {
                            //Parse type
                            if (stateTrue)
                                ParseScriptType(ref funcCtrArray[index].sTrue, strArr, rtb.Lines[cnt]);
                            else
                                ParseScriptType(ref funcCtrArray[index].sFalse, strArr, rtb.Lines[cnt]);
                        }

                    CheckState:
                        if (rtb.Lines[cnt].IndexOf("True") >= 0 && stateTrue == false)
                        {
                            cnt++;
                            stateTrue = true;
                            break;
                        }
                        else if (rtb.Lines[cnt].IndexOf("False") >= 0 && stateTrue)
                        {
                            cnt++;
                            stateTrue = false;
                            break;
                        }
                    }
                }
        }

        private static void ParseScriptType(ref PluginScript pstFunc, string[] strArr, string line)
        {
            int ind = 0;
            int x = 0;
            switch (strArr[0])
            {
                case "byte[]":  //Byte Array
                    ind = pstFunc.pbA.Length;
                    Array.Resize(ref pstFunc.pbA, pstFunc.pbA.Length + 1);
                    pstFunc.pbA[ind].byteA = StringBAToBA(line);

                    pstFunc.pbA[ind].bName = strArr[1];
                    break;
                case "ulong":   //Ulong
                    ind = pstFunc.pulA.Length;
                    Array.Resize(ref pstFunc.pulA, pstFunc.pulA.Length + 1);

                    if (strArr[3].IndexOf("0x") >= 0)
                        pstFunc.pulA[ind].ulongA = (ulong)Convert.ToUInt64(strArr[3].Replace(";", ""), 16);
                    else
                        pstFunc.pulA[ind].ulongA = (ulong)Convert.ToUInt64(strArr[3].Replace(";", ""), 10);

                    pstFunc.pulA[ind].ulName = strArr[1];
                    break;
                case "Add":     //Adds arg2 with arg3 and stores the result in arg1
                case "Div":     //Divides arg2 by arg3 and stores the result in arg1
                case "Mult":    //Multiplies arg2 with arg3 and stores the result in arg1
                case "Sub":     //Subtracts arg3 from arg2 and stores the result in arg1
                case "GetMemU": //Get memory and return the ulong of it
                case "GetMem":  //Get memory
                case "SetMem":  //Set memory
                    ind = pstFunc.funcCalls.Length;
                    Array.Resize(ref pstFunc.funcCalls, pstFunc.funcCalls.Length + 1);
                    pstFunc.funcCalls[ind].fName = strArr[0];
                    pstFunc.funcCalls[ind].argNames = new string[strArr.Length - 1];
                    for (x = 0; x < (strArr.Length - 1); x++)
                        pstFunc.funcCalls[ind].argNames[x] = strArr[x + 1].Replace(",", "");
                    break;
                default:        //Sets a variable to something
                    if (strArr.Length >= 4 && strArr[3].IndexOf("byte[") >= 0)
                    {
                        ind = pstFunc.pbA.Length;
                        Array.Resize(ref pstFunc.pbA, pstFunc.pbA.Length + 1);
                        pstFunc.pbA[ind].byteA = StringBAToBA(line);
                        pstFunc.pbA[ind].bName = strArr[1];
                    }
                    else if (strArr.Length >= 4 && strArr[3].IndexOf("ulong") >= 0)
                    {
                        ind = pstFunc.pulA.Length;
                        Array.Resize(ref pstFunc.pulA, pstFunc.pulA.Length + 1);

                        if (strArr[3].IndexOf("0x") >= 0)
                            pstFunc.pulA[ind].ulongA = (ulong)Convert.ToUInt64(strArr[3].Replace(";", ""), 16);
                        else
                            pstFunc.pulA[ind].ulongA = (ulong)Convert.ToUInt64(strArr[3].Replace(";", ""), 10);

                        pstFunc.pulA[ind].ulName = strArr[1];
                    }
                    else if (strArr.Length >= 3 && pstFunc.funcCalls != null)
                    {
                        if (strArr[1] == "=")
                        {
                            ind = pstFunc.funcCalls.Length;
                            Array.Resize(ref pstFunc.funcCalls, pstFunc.funcCalls.Length + 1);
                            pstFunc.funcCalls[ind].fName = "SetVar";
                            pstFunc.funcCalls[ind].argNames = new string[1];
                            pstFunc.funcCalls[ind].argNames[0] = line.Trim();
                        }
                    }
                    //else if (pstFunc.funcCalls != null) //
                    break;
            }
        }

        private static byte[] StringBAToBA(string byteArr)
        {
            byteArr = byteArr.Trim();
            //byte[] NAME
            if (byteArr.Split(' ').Length == 2)
                return null;

            //byte[] NAME = new byte[SIZE]
            string[] arr = byteArr.Split(' ');
            if (arr.Length == 5 && arr[3].ToLower() == "new")
            {
                string strSize = arr[4].Replace("byte[", "").Replace("]", "");
                int size = 0;
                if (strSize.IndexOf("0x") >= 0)
                    size = Convert.ToInt32(strSize, 16);
                else
                    size = Convert.ToInt32(strSize, 10);
                return new byte[size];
            }
            else if (arr.Length == 4 && arr[2].ToLower() == "new")
            {
                string strSize = arr[3].Replace("byte[", "").Replace("]", "").Replace(";", "");
                int size = 0;
                if (strSize.IndexOf("0x") >= 0)
                    size = Convert.ToInt32(strSize, 16);
                else
                    size = Convert.ToInt32(strSize, 10);
                return new byte[size];
            }

            //byte[] NAME = { BYTES }
            byteArr = misc.sRight(byteArr, byteArr.Length - byteArr.IndexOf('{') - 1);
            byteArr = byteArr.Remove(byteArr.IndexOf("}"));
            string[] array = byteArr.Replace(",", "").Trim().Split(' ');
            byte[] ret = new byte[array.Length];

            for (int x = 0; x < array.Length; x++)
            {
                if (array[x].IndexOf("0x") >= 0)
                    ret[x] = Convert.ToByte(array[x], 16);
                else
                    ret[x] = Convert.ToByte(array[x], 10);
            }
            return ret;
        }

        private static void RunFunc(ref PluginScript func)
        {
            ulong arg1 = 0;
            byte[] arg2 = null;
            //int y = 0;

            for (int x = 0; x < func.funcCalls.Length; x++)
            {
                switch (func.funcCalls[x].fName)
                {
                    case "SetVar":
                        //Check if the the destination variable is defined
                        string[] strArr = func.funcCalls[x].argNames[0].Split(' ');
                        string destVar = strArr[0];
                        int found = 0;
                        int z = 0;
                        for (z = 0; z < func.pbA.Length; z++)
                        {
                            if (destVar == func.pbA[z].bName)
                            {
                                found = 1;
                                break;
                            }
                        }
                        if (found == 0)
                        {
                            for (z = 0; z < func.pulA.Length; z++)
                            {
                                if (destVar == func.pulA[z].ulName)
                                {
                                    found = 2;
                                    break;
                                }
                            }
                        }
                        //Variable is defined
                        if (found != 0)
                        {
                            /*
                            PluginScript temp = new PluginScript();
                            temp.pbA = new PluginByteA[0];
                            temp.pulA = new PluginULongA[0];
                            ParseScriptType(ref temp, strArr, func.funcCalls[x].argNames[0]);
                            RunFunc(ref temp);
                            if (found == 1)
                                func.pbA[z].byteA = temp.pbA[0].byteA;
                            else
                                func.pulA[z].ulongA = temp.pulA[0].ulongA;
                            */
                        }

                        break;
                    case "SetMem":
                        ParseMemRegU(ref arg1, func, x, 0);
                        ParseMemRegB(ref arg2, func, x, 1);

                        Form1.apiSetMem(arg1, arg2);
                        break;
                    case "GetMem":
                        int b = ParseMemRegU(ref arg1, func, x, 0);
                        ParseMemRegB(ref arg2, func, x, 1);

                        Form1.apiGetMem(arg1, ref arg2);
                        func.pbA[b].byteA = arg2;
                        break;
                    case "GetMemU":
                        ulong gmuArg2 = 0;
                        ParseMemRegU(ref arg1, func, x, 0);
                        int e = ParseMemRegU(ref gmuArg2, func, x, 1);
                        int size = 0;
                        if (func.funcCalls[x].argNames[2].IndexOf("0x") >= 0)
                            size = Convert.ToInt32(func.funcCalls[x].argNames[2], 16);
                        else
                            size = Convert.ToInt32(func.funcCalls[x].argNames[2], 10);

                        Form1.apiGetMemU(arg1, ref gmuArg2, size);
                        func.pulA[e].ulongA = gmuArg2;
                        break;
                    case "Add":
                    case "Div":
                    case "Mult":
                    case "Sub":
                        ulong mathArg2 = 0;
                        int off = ParseMemRegU(ref mathArg2, func, x, 0);
                        ParseMemRegU(ref arg1, func, x, 1);
                        ParseMemRegU(ref mathArg2, func, x, 2);

                        switch (func.funcCalls[x].fName)
                        {
                            case "Add":
                                func.pulA[off].ulongA = arg1 + mathArg2;
                                break;
                            case "Div":
                                func.pulA[off].ulongA = arg1 / mathArg2;
                                break;
                            case "Mult":
                                func.pulA[off].ulongA = arg1 * mathArg2;
                                break;
                            case "Sub":
                                func.pulA[off].ulongA = arg1 - mathArg2;
                                break;
                        }
                        break;
                }

            }
        }

        private static int ParseMemRegU(ref ulong arg1, PluginScript func, int x, int off)
        {
            int y = 0;
            while (y < func.pulA.Length && func.pulA[y].ulName != func.funcCalls[x].argNames[off])
                y++;
            if (y == func.pulA.Length)
                arg1 = (ulong)misc.ParseVal(func.funcCalls[x].argNames[off], 1);
            else
                arg1 = func.pulA[y].ulongA;

            return y;
        }

        private static int ParseMemRegB(ref byte[] arg1, PluginScript func, int x, int off)
        {
            int y = 0;
            while (y < func.pbA.Length && func.pbA[y].bName != func.funcCalls[x].argNames[off])
                y++;
            if (y == func.pbA.Length)
                arg1 = BitConverter.GetBytes(misc.ParseVal(func.funcCalls[x].argNames[off], 1));
            else
                arg1 = func.pbA[y].byteA;

            return y;
        }

        public static void DoEventCheckBox(object sender, EventArgs e)
        {
            CheckBox a = (CheckBox)sender;
            if (a.Checked)
                RunFunc(ref funcCtrArray[(int)a.Tag].sTrue);
            else
                RunFunc(ref funcCtrArray[(int)a.Tag].sFalse);
        }

        public static void DoEventButton(object sender, EventArgs e)
        {
            RunFunc(ref funcCtrArray[(int)((Button)sender).Tag].sTrue);
        }

    }
}
