using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NCMemBrowser
{
    public partial class hexGrid : UserControl
    {
        public delegate void NextRowEventHandler(int index, int direction);
        public delegate void ByteChangedEventHandler(object sender, int index, byte[] val);
        public delegate void FocusChangedEventHandler(object sender, int index, int pos, int direction);
        public event NextRowEventHandler NextRow;
        public event ByteChangedEventHandler ByteChanged;
        public event FocusChangedEventHandler FocusChanged;

        bool updateByte = false;

        public hexGrid()
        {
            InitializeComponent();
        }

        private void hexGrid_Resize(object sender, EventArgs e)
        {
            int w = Size.Width / 4, h = Size.Height;
            //Location
            b1.Location = new Point(0 * w, 0);
            b2.Location = new Point(1 * w, 0);
            b3.Location = new Point(2 * w, 0);
            b4.Location = new Point(3 * w, 0);
            //Size
            b1.Size = new Size(w, h);
            b2.Size = new Size(w, h);
            b3.Size = new Size(w, h);
            b4.Size = new Size(w, h);
            //Font
            Font newF = new Font(Font.Name, (float)h * 0.9f, Font.Style, GraphicsUnit.Pixel);
            b1.Font = newF;
            b2.Font = newF;
            b3.Font = newF;
            b4.Font = newF;
        }

        private void byte_SelectionChanged(object sender, EventArgs e)
        {
                RichTextBox byteRTB = (RichTextBox)sender;
                if (byteRTB.SelectionStart < 2)
                    byteRTB.SelectionLength = 1;
        }

        private void byte_Focus(object sender, int off)
        {
            int selStart = 0;
            if (off < 0)
                selStart = 1;
            int val = int.Parse(((RichTextBox)sender).Tag.ToString()) + off;
            ((RichTextBox)sender).SelectionLength = 0;
            switch (val)
            {
                case 0: //Go back
                    NextRow(int.Parse(Tag.ToString()), -1);
                    break;
                case 1: //byte 1
                    b1.Focus();
                    b1.SelectionStart = selStart;
                    byte_SelectionChanged(b1, EventArgs.Empty);
                    break;
                case 2: //byte 2
                    b2.Focus();
                    b2.SelectionStart = selStart;
                    byte_SelectionChanged(b2, EventArgs.Empty);
                    break;
                case 3: //byte 3
                    b3.Focus();
                    b3.SelectionStart = selStart;
                    byte_SelectionChanged(b3, EventArgs.Empty);
                    break;
                case 4: //byte 4
                    b4.Focus();
                    b4.SelectionStart = selStart;
                    byte_SelectionChanged(b4, EventArgs.Empty);
                    break;
                case 5: //Next row
                    NextRow(int.Parse(Tag.ToString()), 1);
                    break;
            }
        }

        private void byte_KeyDown(object sender, KeyEventArgs e)
        {
            RichTextBox byteRTB = (RichTextBox)sender;
            if (e.KeyData == Keys.Left)
            {
                if (byteRTB.SelectionStart > 0)
                    byteRTB.SelectionStart--;
                else
                    byte_Focus(sender, -1);
            }
            else if (e.KeyData == Keys.Down)
            {
                if (FocusChanged != null)
                    FocusChanged(this, Convert.ToInt32(byteRTB.Tag), byteRTB.SelectionStart, 1);
            }
            else if (e.KeyData == Keys.Up)
            {
                if (FocusChanged != null)
                    FocusChanged(this, Convert.ToInt32(byteRTB.Tag), byteRTB.SelectionStart, -1);
            }
            else if (byteRTB.SelectionStart >= 1 && ((e.KeyData == Keys.Right) ||
                (e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9) || (e.KeyData >= Keys.A && e.KeyData <= Keys.F)))
            {
                byte_Focus(sender, 1);
            }
            if ((e.KeyData >= Keys.D0 && e.KeyData <= Keys.D9) || (e.KeyData >= Keys.A && e.KeyData <= Keys.F)) {
                updateByte = true;
            }
        }

        #region Public functions
        
        
        public void SetFocus(int ind, int pos)
        {
            switch (ind)
            {
                case 1: //byte 1
                    b1.Focus();
                    b1.SelectionStart = pos;
                    byte_SelectionChanged(b1, EventArgs.Empty);
                    break;
                case 2: //byte 2
                    b2.Focus();
                    b2.SelectionStart = pos;
                    byte_SelectionChanged(b2, EventArgs.Empty);
                    break;
                case 3: //byte 3
                    b3.Focus();
                    b3.SelectionStart = pos;
                    byte_SelectionChanged(b3, EventArgs.Empty);
                    break;
                case 4: //byte 4
                    b4.Focus();
                    b4.SelectionStart = pos;
                    byte_SelectionChanged(b4, EventArgs.Empty);
                    break;
            }
        }

        public void SetByte(int off, byte[] val, bool update)
        {
            foreach (byte b in val)
            {
                string bStr = b.ToString("X2");
                //Inverted ForeColor
                Color changeCol = Color.FromArgb(255 - ForeColor.R,
                                            255 - ForeColor.G,
                                            255 - ForeColor.B);
                int sStart = 0;
                switch (off)
                {
                    case 0:
                        if (bStr == b1.Text || update)
                            changeCol = ForeColor;
                        sStart = b1.SelectionStart;
                        b1.Text = bStr;
                        b1.SelectionStart = sStart;
                        b1.ForeColor = changeCol;
                        break;
                    case 1:
                        if (bStr == b2.Text || update)
                            changeCol = ForeColor;
                        sStart = b2.SelectionStart;
                        b2.Text = bStr;
                        b2.SelectionStart = sStart;
                        b2.ForeColor = changeCol;
                        break;
                    case 2:
                        if (bStr == b3.Text || update)
                            changeCol = ForeColor;
                        sStart = b3.SelectionStart;
                        b3.Text = bStr;
                        b3.SelectionStart = sStart;
                        b3.ForeColor = changeCol;
                        break;
                    case 3:
                        if (bStr == b4.Text || update)
                            changeCol = ForeColor;
                        sStart = b4.SelectionStart;
                        b4.Text = bStr;
                        b4.SelectionStart = sStart;
                        b4.ForeColor = changeCol;
                        break;
                }
                off++;
            }
        }

        public void SetByte(int off, string val, bool update)
        {
            for (int cnt = 0; cnt < (val.Length / 2); cnt++)
            {
                string bStr = ctlMain.NCInterface.sMid(val, cnt * 2, 2);
                //Inverted ForeColor
                Color changeCol = Color.FromArgb(255 - ForeColor.R,
                                            255 - ForeColor.G,
                                            255 - ForeColor.B);
                int sStart = 0;
                switch (off)
                {
                    case 0:
                        if (bStr == b1.Text || update)
                            changeCol = ForeColor;
                        sStart = b1.SelectionStart;
                        b1.Text = bStr;
                        b1.SelectionStart = sStart;
                        b1.ForeColor = changeCol;
                        break;
                    case 1:
                        if (bStr == b2.Text || update)
                            changeCol = ForeColor;
                        sStart = b2.SelectionStart;
                        b2.Text = bStr;
                        b2.SelectionStart = sStart;
                        b2.ForeColor = changeCol;
                        break;
                    case 2:
                        if (bStr == b3.Text || update)
                            changeCol = ForeColor;
                        sStart = b3.SelectionStart;
                        b3.Text = bStr;
                        b3.SelectionStart = sStart;
                        b3.ForeColor = changeCol;
                        break;
                    case 3:
                        if (bStr == b4.Text || update)
                            changeCol = ForeColor;
                        sStart = b4.SelectionStart;
                        b4.Text = bStr;
                        b4.SelectionStart = sStart;
                        b4.ForeColor = changeCol;
                        break;
                }
                off++;
            }
        }

        #endregion

        private void b1_TextChanged(object sender, EventArgs e)
        {
            if (ByteChanged != null && updateByte)
                ByteChanged(this,
                            int.Parse(b1.Tag.ToString()),
                            new byte[] { Convert.ToByte(b1.Text, 16) });
            updateByte = false;
        }

        private void b2_TextChanged(object sender, EventArgs e)
        {
            if (ByteChanged != null && updateByte)
                ByteChanged(this,
                            int.Parse(b2.Tag.ToString()),
                            new byte[] { Convert.ToByte(b2.Text, 16) });
            updateByte = false;
        }

        private void b3_TextChanged(object sender, EventArgs e)
        {
            if (ByteChanged != null && updateByte)
                ByteChanged(this,
                            int.Parse(b3.Tag.ToString()),
                            new byte[] { Convert.ToByte(b3.Text, 16) });
            updateByte = false;
        }

        private void b4_TextChanged(object sender, EventArgs e)
        {
            if (ByteChanged != null && updateByte)
                ByteChanged(this,
                            int.Parse(b4.Tag.ToString()),
                            new byte[] { Convert.ToByte(b4.Text, 16) });
            updateByte = false;
        }

        private void hexGrid_BackColorChanged(object sender, EventArgs e)
        {
            b1.BackColor = BackColor;
            b2.BackColor = BackColor;
            b3.BackColor = BackColor;
            b4.BackColor = BackColor;
        }

        private void hexGrid_ForeColorChanged(object sender, EventArgs e)
        {
            b1.ForeColor = ForeColor;
            b2.ForeColor = ForeColor;
            b3.ForeColor = ForeColor;
            b4.ForeColor = ForeColor;
        }
    }
}
