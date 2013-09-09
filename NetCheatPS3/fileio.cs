using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace NetCheatPS3
{
    class fileio
    {

        /*
         * Appends res to a NetCheat text dump
         */
        public static void AppendDump(Form1.CodeRes res, String filen)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filen, true))
            {
                file.WriteLine(res.addr + " " + misc.ByteAToStringInt(res.val, " ") + " " + res.align);
            }
        }

        /*
         * Reads a single line in a NetCheat text dump int a CodeRes struct
         * Used only by the search results copy feature
         */
        public static Form1.CodeRes ReadDump(String filen, int index, int align)
        {
            Form1.CodeRes ret = new Form1.CodeRes();
            string a = "";
            int i = 0;

            using (var sr = new StreamReader(filen))
            {
                for (i = 0; i < index; i++)
                    sr.ReadLine();
                a = sr.ReadLine();
            }

            string[] s = a.Split(' ');
            ret.addr = ulong.Parse(s[0]);
            string val = "";
            ret.align = int.Parse(s[s.Length - 1]);
            if (ret.align < 0)
            {
                int lenStr = int.Parse(s[s.Length - 2]);
                for (i = 0; i < lenStr; i++)
                {
                    val = val + s[i + 1];
                }
                ret.val = misc.StringToByteArray(val);
            }
            else
            {
                for (i = 0; i < align; i++)
                {
                    val = int.Parse(s[i + 1]).ToString("X2") + val;
                }
                if (val != "")
                    ret.val = misc.ValueToByteArray(val, align);
            }
            

            ret.state = false; //bool.Parse(s[i]);

            return ret;
        }

        /*
         * Reads a NetCheat text dump into a CodeRes struct array
         */
        public static Form1.CodeRes[] ReadDumpArray(String filen, long start, long stop, int align)
        {
            if (stop <= start) 
                return null;

            Form1.CodeRes[] ret = new Form1.CodeRes[stop - start + 1];
            String[] a = new String[stop - start + 1];
            int i = 0;

            if (File.Exists(filen) == false)
                return null;

            using (var sr = new StreamReader(filen))
            {
                for (i = 0; i <= stop; i++)
                {
                    if (i >= start)
                        a[i - start] = sr.ReadLine();
                    else
                        sr.ReadLine();
                }
            }

            Application.DoEvents();

            for (int x = 0; x <= (stop - start); x++)
            {
                if (a[x] == null)
                    break;
                string[] s = a[x].Split(' ');
                ret[x].addr = ulong.Parse(s[0]);
                string val = "";
                ret[x].align = int.Parse(s[s.Length - 1]);
                if (align == 0)
                    align = ret[x].align;
                if (ret[x].align < 0)
                {
                    int lenStr = int.Parse(s[s.Length - 2]);
                    ret[x].val = new byte[lenStr];
                    for (i = 0; i < lenStr; i++)
                    {
                        //val = int.Parse(s[i + 1]).ToString("X2") + val;
                        //val = val + (char)int.Parse(s[i + 1]);
                        ret[x].val[i] = (byte)int.Parse(s[i + 1]);
                    }
                    //ret[x].val = misc.GetBytesFromString(val);
                }
                else
                {
                    for (i = 0; i < align; i++)
                    {
                        val = int.Parse(s[i + 1]).ToString("X2") + val;
                    }
                    if (val != "")
                        ret[x].val = misc.ValueToByteArray(val, align);
                }

                ret[x].state = false; //bool.Parse(s[i]);
                ret[x].align = align;
                Application.DoEvents();
            }

            return ret;
        }

        /*
         * Deletes the selected codes in the int array blocks
         */
        public static void DeleteDumpBlock(string filen, int[] blocks)
        {
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(filen))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;

                int x = 0, cnt = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (cnt < blocks.Length)
                    {
                        if (x != blocks[cnt])
                            sw.WriteLine(line);
                        else
                            cnt++;
                    }
                    else
                        sw.WriteLine(line);
                    x++;
                }
                Form1.SchResCnt = (ulong)(x - cnt);
            }

            File.Delete(filen);
            File.Move(tempFile, filen);
        }

        /*
         * Opens a code database
         */
        public static Form1.CodeDB[] OpenFile(string file)
        {
            Form1.CodeDB[] ret = null;
            int z = 1;

            if (file == "" || file == null)
            {
                System.Windows.Forms.MessageBox.Show("Error: File path invalid!");
                return ret;
            }

            string[] tempStr;
            tempStr = File.ReadAllLines(file);

            int len = 0, y = 0;
            while (y < tempStr.Length)
            {
                if (tempStr[y] == "}")
                    len++;
                y++;
            }

            ret = new Form1.CodeDB[len];

            for (int x = 0; z < tempStr.Length; x++)
            {
                ret[x].state = bool.Parse(tempStr[z]); z++;
                ret[x].name = tempStr[z]; z++;

                while (tempStr[z] != "}")
                {
                    ret[x].codes += tempStr[z] + '\n';
                    z++;
                }
                z += 2;
            }

            return ret;
        }

        /*
         * Saves the code database save into file
         */
        public static void SaveFile(string file, Form1.CodeDB save)
        {
            if (file == "" || file == null)
            {
                System.Windows.Forms.MessageBox.Show("Error: File path invalid!");
                return;
            }

            string[] str = { "{", save.state.ToString(), save.name, save.codes, "}\n" };
            System.IO.File.WriteAllLines(file, str);
        }

        /*
         * Saves all codes into file
         */
        public static void SaveFileAll(string file)
        {
            if (file == "" || file == null)
            {
                System.Windows.Forms.MessageBox.Show("Error: File path invalid!");
                return;
            }

            if (File.Exists(file))
                File.Delete(file);
            using (System.IO.StreamWriter fd = new System.IO.StreamWriter(file, true))
            {
                for (int x = 0; x <= Form1.CodesCount; x++)
                {
                    fd.WriteLine("{");
                    fd.WriteLine(Form1.Codes[x].state.ToString());
                    fd.WriteLine(Form1.Codes[x].name);
                    fd.WriteLine(Form1.Codes[x].codes);
                    fd.WriteLine("}");
                }
            }
        }

        /*
         * Obsolete
         * Opens filename and appends byteArray to the end of it
         */
        public static void SaveDump(byte[] byteArray, string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Append, 
                System.IO.FileAccess.Write);

            fs.Write(byteArray, 0, byteArray.Length);
            fs.Close();
        }

        /*
         * Saves a NetCheat Memory Range file
         */
        public static void SaveRangeFile(string file, System.Windows.Forms.ListView save)
        {
            if (file == "" || file == null)
            {
                System.Windows.Forms.MessageBox.Show("Error: File path invalid!");
                return;
            }

            string[] str = new string[save.Items.Count * 2];
            int y = 0;
            for (int x = 0; x < (str.Length/2); x++)
            {
                str[y] = save.Items[x].SubItems[0].Text;
                str[y+1] = save.Items[x].SubItems[1].Text;
                y += 2;
            }
            System.IO.File.WriteAllLines(file, str);
        }

        /*
         * Opens a NetCheat Memory Range file
         */
        public static System.Windows.Forms.ListView OpenRangeFile(string file)
        {
            if (file == "" || file == null)
            {
                System.Windows.Forms.MessageBox.Show("Error: File path invalid!");
                return null;
            }

            System.Windows.Forms.ListView ret = new System.Windows.Forms.ListView();

            string[] fileArr = File.ReadAllLines(file);
            string[] str = new string[2];
            ret.Items.Clear();

            for (int x = 0; x < fileArr.Length; x += 2)
            {
                str[0] = fileArr[x];
                str[1] = fileArr[x+1];
                ListViewItem a = new ListViewItem(str);
                ret.Items.Add(a);
            }
            return ret;
        }

    }
}
