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
    /// <summary>
    /// A knob control that holds an integer value. Used for the settup window
    /// </summary>
    public partial class Knob : CircleProgressControl
    {
        /// <summary>
        /// Fired when the value of this knob is changed
        /// </summary>
        public event EventHandler ValueChanged;

        /*
         * Private backing fields
         */
        private int _minValue = 0;
        private int _step = 1;
        private string _text;
        private Font _numberFont = new Font(DefaultFont.FontFamily, 32f, FontStyle.Bold);

        public Knob()
        {
            InitializeComponent();

            //Sets superclass's settings
            CircleTrackColor = Color.FromArgb(10, 10, 10);
            Boldness = 15f;
        }


        /// <summary>
        /// The minimum value of this knob
        /// </summary>
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

        /// <summary>
        /// The amount the knob's value will change by when clicked
        /// </summary>
        public virtual int Step
        {
            get => _step;
            set
            {
                _step = Math.Max(1, value);
            }
        }

        /// <summary>
        /// Is the knob uneditable?
        /// </summary>
        public virtual bool ReadOnly { get; set; }

        
        /// <summary>
        /// Gets or sets the label text of the knob
        /// </summary>
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

        /// <summary>
        /// Gets or sets the font that will be used to render the number on the knob
        /// </summary>
        public virtual Font NumberFont
        {
            get => _numberFont;
            set
            {
                _numberFont = value ?? throw new ArgumentNullException();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the string that will be displayed as the title on the knob
        /// </summary>
        /// <returns></returns>
        protected virtual string GetLabelString()
        {
            return Text;
        }

        /// <summary>
        /// Gets the string that will be displayed as the number on the knob
        /// </summary>
        /// <returns></returns>
        protected virtual string GetValueString()
        {
            return Value.ToString();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            //Draws the ring. Done in superclass
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

        /// <summary>
        /// Sets the value of the knob, making sure it's in range
        /// </summary>
        /// <param name="value"></param>
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
