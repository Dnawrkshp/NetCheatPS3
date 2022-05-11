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
    public partial class updateForm : Form
    {
        /* Arguments */
        public string Title = "";
        public string UpdateStr = "";
        public int Return = -1;

        public updateForm()
        {
            InitializeComponent();
        }

        private void updateForm_Load(object sender, EventArgs e)
        {
            ResizeFromWidth(GetLargestWidth(UpdateStr.Split('\n')) - 10);

            titleLabel.Text = Title;
            titleLabel.BackColor = BackColor;
            titleLabel.ForeColor = ForeColor;

            updateBox.Text = UpdateStr;
            updateBox.SelectionStart = 0;
            updateBox.SelectionLength = 0;
            updateBox.BackColor = BackColor;
            updateBox.ForeColor = ForeColor;

            yesButt.BackColor = BackColor;
            yesButt.ForeColor = ForeColor;
            noButt.BackColor = BackColor;
            noButt.ForeColor = ForeColor;

            Application.DoEvents();
            this.Focus();
        }

        int GetLargestWidth(string[] strs)
        {
            Graphics g = updateBox.CreateGraphics();

            int width = 0;

            foreach (string str in strs)
            {
                int tempWidth = (int)g.MeasureString(UpdateStr, updateBox.Font).Width;
                if (tempWidth > width)
                    width = tempWidth;
            }

            g.Dispose();

            return width;
        }

        void ResizeFromWidth(int width)
        {
            updateBox.Width = width;
            titleLabel.Width = width;


            Width = width + 28;

            noButt.Location = new Point(Width - 14 - noButt.Width, Height - 10 - noButt.Height);
        }

        private void yesButt_Click(object sender, EventArgs e)
        {
            Return = 1;
        }

        private void noButt_Click(object sender, EventArgs e)
        {
            Return = 0;
        }

        private void updateForm_Resize(object sender, EventArgs e)
        {

        }
    }
}
