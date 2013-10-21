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
    public partial class PluginForm : Form
    {
        public string plugAuth = "";
        public string plugDesc = "";
        public string plugName = "";
        public string plugVers = "";
        public string plugText = "";
        public SizeF plugScale = new SizeF(1, 1);
        int resizeForm = 2;

        public PluginForm()
        {
            InitializeComponent();
        }

        private void PluginForm_Load(object sender, EventArgs e)
        {
            //this.Dock = DockStyle.Fill;
        }

        /* Resize */
        private void PluginForm_Shown(object sender, EventArgs e)
        {
            resizeForm = 0;
            Plugin_Resize(null, null);
        }

        /* Resize the form based on the user control */
        public void Plugin_Resize(object sender, EventArgs e)
        {
            resizeForm--;
            if (resizeForm <= 0)
            {
                //Find the controls with the largest Left and Top values
                int maxLeft = 0;
                int maxTop = 0;

                foreach (Control ctrl in Controls[0].Controls)
                {
                    if (ctrl.Visible)
                    {
                        int width = ctrl.Width + ctrl.Left;
                        int height = ctrl.Height + ctrl.Top;
                        if (width > maxLeft)
                            maxLeft = width;
                        if (height > maxTop)
                            maxTop = height;
                    }
                }

                resizeForm = 2;
                Size newSize = new Size(maxLeft + 20, maxTop + 40);
                MinimumSize = newSize;
                MaximumSize = Controls[0].MaximumSize;
            }
        }
    }
}
