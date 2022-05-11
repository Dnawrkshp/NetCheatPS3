using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginInterface;

namespace NCMemBrowser
{
    public partial class ctlMain : UserControl
    {
        public int maxSize = 0x40;
        public ulong pstartAddr = 0x017F4000;
        private bool skipResizeUpdate = false;
        // Declare a Name property of type string:
        public ulong startAddr
        {
            get
            {
                return pstartAddr;
            }
            set
            {
                pstartAddr = value;
                UpdateAddr = true;
            }
        }

        public byte[] dataArray;
        public hexGrid[] dataGrids;
        public addrGrid[] addrGrids;
        public bool ColorUpdatedValues = false;
        Timer tRef = new Timer();
        public bool UpdateAddr = true;

        public static IPluginHost NCInterface = null;

        public ctlMain()
        {
            InitializeComponent();

            dataArray = new byte[maxSize];
            dataGrids = new hexGrid[maxSize / 4];
            addrGrids = new addrGrid[maxSize / 4];

            addrGrids[0] = addrGrid1;
            addrGrids[0].GotoAddress += new addrGrid.GotoAddressEventHandler(addr_HandleGotoAddr);

            dataGrids[0] = hexGrid1;
            dataGrids[0].NextRow += new hexGrid.NextRowEventHandler(byte_HandleNextRow);
            dataGrids[0].ByteChanged += new hexGrid.ByteChangedEventHandler(byte_HandleByteChange);
            dataGrids[0].FocusChanged += new hexGrid.FocusChangedEventHandler(byte_HandleFocusChange);

            initGridControls();
        }

        private void initGridControls()
        {
            Array.Resize(ref dataArray, maxSize);
            Array.Resize(ref dataGrids, maxSize / 4);
            Array.Resize(ref addrGrids, maxSize / 4);

            memPanel.Controls.Clear();
            skipResizeUpdate = true;

            Point loc = new Point(3, 3);

            for (int x = 0; x < (maxSize / 4); x++)
            {
                addrGrids[x] = new addrGrid();
                addrGrids[x].GotoAddress += new addrGrid.GotoAddressEventHandler(addr_HandleGotoAddr);
                addrGrids[x].Size = new Size(addrGrid1.Size.Width, addrGrid1.Size.Height);
                addrGrids[x].Location = loc;
                addrGrids[x].Visible = true;
                addrGrids[x].Font = Font;
                addrGrids[x].Tag = x.ToString();

                dataGrids[x] = new hexGrid();
                dataGrids[x].NextRow += new hexGrid.NextRowEventHandler(byte_HandleNextRow);
                dataGrids[x].ByteChanged += new hexGrid.ByteChangedEventHandler(byte_HandleByteChange);
                dataGrids[x].FocusChanged += new hexGrid.FocusChangedEventHandler(byte_HandleFocusChange);
                dataGrids[x].Size = new Size(hexGrid1.Size.Width, hexGrid1.Size.Height);
                dataGrids[x].Location = new Point(loc.X + addrGrid1.Width + 10, loc.Y);
                dataGrids[x].Visible = true;
                dataGrids[x].Font = Font;
                dataGrids[x].Tag = x.ToString();

                memPanel.Controls.Add(dataGrids[x]);
                memPanel.Controls.Add(addrGrids[x]);

                dataGrids[x].BackColor = BackColor;
                dataGrids[x].ForeColor = ForeColor;
                addrGrids[x].BackColor = BackColor;
                addrGrids[x].ForeColor = ForeColor;

                loc.Y += hexGrid1.Height;
            }

            memPanel.Size = new Size(memPanel.Size.Width, loc.Y);
            memPanel.Location = new Point(3, 3);
            Height = loc.Y + 6;
            skipResizeUpdate = false;
        }

