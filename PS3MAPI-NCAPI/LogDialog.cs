// ******************************************************************
//
//	If this code works it was written by:
//		Malcolm
//		MamSoft / Manniff Computers
//		© 2008 - 2008...
//
//	if not, I have no idea who wrote it.
//
// ******************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PS3MAPI_NCAPI
{
	public partial class LogDialog : Form
	{
        PS3MAPI PS3MAPI = null;
        public LogDialog()
		{
			InitializeComponent();
		}

        public LogDialog(PS3MAPI MyPS3MAPI)
            : this()
        {
            PS3MAPI = MyPS3MAPI;
        }

        private void LogDialog_Refresh(object sender, EventArgs e)
        {
            if (PS3MAPI != null) tB_Log.Text = PS3MAPI.Log;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
	}
}
