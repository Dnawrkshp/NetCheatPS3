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
    public partial class addrGrid : UserControl
    {
        public delegate void GotoAddressEventHandler(object sender, uint index);
        public event GotoAddressEventHandler GotoAddress;

        public addrGrid()
        {
            InitializeComponent();
        }

        private void addrGrid_Resize(object sender, EventArgs e)
        {
            int w = Size.Width, h = Size.Height;
            //Location
            addrBox.Location = new Point(0 * w, 0);
            //Size
            addrBox.Size = new Size(w, h);
            //Font
            Font newF = new Font(Font.Name, (float)h * 0.9f, Font.Style, GraphicsUnit.Pixel);
            addrBox.Font = newF;
        }

        public void SetText(string text)
        {
            addrBox.Text = text.PadLeft(8, '0');
        }

        private void addrBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                if (GotoAddress != null)
                    GotoAddress(sender, uint.Parse(Tag.ToString()));
            }
        }

        private void addrGrid_ForeColorChanged(object sender, EventArgs e)
        {
            addrBox.ForeColor = ForeColor;
        }

        private void addrGrid_BackColorChanged(object sender, EventArgs e)
        {
            addrBox.BackColor = BackColor;
        }
    }
}
