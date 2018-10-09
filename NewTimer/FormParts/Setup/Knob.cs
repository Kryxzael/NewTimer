using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts.Setup
{
    public partial class Knob : UserControl
    {
        public Knob()
        {
            InitializeComponent();
            lblText.BackColor = Color.Transparent;
            lblValue.BackColor = Color.Transparent;
            lblText.MouseDown += (s, e) => OnMouseDown(e);
            lblValue.MouseDown += (s, e) => OnMouseDown(e);
        }


        public event EventHandler ValueChanged;

        int _minValue;
        public int MinValue
        {
            get => _minValue;
            set
            {
                _minValue = Math.Min(MaxValue - 1, value);
                Value = Value;
            }
        }

        int _maxValue = 10;
        public int MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = Math.Max(MinValue + 1, value);
                Value = Value;
            }
        }

        int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = Math.Max(MinValue, Math.Min(MaxValue, value));
                ValueChanged?.Invoke(this, new EventArgs());
                Refresh();
            }
        }

        int _step = 1;
        public int Step
        {
            get => _step;
            set
            {
                _step = Math.Max(1, value);
            }
        }

        public bool RenderPlussOne { get; set; }

        string _text;
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get => _text;
            set
            {
                lblText.Text = value;
                _text = value;
                Refresh();
            }
        }

        public bool ReadOnly { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawKnob(e.Graphics);
        }

        public override void Refresh()
        {
            base.Refresh();
            lblValue.Text = Value.ToString();

        }

        void DrawKnob(Graphics g)
        {
            const int W = 10;

            using (Brush bg = new SolidBrush(Color.FromArgb(0x22, 0x22, 0x22)))
            using (Brush b = new SolidBrush(Color.FromArgb(0x44, 0x44, 0x44)))
            using (Brush f = new SolidBrush(ForeColor))
            {
                g.FillEllipse(b, 0, 0, Width, Height);
                g.FillPie(f, new Rectangle(0, 0, Width, Height), -90, ((Value - MinValue) / (float)(MaxValue - MinValue + (RenderPlussOne ? 1 : 0))) * 360);
                g.FillEllipse(bg, W, W, Width - 2 * W, Height - 2 * W);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (ReadOnly)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                Value += Step;
            }
            else if (e.Button == MouseButtons.Right)
            {
                Value -= Step;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                Value = MinValue;
            }
        }
    }
}
