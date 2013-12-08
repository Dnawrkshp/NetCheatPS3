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
        public static ConstCode[] ConstCodes = new ConstCode[0];

        //Struct for the API to constant write codes
        public struct ConstCode
        {
            public Form1.CodeDB Codes;  //Parsed codes to run
            public uint ID;             //ID to distinguish (used for removing ConstCodes)
        }

        static int connected = 0;

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

                if (Form1.ConstantLoop == 1 && (Form1.apiDLL == 0 || connected == 2))
                {
                    sleepThread = false;
                    for (int x = 0; x <= Form1.CodesCount; x++)
                    {
                        //Plugin codes
                        for (int y = 0; y < ConstCodes.Length; y++)
                        {
                            try
                            {
                                //Make sure the code is valid
                                if (ConstCodes[y].Codes.state)
                                    WriteToPS32(ConstCodes[y].Codes);
                                sleepThread |= ConstCodes[y].Codes.state;
                            }
                            catch
                            {

                            }
                        }

                        //User codes
                        sleepThread |= Form1.Codes[x].state;
                        if (Form1.Codes[x].state == true && Form1.ConstantLoop == 1)
                            WriteToPS32(Form1.Codes[x]);
                    }
                }

                if (!sleepThread)
                    System.Threading.Thread.Sleep(1000);
            }
        }

        public static int ConnectPS3()
        {
            try
            {
                if (Form1.apiDLL == 0) //TMAPI
                {
                    if (Form1.PS3.ConnectTarget())
                        return 1;
                }
                else //CCAPI
                {
                    if (Form1.IPAddrStr != "" && Form1.PS3.ConnectTarget(Form1.IPAddrStr))
                        return 1;
                }
            }
            catch
            {
            }

            return 0;
        }

        public static int AttachPS3()
        {
            if (Form1.PS3.AttachProcess())
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
            int ind = ConstCodes.Length;
            uint ID = IDCounter;
            IDCounter++;

            //Make room for the new ConstCode
            Array.Resize(ref ConstCodes, ind + 1);

            //Set the values
            ConstCodes[ind].Codes.codes = code;
            ConstCodes[ind].Codes.state = state;
            ParseCodeString(code, ref ConstCodes[ind].Codes);
            //ConstCodes[ind].Codes = code;
            ConstCodes[ind].ID = ID;

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
            for (ind = 0; ind < ConstCodes.Length; ind++)
                if (ConstCodes[ind].ID == ID)
                    break;
            //If the code hasn't been found exit
            if (ind == ConstCodes.Length)
                return;

            //Code found so disable constant writing
            int cLoop = Form1.ConstantLoop;
            Form1.ConstantLoop = 0;

            for (int x = ind; x < (ConstCodes.Length - 1); x++)
            {
                ConstCodes[x].Codes = ConstCodes[x + 1].Codes;
                ConstCodes[x].ID = ConstCodes[x + 1].ID;
            }
            //Truncate the end
            Array.Resize(ref ConstCodes, ConstCodes.Length - 1);

            Form1.ConstantLoop = cLoop;
        }

        /*
         * Sets the code with an ID of ID's state to state
         */
        public static void ConstCodeSetState(uint ID, bool state)
        {
            for (int ind = 0; ind < ConstCodes.Length; ind++)
                if (ConstCodes[ind].ID == ID)
                {
                    ConstCodes[ind].Codes.state = state;
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
            for (int ind = 0; ind < ConstCodes.Length; ind++)
                if (ConstCodes[ind].ID == ID)
                    return ConstCodes[ind].Codes.state;
            return false;
        }

        /*
         * Loops through each code and calls ProcPreProcCode to run them
         */
        public static void WriteToPS32(Form1.CodeDB Codes)
        {
            if (Codes.codes == null || Codes.CData == null)
                return;
            //splitList = Form1.Codes[ind].codes.Split('\n');
            int skip = 0;

            for (int x = 0; x < Codes.CData.Length; x++)
            {

                if (Codes.CData[x].type == '\0')
                    break;

                if (skip > 0)
                {
                    skip--;
                    goto SkipCodes;
                }
                skip = ProcPreProcCode(Codes, x);

            SkipCodes: ;
            }
        }

        /*
         * Runs codes that have already been processed
         */
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

        /*
         * Called whenever the CodeBox's event TextChanged is raised
         * Processes the codes
         */
        public static void UpdateCData(string data, int ind)
        {
            //Remove comments
            var re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/";
            data = System.Text.RegularExpressions.Regex.Replace(data, re, "$1");
            ParseCodeString(data, ref Form1.Codes[ind]);
        }

        /*
         * Processes codes in the string array splitList
         */
        public static void ParseCodeString(string data, ref Form1.CodeDB ret)
        {
            int strcnt = 0, skip = 0, cnt = 0;
            splitList = data.Split('\n');
            ret.CData = new Form1.CodeData[0];

            foreach (string s in splitList)
            {

                int check = CheckForComment(splitList[strcnt]);
                if (check == 1 || Form1.bComment == true || splitList[strcnt] == "")
                    goto SkipUpdateCD;

                if (skip > 0)
                {
                    skip--;
                    goto SkipUpdateCD;
                }

                String code = misc.sLeft(splitList[strcnt], 19);
                String val = misc.sRight(code, 8);

                if (val.Length != 8 || code.Length != 19)
                    goto SkipUpdateCD;

                Array.Resize(ref ret.CData, ret.CData.Length + 1);

                if (cnt != 0 && ret.CData[cnt - 1].type != '6')
                    ret.CData[cnt].addr = ulong.Parse(misc.sMid(code, 2, 8), NumberStyles.HexNumber);
                else if (cnt == 0)
                    ret.CData[cnt].addr = ulong.Parse(misc.sMid(code, 2, 8), NumberStyles.HexNumber);
                ret.CData[cnt].type = splitList[strcnt][0];
                switch (ret.CData[cnt].type)
                {
                    case '0': //8 bit write
                        if (val.IndexOf(' ') >= 0)
                            break;
                        ret.CData[cnt].val = new byte[] { byte.Parse(misc.sLeft(val, 2), NumberStyles.HexNumber) };
                        break;
                    case '1': //16 bit write
                        if (val.IndexOf(' ') >= 0)
                            break;
                        byte[] sVal = BitConverter.GetBytes(int.Parse(misc.ReverseE(misc.sLeft(val, 4), 4), NumberStyles.HexNumber));
                        ret.CData[cnt].val = new byte[2] { sVal[0], sVal[1] };
                        break;
                    case '2': //32 bit write
                        if (val.IndexOf(' ') >= 0)
                            break;
                        ret.CData[cnt].val = BitConverter.GetBytes(int.Parse(misc.ReverseE(misc.sLeft(val, 8), 8), NumberStyles.HexNumber));
                        break;
                    case '6': //Pointer write
                        ret.CData[cnt].val = BitConverter.GetBytes(int.Parse(misc.ReverseE(misc.sLeft(val, 8), 8), NumberStyles.HexNumber));
                        int y = 1;

                        if (splitList.Length <= (cnt + y))
                            break;

                        //Skip comments and whatnot
                        check = CheckForComment(splitList[cnt + y]);
                        if (check == 1 || Form1.bComment == true)
                            y++;
                        else if (check == 2)
                        {
                            while (Form1.bComment == true)
                            {
                                y++;

                                if (splitList.Length <= (cnt + y))
                                    break;

                                CheckForComment(splitList[cnt + y]);
                            }
                        }

                        byte[] pretByte = new byte[0x4];
                        Form1.apiGetMem(ret.CData[cnt].addr, ref pretByte);

                        if (splitList.Length <= (cnt + y))
                            break;

                        Array.Resize(ref ret.CData, ret.CData.Length + 1);
                        ret.CData[cnt + 1].addr = (misc.ByteArrayToLong(pretByte, 0, 4) + ulong.Parse(misc.sLeft(val, 8), NumberStyles.HexNumber));
                        ret.CData[cnt + 1].val = BitConverter.GetBytes(int.Parse(misc.sRight(splitList[cnt + y], 8), System.Globalization.NumberStyles.HexNumber));
                        ret.CData[cnt + 1].type = splitList[cnt + y][0];
                        break;
                    case 'D':
                    case 'E': //Joker
                        skip = ParseJokerD(splitList, strcnt, ref ret, cnt);
                        skip -= (cnt + 1);
                        cnt = skip + cnt;
                        break;
                }

                cnt++;
            SkipUpdateCD:
                strcnt++;
            }
        }

        /*
         * Processes joker code types (both D and E type)
         */
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

    }
}
