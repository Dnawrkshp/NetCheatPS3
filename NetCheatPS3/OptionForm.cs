﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetCheatPS3
{
    public partial class OptionForm : Form
    {
        Color[] bColors = new Color[1];
        Color[] origColors = new Color[2];
        bool origColorChanged = false;
        bool updPB = true;

        public OptionForm()
        {
            InitializeComponent();
        }

        private void OptionForm_Shown(object sender, EventArgs e)
        {
            int x = 0;

            origColors[0] = Form1.ncBackColor;
            origColors[1] = Form1.ncForeColor;

            //Color options
            bcolCB.SelectedIndex = -1;
            fcolCB.SelectedIndex = -1;

            updPB = false;
            bcolR.Text = Form1.ncBackColor.R.ToString();
            bcolG.Text = Form1.ncBackColor.G.ToString();
            fcolR.Text = Form1.ncForeColor.R.ToString();
            fcolG.Text = Form1.ncForeColor.G.ToString();
            updPB = true;
            bcolB.Text = Form1.ncBackColor.B.ToString();
            fcolB.Text = Form1.ncForeColor.B.ToString();

            bColors[0] = Color.FromArgb(0, 130, 210);
            if (Form1.ncBackColor.Equals(bColors[0]))
                bcolCB.SelectedIndex = 0;
            else if (Form1.ncForeColor.Equals(bColors[0]))
                fcolCB.SelectedIndex = 0;

            KnownColor[] colors = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            foreach (KnownColor kCol in colors)
            {
                Color color = Color.FromKnownColor(kCol);
                Array.Resize(ref bColors, bColors.Length + 1);
                bColors[bColors.Length - 1] = color;
                bcolCB.Items.Add(color.Name);
                if (color.R == Form1.ncBackColor.R && color.G == Form1.ncBackColor.G && color.B == Form1.ncBackColor.B)
                    bcolCB.SelectedIndex = bColors.Length - 1;
                fcolCB.Items.Add(color.Name);
                if (color.R == Form1.ncForeColor.R && color.G == Form1.ncForeColor.G && color.B == Form1.ncForeColor.B)
                    fcolCB.SelectedIndex = bColors.Length - 1;
            }

            if (bcolCB.Text == "")
                bcolCB.Text = "Custom";
            if (fcolCB.Text == "")
                fcolCB.Text = "Custom";
        }

        private string ParseKeyData(Keys Key)
        {
            string ret = Key.ToString().Replace(", ", " + ")
                            .Replace("D0", "0")
                            .Replace("D1", "1")
                            .Replace("D2", "2")
                            .Replace("D3", "3")
                            .Replace("D4", "4")
                            .Replace("D5", "5")
                            .Replace("D6", "6")
                            .Replace("D7", "7")
                            .Replace("D8", "8")
                            .Replace("D9", "9");
            return ret;
        }

        private void bcolCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            updPB = false;
            if (bcolCB.SelectedIndex >= 0)
            {
                bcolR.Text = bColors[bcolCB.SelectedIndex].R.ToString();
                bcolG.Text = bColors[bcolCB.SelectedIndex].G.ToString();
                bcolB.Text = bColors[bcolCB.SelectedIndex].B.ToString();
            }

            Color bColor = Color.FromArgb(int.Parse(bcolR.Text), int.Parse(bcolG.Text), int.Parse(bcolB.Text));
            UpdatePB(ref bcolPB, bColor);
            updPB = true;
        }

        private void UpdatePB(ref PictureBox pb, Color col)
        {
            if (pb.Name == "bcolPB")
            {
                Form1.ncBackColor = col;
                Form1.Instance.BackColor = col;
                Form1.HandlePluginControls(Form1.Instance.Controls);
                origColorChanged = true;
            }
            else if (pb.Name == "fcolPB")
            {
                Form1.ncForeColor = col;
                Form1.Instance.ForeColor = col;
                Form1.HandlePluginControls(Form1.Instance.Controls);
                Form1.HandlePluginControls(Form1.FRManager.Controls);
                origColorChanged = true;
            }

            Bitmap MyImage = new Bitmap(bcolPB.Width, bcolPB.Height);
            using (Graphics gfx = Graphics.FromImage(MyImage))
            using (SolidBrush brush = new SolidBrush(col))
                gfx.FillRectangle(brush, 0, 0, MyImage.Width, MyImage.Height);
            pb.Image = MyImage;
        }

        private void bcolR_TextChanged(object sender, EventArgs e)
        {
            if (bcolR.Text != "" && updPB)
            {
                Color bColor = Color.FromArgb(int.Parse(bcolR.Text), int.Parse(bcolG.Text), int.Parse(bcolB.Text));
                UpdatePB(ref bcolPB, bColor);
            }
        }

        private void bcolG_TextChanged(object sender, EventArgs e)
        {
            if (bcolG.Text != "" && updPB)
            {
                Color bColor = Color.FromArgb(int.Parse(bcolR.Text), int.Parse(bcolG.Text), int.Parse(bcolB.Text));
                UpdatePB(ref bcolPB, bColor);
            }
        }

        private void bcolB_TextChanged(object sender, EventArgs e)
        {
            if (bcolB.Text != "" && updPB)
            {
                Color bColor = Color.FromArgb(int.Parse(bcolR.Text), int.Parse(bcolG.Text), int.Parse(bcolB.Text));
                UpdatePB(ref bcolPB, bColor);
            }
        }

        private void fcolCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            updPB = false;
            if (fcolCB.SelectedIndex >= 0)
            {
                fcolR.Text = bColors[fcolCB.SelectedIndex].R.ToString();
                fcolG.Text = bColors[fcolCB.SelectedIndex].G.ToString();
                fcolB.Text = bColors[fcolCB.SelectedIndex].B.ToString();
            }

            Color bColor = Color.FromArgb(int.Parse(fcolR.Text), int.Parse(fcolG.Text), int.Parse(fcolB.Text));
            UpdatePB(ref fcolPB, bColor);
            updPB = true;
        }

        private void fcolR_TextChanged(object sender, EventArgs e)
        {
            if (fcolR.Text != "" && updPB)
            {
                Color bColor = Color.FromArgb(int.Parse(fcolR.Text), int.Parse(fcolG.Text), int.Parse(fcolB.Text));
                UpdatePB(ref fcolPB, bColor);
            }
        }

        private void fcolG_TextChanged(object sender, EventArgs e)
        {
            if (fcolG.Text != "" && updPB)
            {
                Color bColor = Color.FromArgb(int.Parse(fcolR.Text), int.Parse(fcolG.Text), int.Parse(fcolB.Text));
                UpdatePB(ref fcolPB, bColor);
            }
        }

        private void fcolB_TextChanged(object sender, EventArgs e)
        {
            if (fcolB.Text != "" && updPB)
            {
                Color bColor = Color.FromArgb(int.Parse(fcolR.Text), int.Parse(fcolG.Text), int.Parse(fcolB.Text));
                UpdatePB(ref fcolPB, bColor);
            }
        }

        /* Brings up the Input Box with the arguments of a */
        public Form1.IBArg[] CallIBox(Form1.IBArg[] a)
        {
            InputBox ib = new InputBox();

            ib.Arg = a;
            ib.fmHeight = this.Height;
            ib.fmWidth = this.Width;
            ib.fmLeft = this.Left;
            ib.fmTop = this.Top;
            ib.TopMost = true;
            ib.BackColor = Form1.ncBackColor;
            ib.ForeColor = Form1.ncForeColor;
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

        private void okayButt_Click(object sender, EventArgs e)
        {
            if (Form1.settFile == "")
                return;

            //KeyBinds
            //for (int x = 0; x < Form1.keyBinds.Length; x++)
            //{
            //    string key = Form1.keyBinds[x].GetHashCode().ToString();
            //    fd.WriteLine(key);
            //}

            //Colors
            Form1.ncBackColor = Color.FromArgb(int.Parse(bcolR.Text), int.Parse(bcolG.Text), int.Parse(bcolB.Text));
            Form1.ncForeColor = Color.FromArgb(int.Parse(fcolR.Text), int.Parse(fcolG.Text), int.Parse(fcolB.Text));

            //API
            //Form1.apiDLL = Global.APIs.AvailableAPIs.GetIndex(Form1.curAPI);
            //Form1.PS3.ChangeAPI((Form1.apiDLL == 0) ? PS3Lib.SelectAPI.TargetManager : PS3Lib.SelectAPI.ControlConsole);

            Form1.SaveOptions();
            origColorChanged = false;

            origColors[0] = Form1.ncBackColor;
            origColors[1] = Form1.ncForeColor;
        }

        private void cancButt_Click(object sender, EventArgs e)
        {
            if (origColorChanged)
            {
                Form1.ncBackColor = origColors[0];
                Form1.ncForeColor = origColors[1];
                Form1.Instance.BackColor = origColors[0];
                Form1.Instance.ForeColor = origColors[1];
                Form1.HandlePluginControls(Form1.Instance.Controls);
                Form1.HandlePluginControls(Form1.FRManager.Controls);
            }

            this.Dispose();
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("www.Github.com/Dnawrkshp/NetCheatPS3/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=werewu45%40yahoo%2ecom&lc=US&item_name=Dnawrkshp%27s%20effort&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted");
        }

        private void ccapiDLL_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tmapiDLL_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}
