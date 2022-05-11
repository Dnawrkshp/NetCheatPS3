using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace PCAPI_NCAPI
{
    public partial class AttachForm : Form
    {
        List<Process> procs;

        public int returnProcessID = -1;

        public AttachForm()
        {
            InitializeComponent();
        }

        private void AttachForm_Load(object sender, EventArgs e)
        {
            procs = Process.GetProcesses().ToList();
            procs.Sort((a, b) => a.Id.CompareTo(b.Id));

            foreach (Process p in procs)
            {
                listBox1.Items.Add(p.Id.ToString("X8") + "-" + p.ProcessName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            returnProcessID = procs[listBox1.SelectedIndex].Id;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            returnProcessID = -1;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            procs = Process.GetProcesses().ToList();
            procs.Sort((a, b) => a.Id.CompareTo(b.Id));

            foreach (Process p in procs)
            {
                listBox1.Items.Add(p.Id.ToString("X8") + "-" + p.ProcessName);
            }
        }
    }
}
