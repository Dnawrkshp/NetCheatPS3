using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace NetCheatPS3
{
    class codes
    {

        public static string[] splitList;
        public static int cnt = 0;
        public static List<ConstCode> ConstCodes = new List<ConstCode>();

        //Struct for the API to constant write codes
        public struct ConstCode
        {
            public Form1.CodeDB Codes;  //Parsed codes to run
            public uint ID;             //ID to distinguish (used for removing ConstCodes)
        }

        public static Form1.ncCodeType[] ncCodeTypes = new Form1.ncCodeType[0];

        static int connected = 0;

        #region Constant Writing Thread

        /*
         * Constantly writes codes by looping through each list and calling WriteToPS32 to run them
         */
        public static void BeginConstWriting()
        {
            bool sleepThread = false;
            while (true)
            {
                if (!Form1.connected)
                    connected = 0;
                if (Form1.connected && connected == 0)
                    connected = ConnectPS3();
                if (connected == 1 && Form1.attached)
                    connected = AttachPS3();

                if (Form1.ConstantLoop == 1 && connected == 2)
                {
                    sleepThread = false;
                    for (int x = 0; x <= Form1.CodesCount; x++)
                    {
                        //Plugin codes
                        for (int y = 0; y < ConstCodes.Count; y++)
                        {
                            try
                            {
                                //Make sure the code is valid
                                if (ConstCodes[y].Codes.state)
                                    WriteToPS32(ConstCodes[y].Codes, true);
                                sleepThread |= ConstCodes[y].Codes.state;
                            }
                            catch
                            {

                            }
                        }

                        //User codes
                        if (x < Form1.Codes.Count)
                        {
                            sleepThread |= Form1.Codes[x].state;
                            if (Form1.Codes[x].state == true && Form1.ConstantLoop == 1)
                                WriteToPS32(Form1.Codes[x], true);
                        }
                        else
                            sleepThread = true;
                    }
                }

                if (!sleepThread || connected == 0)
                    System.Threading.Thread.Sleep(1000);
            }
        }

        public static bool ConnectAndAttach(bool stopProc = false)
        {
            int connect = codes.ConnectPS3();
            if (connect == 1)
                connect = codes.AttachPS3(stopProc);

            if (connect != 2)
                return false;
            else
                return true;
        }

        public static int ConnectPS3()
        {
            try
            {
                if (Form1.curAPI.Instance.Connect())
                    return 1;
            }
            catch
            {
            }

            return 0;
        }

        public static int AttachPS3(bool stopProc = false)
        {
            //if (Form1.PS3.AttachProcess)
            if (Form1.curAPI.Instance.Attach())
                return 2;

            return 0;
        }

        /*
         * Adds a code to the ConstCodes array
         * Returns the ID of the code
         */
        static uint IDCounter = 0;
        public static uint ConstCodeAdd(string code, bool state)
        {
            //int ind = ConstCodes.Count;
            uint ID = IDCounter;
            IDCounter++;

            //Make room for the new ConstCode
            //Array.Resize(ref ConstCodes, ind + 1);

            //Set the values
            ConstCode c = new ConstCode();
            c.Codes.codes = code;
            c.Codes.state = state;

            //ConstCodes[ind].Codes.codes = code;
            //ConstCodes[ind].Codes.state = state;
            c.Codes = ParseCodeString(code, c.Codes);
            //ConstCodes[ind].Codes = code;
            c.ID = ID;
            ConstCodes.Add(c);

            //If the state is true make sure that ConstantWriting is on
            if (state)
                Form1.ConstantLoop = 1;

            //Return the ID
            return ID;
        }

        /*
         * Deletes the code with an ID equal to ID
         */
        public static void ConstCodeRemove(uint ID)
        {
            int ind = 0;
            //Find the code
            for (ind = 0; ind < ConstCodes.Count; ind++)
                if (ConstCodes[ind].ID == ID)
                    break;
            //If the code hasn't been found exit
            if (ind == ConstCodes.Count)
                return;

            //Code found so disable constant writing
            int cLoop = Form1.ConstantLoop;
            Form1.ConstantLoop = 0;

            ConstCodes.RemoveAt(ind);

            Form1.ConstantLoop = cLoop;
        }

        /*
         * Sets the code with an ID of ID's state to state
         */
        public static void ConstCodeSetState(uint ID, bool state)
        {
            for (int ind = 0; ind < ConstCodes.Count; ind++)
                if (ConstCodes[ind].ID == ID)
                {
                    ConstCode c = ConstCodes[ind];
                    c.Codes.state = state;
                    ConstCodes[ind] = c;
                    if (state)
                        Form1.ConstantLoop = 1;
                    break;
                }
        }

        /*
         * Gets the code with an ID of ID's state
         */
        public static bool ConstCodeGetState(uint ID)
        {
            for (int ind = 0; ind < ConstCodes.Count; ind++)
                if (ConstCodes[ind].ID == ID)
                    return ConstCodes[ind].Codes.state;
            return false;
        }

        #endregion

        /*
         * Loops through each code (if it is a write) it grabs what the data is
         */
        public static Form1.ncCode[] CreateBackupPS3(Form1.CodeDB Codes)
        {
            if (Codes.CData == null)
                return new Form1.ncCode[0];

            Form1.ncCode[] ret = new Form1.ncCode[0];
            int isPointerWrite = 0;
            ulong arg1 = 0;

            foreach (Form1.ncCode nC in Codes.CData)
            {
                if (nC.codeType == '0' || nC.codeType == '1' || nC.codeType == '2')
                {
                    Array.Resize(ref ret, ret.Length + 1);
                    int ind = ret.Length - 1;
                    ret[ind] = new Form1.ncCode();
                    ret[ind].codeType = nC.codeType;
                    ret[ind].codeArg0 = nC.codeArg0;
                    if (isPointerWrite == 1)
                    {
                        ret[ind].codeArg1 = arg1;
                        isPointerWrite = 0;
                    }
                    else if (isPointerWrite == 2)
                    {
                        byte[] arg1_BA = BitConverter.GetBytes((uint)arg1);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(arg1_BA);
                        ret[ind].codeArg2 = arg1_BA;
                        isPointerWrite = 0;
                    }
                    else
                        ret[ind].codeArg1 = nC.codeArg1;
                    ret[ind].codeArg2 = new byte[nC.codeArg2.Length];
                    Form1.apiGetMem(ret[ind].codeArg1, ref ret[ind].codeArg2);

                }
                else if (nC.codeType == '6')
                {
                    byte[] arg1_BA = BitConverter.GetBytes((uint)arg1);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(arg1_BA);

                    byte[] addr = new byte[4];
                    ulong arg1Addr = (isPointerWrite == 1) ? arg1 : nC.codeArg1;
                    byte[] arg2Off = (isPointerWrite == 2) ? arg1_BA : nC.codeArg2;
                    Form1.apiGetMem(arg1Addr, ref addr);
                    if (!Form1.doFlipArray)
                        Array.Reverse(addr);
                    ulong newAddrUL = BitConverter.ToUInt32(addr, 0);
                    newAddrUL += misc.ByteArrayToLong(arg2Off, 0, arg2Off.Length);
                    arg1 = newAddrUL;
                    isPointerWrite = ((nC.codeArg0 == 1) ? 2 : 1);
                }
            }

            return ret;
        }

        /*
         * Loops through each code and calls ProcPreProcCode to run them
         */
        public static void WriteToPS32(Form1.CodeDB Codes, bool isCWrite = false)
        {
            if (Codes.codes == null || Codes.CData == null)
                return;
            //splitList = Form1.Codes[ind].codes.Split('\n');
            int skip = 0;

            for (int x = 0; x < Codes.CData.Length; x++)
            {
                if (skip > 0)
                {
                    skip--;
                    goto SkipCodes;
                }
                Form1.ncCodeType tempCT = ncCodeTypes.Where(i => i.Command == Codes.CData[x].codeType).FirstOrDefault();
                if (tempCT.Command != '\0')
                    skip = tempCT.ExecCode(x, ref Codes, isCWrite);

            SkipCodes: ;
            }
        }

        /*
         * Runs codes that have already been processed
         */
        /*
        public static int ProcPreProcCode(Form1.CodeDB Codes, int cnt)
        {
            ulong addr = Codes.CData[cnt].addr;
            byte[] val = Codes.CData[cnt].val;
            byte[] jval = Codes.CData[cnt].jbool;
            char type = Codes.CData[cnt].type;
            int skip = 0;

            //Make upper case
            if (type >= 0x61)
                type -= (char)0x20;

            switch (type)
            {
                case '0':
                    byte[] stw0 = new byte[1];
                    stw0[0] = val[val.Length - 1];
                    Form1.apiSetMem(addr, stw0);
                    break;
                case '1':
                    byte[] stw1 = new byte[2];
                    stw1[0] = val[val.Length - 2];
                    stw1[1] = val[val.Length - 1];
                    Form1.apiSetMem(addr, val);
                    break;
                case '2':
                    Form1.apiSetMem(addr, val);
                    //PS3TMAPI.ProcessSetMemory(0, PS3TMAPI.UnitType.PPU, Form1.ProcessID, 0, addr, val);
                    break;
                case '6':
                    byte[] pretByte = new byte[4];
                    Form1.apiGetMem(addr, ref pretByte);
                    //apiGetMem(addr, ref pretByte);

                    Codes.CData[cnt + 1].addr = (misc.ByteArrayToLong(pretByte, 0, 4) + misc.ByteArrayToLong(val, 0, 4));
                    //ProcPreProcCode(Codes, cnt + 1);
                    break;
                case 'D': case 'E':
                    int size = Codes.CData[cnt].jsize;
                    skip = size;

                    byte[] retByte = new byte[0x4];
                    Form1.apiGetMem(addr, ref retByte);
                    //apiGetMem(addr, ref retByte);

                    bool ret = false;
                    if (type == 'D')
                        ret = misc.ArrayCompare(jval, retByte, new byte[1], 4, 0, 0, Form1.compEq);
                    else if (type == 'E')
                        ret = misc.ArrayCompare(jval, retByte, new byte[1], 4, 0, 0, Form1.compANEq);

                    if (ret == false)
                        break;

                    for (int x = (cnt + 1); x < (size + cnt + 1); x++)
                        ProcPreProcCode(Codes, x);
                    break;
            }
            
            return skip;
        }
         */

        /*
         * Obsolete
         * Now using Regular Expressions to remove comments
         */
        public static int CheckForComment(String text)
        {

            //Comment checks
            if (text.Length < 19)
                return 1;

            splitList[cnt] = misc.sLeft(text, 19);

            //There has to be a space there
            if (text[10] != ' ' || text[1] != ' ')
                return 1;

            //Check each character
            int spCnt = 0;
            foreach (char c in splitList[cnt].ToUpper())
            {
                if (((c < 0x30 || c > 0x46) && c != ' ') || spCnt > 2)
                    return 1;
                if (c == ' ')
                    spCnt++;
            }

            return 0;
        }

        public static bool isCodeValid(Form1.ncCode c)
        {
            if (c.codeType != null && c.codeType != '\0') //valid code
                if (c.codeArg2 != null && c.codeArg2.Length != 0)
                    if (c.codeArg1_BA != null && c.codeArg1_BA.Length != 0)
                        return true;
            return false;
        }

        public static Form1.ncCode[] ParseCodeStringFull(string data)
        {
            //Remove comments
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            data = System.Text.RegularExpressions.Regex.Replace(data, re, "$1");

            if (data == "" || data == null)
                return new Form1.ncCode[1];

            Form1.CodeDB db = new Form1.CodeDB();
            db = ParseCodeString(data, db);

            if (db.CData == null || db.CData.Length == 0)
                return new Form1.ncCode[1];
            return db.CData;
        }

        /*
         * Called whenever the CodeBox's event TextChanged is raised
         * Processes the codes
         */
        public static void UpdateCData(string data, int ind)
        {
            //Remove comments
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            data = System.Text.RegularExpressions.Regex.Replace(data, re, "$1");
            Form1.Codes[ind] = ParseCodeString(data, Form1.Codes[ind]);
        }

        /*
         * Processes codes in the string array splitList
         */
        public static Form1.CodeDB ParseCodeString(string data, Form1.CodeDB db)
        {
            if (data == null)
            {
                return db;
            }

            Form1.CodeDB ret = db;

            splitList = data.Split('\n');
            ret.CData = new Form1.ncCode[0];

            foreach (string s in splitList)
            {
                if (s != "" && s != "\r" && s.Split(' ').Length > 2)
                {
                    Form1.ncCode temp = new Form1.ncCode();
                    Form1.ncCodeType tempCT = ncCodeTypes.Where(i => i.Command == s[0]).FirstOrDefault();
                    if (tempCT.Command != '\0')
                        temp = tempCT.ParseCode(s.Replace("\r", ""));
                    if (temp.codeArg2 != null)
                    {
                        Array.Resize(ref ret.CData, ret.CData.Length + 1);
                        ret.CData[ret.CData.Length - 1] = temp;
                    }
                }
            }

            return ret;
        }

        /*
         * Processes joker code types (both D and E type)
         */
        /*
        public static int ParseJokerD(string[] splitList, int cnt, ref Form1.CodeDB Code, int ind2)
        {
            String code = misc.sLeft(splitList[cnt], 19);
            String val = misc.sRight(code, 8);
            if (val.IndexOf(' ') >= 0)
                return 0;
            Code.CData[ind2].jsize = int.Parse(misc.sLeft(val, 2), NumberStyles.HexNumber);
            Array.Resize(ref Code.CData, Code.CData.Length + Code.CData[ind2].jsize);
            int x = 0, y = 0, ret = Code.CData[ind2].jsize;

            Code.CData[ind2].jbool = BitConverter.GetBytes(int.Parse(misc.ReverseE("00" + misc.sRight(val, 6), 8), NumberStyles.HexNumber));

            ind2++;
            for (x = 0; x < ret; x++)
            {
                do
                {
                    y++;
                    if ((cnt + y) >= splitList.Length)
                        return ind2;
                    code = misc.sLeft(splitList[cnt + y], 19);
                } while (code == null || code == "" || CheckForComment(code) == 1);

                int len = 0;
                Code.CData[ind2].type = code[0];

                switch (Code.CData[ind2].type)
                {
                    case '0':
                        len = 2;
                        break;
                    case '1':
                        len = 4;
                        break;
                    case '2': case '6': case 'D': case 'E':
                        len = 8;
                        break;
                }

                Code.CData[ind2].addr = ulong.Parse(misc.sMid(code, 2, 8), NumberStyles.HexNumber);
                Code.CData[ind2].val = BitConverter.GetBytes(int.Parse(misc.ReverseE(misc.sRight(code, len), 8), NumberStyles.HexNumber));
                ind2++;
            }

            return ind2;
        }
         */


        #region CodeType Parsers

        public static Form1.ncCode parseTextCode(string code)
        {
            Form1.ncCode ret = parseCode(code);
            if (code == null || code == "" || ret.codeArg2 == null)
                return new Form1.ncCode();

            string[] codeArr = code.Split(' ');
            string writeStr = "";
            for (int off = 2; off < codeArr.Length; off++)
                writeStr += codeArr[off] + " ";
            writeStr = writeStr.TrimEnd(' ');
            ret.codeArg2 = misc.StringToByteArray(writeStr);
            Array.Resize(ref ret.codeArg2, ret.codeArg2.Length + 1);

            return ret;
        } 

        public static Form1.ncCode parseCode(string code)
        {
            Form1.ncCode ret = new Form1.ncCode();
            if (code == null || code == "")
                return ret;

            string[] codeArr = code.Split(' ');
            for (int x = 0; x < 3; x++)
            {
                if (codeArr[x] != "")
                {
                    try
                    {
                        switch (x)
                        {
                            case 0:
                                ret.codeType = codeArr[0][0];
                                if (codeArr[0].Length > 1)
                                    ret.codeArg0 = uint.Parse(misc.sRight(codeArr[0], codeArr[0].Length - 1), System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 1:
                                try
                                {
                                    ret.codeArg1 = ulong.Parse(codeArr[1], System.Globalization.NumberStyles.HexNumber);
                                }
                                catch { }
                                ret.codeArg1_BA = misc.StringBAToBA(codeArr[1]);
                                break;
                            case 2:
                                switch (ret.codeType)
                                {
                                    case '1':
                                        ret.codeArg2 = new byte[0];
                                        break;
                                    case '2':
                                        Single flt = Single.Parse(codeArr[2]);
                                        ret.codeArg2 = BitConverter.GetBytes(flt);
                                        Array.Reverse(ret.codeArg2);
                                        break;
                                    default:
                                        ret.codeArg2 = misc.StringBAToBA(codeArr[2]);
                                        break;
                                }
                                break;
                        }
                    }
                    catch { }
                }
            }

            if (ret.codeType != '\0' && ret.codeArg2 != null)
                return ret;
            else
                return new Form1.ncCode();
        }

        public static int execByteWrite(int cnt, ref Form1.CodeDB cDB, bool isCWrite)
        {
            byte[] t;
            int x = 0;

            switch (cDB.CData[cnt].codeArg0)
            {
                case 1: //OR
                    t = new byte[cDB.CData[cnt].codeArg2.Length];
                    Form1.apiGetMem(cDB.CData[cnt].codeArg1, ref t);
                    for (x = 0; x < t.Length; x++)
                        t[x] |= cDB.CData[cnt].codeArg2[x];

                    Form1.apiSetMem(cDB.CData[cnt].codeArg1, t);
                    break;
                case 2: //AND
                    t = new byte[cDB.CData[cnt].codeArg2.Length];
                    Form1.apiGetMem(cDB.CData[cnt].codeArg1, ref t);
                    for (x = 0; x < t.Length; x++)
                        t[x] &= cDB.CData[cnt].codeArg2[x];

                    Form1.apiSetMem(cDB.CData[cnt].codeArg1, t);
                    break;
                case 3: //XOR
                    t = new byte[cDB.CData[cnt].codeArg2.Length];
                    Form1.apiGetMem(cDB.CData[cnt].codeArg1, ref t);
                    for (x = 0; x < t.Length; x++)
                        t[x] ^= cDB.CData[cnt].codeArg2[x];

                    Form1.apiSetMem(cDB.CData[cnt].codeArg1, t);
                    break;
                default:
                    Form1.apiSetMem(cDB.CData[cnt].codeArg1, cDB.CData[cnt].codeArg2);
                    break;
            }

            return 0;
        }

        public static int execMultilineCondensed(int cnt, ref Form1.CodeDB cDB, bool isCWrite)
        {
            //Requires 2 lines
            if ((cnt + 1) >= cDB.CData.Length)
                return 0;

            string vincStr = cDB.CData[cnt + 1].codeArg0.ToString("X");
            byte[] vinc = misc.StringBAToBA(vincStr.PadLeft((vincStr.Length % 2) + vincStr.Length, '0'));
            byte[] tWrite = new byte[cDB.CData[cnt].codeArg2.Length];
            Array.Copy(cDB.CData[cnt].codeArg2, tWrite, tWrite.Length);

            ulong addr = cDB.CData[cnt].codeArg1;
            ulong max = misc.ByteArrayToLong(cDB.CData[cnt + 1].codeArg2, 0, cDB.CData[cnt + 1].codeArg2.Length);


            for (ulong x = 0; x < max; x++)
            {
                Form1.apiSetMem(addr + (x * cDB.CData[cnt + 1].codeArg1), tWrite);
                if (cDB.CData[cnt + 1].codeArg0 != 0)
                    tWrite = misc.addBA(tWrite, vinc);
            }

            return 1;
        }

        public static int execPointerExecute(int cnt, ref Form1.CodeDB cDB, bool isCWrite)
        {
            byte[] newAddrBA = new byte[4];
            Form1.apiGetMem(cDB.CData[cnt].codeArg1, ref newAddrBA);
            if (Form1.doFlipArray)
                Array.Reverse(newAddrBA);
            ulong newAddrUL = BitConverter.ToUInt32(newAddrBA, 0);
            newAddrUL += misc.ByteArrayToLong(cDB.CData[cnt].codeArg2, 0, cDB.CData[cnt].codeArg2.Length);

            if (cDB.CData[cnt].codeArg0 == 0)
            {
                if (cnt + 1 < cDB.CData.Length)
                    cDB.CData[cnt + 1].codeArg1 = newAddrUL;
            }
            else
            {
                byte[] arg2_BA = BitConverter.GetBytes((uint)newAddrUL);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(arg2_BA);
                if (cnt + 1 < cDB.CData.Length)
                    cDB.CData[cnt + 1].codeArg2 = arg2_BA;
            }
            return 0;
        }

        public static int execEQConditionalExecute(int cnt, ref Form1.CodeDB cDB, bool isCWrite)
        {
            bool execute = false;
            byte[] cmp = new byte[cDB.CData[cnt].codeArg2.Length];
            Form1.apiGetMem(cDB.CData[cnt].codeArg1, ref cmp);
            if (!Form1.doFlipArray)
                Array.Reverse(cmp);
            execute = misc.ArrayCompare(cmp, cDB.CData[cnt].codeArg2, null, Form1.compEq);

            if (!execute)
                return (int)cDB.CData[cnt].codeArg0;
            else
                return 0;
        }

        public static int execMUConditionalExecute(int cnt, ref Form1.CodeDB cDB, bool isCWrite)
        {
            bool execute = false;
            byte[] cmp = new byte[cDB.CData[cnt].codeArg2.Length];
            Form1.apiGetMem(cDB.CData[cnt].codeArg1, ref cmp);
            execute = misc.ArrayCompare(cDB.CData[cnt].codeArg2, cmp, null, Form1.compANEq);

            if (!execute)
                return (int)cDB.CData[cnt].codeArg0;
            else
                return 0;
        }

        public static int execCopyBytes(int cnt, ref Form1.CodeDB cDB, bool isCWrite)
        {
            byte[] copied = new byte[cDB.CData[cnt].codeArg0];
            Form1.apiGetMem(cDB.CData[cnt].codeArg1, ref copied);
            if (!Form1.doFlipArray)
                Array.Reverse(copied);
            Form1.apiSetMem(misc.ByteArrayToLong(cDB.CData[cnt].codeArg2, 0, 4), copied);
            return 0;
        }

        private static byte[] copiedBytes;
        public static int execCopyPasteBytes(int cnt, ref Form1.CodeDB cDB, bool isCWrite)
        {
            uint type = cDB.CData[cnt].codeArg0;

            if (type == 1) //copy
            {
                ulong cSize = misc.ByteArrayToLong(cDB.CData[cnt].codeArg2, 0, cDB.CData[cnt].codeArg2.Length);
                copiedBytes = new byte[cSize];
                Form1.apiGetMem(cDB.CData[cnt].codeArg1, ref copiedBytes);
                if (!Form1.doFlipArray)
                    Array.Reverse(copiedBytes);
            }
            else if (type == 2) //paste
            {
                if (copiedBytes == null)
                    return 0;

                Form1.apiSetMem(cDB.CData[cnt].codeArg1, copiedBytes);
            }

            return 0;
        }

        public static int execFindReplace(int cnt, ref Form1.CodeDB cDB, bool isCWrite)
        {
            if ((cnt + 1) >= cDB.CData.Length)
                return 0;
            byte[] ogp = cDB.CData[cnt + 1].codeArg1_BA;
            byte[] cop = cDB.CData[cnt + 1].codeArg2;
            ulong[] addrs = FindReplace_SearchMemory(cDB.CData[cnt].codeArg1,
                misc.ByteArrayToLong(cDB.CData[cnt].codeArg2, 0, 4),
                1,
                ogp,
                (int)cDB.CData[cnt].codeArg0);

            for (int x = 0; x < addrs.Length; x++)
            {
                if (!isCWrite)
                    Form1.FRManager.AddItem((uint)addrs[x], ogp, cop);
                Form1.apiSetMem(addrs[x], cop);
            }

            return 1;
        }

        static ulong[] FindReplace_SearchMemory(ulong start, ulong stop, int align, byte[] input, int maxResults)
        {
            if (align <= 0)
                align = 1;

            ulong[] ret = new ulong[0];
            ulong size = 0x10000;

            for (ulong x = start; x < stop; x += size)
            {
                if (x < start || x > stop)
                    break;

                //Get address based off of range
                x = misc.ParseSchAddr(x);

                byte[] mem = Global.Plugins.GetMemory(x, (int)size);
                for (int y = 0; y < (int)size - (input.Length - align); y += align)
                {
                    if ((x + (ulong)y) >= stop)
                        return ret;

                    if ((x + (ulong)y) >= 0x00CFFFFC)
                    {

                    }

                    byte[] temp = new byte[input.Length];
                    Array.Copy(mem, y, temp, 0, input.Length);
                    if (misc.ArrayCompare(temp, input, null, Form1.compEq))
                    {
                        Array.Resize(ref ret, ret.Length + 1);
                        ret[ret.Length - 1] = x + (ulong)y;

                        if (maxResults > 0 && ret.Length >= maxResults)
                            return ret;
                    }
                }
                x -= (ulong)(input.Length - align);

                //System.Windows.Forms.Application.DoEvents();
            }

            return ret;
        }

        #endregion

    }
}
