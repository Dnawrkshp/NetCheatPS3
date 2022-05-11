using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetCheatPS3
{
    public partial class ConditionalEditor : Form
    {
        public string code = "";
        public RichTextBox rtb = null;
        public bool isEditing = false;

        private int oldIndex = 0;
        private CondCode cond = new CondCode();

        private int SelStart = 0;
        private int SelEnd = 0;

        private struct CondCode
        {
            public uint addr;
            public string value;
            public string codes;
            public CodeType type;
        }

        private enum CodeType : int
        {
            Hex = 0,
            Dec = 1,
            Float = 2,
            Double = 3,
            Text = 4
        }

        public ConditionalEditor()
        {
            InitializeComponent();
        }

        private void ConditionalEditor_Shown(object sender, EventArgs e)
        {
            cbType.SelectedIndex = 0;
            cbComp.SelectedIndex = 0;

            if (!isEditing)
            {
                tbCodes.Text = String.Join("\r\n", code.Split(new char[] { '\r', '\n' }));

                cond = new CondCode();
                cond.addr = 0;
                cond.codes = tbCodes.Text;
                cond.value = "00000000";
                cond.type = CodeType.Hex;
            }
            else
            {
                Form1.ncCode ncCd = codes.ParseCodeStringFull(code)[0];
                string text = ""; // ncCd.codeType.ToString() + " " + ncCd.codeArg1.ToString("X8") + " " + misc.ByteAToStringHex(ncCd.codeArg2, "") + "\r\n";
                int selStart = rtb.SelectionStart, selEnd = 0;
                SelStart = selStart;

                while (rtb.Text[selStart] != '\n')
                    selStart++;
                selStart++;

                

                for (int x = (int)ncCd.codeArg0; x > 0;)
                {
                    if (selStart >= rtb.Text.Length)
                        break;

                    selEnd = selStart + 1;
                    while (selEnd < rtb.Text.Length && rtb.Text[selEnd] != '\n')
                        selEnd++;

                    string line = rtb.Text.Substring(selStart, selEnd - selStart);
                    if (codes.ParseCodeStringFull(line)[0].codeType != '\0')
                    {
                        text += line + "\r\n";
                        x--;
                    }
                    else
                        text += ((line.IndexOf("\n") >= 0 && line.IndexOf("\r") < 0) ? line.Replace("\n", "\r\n") : line) + "\r\n";

                    selStart = selEnd + 1;
                }

                SelEnd = selEnd;

                cond = new CondCode();
                cond.addr = (uint)ncCd.codeArg1;
                cond.codes = text;
                cond.value = misc.ByteAToStringHex(ncCd.codeArg2, "");
                cond.type = CodeType.Hex;
                

                tbCodes.Text = text;
                tbAddr.Text = ncCd.codeArg1.ToString("X8");
                tbValue.Text = cond.value;
                cbType.SelectedIndex = 0;
                cbComp.SelectedIndex = ncCd.codeType.ToString().ToUpper() == "D" ? 0 : 1;
            }

        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oldIndex != cbType.SelectedIndex)
            {
                cond.type = (CodeType)cbType.SelectedIndex;

                string hex = ConvertToHex(tbValue.Text, (CodeType)oldIndex);
                string val = ConvertFromHex(hex, cond.type);
                if (val != "")
                {
                    tbValue.Text = val;
                    oldIndex = cbType.SelectedIndex;
                }
                else
                {
                    cond.type = (CodeType)oldIndex;
                    cbType.SelectedIndex = oldIndex;
                }
            }
        }

        private void buttOkay_Click(object sender, EventArgs e)
        {
            int codeCnt = 0;
            string[] codeLines = cond.codes.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int x = 0; x < codeLines.Length; x++)
            {
                if (codes.isCodeValid(codes.ParseCodeStringFull(codeLines[x])[0]))
                    codeCnt++;
            }

            string totalCode = ((cbComp.SelectedIndex == 0) ? "D" : "E") + codeCnt.ToString("X") + " " + cond.addr.ToString("X8") + " " + ConvertToHex(cond.value, cond.type) + "\r\n";
            totalCode += cond.codes;

            if (totalCode.EndsWith("\r\n"))
                totalCode = totalCode.Remove(totalCode.Length - 2, 2);

            if (isEditing)
            {
                rtb.Text = rtb.Text.Remove(SelStart, SelEnd - SelStart);
                rtb.Text = rtb.Text.Insert(SelStart, totalCode);
            }
            else
            {
                rtb.SelectedText = totalCode;
            }

            Close();
        }

        private void buttCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string ConvertToHex(string value, CodeType type)
        {
            switch (type)
            {
                case CodeType.Dec:
                    ulong dec = Convert.ToUInt64(value);
                    return dec.ToString("X");
                case CodeType.Double:
                    double dbl = Convert.ToDouble(value);
                    byte[] dba = BitConverter.GetBytes(dbl);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(dba);
                    return misc.ByteAToStringHex(dba, "");
                case CodeType.Float:
                    float flt = Convert.ToSingle(value);
                    byte[] fba = BitConverter.GetBytes(flt);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(fba);
                    return misc.ByteAToStringHex(fba, "");
                case CodeType.Text:
                    return misc.ByteAToStringHex(misc.StringToByteArray(value), "");
            }

            return value;
        }

        private string ConvertFromHex(string value, CodeType type)
        {
            try
            {
                switch (type)
                {
                    case CodeType.Dec:
                        return Convert.ToUInt64(value, 16).ToString();
                    case CodeType.Double:
                        byte[] dba = misc.StringBAToBA(value.PadLeft(16, '0'));
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(dba);
                        return BitConverter.ToDouble(dba, 0).ToString("G");
                    case CodeType.Float:
                        byte[] fba = misc.StringBAToBA(value.PadLeft(8, '0'));
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(fba);
                        return BitConverter.ToSingle(fba, 0).ToString("G");
                    case CodeType.Text:
                        byte[] tba = misc.StringBAToBA(value);
                        return misc.ByteAToString(tba, "");
                }

                return value;
            }
            catch { }

            return "";
        }

        private void tbValue_TextChanged(object sender, EventArgs e)
        {
            cond.value = tbValue.Text;
            cond.type = (CodeType)cbType.SelectedIndex;
        }

        private void tbAddr_TextChanged(object sender, EventArgs e)
        {
            cond.addr = 0;
            try
            {
                cond.addr = Convert.ToUInt32(tbAddr.Text, 16);
            }
            catch { }
        }

        private void tbCodes_TextChanged(object sender, EventArgs e)
        {
            cond.codes = tbCodes.Text;
        }


    }
}
