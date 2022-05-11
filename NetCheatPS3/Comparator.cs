using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NetCheatPS3
{
    public class Comparator
    {

        #region Declarations

        public bool _shouldStopSearch = false;

        public delegate void InitialSearch(int start, int stop, ulong addr, int typeIndex, string[] args);
        public struct ncSearcher
        {
            public InitialSearch InitialSearch;         //Function that searches initially through the memory
            public string Name;                         //Name of the search
            public string[] Args;                       //Names of arguments passed when searching
            public string[] Exceptions;                 //Types that are incompatible (array of names)
        }

        public List<ncSearcher> SearchComparisons = new List<ncSearcher>();
        public List<SearchControl.ncSearchType> SearchTypes = new List<SearchControl.ncSearchType>();
        public List<SearchValue> SearchArgs = new List<SearchValue>();
        public List<SearchListView.SearchListViewItem> Items = new List<SearchListView.SearchListViewItem>();

        #endregion

        #region Search Types

        public void ThreadInitSearch(object[] args)
        {
            if (codes.ConnectAndAttach())
            {
                Form1.Instance.progBar.Value = 0;
                ncSearcher searcher = (ncSearcher)args[0];
                int start = (int)((uint)args[1]);
                int stop = (int)((uint)args[2]);
                ulong addrFrom = (ulong)args[3];
                int index = (int)args[4];
                string[] passArgs = (string[])args[5];
                searcher.InitialSearch(start, stop, addrFrom, index, passArgs);
                Form1.Instance.progBar.Value = 0;

                Form1.Instance.SetSearchStr("New Search");
                _shouldStopSearch = false;
            }
        }

        public void LoadSearch()
        {
            //Search Comparisons
            ncSearcher ncS = new ncSearcher();

            //Equal To
            ncS.Name = "Equal To";
            ncS.Args = new string[] { "Old Val", "New Val" };
            ncS.InitialSearch = new InitialSearch(EqualTo_InitSearch);
            ncS.Exceptions = new string[0];
            SearchComparisons.Add(ncS);

            //Not Equal To
            ncS.Name = "Not Equal To";
            ncS.Args = new string[] { "Old Val", "New Val" };
            ncS.InitialSearch = new InitialSearch(NotEqualTo_InitSearch);
            ncS.Exceptions = new string[0];
            SearchComparisons.Add(ncS);

            //Less Than
            ncS.Name = "Less Than";
            ncS.Args = new string[] { "Old Val", "New Val" };
            ncS.InitialSearch = new InitialSearch(LessThan_InitSearch);
            ncS.Exceptions = new string[0];
            SearchComparisons.Add(ncS);

            //Less Than or Equal
            ncS.Name = "Less Than or Equal";
            ncS.Args = new string[] { "Old Val", "New Val" };
            ncS.InitialSearch = new InitialSearch(LessThanEqualTo_InitSearch);
            ncS.Exceptions = new string[0];
            SearchComparisons.Add(ncS);

            //Greater Than
            ncS.Name = "Greater Than";
            ncS.Args = new string[] { "Old Val", "New Val" };
            ncS.InitialSearch = new InitialSearch(GreaterThan_InitSearch);
            ncS.Exceptions = new string[0];
            SearchComparisons.Add(ncS);

            //Greater Than or Equal
            ncS.Name = "Greater Than or Equal";
            ncS.Args = new string[] { "Old Val", "New Val" };
            ncS.InitialSearch = new InitialSearch(GreaterThanEqualTo_InitSearch);
            ncS.Exceptions = new string[0];
            SearchComparisons.Add(ncS);

            //Search Types
            SearchControl.ncSearchType ncST = new SearchControl.ncSearchType();

            //1 byte
            ncST.ByteSize = 1;
            ncST.Name = "1 byte";
            ncST.ToItem = new SearchControl.SearchToItem(standardByte_ToItem);
            ncST.BAToString = new SearchControl.ByteAToString(sType1B_ToString);
            ncST.CheckboxName = "Hex";
            ncST.DefaultValue = "00";
            ncST.CheckboxConvert = new SearchControl.CheckboxConvert(ConvertHexDec);
            ncST.ToByteArray = new SearchControl.StringToByteA(sType1B_ToByteArray);
            ncST.ItemToString = new SearchControl.ToSListViewItem(sType1B_ItemToString);
            ncST.ItemToLString = new SearchControl.ParseItemToListString(standardByte_ItemToLString);
            ncST.Initialize = new SearchControl.TypeInitialize(NullTypeInitialize);
            ncST.ignoreAlignment = false;
            SearchTypes.Add(ncST);

            //2 bytes
            ncST.ByteSize = 2;
            ncST.Name = "2 bytes";
            ncST.ToItem = new SearchControl.SearchToItem(standardByte_ToItem);
            ncST.BAToString = new SearchControl.ByteAToString(sType2B_ToString);
            ncST.CheckboxName = "Hex";
            ncST.DefaultValue = "0000";
            ncST.CheckboxConvert = new SearchControl.CheckboxConvert(ConvertHexDec);
            ncST.ToByteArray = new SearchControl.StringToByteA(sType2B_ToByteArray);
            ncST.ItemToString = new SearchControl.ToSListViewItem(sType2B_ItemToString);
            ncST.ItemToLString = new SearchControl.ParseItemToListString(standardByte_ItemToLString);
            ncST.Initialize = new SearchControl.TypeInitialize(NullTypeInitialize);
            ncST.ignoreAlignment = false;
            SearchTypes.Add(ncST);

            //4 bytes
            ncST.ByteSize = 4;
            ncST.Name = "4 bytes";
            ncST.ToItem = new SearchControl.SearchToItem(standardByte_ToItem);
            ncST.BAToString = new SearchControl.ByteAToString(sType4B_ToString);
            ncST.CheckboxName = "Hex";
            ncST.DefaultValue = "00000000";
            ncST.CheckboxConvert = new SearchControl.CheckboxConvert(ConvertHexDec);
            ncST.ToByteArray = new SearchControl.StringToByteA(sType4B_ToByteArray);
            ncST.ItemToString = new SearchControl.ToSListViewItem(sType4B_ItemToString);
            ncST.ItemToLString = new SearchControl.ParseItemToListString(standardByte_ItemToLString);
            ncST.Initialize = new SearchControl.TypeInitialize(NullTypeInitialize);
            ncST.ignoreAlignment = false;
            SearchTypes.Add(ncST);

            //8 bytes
            ncST.ByteSize = 8;
            ncST.Name = "8 bytes";
            ncST.ToItem = new SearchControl.SearchToItem(standardByte_ToItem);
            ncST.BAToString = new SearchControl.ByteAToString(sType8B_ToString);
            ncST.CheckboxName = "Hex";
            ncST.DefaultValue = "0000000000000000";
            ncST.CheckboxConvert = new SearchControl.CheckboxConvert(ConvertHexDec);
            ncST.ToByteArray = new SearchControl.StringToByteA(sType8B_ToByteArray);
            ncST.ItemToString = new SearchControl.ToSListViewItem(sType8B_ItemToString);
            ncST.ItemToLString = new SearchControl.ParseItemToListString(standardByte_ItemToLString);
            ncST.Initialize = new SearchControl.TypeInitialize(NullTypeInitialize);
            ncST.ignoreAlignment = false;
            SearchTypes.Add(ncST);

            //X bytes
            ncST.ByteSize = 0;
            ncST.Name = "X bytes";
            ncST.ToItem = new SearchControl.SearchToItem(standardByte_ToItem);
            ncST.BAToString = new SearchControl.ByteAToString(sTypeXB_ToString);
            ncST.CheckboxName = "";
            ncST.DefaultValue = "00000000";
            ncST.CheckboxConvert = new SearchControl.CheckboxConvert(ConvertHexDec);
            ncST.ToByteArray = new SearchControl.StringToByteA(sTypeXB_ToByteArray);
            ncST.ItemToString = new SearchControl.ToSListViewItem(sTypeXB_ItemToString);
            ncST.ItemToLString = new SearchControl.ParseItemToListString(standardByte_ItemToLString);
            ncST.Initialize = new SearchControl.TypeInitialize(sTypeXB_Initialize);
            ncST.ignoreAlignment = true;
            SearchTypes.Add(ncST);

            //Populate Search combo box
            Form1.Instance.ResetSearchCompBox();
        }

        #region 1 Byte

        byte[] sType1B_ToByteArray(string str)
        {
            return StringToByteArray(str, 1 * 2);
        }

        string sType1B_ToString(byte[] val)
        {
            return standardByte_ToString(val, 1);
        }

        string sType1B_ItemToString(SearchListView.SearchListViewItem item)
        {
            return "0 " + item.addr.ToString("X8") + " " + SearchTypes[item.align].BAToString(item.newVal);
        }

        #endregion

        #region 2 Bytes

        byte[] sType2B_ToByteArray(string str)
        {
            return StringToByteArray(str, 2 * 2);
        }

        string sType2B_ToString(byte[] val)
        {
            return standardByte_ToString(val, 2);
        }

        string sType2B_ItemToString(SearchListView.SearchListViewItem item)
        {
            return "0 " + item.addr.ToString("X8") + " " + SearchTypes[item.align].BAToString(item.newVal);
        }

        #endregion

        #region 4 Bytes

        byte[] sType4B_ToByteArray(string str)
        {
            return StringToByteArray(str, 4 * 2);
        }

        string sType4B_ToString(byte[] val)
        {
            return standardByte_ToString(val, 4);
        }

        string sType4B_ItemToString(SearchListView.SearchListViewItem item)
        {
            return "0 " + item.addr.ToString("X8") + " " + SearchTypes[item.align].BAToString(item.newVal);
        }

        #endregion

        #region 8 Bytes

        byte[] sType8B_ToByteArray(string str)
        {
            return StringToByteArray(str, 8 * 2);
        }

        string sType8B_ToString(byte[] val)
        {
            return standardByte_ToString(val, 8);
        }

        string sType8B_ItemToString(SearchListView.SearchListViewItem item)
        {
            return "0 " + item.addr.ToString("X8") + " " + SearchTypes[item.align].BAToString(item.newVal);
        }

        #endregion

        #region X Bytes

        byte[] sTypeXB_ToByteArray(string str)
        {
            int len = str.Length;
            if ((len % 2) == 1)
                len++;
            return StringToByteArray(str, len);
        }

        string sTypeXB_ToString(byte[] val)
        {
            return standardByte_ToString(val, val.Length);
        }

        void sTypeXB_Initialize(string arg, int typeIndex)
        {
            int len = arg.Length;
            if ((len % 2) == 1)
                len++;
            SearchControl.ncSearchType type = SearchTypes[typeIndex];
            type.ByteSize = len / 2;
            SearchTypes[typeIndex] = type;
        }

        string sTypeXB_ItemToString(SearchListView.SearchListViewItem item)
        {
            return "0 " + item.addr.ToString("X8") + " " + SearchTypes[item.align].BAToString(item.newVal);
        }

        #endregion

        #region Text

        byte[] sTypeText_ToByteArray(string str)
        {
            byte[] ret = new byte[str.Length];
            for (int x = 0; x < str.Length; x++)
            {
                ret[x] = (byte)((char)str[x]);
            }

            return ret;
        }

        string sTypeText_ToString(byte[] val)
        {
            return misc.ByteAToString(val, "");
        }

        void sTypeText_Initialize(string arg, int typeIndex)
        {
            int len = arg.Length;
            if ((len % 2) == 1)
                len++;
            SearchControl.ncSearchType type = SearchTypes[typeIndex];
            type.ByteSize = len;
            SearchTypes[typeIndex] = type;
        }

        string sTypeText_ItemToString(SearchListView.SearchListViewItem item)
        {
            return "1 " + item.addr.ToString("X8") + " " + SearchTypes[item.align].BAToString(item.newVal);
        }

        string[] sTypeText_ItemToLString(SearchListView.SearchListViewItem item)
        {
            SearchControl.ncSearchType type = SearchTypes[item.align];
            string[] ret = new string[4];
            ret[0] = item.addr.ToString("X8");
            ret[1] = misc.ByteAToString(item.newVal, "");
            ret[2] = "Invalid";
            ret[3] = type.Name;
            return ret;
        }

        #endregion

        void NullTypeInitialize(string arg, int typeIndex)
        {

        }

        string NullCheckboxConvert(string val, bool state)
        {
            return val;
        }

        string ConvertHexDec(string val, bool isHex)
        {
            try
            {
                if (!isHex)
                {
                    ulong cval = Convert.ToUInt64(val, 16);
                    return cval.ToString();
                }
                else
                {
                    ulong cval = Convert.ToUInt64(val);
                    return cval.ToString("X");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return "";
        }

        byte[] StringToByteArray(string val, int size)
        {
            val = val.PadLeft(size, '0');
            val = misc.sLeft(val, size);

            byte[] ret = new byte[size / 2];
            for (int x = 0; x < size; x += 2)
                ret[x / 2] = byte.Parse(misc.sMid(val, x, 2), System.Globalization.NumberStyles.HexNumber);
            return ret;
        }

        SearchListView.SearchListViewItem standardByte_ToItem(ulong addr, byte[] newVal, byte[] oldVal, int typeIndex, uint misc = 0)
        {
            SearchListView.SearchListViewItem ret = new SearchListView.SearchListViewItem();

            ret.addr = (uint)addr;
            ret.align = typeIndex;
            ret.oldVal = oldVal;
            ret.newVal = newVal;
            ret.misc = misc;

            return ret;
        }

        string[] standardByte_ItemToLString(SearchListView.SearchListViewItem item)
        {
            SearchControl.ncSearchType type = SearchTypes[item.align];
            string[] ret = new string[4];
            int size = type.ByteSize;
            string post = "";
            if (type.ByteSize > 8)
            {
                size = 8;
                post = "...";
            }

            ulong val = misc.ByteArrayToLong(item.newVal, 0, size);
            ret[0] = item.addr.ToString("X8");
            ret[1] = val.ToString("X" + (type.ByteSize * 2).ToString()) + post;
            ret[2] = val.ToString();
            ret[3] = type.Name;
            return ret;
        }

        string standardByte_ToString(byte[] val, int size)
        {
            string ret = "";
            for (int x = 0; x < size; x++)
            {
                if (x < val.Length)
                    ret += val[x].ToString("X2");
                else
                    ret += "00";
            }
            return ret;
        }

        #endregion

        #region Initial Searches

        void standardByte_InitSearch(int start, int stop, ulong addr, int typeIndex, string[] args, int cmpIntType)
        {
            FileStream fsIN1 = new FileStream(Form1.inputBins[0], FileMode.Open);
            FileStream fsIN2 = new FileStream(Form1.inputBins[1], FileMode.Open);

            SearchControl.ncSearchType type = SearchTypes[typeIndex];
            type.Initialize((string)args[0], typeIndex);
            type = SearchTypes[typeIndex];

            bool updateCont = false;
            int size = 0x1000;
            if ((stop - start) <= size)
                updateCont = true;

            byte[] oldBA = type.ToByteArray(args[0]);
            byte[] newBA = type.ToByteArray(args[1]);

            byte[] cmp1, cmp2;

            if (!updateCont)
                Form1.Instance.progBar.Maximum = (stop - start) / size;
            else
                Form1.Instance.progBar.Maximum = (stop - start);

            byte[] cmpArray = type.ToByteArray((string)args[0]);
            int resCnt = 0;
            int incSize = type.ByteSize;
            if (type.ignoreAlignment)
                incSize = 1;
            //List<SearchListView.SearchListViewItem> itemsToAdd = new List<SearchListView.SearchListViewItem>();

            byte[] tempArr = new byte[type.ByteSize], tempArr2 = new byte[type.ByteSize];
            for (int xAddr = start; xAddr < stop; xAddr += size)
            {
                if (_shouldStopSearch)
                    break;

                cmp2 = null;
                cmp1 = null;

                ulong curAddr = (ulong)xAddr + addr;
                cmp1 = ReadBytes(fsIN1, xAddr, size);
                if (cmp1 != null)
                {
                    for (int x = 0; x <= (size - type.ByteSize); x += incSize)
                    {
                        if (_shouldStopSearch)
                            break;
                        Array.Copy(cmp1, x, tempArr, 0, type.ByteSize);

                        bool isTrue = misc.ArrayCompare(tempArr, oldBA, null, cmpIntType);
                        if (isTrue)
                        {
                            if (cmp2 == null)
                                cmp2 = ReadBytes(fsIN2, xAddr, size);
                            Array.Copy(cmp2, x, tempArr2, 0, type.ByteSize);

                            isTrue = misc.ArrayCompare(tempArr2, newBA, null, cmpIntType);
                            if (isTrue)
                            {
                                Items.Add(type.ToItem(curAddr + (ulong)x, (byte[])tempArr2.Clone(), (byte[])tempArr.Clone(), typeIndex));
                                resCnt++;
                                Form1.Instance.progBar.printText = "Results: " + resCnt.ToString();
                            }
                        }
                        if (updateCont)
                        {
                            Form1.Instance.progBar.Increment(incSize);
                            Application.DoEvents();
                        }
                    }
                }

                //Thread.Sleep(100);
                if (!updateCont)
                {
                    Form1.Instance.progBar.Increment(1);
                    Application.DoEvents();
                }
            }

            fsIN1.Close();
            fsIN2.Close();
        }

        void EqualTo_InitSearch(int start, int stop, ulong addr, int typeIndex, string[] args)
        {
            standardByte_InitSearch(start, stop, addr, typeIndex, args, Form1.compEq);
        }

        void NotEqualTo_InitSearch(int start, int stop, ulong addr, int typeIndex, string[] args)
        {
            standardByte_InitSearch(start, stop, addr, typeIndex, args, Form1.compNEq);
        }

        void LessThan_InitSearch(int start, int stop, ulong addr, int typeIndex, string[] args)
        {
            standardByte_InitSearch(start, stop, addr, typeIndex, args, Form1.compLT);
        }

        void LessThanEqualTo_InitSearch(int start, int stop, ulong addr, int typeIndex, string[] args)
        {
            standardByte_InitSearch(start, stop, addr, typeIndex, args, Form1.compLTE);
        }

        void GreaterThan_InitSearch(int start, int stop, ulong addr, int typeIndex, string[] args)
        {
            standardByte_InitSearch(start, stop, addr, typeIndex, args, Form1.compGT);
        }

        void GreaterThanEqualTo_InitSearch(int start, int stop, ulong addr, int typeIndex, string[] args)
        {
            standardByte_InitSearch(start, stop, addr, typeIndex, args, Form1.compGTE);
        }

        #endregion

        private byte[] ReadBytes(FileStream fs, int off, int size)
        {
            var data = new byte[size];
            int actualRead;
            actualRead = 0;
            fs.Position = off;

            do
            {
                actualRead += fs.Read(data, actualRead, size - actualRead);
            }
            while (actualRead < size && fs.Position < fs.Length);

            return data;
        }

    }
}
