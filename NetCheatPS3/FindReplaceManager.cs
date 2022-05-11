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
    public partial class FindReplaceManager : Form
    {
        SearchListView searchListView1 = new SearchListView();
        List<byte[]> currentItemValues = new List<byte[]>();

        bool useTime = false;

        public FindReplaceManager()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(thisFormClosing);
        }

        void thisFormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            

            e.Cancel = true;
        }

        public void AddItem(uint addr, byte[] ogp, byte[] cop)
        {
            SearchListView.SearchListViewItem slvi = new SearchListView.SearchListViewItem();
            if (useTime)
                slvi.align = (DateTime.Now.Hour << 24) | (DateTime.Now.Minute << 16) | (DateTime.Now.Second);
            slvi.addr = addr;
            slvi.oldVal = ogp;
            slvi.newVal = cop;
            currentItemValues.Add(cop);
            searchListView1.AddItemRange(new SearchListView.SearchListViewItem[] { slvi });
            searchListView1.Refresh();
        }

        private void FindReplaceManager_Shown(object sender, EventArgs e)
        {
            searchListView1.VerticalScroll.Value = searchListView1.TotalCount;
        }

        private void FindReplaceManager_Resize(object sender, EventArgs e)
        {
            searchListView1.Size = new Size(this.Size.Width - 14, this.Size.Height - 38);
        }

        private void FindReplaceManager_Load(object sender, EventArgs e)
        {
            if (useTime)
                searchListView1.SetColumnNames(new string[] { "Address", "Original", "New", "Time" });
            else
                searchListView1.SetColumnNames(new string[] { "Address", "Original", "New", "Current" });
            searchListView1.Location = new Point(0, 0);
            searchListView1.Size = new Size(this.Size.Width - 14, this.Size.Height - 38);
            searchListView1.SetCMenuStrip(contextMenuStrip1);
            searchListView1.SetParseItem(ParseItem);
            searchListView1.multiSelect = false;
            searchListView1.isUsingListA = true;
            this.Controls.Add(searchListView1);
        }

        public string[] ParseItem(SearchListView.SearchListViewItem item, int ind)
        {
            string[] ret = new string[4];

            ret[0] = item.addr.ToString("X8");
            ret[1] = misc.ByteAToStringHex(item.oldVal, "");
            ret[2] = misc.ByteAToStringHex(item.newVal, "");
            if (useTime)
            {
                int hour = item.align >> 24;
                int minute = (item.align >> 16) & 0xFF;
                int seconds = item.align & 0xFFFF;
                ret[3] = hour.ToString() + ":" + minute.ToString().PadLeft(2, '0') + ":" + seconds.ToString().PadLeft(2, '0');
            }
            else
            {
                if (currentItemValues[ind].Length == 0)
                    RefreshItemCurrent(item, ind);
                ret[3] = misc.ByteAToStringHex(currentItemValues[ind], "");
            }

            return ret;
        }

        void RefreshItemCurrent(SearchListView.SearchListViewItem item, int ind)
        {
            byte[] curr = new byte[item.oldVal.Length];
            Form1.apiGetMem((ulong)item.addr, ref curr);
            currentItemValues[ind] = curr;
        }

        #region Context Menu Strip

        private void revertToOriginalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (searchListView1.SelectedIndices.Count <= 0)
                return;
            int ind = searchListView1.SelectedIndices[0];

            SearchListView.SearchListViewItem item = searchListView1.GetItemAtIndex(ind);
            Form1.apiSetMem((ulong)item.addr, item.oldVal);
            RefreshItemCurrent(item, ind);
            searchListView1.Refresh();
        }

        private void rewriteNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (searchListView1.SelectedIndices.Count <= 0)
                return;
            int ind = searchListView1.SelectedIndices[0];

            SearchListView.SearchListViewItem item = searchListView1.GetItemAtIndex(ind);
            Form1.apiSetMem((ulong)item.addr, item.newVal);
            RefreshItemCurrent(item, ind);
            searchListView1.Refresh();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (searchListView1.SelectedIndices.Count <= 0)
                return;
            int ind = searchListView1.SelectedIndices[0];
            searchListView1.RemoveItemAt(ind);
            currentItemValues.RemoveAt(ind);
            searchListView1.ClearSelectedIndices();
            searchListView1.Refresh();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            searchListView1.ClearSelectedIndices();
            currentItemValues.Clear();
            searchListView1.ClearItems();
            searchListView1.Refresh();
        }

        private void refreshAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (searchListView1.SelectedIndices.Count <= 0)
                return;
            for (int x = 0; x < searchListView1.TotalCount; x++)
                RefreshItemCurrent(searchListView1.GetItemAtIndex(x), x);
            searchListView1.Refresh();
        }

        #endregion

    }
}