        private void byte_HandleNextRow(int index, int direction)
        {
            if (direction > 0)
            {
                if (index < ((maxSize / 4) - 1))
                {
                    dataGrids[index + 1].Focus();
                    dataGrids[index + 1].SetFocus(1, 0);
                }
                else
                {

                }
            }
            else
            {
                if (index > 0)
                {
                    dataGrids[index - 1].Focus();
                    dataGrids[index - 1].SetFocus(4, 1);
                }
                else
                {

                }
            }
        }

        private void byte_HandleFocusChange(object sender, int index, int pos, int direction)
        {
            int arrayIndex = Convert.ToInt32(((hexGrid)sender).Tag);
            if (direction > 0)
            {
                if (arrayIndex == ((maxSize / 4) - 1)) {
                    if (pos != 0)
                        pos--;
                    dataGrids[arrayIndex].SetFocus(index, pos);
                    startAddr += (ulong)(direction * 4);
                }
                else if (arrayIndex < ((maxSize / 4) - 1))
                {
                    dataGrids[arrayIndex + direction].Focus();
                    dataGrids[arrayIndex + direction].SetFocus(index, pos);
                }
            }
            else
            {
                if (arrayIndex == 0)
                {
                    dataGrids[arrayIndex].SetFocus(index, pos);
                    startAddr += (ulong)(direction * 4);
                }
                else if (arrayIndex > 0)
                {
                    dataGrids[arrayIndex + direction].Focus();
                    dataGrids[arrayIndex + direction].SetFocus(index, pos);
                }
            }
            ColorUpdatedValues = true;
            refresh_TickEvent(null, null);
        }

        private void byte_HandleByteChange(object sender, int index, byte[] val)
        {
            uint addr = (Convert.ToUInt32(((hexGrid)sender).Tag) * 4) + (uint)startAddr + (uint)index - 1;
            NCInterface.SetMemory((ulong)addr, val);
        }

        private void ctlMain_Load(object sender, EventArgs e)
        {
            refresh_TickEvent(null, null);
            tRef.Interval = 250;
            tRef.Enabled = true;
            tRef.Tick += new EventHandler(refresh_TickEvent);
            tRef.Start();

            dataGrids[0].Focus();
            dataGrids[0].SetFocus(1, 0);
        }

        private void refresh_TickEvent(object sender, EventArgs e)
        {
            dataArray = NCInterface.GetMemory(startAddr, maxSize);
            for (uint x = 0; x < maxSize; x += 4)
            {
                if (UpdateAddr)
                    addrGrids[x / 4].SetText((startAddr + x).ToString("X8"));
                dataGrids[x / 4].SetByte(0, new byte[] { dataArray[x], dataArray[x + 1], dataArray[x + 2], dataArray[x + 3] }, ColorUpdatedValues);
            }
            if (UpdateAddr)
                UpdateAddr = false;
            ColorUpdatedValues = false;
        }

        private void addr_HandleGotoAddr(object sender, uint index)
        {
            RichTextBox rtb = (RichTextBox)sender;

            ulong newAddr = ulong.Parse(rtb.Text, System.Globalization.NumberStyles.HexNumber);
            newAddr -= index * 4;
            startAddr = newAddr;
        }

        private void memSizeVal_ValueChanged(object sender, EventArgs e)
        {
            memSizeVal.Value -= memSizeVal.Value % 4;
            maxSize = (int)memSizeVal.Value;
            initGridControls();
            UpdateAddr = true;
        }

        private void ctlMain_ClientSizeChanged(object sender, EventArgs e)
        {
            //Calculate the memory size
            int val = (Height / dataGrids[0].Height) * 4;
            if (maxSize != val && !skipResizeUpdate)
            {
                maxSize = val;
                initGridControls();
                UpdateAddr = true;
                memSizeVal.Value = (decimal)maxSize;
            }
        }

        private void refMSVal_ValueChanged(object sender, EventArgs e)
        {
            if (refMSVal.Value == 0)
                tRef.Stop();
            else
            {
                tRef.Interval = (int)refMSVal.Value;
                tRef.Start();
            }
        }

        private void refButton_Click(object sender, EventArgs e)
        {
            refresh_TickEvent(null, null);
        }
    }
}
