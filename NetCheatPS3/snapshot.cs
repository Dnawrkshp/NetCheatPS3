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
    public partial class snapshot : UserControl
    {
        public static string version = "1.0.0";
        public static string name = "PS3 Snapshotter";
        public static string author = "Dnawrkshp";
        public static string tabName = "PS3 Snapshotter";
        public static string desc = "Takes a snapshot of the current frame using the 0xC0000000 area. " +
            "Each game has a different offset so you have to find it yourself. " +
            "The options HD TV and SD TV is to mark your PS3's output.\n" +
            "Lastly CCAPI is INCREDIBLY slow. I'd suggest only using this with the TMAPI.";

        string globalSave = "";

        public snapshot()
        {
            InitializeComponent();
        }

        private void makeSnapshot_Click(object sender, EventArgs e)
        {
            bool pause = snapPauseGame.Checked;
            Bitmap snapShot;

            //if (pause && Form1.apiDLL == 0)
            //        PS3Lib.NET.PS3TMAPI.ProcessAttach(0, PS3Lib.NET.PS3TMAPI.UnitType.PPU, PS3Lib.TMAPI.Parameters.ProcessID);

            if (pause)
                Form1.curAPI.Instance.PauseProcess();

            ulong startAddr = Convert.ToUInt32(frameStart.Text, 16);
            if (sdRadButt.Checked)
            {
                //snapShot = getSDSnap(720, 480, startAddr);
                snapShot = getSDPreview(720, 480, startAddr, 4);
            }
            else if (hdRadButt.Checked)
                snapShot = getHDSnap(1280, 720, startAddr);
                //snapShot = getHDPreview(1280, 720, startAddr, 8);
            else
            {
                //snapShot = getHDSnap(2048, 1080, startAddr);
                snapShot = getHDPreview(2048, 1080, startAddr, 4);
            }

            //if (pause)
            //    PS3Lib.NET.PS3TMAPI.ProcessContinue(0, PS3Lib.TMAPI.Parameters.ProcessID);

            if (pause)
                Form1.curAPI.Instance.ContinueProcess();

            //snapShot.Save(file);
            snapPB.Image = (Bitmap)snapShot.Clone();
            //snapPB.Scale(snapShot.Size);
            snapShot.Dispose();
            globalSave = "";
        }

        private void saveSnap_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "Bitmap files (*.bmp)|*.bmp|All files (*.*)|*.*";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                snapPB.Image.Save(fd.FileName);
                globalSave = fd.FileName;
                MessageBox.Show("Saved");
            }
        }

        bool stopFindFrame = false;
        private void findFrame_Click(object sender, EventArgs e)
        {
            if (findFrame.Text == "Stop")
            {
                stopFindFrame = true;
                findFrame.Text = "Find";
                return;
            }
            else
                findFrame.Text = "Stop";

            ulong addr = Convert.ToUInt32(frameStart.Text, 16);
            for (ulong a = addr; a < 0xCF900000; a += 0x100000)
            {
                frameStart.Text = a.ToString("X8");
                makeSnapshot_Click(null, null);
                Application.DoEvents();
                if (stopFindFrame)
                    break;
            }

            stopFindFrame = false;
            findFrame.Text = "Find";
        }

        Bitmap getSDPreview(int width, int height, ulong rStart, int ratio = 2)
        {
            Bitmap ret = new Bitmap(width, height);
            ulong rCnt = 0, incVal = 0xC00;
            int cnt = 0;

            System.Drawing.Imaging.BitmapData data = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height),
               System.Drawing.Imaging.ImageLockMode.ReadWrite,
               System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int stride = data.Stride;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                {

                    for (cnt = 0; cnt < height; cnt++)
                    {
                        rCnt = rStart + ((ulong)cnt * incVal);
                        byte[] retByte = new byte[0xB40];
                        Form1.apiGetMem(rCnt, ref retByte);
                        for (int x = 0; x < retByte.Length; x += 4)
                        {
                            for (int cntY = 0; cntY < ratio; cntY++)
                            {
                                int curY = (cnt + cntY);
                                if (curY < ret.Height)
                                {
                                    ptr[((x * 3) / 4) + (curY * stride)] = retByte[x + 3];
                                    ptr[((x * 3) / 4) + (curY * stride + 1)] = retByte[x + 2];
                                    ptr[((x * 3) / 4) + (curY * stride + 2)] = retByte[x + 1];
                                }
                            }
                        }
                    }
                }
            }
            ret.UnlockBits(data);

            return ret;
        }

        Bitmap getHDPreview(int width, int height, ulong rStart, int ratio = 2)
        {
            Bitmap ret = new Bitmap(width, height);
            ulong rCnt = 0;
            int cnt = 0;

            System.Drawing.Imaging.BitmapData data = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int stride = data.Stride;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                {

                    byte[] readBytes = new byte[width * 4];
                    for (cnt = 0; cnt < height; cnt += ratio)
                    {
                        Form1.apiGetMem(rStart + (ulong)(cnt * width * 4), ref readBytes);
                        rCnt = rStart + (ulong)(cnt * width);
                        int x = 0;
                        for (int off = (cnt * width * 4); off < ((cnt + 1) * width * 4); off += 4)
                        {
                            for (int cntY = 0; cntY < ratio; cntY++)
                            {
                                ptr[(x * 3) + ((cnt + cntY) * stride)] = readBytes[(x * 4) + 3];
                                ptr[(x * 3) + ((cnt + cntY) * stride + 1)] = readBytes[(x * 4) + 2];
                                ptr[(x * 3) + ((cnt + cntY) * stride + 2)] = readBytes[(x * 4) + 1];
                            }
                            x++;
                        }
                    }
                }
            }
            ret.UnlockBits(data);

            return ret;
        }

        Bitmap getSDSnap(int width, int height, ulong rStart)
        {
            Bitmap ret = new Bitmap(width, height);
            ulong rCnt = 0, incVal = 0xC00;
            int cnt = 0;

            /*
            for (cnt = 0; cnt < (height / 2); cnt++)
            {
                rCnt = rStart + ((ulong)cnt * incVal);
                byte[] retByte = new byte[0xB40];
                apiGetMem(rCnt, ref retByte);
                for (int x = 0; x < retByte.Length; x += 4)
                {
                    ret.SetPixel(x / 4, cnt * 2, Color.FromArgb(
                        retByte[x + 1], retByte[x + 2], retByte[x + 3]));
                    ret.SetPixel(x / 4, (cnt * 2) + 1, Color.FromArgb(
                        retByte[x + 1], retByte[x + 2], retByte[x + 3]));
                }
            }
            */

            System.Drawing.Imaging.BitmapData data = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height),
               System.Drawing.Imaging.ImageLockMode.ReadWrite,
               System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int stride = data.Stride;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                // Check this is not a null area
                //if (!areaToPaint.IsEmpty)
                {
                    // Go through the draw area and set the pixels as they should be
                    //for (int y = areaToPaint.Top; y < areaToPaint.Bottom; y++)
                    //{
                    //    for (int x = areaToPaint.Left; x < areaToPaint.Right; x++)
                    //    {
                    //        // layer.GetBitmap().SetPixel(x, y, m_colour);
                    //        ptr[(x * 3) + y * stride] = m_colour.B;
                    //        ptr[(x * 3) + y * stride + 1] = m_colour.G;
                    //        ptr[(x * 3) + y * stride + 2] = m_colour.R;
                    //    }
                    //}

                    for (cnt = 0; cnt < height; cnt++)
                    {
                        rCnt = rStart + ((ulong)cnt * incVal);
                        byte[] retByte = new byte[0xB40];
                        Form1.apiGetMem(rCnt, ref retByte);
                        for (int x = 0; x < retByte.Length; x += 4)
                        {
                            //ret.SetPixel(x / 4, cnt, Color.FromArgb(
                            //    retByte[x + 1], retByte[x + 2], retByte[x + 3]));
                            ptr[((x * 3) / 4) + (cnt * stride)] = retByte[x + 3];
                            ptr[((x * 3) / 4) + (cnt * stride + 1)] = retByte[x + 2];
                            ptr[((x * 3) / 4) + (cnt * stride + 2)] = retByte[x + 1];
                        }
                    }
                }
            }
            ret.UnlockBits(data);

            return ret;
        }

        Bitmap getA4R4B4G4Snap(int width, int height, ulong rStart)
        {
            Bitmap ret = new Bitmap(width, height);
            ulong rCnt = 0, stride = (ulong)(2 * width);
            int cnt = 0;

            for (cnt = 0; cnt < height; cnt++)
            {
                rCnt = rStart + ((ulong)cnt * stride);
                byte[] retByte = new byte[stride];
                Form1.apiGetMem(rCnt, ref retByte);
                for (int x = 0; x < retByte.Length; x += 2)
                {
                    ret.SetPixel(x / 2, cnt, Color.FromArgb(
                        (retByte[x + 0] & 0x0F) * 0x11,
                        (retByte[x + 0] & 0xF0 >> 4) * 0x11,
                        (retByte[x + 1] & 0x0F) * 0x11,
                        (retByte[x + 1] & 0xF0 >> 4) * 0x11));
                }
            }

            return ret;
        }

        Bitmap getHDSnap(int width, int height, ulong rStart)
        {
            Bitmap ret = new Bitmap(width, height);
            ulong rCnt = 0;
            int cnt = 0;

            /*
            for (cnt = 0; cnt < height; cnt++)
            {
                rCnt = rStart + (ulong)(cnt * width);
                byte[] retByte = new byte[width * 4];
                Form1.apiGetMem(rCnt, ref retByte);
                for (int x = 0; x < retByte.Length; x += 4)
                {
                    ret.SetPixel(x / 4, cnt, Color.FromArgb(
                        retByte[x + 1], retByte[x + 2], retByte[x + 3]));
                }
            }
            */

            System.Drawing.Imaging.BitmapData data = ret.LockBits(new Rectangle(0, 0, ret.Width, ret.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int stride = data.Stride;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                // Check this is not a null area
                //if (!areaToPaint.IsEmpty)
                {
                    // Go through the draw area and set the pixels as they should be
                    //for (int y = areaToPaint.Top; y < areaToPaint.Bottom; y++)
                    //{
                    //    for (int x = areaToPaint.Left; x < areaToPaint.Right; x++)
                    //    {
                    //        // layer.GetBitmap().SetPixel(x, y, m_colour);
                    //        ptr[(x * 3) + y * stride] = m_colour.B;
                    //        ptr[(x * 3) + y * stride + 1] = m_colour.G;
                    //        ptr[(x * 3) + y * stride + 2] = m_colour.R;
                    //    }
                    //}

                    byte[] readBytes = new byte[width * height * 4];
                    Form1.apiGetMem(rStart, ref readBytes);
                    for (cnt = 0; cnt < height; cnt++)
                    {
                        rCnt = rStart + (ulong)(cnt * width);
                        int x = 0;
                        for (int off = (cnt * width * 4); off < ((cnt + 1) * width * 4); off += 4)
                        {
                            ptr[(x * 3) + (cnt * stride)] = readBytes[off + 3];
                            ptr[(x * 3) + (cnt * stride + 1)] = readBytes[off + 2];
                            ptr[(x * 3) + (cnt * stride + 2)] = readBytes[off + 1];
                            //ret.SetPixel(x, cnt, Color.FromArgb(
                            //    readBytes[off + 1], readBytes[off + 2], readBytes[off + 3]));
                            x++;
                        }
                    }
                }
            }
            ret.UnlockBits(data);

            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int width = 1280;
            int height = 720;
            ulong rStart = 0;

            Bitmap ret = new Bitmap(width, height);
            ulong rCnt = 0;
            int cnt = 0;

            byte[] readBytes = new byte[width * height * 4];
            FileStream fs = File.OpenRead("C:/Users/Dan/Desktop/dump.raw");
            fs.Read(readBytes, 0, readBytes.Length);
            for (cnt = 0; cnt < height; cnt++)
            {
                rCnt = rStart + (ulong)(cnt * width);
                //byte[] retByte = new byte[width * 4];
                //Form1.apiGetMem(rCnt, ref retByte);
                int x = 0;
                for (int off = (cnt * width * 4); off < ((cnt + 1) * width * 4); off += 4)
                {
                    ret.SetPixel(x, cnt, Color.FromArgb(
                        readBytes[off + 1], readBytes[off + 2], readBytes[off + 3]));
                    x++;
                }
            }

            snapPB.Image = (Bitmap)ret.Clone();
            ret.Dispose();
        }

        Bitmap getHDSnap2(int width, int height, ulong rStart)
        {
            Bitmap ret = new Bitmap(width, height);
            ulong rCnt = 0;
            int cnt = 0;

            byte[] readBytes = new byte[width * height * 4];
            FileStream fs = File.OpenRead("C:/Users/Dan/Desktop/ghosts1080pC0-C1.raw");
            fs.Seek((long)(rStart - 0xC0000000), SeekOrigin.Begin);
            fs.Read(readBytes, 0, readBytes.Length);
            fs.Close();
            for (cnt = 0; cnt < height; cnt++)
            {
                rCnt = rStart + (ulong)(cnt * width);
                int x = 0;
                for (int off = (cnt * width * 4); off < ((cnt + 1) * width * 4); off += 4)
                {
                    ret.SetPixel(x, cnt, Color.FromArgb(
                        readBytes[off + 1], readBytes[off + 2], readBytes[off + 3]));
                    x++;
                }
            }

            return ret;
        }

        byte[] copyBytes(byte[] a, int off, int len)
        {
            byte[] ret = new byte[len];
            for (int x = 0; x < len; x++)
                ret[x] = a[off + x];

            return ret;
        }

        public void snapshot_Resize(object sender, EventArgs e)
        {
            //489, 386
            try
            {
                if (sender == null || this.Size == null)
                    return;
                Form s = (sender as Form);
                if (s == null)
                {
                    s = Form1.Instance;
                }

                this.Size = new Size(s.Width - 15, s.Height - 40);
                snapPB.Location = new Point(4, 61);
                snapPB.Size = new Size(this.Width - 9, this.Height - 66);
            }
            catch { }
        }

        bool findStopDump = false;
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (button1.Text == "Stop")
            {
                findStopDump = true;
                button1.Text = "Find";
                return;
            }
            else
                button1.Text = "Stop";

            int width = 1000;
            for (int a = width; a < 2500; a++)
            {
                frameStart.Text = a.ToString();
                Bitmap snapShot = getHDSnap(a, 180, 0xC0000000);
                snapPB.Image = (Bitmap)snapShot.Clone();
                Application.DoEvents();
                if (findStopDump)
                    break;
            }

            findStopDump = false;
            button1.Text = "Find";
        }

        bool stopVid = false;
        const int vidWidth = 720, vidHeight = 480;
        static byte[] frame = new byte[vidWidth * vidHeight * 4];
        static List<byte[]> frameLines = new List<byte[]>();
        System.Threading.Thread loopGetFrame;
        bool updateFrame = false;
        void threadGetFrame()
        {
            frameLines.Clear();
            for (int x = 0; x < vidHeight; x++)
                frameLines.Add(new byte[vidWidth * 4]);

            codes.ConnectPS3();
            codes.AttachPS3();
            while (loopGetFrame.IsAlive)
            {
                for (int y = 0; y < frameLines.Count; y++)
                {
                    byte[] newLine = new byte[vidWidth * 4];
                    Form1.apiGetMem(0xC0000000 + (ulong)(y * 0xC00), ref newLine);
                    frameLines[y] = newLine;
                    updateFrame = true;
                }
                updateFrame = true;
            }
        }

        private void vidFrame_Click(object sender, EventArgs e)
        {
            if (vidFrame.Text == "Stop")
            {
                stopVid = true;
                return;
            }
            else
            {
                //loopGetFrame = new System.Threading.Thread(threadGetFrame);
                //loopGetFrame.Start();
                vidFrame.Text = "Stop";
            }

            //ulong incVal = 0xC00;
            Bitmap snapShot = new Bitmap(vidWidth, vidHeight);
            while (!stopVid)
            {
                ulong startAddr = Convert.ToUInt32(frameStart.Text, 16);
                if (sdRadButt.Checked)
                {
                    //snapShot = getSDSnap(720, 480, startAddr);
                    snapShot = getSDPreview(720, 480, startAddr, 16);
                }
                else if (hdRadButt.Checked)
                    snapShot = getHDSnap(1280, 720, startAddr);
                else
                {
                    //snapShot = getHDSnap(2048, 1080, startAddr);
                    snapShot = getHDPreview(2048, 1080, startAddr, 4);
                }

                snapPB.Image = (Bitmap)snapShot.Clone();
                Application.DoEvents();
                //for (int cnt = 0; cnt < frameLines.Count; cnt++)
                //{
                //    for (int x = 0; x < (vidWidth * 4); x += 4)
                //    {
                //        ret.SetPixel(x / 4, cnt, Color.FromArgb(
                //            frameLines[cnt][x + 1], frameLines[cnt][x + 2], frameLines[cnt][x + 3]));
                //         if ((x % 1000) == 0)
                //             Application.DoEvents();
                //    }
                //}
                //if (updateFrame)
                //{
                //    snapPB.Image = ret;
                //    updateFrame = false;
                //}
            }

            //loopGetFrame.Abort();
            vidFrame.Text = "Refresh";
            stopVid = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ulong startAddr = Convert.ToUInt32(frameStart.Text, 16);
            Bitmap snapShot = getA4R4B4G4Snap(32, 32, startAddr);

            snapPB.Image = (Bitmap)snapShot;
        }

        private void saveSnap_KeyUp(object sender, KeyEventArgs e)
        {
            if (globalSave == "")
            {
                string file = "tempSNAPSHOT.bmp";
                Bitmap bmp = new Bitmap(snapPB.Image.Size.Width, snapPB.Image.Size.Height);
                bmp = (Bitmap)snapPB.Image.Clone();
                bmp.Save(file);
                globalSave = file;
            }

            System.Collections.Specialized.StringCollection paths = new System.Collections.Specialized.StringCollection();
            paths.Add(Application.StartupPath + "\\" + globalSave);
            Clipboard.SetFileDropList(paths);
        }
    }
}
