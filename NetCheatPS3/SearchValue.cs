using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetCheatPS3
{
    public partial class SearchValue : UserControl
    {
        public SearchValue()
        {
            InitializeComponent();
        }

        private Color _fore = Color.Black;
        private Color _back = Color.White;
        public string TagValue = "";

        public Color Fore
        {
            get { return _fore; }
            set
            {
                _fore = value;
                nameLabel.ForeColor = _fore;
                boolBox.ForeColor = _fore;
                valBox.ForeColor = _fore;
            }
        }

        public Color Back
        {
            get { return _back; }
            set
            {
                _back = value;
                nameLabel.BackColor = _back;
                boolBox.BackColor = _back;
                valBox.BackColor = _back;
            }
        }

        private SearchControl.CheckboxConvert _cboxConvert;
        private bool _defVal;
        public void SetSValue(string name, string value, string cboxName, bool defVal, bool curState, SearchControl.CheckboxConvert cboxConvert)
        {
            nameLabel.Text = name;
            valBox.Text = value;

            if (cboxName != "")
            {
                boolBox.Text = cboxName;
                boolBox.Checked = curState;
            }
            else
                boolBox.Visible = false;

            _cboxConvert = cboxConvert;
            _defVal = defVal;

            SearchValue_Resize(null, null);
        }

        public string getValue()
        {
            return valBox.Text;
        }

        public string GetDefValue()
        {
            if (boolBox.Checked != _defVal && boolBox.Visible)
                return _cboxConvert.Invoke(valBox.Text, _defVal);
            else
                return valBox.Text;
        }

        public bool GetState()
        {
            return boolBox.Checked;
        }

        private void boolBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_cboxConvert != null)
            {
                valBox.Text = _cboxConvert.Invoke(valBox.Text, boolBox.Checked);
            }
        }

        private void SearchValue_Resize(object sender, EventArgs e)
        {
            nameLabel.Location = new Point(5, 2);
            boolBox.Location = new Point(Width - boolBox.Width - 5, 2);
            valBox.Location = new Point(nameLabel.Width + 10, 0);
            if (boolBox.Visible)
                valBox.Width = Width - (valBox.Location.X + boolBox.Width + 10);
            else
                valBox.Width = Width - (valBox.Location.X + 5);

            Height = nameLabel.Height + 10;
        }
    }
}
