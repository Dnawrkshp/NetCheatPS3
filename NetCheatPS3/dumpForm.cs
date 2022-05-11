using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace NetCheatPS3
{
    public partial class dumpForm : Form
    {
        public ulong start;
        public ulong stop;

        SaveFileDialog fd = new SaveFileDialog();

        public dumpForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Close")
            {
                Close();
                return;
            }

            fd.Filter = "ELF Files (*.elf)|*.elf|Binary files (*.bin)|*.bin|All files (*.*)|*.*";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                uint offset = 0x00010000;
                int buffer = Extension.ReadInt32(offset + 0x1C);

                byte[] dbuf = new byte[buffer];
                Form1.apiGetMem(offset, ref dbuf);
                File.WriteAllBytes(fd.FileName, dbuf);

                label1.Text = "Dumped to " + new FileInfo(fd.FileName).Name;
                button1.Visible = false;
                button2.Text = "Close";
            }
        }

        bool doClose = false;
        private void button2_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Cancel")
            {
                doClose = true;
                return;
            }
            else if ((sender as Button).Text == "Close")
            {
                Close();
                return;
            }

            fd.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
            fd.RestoreDirectory = true;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                button1.Visible = false;
                button2.Text = "Cancel";

                ProgressBar progBar = new ProgressBar();
                progBar.Size = button1.Size;
                progBar.Location = button1.Location;
                button1.Visible = false;
                this.Controls.Add(progBar);


                if (File.Exists(fd.FileName))
                    File.Delete(fd.FileName);
                System.IO.FileStream fs = new System.IO.FileStream(fd.FileName, System.IO.FileMode.CreateNew,
                    System.IO.FileAccess.Write);

                try
                {
                    ulong usize = 0x10000;
                    progBar.Value = 0;
                    progBar.Maximum = (int)(misc.ParseRealDifDump(start, stop) / usize);

                    for (ulong x = start; x < stop; x += usize)
                    {
                        if (doClose)
                        {
                            fs.Close();
                            File.Delete(fd.FileName);
                            Close();
                        }

                        if (x != misc.ParseSchAddr(x))
                        {
                            int maCnt = 0;
                            uint size = 0;
                            while (maCnt < misc.MemArray.Length && x > misc.MemArray[maCnt])
                                maCnt++;

                            if (maCnt < misc.MemArray.Length)
                            {
                                if (maCnt == 0)
                                    size = (uint)misc.MemArray[maCnt];
                                else
                                    size = (uint)misc.MemArray[maCnt] - (uint)misc.MemArray[maCnt - 1];

                                ulong incSiz = 0;
                                for (ulong sz = 0x100000; incSiz < size; incSiz += 0x100000)
                                {
                                    if (incSiz > size)
                                        sz = (size - (incSiz - 0x100000));
                                    fs.Write(new byte[sz], 0, (int)sz);
                                }
                                x = misc.MemArray[maCnt];
                            }

                            int newVal = misc.ParseRealDif((ulong)start, x, usize);
                            if ((int)newVal <= progBar.Maximum)
                                progBar.Value = (int)newVal;
                        }
                        else
                        {
                            if ((x + usize) > stop)
                                usize = (stop - x);

                            byte[] retByte = new byte[usize];

                            Form1.apiGetMem(x, ref retByte);
                            fs.Write(retByte, 0, retByte.Length);

                            progBar.Increment(1);
                        }
                        Application.DoEvents();
                    }

                    fs.Close();
                }
                catch (Exception)
                {
                    fs.Close();
                }

                //progBar.Value = 0;
                label1.Text = "Dumped to " + new FileInfo(fd.FileName).Name;
                //progBar.Visible = 
            }
        }

        private void dumpForm_Shown(object sender, EventArgs e)
        {
            uint offset = 0x00010000;
            int buffer = Extension.ReadInt32(offset + 0x1C);

            button2.Text = "Dump Select Region (0x" + start.ToString("X8") + " - 0x" + stop.ToString("X8") + ")";
            button1.Text = "Dump EBOOT Region (0x00010000 - 0x" + (offset + buffer).ToString("X8") + ")";
        }


    }
}
