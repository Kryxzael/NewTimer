using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CircleControl;

namespace NewTimer.FormParts.Setup
{
    public partial class Knob : CircleProgressControl
    {
        public event EventHandler ValueChanged;

        public Knob()
        {
            InitializeComponent();
            CircleTrackColor = Color.FromArgb(10, 10, 10);
            Boldness = 15f;
        }

        int _minValue = 0;
        public virtual int MinValue
        {
            get => _minValue;
            set
            {
                if (_minValue > MaxValue)
                {
                    throw new InvalidOperationException("MinValue cannot be greater than MaxValue");
                }

                _minValue = value;
                SetValueClamped(value);
            }
        }


        int _step = 1;
        public virtual int Step
        {
            get => _step;
            set
            {
                _step = Math.Max(1, value);
            }
        }

        public virtual bool ReadOnly { get; set; }

        private string _text;
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate();
            }
        }

        private Font _numberFont = new Font(DefaultFont.FontFamily, 32f, FontStyle.Bold);
        public virtual Font NumberFont
        {
            get => _numberFont;
            set
            {
                _numberFont = value ?? throw new ArgumentNullException();
                Invalidate();
            }
        }

        protected virtual string GetLabelString()
        {
            return Text;
        }

        protected virtual string GetValueString()
        {
            return Value.ToString();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (SolidBrush foreBrush = new SolidBrush(ForeColor))
            {
                //Draw title
                e.Graphics.DrawString(
                    s: GetLabelString(), 
                    font: Font, 
                    brush: foreBrush, 
                    layoutRectangle: new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y + e.ClipRectangle.Height / 5, e.ClipRectangle.Width, e.ClipRectangle.Height / 5),
                    format: new StringFormat() { Alignment = StringAlignment.Center }
                );

                //Draw number
                e.Graphics.DrawString(
                    s: GetValueString(),
                    font: NumberFont,
                    brush: foreBrush,
                    layoutRectangle: e.ClipRectangle,
                    format: new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
                );
            }



        }

        private void SetValueClamped(int value)
        {
            Value = Math.Min(Math.Max(MinValue, value), MaxValue);
            ValueChanged?.Invoke(this, new EventArgs());
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            //Knob is readonly. Ignore input
            if (ReadOnly)
            {
                return;
            }

            //Handle mouse press
            switch (e.Button)
            {
                //Increment the value
                case MouseButtons.Left:
                    if (ModifierKeys.HasFlag(Keys.Shift))
                    {
                        goto case MouseButtons.Middle;
                    }
                    SetValueClamped((int)Value + Step);
                    break;

                //Decrement the value
                case MouseButtons.Right:
                    SetValueClamped((int)Value - Step);
                    break;

                //Reset the value
                case MouseButtons.Middle:
                    SetValueClamped(MinValue);
                    break;
            }
        }
    }
}
