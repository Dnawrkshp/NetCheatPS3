using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetCheatPS3
{
    public static class misc
    {
        /* Memory range */
        public static uint[] MemArray;

        public static byte[] addBA(byte[] a, byte[] b)
        {
            int aEnd = a.Length - 1, bEnd = b.Length - 1;
            byte r;
            bool carry = false;

            while (aEnd >= 0 && bEnd >= 0)
            {
                carry = addByte(out r, a[aEnd], b[bEnd]);
                if (carry)
                {
                    if (aEnd > 0)
                    {
                        a[aEnd] = r;
                        while (carry && aEnd > 0)
                        {
                            aEnd--; bEnd--;
                            carry = addByte(out r, a[aEnd], 1);
                            a[aEnd] = r;

                            
                        }
                    }
                    else
                    {
                        return a;
                    }
                }
                else
                {
                    a[aEnd] = r;
                    aEnd--; bEnd--;
                }
                

            }

            return a;
        }

        public static bool addByte(out byte r, byte a, byte b)
        {
            uint res = (uint)a + (uint)b;
            if (res > 255)
            {
                r = (byte)(res - 0x100);
                return true;
            }
            else
            {
                r = (byte)(a + b);
                return false;
            }

        }

        /*
         * Reverses the byte array and returns it
         */
        public static byte[] rev(byte[] b)
        {
            Array.Reverse(b, 0, b.Length);
            return b;

            byte[] ret = new byte[b.Length];
            int inc = 4;
            if (inc > b.Length)
                inc = b.Length;

            for (int x = 0; x < ret.Length; x += inc)
            {
                for (int y = 0; y < inc; y++)
                //    ret[x + y] = b[x + (inc - y - 1)];
                    ret[x + y] = b[b.Length - x - y - 1];
            }
            return ret;
        }

        /* 
         * Reverses the byte array if Form1.doFlipArray and returns it
         */
        public static byte[] revif(byte[] b)
        {
            if (Form1.doFlipArray)
                return rev(b);
            return b;
        }

        /* 
         * Reverses the byte array if not Form1.doFlipArray and returns it
         */
        public static byte[] notrevif(byte[] b)
        {
            if (!Form1.doFlipArray)
                return rev(b);
            return b;
        }

        /*
         * Converts a value into the appropriate byte array
         */
        public static byte[] ValueToByteArray(string val, int align)
        {
            byte[] ret = null;

            switch (align)
            {
                case 1: //1 byte
                    ret = new Byte[] { byte.Parse(val, System.Globalization.NumberStyles.HexNumber) };
                    break;
                case 2: //2 bytes
                    ret = BitConverter.GetBytes(UInt16.Parse(val, System.Globalization.NumberStyles.HexNumber));
                    break;
                case 4: //4 bytes
                    ret = BitConverter.GetBytes(UInt32.Parse(val, System.Globalization.NumberStyles.HexNumber));
                    break;
                case 8: //8 bytes
                    ret = BitConverter.GetBytes(ulong.Parse(val, System.Globalization.NumberStyles.HexNumber));
                    break;
                case -2:

                    break;
                default:
                    ret = StringToByteArray(val);
                    break;
            }

            return ret;
        }

        /*
         * Converts array at offset of length size to a ulong
         */
        public static ulong ByteArrayToLong(byte[] array, int offset, int size)
        {
            if (array == null)
                return 0;

            int pos = 0, x = 0;
            ulong result = 0;
            //foreach (byte by in array)
            for (x = size; x > 0; x--) 
            {
                if ((x - 1 + offset) >= array.Length)
                    result += 0;
                else
                    result += (ulong)((ulong)array[x - 1 + offset] << pos);
                pos += 8;
            }
            return result;
        }

        /*
         * Parses address and returns the proper address based on the Memory Range
         */
        public static ulong ParseSchAddr(ulong addr)
        {
            int x = 0;
            if (MemArray != null)
            {
                for (x = 0; x < (MemArray.Length - 1); x += 2)
                {
                    bool a = false;
                    if (x == 0)
                        a = true;
                    else
                        a = addr >= MemArray[x-1];
                    if (a && addr < MemArray[x])
                        return MemArray[x];
                }

                if (addr >= MemArray[MemArray.Length-1])
                    return 0;
            }
            return addr;
        }

        /*
         * Parses the difference of a and b based on the Memory Range
         * This returns the result divided by 0x500 and as an integer
         */
        public static int ParseRealDif(ulong a, ulong b, ulong div)
        {
            ulong ret = 0; //b - a;
            if (MemArray != null)
            {
                int x = 0;
                for (x = 1; x < MemArray.Length; x += 2)
                {
                    if (b > MemArray[x] && a < MemArray[x - 1])
                        ret += (MemArray[x] - MemArray[x - 1]);
                }

                ulong ret2 = 0;
                int ret2SameRegion = 0;
                for (x = 1; x < MemArray.Length; x += 2)
                {
                    if (ret2SameRegion != 2)
                        ret2SameRegion = 0;
                    if (a >= MemArray[x - 1] && a <= MemArray[x])
                    {
                        ret2 += (MemArray[x] - a);
                        ret2SameRegion = 1;
                    }
                    if (b >= MemArray[x - 1] && b <= MemArray[x])
                    {
                        ret2 += (b - MemArray[x - 1]);
                        if (ret2SameRegion == 1)
                            ret2SameRegion = 2;
                    }
                }

                if (ret2SameRegion == 2)
                    ret += (b - a);
                else
                    ret += ret2;
            }

            if (ret == 0)
                ret = b - a;
            return (int)((ret / div) + 1);
        }

        /*
         * Parses the difference of a and b based on the Memory Range
         * This returns the real result as a ulong
         */
        public static ulong ParseRealDifDump(ulong a, ulong b)
        {
            ulong ret = b - a;
            int x = 0;
            if (MemArray != null)
            {
                for (x = 2; x < MemArray.Length; x += 2)
                {
                    if (b > MemArray[x] && a < MemArray[x])
                        ret -= (MemArray[x] - MemArray[x - 1]);
                }
            }
            return ret;
        }

        /*
         * Compares a and b (and c in the case of certain modes)
         * Used by the searching stuff
         */
        public static bool ArrayCompare(byte[] a, byte[] b, int size, int aOff, int bOff, int mode)
        {
            if (a == null || b == null || size <= 0)
                return false;
            if ((bOff + size) > b.Length || (aOff + size) > a.Length)
                return false;
            UInt64 intA = (UInt64)ByteArrayToLong(a, aOff, size);
            UInt64 intB = (UInt64)ByteArrayToLong(b, bOff, size);

            return ArrayCompare(intA, intB, 0, mode);
        }

        public static bool ArrayCompareText(byte[] a, byte[] b, bool isMatchCase, int mode)
        {
            byte[] bNew = (byte[])b.Clone();
            if (!isMatchCase)
            {
                int x = 0;
                for (x = 0; x < bNew.Length; x++)
                    if (bNew[x] > 0x60)
                        bNew[x] -= 0x20;
            }

            return ArrayCompare(a, bNew, null, mode);
        }

        public static bool ArrayCompare(byte[] a, byte[] b, byte[] c, int size, int aOff, int bOff, int mode)
        {
            if (a == null || b == null || c == null || size <= 0)
                return false;
            if ((bOff + size) > b.Length || (aOff + size) > a.Length)
                return false;
            UInt64 intA = (UInt64)ByteArrayToLong(a, aOff, size);
            UInt64 intB = (UInt64)ByteArrayToLong(b, bOff, size);
            UInt64 intC = (UInt64)ByteArrayToLong(c, aOff, size);

            return ArrayCompare(intA, intB, intC, mode);
        }

        public static bool ArrayCompare(ulong a, byte[] b, ulong c, int size, int bOff, int mode)
        {
            if (b == null || (bOff + size) > b.Length)
                return false;

            ulong intB = ByteArrayToLong(b, bOff, size);

            return ArrayCompare(a, intB, c, mode);
        }
        
        public static bool ArrayCompare(ulong intA, ulong intB, ulong intC, int mode)
        {
            switch (mode)
            {
                case Form1.compEq:
                    if (intB == intA)
                        return true;
                    break;
                case Form1.compNEq:
                    if (intB != intA)
                        return true;
                    break;
                case Form1.compLT:
                    if (intB < intA)
                        return true;
                    break;
                case Form1.compLTE:
                    if (intB <= intA)
                        return true;
                    break;
                case Form1.compGT:
                    if (intB > intA)
                        return true;
                    break;
                case Form1.compGTE:
                    if (intB >= intA)
                        return true;
                    break;
                case Form1.compVBet:
                    if ((intB >= intA) && (intB <= intC))
                        return true;
                    break;
                case Form1.compINC:
                    if ((intA + intC) == intB)
                        return true;
                    break;
                case Form1.compDEC:
                    if ((intA - intC) == intB)
                        return true;
                    break;
                case Form1.compChg:
                    if (intC != intB)
                        return true;
                    break;
                case Form1.compUChg:
                    if (intC == intB)
                        return true;
                    break;
                case Form1.compANEq:
                    if ((intB & intA) == intA)
                        return true;
                    break;
            }

            return false;
        }

        /*
         * Byte array version of searching
         * Gets rid of the need for converting to ulong
         * Supposedly speeds the process up but there maybe be a line of intersection
         */
        public static bool ArrayCompare(byte[] argA, byte[] argB, byte[] argC, int mode)
        {
            if (argA == null || argB == null)
                return false;

            int cnt = 0;
            byte[] a = new byte[0], b = new byte[0], c = new byte[0];
            if (argA != null)
                a = (byte[])argA.Clone();
            if (argB != null)
                b = (byte[])argB.Clone();
            if (argC != null)
                c = (byte[])argC.Clone();
            else
                c = null;

            if (argB.Length < argA.Length)
                return false;

            switch (mode)
            {
                case Form1.compEq:
                    if (isBAEqual(a, b))
                        return true;
                    break;
                case Form1.compNEq:
                    if (!isBAEqual(a, b))
                        return true;
                    break;
                case Form1.compLT:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        if ((cnt == 0) ? ((sbyte)b[cnt] < (sbyte)a[cnt]) : ((byte)b[cnt] < (byte)a[cnt]))
                            return true;
                        else if (b[cnt] != a[cnt])
                            return false;
                    break;
                case Form1.compLTE:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        if ((cnt == 0) ? ((sbyte)b[cnt] < (sbyte)a[cnt]) : ((byte)b[cnt] < (byte)a[cnt]))
                            return true;
                        else if (isBAEqual(a, b))
                            return true;
                        else if (b[cnt] != a[cnt])
                            return false;
                    break;
                case Form1.compLTU:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        if ((byte)b[cnt] < (byte)a[cnt])
                            return true;
                        else if (b[cnt] != a[cnt])
                            return false;
                    break;
                case Form1.compLTEU:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        if ((byte)b[cnt] < (byte)a[cnt])
                            return true;
                        else if (isBAEqual(a, b))
                            return true;
                        else if (b[cnt] != a[cnt])
                            return false;
                    break;
                case Form1.compGT:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        if ((cnt == 0) ? ((sbyte)b[cnt] > (sbyte)a[cnt]) : ((byte)b[cnt] > (byte)a[cnt]))
                            return true;
                        else if (b[cnt] != a[cnt])
                            return false;
                    break;
                case Form1.compGTE:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        if ((cnt == 0) ? ((sbyte)b[cnt] > (sbyte)a[cnt]) : ((byte)b[cnt] > (byte)a[cnt]))
                            return true;
                        else if (isBAEqual(a, b))
                            return true;
                        else if (b[cnt] != a[cnt])
                            return false;
                    break;
                    case Form1.compGTU:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        if ((byte)b[cnt] > (byte)a[cnt])
                            return true;
                        else if (b[cnt] != a[cnt])
                            return false;
                    break;
                case Form1.compGTEU:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        if ((byte)b[cnt] > (byte)a[cnt])
                            return true;
                        else if (isBAEqual(a, b))
                            return true;
                        else if (b[cnt] != a[cnt])
                            return false;
                    break;
                case Form1.compVBet:
                    if (c == null || c.Length < a.Length)
                        return false;

                    //if ((intB >= intA) && (intB <= intC))
                    if (ArrayCompare(a, b, null, Form1.compGTEU) && ArrayCompare(c, b, null, Form1.compLTEU))
                        return true;

                    break;
                case Form1.compINC:
                    if (c == null || c.Length < a.Length)
                        return false;

                    for (cnt = 0; cnt < a.Length; cnt++)
                        a[cnt] += c[cnt];

                    if (isBAEqual(a, b))
                        return true;
                    break;
                case Form1.compDEC:
                    if (c == null || c.Length < a.Length)
                        return false;

                    for (cnt = 0; cnt < a.Length; cnt++)
                        a[cnt] -= c[cnt];

                    if (isBAEqual(a, b))
                        return true;
                    break;
                case Form1.compChg:
                    if (c == null || b.Length < c.Length)
                        return false;

                    if (!isBAEqual(b, c))
                        return true;
                    break;
                case Form1.compUChg:
                    if (c == null || b.Length < c.Length)
                        return false;

                    if (isBAEqual(b, c))
                        return true;
                    break;
                case Form1.compANEq:
                    for (cnt = 0; cnt < a.Length; cnt++)
                        b[cnt] &= a[cnt];

                    if (isBAEqual(a, b))
                        return true;
                    break;
            }

            return false;
        }


        public static bool ArrayCompare(byte[] a, byte[] b, byte[] c, int mode, bool matchCase)
        {
            if (b.Length < a.Length)
                return false;

            if (!matchCase)
            {
                //Make both arrays lowercase
                for (int x = 0; x < a.Length; x++)
                {
                    if (a[x] >= 0x61 && a[x] <= 0x7E)
                        a[x] -= 0x20;
                    if (b[x] >= 0x61 && b[x] <= 0x7E)
                        b[x] -= 0x20;
                    if (c != null)
                        if (c[x] >= 0x61 && c[x] <= 0x7E)
                            c[x] -= 0x20;
                }
            }

            return ArrayCompare(a, b, c, mode);
        }

        /*
         * Determines whether the byte array a is equal to the byte array b
         */
        private static bool isBAEqual(byte[] a, byte[] b)
        {
            for (int cnt = 0; cnt < a.Length; cnt++)
                if (a[cnt] != b[cnt])
                    return false;

            return true;
        }

        /*
         * Determines whether the byte array a is equal to the byte array b at index off
         */
        private static bool isBAEqual(byte[] a, byte[] b, int off)
        {
            for (int cnt = 0; cnt < a.Length; cnt++)
            {
                if ((cnt + off) >= b.Length)
                    return false;
                if (a[cnt] != b[cnt + off])
                    return false;
            }

            return true;
        }

        /*
         * Removes the filename in a path
         * The returned path doesn't end with a '\\' or a '/'
         */
        public static string DirOf(string path)
        {
            int dirLen = path.LastIndexOf('\\');
            if (dirLen < 0)
                dirLen = path.LastIndexOf('/');

            return sLeft(path, dirLen);
        }

        /*
         * Removes the directory path from path
         * The returned filename maintains its extension
         */
        public static string FileOf(string path)
        {
            int dirLen = path.LastIndexOf('\\');
            if (dirLen < 0)
                dirLen = path.LastIndexOf('/');

            return sRight(path, path.Length - dirLen - 1);
        }

        /*
         * Obsolete, I can use String.PadLeft(size, '0')
         * Appends 0's to the left of a string until size is reached
         */
        public static string Pad(String text, int size)
        {

            if (text.Length > size)
                return sLeft(text, size);

            while (text.Length < size)
                text = "0" + text;

            return text;
        }

        /*
         * Converts a string byte array to byte array representing its hexadecimal form
         * "01020304" = { 0x01, 0x02, 0x03, 0x04 }
         */
        public static byte[] StringBAToBA(string str)
        {
            if (str == null || (str.Length % 2) == 1)
                return new byte[0];

            byte[] ret = new byte[str.Length / 2];
            for (int x = 0; x < str.Length; x += 2)
                ret[x / 2] = byte.Parse(sMid(str, x, 2), System.Globalization.NumberStyles.HexNumber);

            return ret;
        }

        /*
         * Converts a string to byte array representing its hexadecimal form
         */
        public static byte[] StringToByteArray(string str)
        {
            if (str == null)
                return new byte[1];
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        /*
         * Converts a byte array into an integer string
         * Split allows there to be separator
         */
        public static string ByteAToStringInt(byte[] a, string split)
        {
            int x = 0;
            string ret = "";
            for (x = 0; x < a.Length; x++)
                ret = ret + a[x] + split;
            if (split.Length > 0)
                return ret.TrimEnd(split.ToCharArray());
            else
                return ret;
        }

        /*
         * Converts a byte array into an hex string
         * Split allows there to be separator
         */
        public static string ByteAToStringHex(byte[] val, string split)
        {
            string ret = "";
            for (int x = 0; x < val.Length; x++)
                ret += val[x].ToString("X2") + split;
            if (split.Length > 0)
                return ret.TrimEnd(split.ToCharArray());
            else
                return ret;
        }

        /*
         * Converts a byte array into a string
         * Split allows there to be separator
         */
        public static string ByteAToString(byte[] a, string split)
        {
            int x = 0;
            string ret = "";
            for (x = 0; x < a.Length; x++)
                ret = ret + (char)a[x] + split;
            return ret;
        }

        /*
         * Reverses the endian of a hexadecimal string
         */
        public static string ReverseE(String text, int size)
        {
            String ret = "";
            int x = 0;

            if (size < text.Length)
                text = sLeft(text, size);

            for (x = 2; x <= size; x += 2)
                ret = ret + sMid(text, size - x, 2);

            return ret;
        }

        //Equivalent to VB6's Left function (grabs length many left most characters in text)
        public static string sLeft(string text, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
            else if (length == 0 || text.Length == 0)
                return "";
            else if (text.Length < length)
                return text;
            else
                return text.Substring(0, length);
        }

        //Equivalent to VB6's Right function (grabs length many right most characters in text)
        public static string sRight(string text, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
            else if (length == 0 || text.Length == 0)
                return "";
            else if (text.Length <= length)
                return text;
            else
                return text.Substring(text.Length - length, length);
        }

        //Equivalent to VB6's Mid function
        public static string sMid(string text, int index, int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
            else if (length == 0 || text.Length == 0)
                return "";
            else if (text.Length < (length + index))
                return text;
            else
                return text.Substring(index, length);
        }

        /*
         * Converts ret into a list result based on align
         */
        public static Form1.ListRes GetlvVals(int align, byte[] ret, int off)
        {
            Form1.ListRes a = new Form1.ListRes();
            ulong tempInt = 0;
            byte[] temp = null;
            if (align > 0)
                temp = new byte[align];
            else if (align == -1)
                temp = new byte[ret.Length];

            switch (align)
            {
                case 1:
                    temp[0] = ret[off];
                    a.HexVal = misc.sRight(temp[0].ToString("X2"), 2);
                    a.DecVal = int.Parse(a.HexVal, System.Globalization.NumberStyles.HexNumber).ToString();
                    a.AlignStr = "1 byte";
                    break;
                case 2:
                    temp[0] = ret[off]; temp[1] = ret[off + 1];
                    tempInt = BitConverter.ToUInt16(temp, 0);
                    a.HexVal = misc.ReverseE(tempInt.ToString("X4"), 4);
                    a.DecVal = int.Parse(a.HexVal, System.Globalization.NumberStyles.HexNumber).ToString();
                    a.AlignStr = "2 bytes";
                    break;
                case 4:
                    temp[0] = ret[off]; temp[1] = ret[off + 1]; temp[2] = ret[off + 2]; temp[3] = ret[off + 3];
                    tempInt = BitConverter.ToUInt32(temp, 0);
                    a.HexVal = misc.ReverseE(misc.sRight(tempInt.ToString("X8"), 8), 8);
                    a.DecVal = int.Parse(a.HexVal, System.Globalization.NumberStyles.HexNumber).ToString();
                    a.AlignStr = "4 bytes";
                    break;
                case 8:
                    temp[0] = ret[off]; temp[1] = ret[off + 1]; temp[2] = ret[off + 2]; temp[3] = ret[off + 3];
                    temp[4] = ret[off + 4]; temp[5] = ret[off + 5]; temp[6] = ret[off + 6]; temp[7] = ret[off + 7];
                    tempInt = BitConverter.ToUInt64(temp, 0);
                    a.HexVal = misc.ReverseE(misc.sRight(tempInt.ToString("X16"), 16), 16);
                    a.DecVal = Int64.Parse(a.HexVal, System.Globalization.NumberStyles.HexNumber).ToString();
                    a.AlignStr = "8 bytes";
                    break;
                case -1:
                    a.HexVal = Encoding.Default.GetString(ret);
                    a.DecVal = "null";
                    a.AlignStr = "Text";
                    break;
                case -2:
                    temp = new byte[0];
                    for (int b = 0; b < (ret.Length - off); b++)
                    {
                        if (b > 8)
                            break;
                        Array.Resize(ref temp, b + 1);
                        temp[b] = ret[off + b];
                    }
                    if (BitConverter.IsLittleEndian) Array.Reverse(temp);
                    tempInt = misc.ByteArrayToLong(temp, 0, temp.Length);
                    string val = tempInt.ToString("X");
                    if ((val.Length % 2) != 0)
                        val = "0" + val;
                    a.HexVal = misc.ReverseE(misc.sRight(val, val.Length), val.Length);
                    a.DecVal = Int64.Parse(a.HexVal, System.Globalization.NumberStyles.HexNumber).ToString();
                    if (temp.Length > 8)
                        a.HexVal += "...";
                    a.AlignStr = ret.Length.ToString() + " byte(s)";
                    break;
            }

            return a;
        }

    }
}
