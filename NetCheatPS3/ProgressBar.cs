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
    public partial class ProgressBar : UserControl
    {
        public static bool progTaskBarError = false;

        private Color _progressColor = DefaultForeColor;
        public Color progressColor
        {
            get { return _progressColor; }
            set
            {
                _progressColor = value;
                Refresh();
            }
        }

        private string _printText = "";
        public string printText
        {
            get { return _printText; }
            set
            {
                _printText = value;
                Refresh();
            }
        }

        private int _value = 0;
        public int Value
        {
            get { return _value; }
            set
            {
                if (Form1.Instance != null)
                {
                    if (value == 0 && _value > 0)
                    {
                        if (Form1.Instance.ContainsFocus)
                        {
                            TaskbarProgress.SetState(Form1.Instance.Handle, TaskbarProgress.TaskbarStates.NoProgress);
                        }
                        else
                        {
                            TaskbarProgress.SetState(Form1.Instance.Handle, TaskbarProgress.TaskbarStates.Paused);
                            progTaskBarError = true;
                            TaskbarProgress.SetValue(Form1.Instance.Handle, 1, 1);
                        }
                    }
                    else
                    {
                        TaskbarProgress.SetState(Form1.Instance.Handle, TaskbarProgress.TaskbarStates.Indeterminate);
                        TaskbarProgress.SetValue(Form1.Instance.Handle, value, this.Maximum);
                    }
                }

                _value = value;
                Refresh();
            }
        }

        private int _maximum = 0;
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                Refresh();
            }
        }

        public void Increment(int inc)
        {
            if ((Value + inc) <= Maximum)
                Value += inc;
        }

        public ProgressBar()
        {
            InitializeComponent();
        }

        private void ProgressBar_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void ProgressBar_Resize(object sender, EventArgs e)
        {
            pictureBox1.Size = Size;
            pictureBox1.Location = new Point(0, 0);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (_maximum > 0)
            {
                float ratio = ((float)Value / (float)Maximum);

                //Draw background text
                Brush brush = new SolidBrush(ForeColor);
                string text = (ratio * 100f).ToString("0.00") + "%";
                if (printText != "")
                    text += " - " + printText;

                SizeF strSize = e.Graphics.MeasureString(text, Font);
                Rectangle rect = new Rectangle((Width / 2) - (int)(strSize.Width / 2), (Height / 2) - (int)(strSize.Height / 2), Width, Height);
                e.Graphics.DrawString(text, Font, brush, rect);

                //Draw progress
                brush = new SolidBrush(ForeColor);
                rect = new Rectangle(0, 0, (int)(ratio * Width), Height);
                e.Graphics.FillRectangle(brush, rect);

                //Draw foreground text
                int dif = (int)((int)(ratio * Width) - strSize.Width);
                if (dif > 0)
                {
                    brush = new SolidBrush(BackColor);
                    e.Graphics.Clip = new Region(rect);
                    RectangleF rectf = new RectangleF((Width / 2) - (int)(strSize.Width / 2), (Height / 2) - (int)(strSize.Height / 2), Width, Height);
                    e.Graphics.DrawString(text, Font, brush, rectf, new StringFormat(StringFormatFlags.NoWrap));
                }
            }
        }

        Color inverseColor(Color col)
        {
            return Color.FromArgb(255 - col.R, 255 - col.B, 255 - col.G);
        }

    }
}
