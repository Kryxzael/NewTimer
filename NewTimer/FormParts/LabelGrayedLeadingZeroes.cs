using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer.FormParts
{
    /// <summary>
    /// A standard label that will gray out leading zeros
    /// </summary>
    public class LabelGrayedLeadingZeros : Label
    {

        /*
         * Private Backing fields
         */

        private Color _leadingZeros = Color.Gray;
        private float _progress;
        private bool _renderLeadingZeros;
        private Color _hiColor;

        private string _oldText;
        private Dictionary<int, DigitRollOverAnimation> _animations = new Dictionary<int, DigitRollOverAnimation>();

        /// <summary>
        /// If set, animations will never play
        /// </summary>
        public static bool BypassAllAnimations { get; set; }

        /// <summary>
        /// Get or sets the amount of the label that should be colored with highlighting color. Intended range is 0 through 1
        /// </summary>
        public float Progress
        {
            get => _progress;
            set => _progress = Math.Max(0, Math.Min(1, value));
        }

        /// <summary>
        /// Gets or sets whether the animation should progress up or down
        /// </summary>
        public bool RollUp { get; set; }

        /// <summary>
        /// Gets or sets the animation time of each roll. Set to 0 for instant change
        /// </summary>
        public TimeSpan AnimationTime { get; set; }

        /// <summary>
        /// Gets or sets the leading zeros' color
        /// </summary>
        public Color LeadingZerosColor
        {
            get => _leadingZeros;
            set
            {
                _leadingZeros = value;
                Invalidate();
            }
        }


        /// <summary>
        /// False by default. If true, leading zeros will be rendered
        /// </summary>
        public bool RenderLeadingZeros
        {
            get => _renderLeadingZeros;
            set
            {
                _renderLeadingZeros = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color that will be used for progress highlighting
        /// </summary>
        public Color HighlightColor
        {
            get => _hiColor;
            set
            {
                _hiColor = value;
                Refresh();
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            for (int i = 0; i < Math.Min(_oldText?.Length ?? 0, Text.Length); i++)
            {
                if (Text[i] != _oldText[i])
                    _animations[i] = new DigitRollOverAnimation(_oldText[i], AnimationTime);
            }

            _oldText = Text;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int offset = 0;
            bool renderGrays = !RenderLeadingZeros;

            /*
             * It might be weird that the background text code uses the highlight color and vice versa.
             * This is to make the effect go up and not down
             * It was done on purpose
             */

            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                _animations.TryGetValue(i, out DigitRollOverAnimation animation);

                Rectangle offsetClipRect = e.ClipRectangle;
                offsetClipRect.X += offset;
                offsetClipRect.Width -= offset;

                if (c != '0')
                {
                    renderGrays = false;
                }

                if (animation != null && animation.AnimationProgress >= 1f)
                {
                    animation = null;
                    _animations.Remove(i);
                }    

                if (animation == null)
                {
                    drawDigit(c, offsetClipRect, true);
                }
                else
                {
                    Rectangle oldDigitRect = offsetClipRect;
                    Rectangle newDigitRect = offsetClipRect;

                    if (!BypassAllAnimations)
                    {
                        if (RollUp)
                        {
                            oldDigitRect.Y -= (int)(oldDigitRect.Height * animation.AnimationProgress);
                            newDigitRect.Y -= (int)(newDigitRect.Height * animation.AnimationProgress);
                            newDigitRect.Y += oldDigitRect.Height;
                        }
                        else
                        {
                            oldDigitRect.Y += (int)(oldDigitRect.Height * animation.AnimationProgress);
                            newDigitRect.Y += (int)(newDigitRect.Height * animation.AnimationProgress);
                            newDigitRect.Y -= oldDigitRect.Height;
                        }

                        drawDigit(animation.FromValue, oldDigitRect, false);
                    }

                    drawDigit(c, newDigitRect, true);
                }
                

                void drawDigit(char digit, Rectangle bounds, bool addToMessurements)
                {
                    //Draws the "background text"
                    TextRenderer.DrawText(
                        dc: e.Graphics,
                        text: digit.ToString(),
                        font: Font,
                        bounds: bounds,
                        foreColor: renderGrays ? LeadingZerosColor : HighlightColor,
                        flags: TextFormatFlags.NoPadding
                    );

                    //AdjustedProgress takes the visible height of the font into account.
                    //The font-height and the visible part of its characters are different
                    //The numbers below have been painstakingly picked and tweaked for the best result.
                    //They don't have any derivable meaning, so don't go looking for it
                    float adjustedProgress = (_progress * 0.725f) + 0.185f;
                    bounds.Height = (int)(bounds.Height * adjustedProgress);

                    //Draw the char
                    TextRenderer.DrawText(
                        dc: e.Graphics,
                        text: digit.ToString(),
                        font: Font,
                        bounds: bounds,
                        foreColor: renderGrays ? LeadingZerosColor : ForeColor,
                        flags: TextFormatFlags.NoPadding
                    );

                    if (addToMessurements)
                    {
                        //Add char's width to offset variable
                        offset += TextRenderer.MeasureText(
                            dc: e.Graphics,
                            text: digit.ToString(),
                            font: Font,
                            proposedSize: offsetClipRect.Size,
                            flags: TextFormatFlags.NoPadding
                        ).Width;
                    }
                }
            }
        }

        private class DigitRollOverAnimation
        {
            public char FromValue  { get; }

            public float AnimationProgress
            {
                get
                {
                    long now = DateTime.Now.Ticks;
                    long start = AnimationStart.Ticks;
                    long end = AnimationEnd.Ticks;

                    float duration = (now - start) / (float)(end - start);
                    return Math.Max(Math.Min(1, duration), 0);
                }
            }

            private DateTime AnimationStart { get; }
            private DateTime AnimationEnd { get; }

            public DigitRollOverAnimation(char fromValue, TimeSpan animationDuration)
            {
                FromValue = fromValue;
                AnimationStart = DateTime.Now;
                AnimationEnd = AnimationStart + animationDuration;
            }
        }
    }
}
