using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewTimer
{
    public class DigitalNumber : PictureBox
    {
        const int C = 220;

        Graphics g;

        bool IMAGE_OVERRIDE = false;


        /// <summary>
        /// Creates a new digit control
        /// </summary>
        public DigitalNumber()
        {
            DigitCount = 1;
            BackgroundImageLayout = ImageLayout.Stretch;

        }

        int dCount = 1;
        /// <summary>
        /// Gets or sets the amount of digits this number control will have
        /// </summary>
        public int DigitCount
        {
            get
            {
                return dCount;
            }
            set
            {
                if (value <= 0)
                {
                    value = 1;
                }
                dCount = value;


                IMAGE_OVERRIDE = true;

                BackgroundImage?.Dispose();
                g?.Dispose();
                BackgroundImage = new Bitmap(C * value, 353);
                g = Graphics.FromImage(BackgroundImage);

                Refresh();

                IMAGE_OVERRIDE = false;
            }
        }

        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }

            set
            {
                if (!IMAGE_OVERRIDE)
                {
                    return;
                }
                base.BackgroundImage = value;
            }
        }

        int val;

        /// <summary>
        /// Gets or sets the value of this digitalnumber control
        /// </summary>
        public int Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the color used to display the active components of this number control
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }

            set
            {
                base.ForeColor = value;
                Refresh();
            }
        }

        Color deaColor = Color.FromArgb(32, Color.Black);
        /// <summary>
        /// Gets or sets the color used to draw deactivated components in this number control
        /// </summary>
        public Color DeactivatedColor
        {
            get
            {
                return deaColor;
            }
            set
            {
                deaColor = value;
                Refresh();
            }
        }

        bool shwLead0;

        /// <summary>
        /// Gets or sets wether this control should fill all blank spaces with zeros
        /// </summary>
        public bool ShowLeadingZeros
        {
            get
            {
                return shwLead0;
            }
            set
            {
                shwLead0 = value;
                Refresh();
            }
        }



        /// <summary>
        /// Updates the image of the control
        /// </summary>
        public override void Refresh()
        {
            g.Clear(Color.Transparent);
            string numstr = Value.ToString().PadLeft(DigitCount, ShowLeadingZeros ? '0' : ' ');

            if (numstr.Length > DigitCount)
            {
                numstr = new string('9', DigitCount);
            }

            for (int i = 0; i < DigitCount; i++)
            {
                Bitmap tmp = Colorize(GetDigitImage(numstr[i]), ForeColor, DeactivatedColor);
                g.DrawImage(tmp, i * C, 0);
                tmp.Dispose();
                GC.Collect();
            }

            base.Refresh();
        }

        /// <summary>
        /// Sets the color scheme of a digit
        /// </summary>
        /// <param name="originalImage">Image to colorize</param>
        /// <returns></returns>
        static Bitmap Colorize(Bitmap originalImage, Color foreColor, Color backColor)
        {
            //Create output bitmap
            Bitmap _ = new Bitmap(originalImage.Width, originalImage.Height);

            //Create background color remapper
            ColorMap bgMap = new ColorMap();
            bgMap.OldColor = Color.FromArgb(0xFF, 0xFF, 0xFF);
            bgMap.NewColor = backColor;

            //Create foreground color remapper
            ColorMap fgMap = new ColorMap();
            fgMap.OldColor = Color.FromArgb(0x0, 0x0, 0x0);
            fgMap.NewColor = foreColor;

            //Create image attributes object from colormaps
            ImageAttributes attr = new ImageAttributes();
            attr.SetRemapTable(new ColorMap[] { bgMap, fgMap });

            //Draw original image to new image using attributes created
            Graphics g = Graphics.FromImage(_);
            g.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, attr);
            g.Dispose();
            attr.Dispose();

            return _;

        }

        /// <summary>
        /// Returns a single digit image
        /// </summary>
        /// <param name="n"></param>
        static Bitmap GetDigitImage(char n)
        {
            switch (n)
            {
                case '0':
                    return Properties.Resources.zero;
                case '1':
                    return Properties.Resources.one;
                case '2':
                    return Properties.Resources.two;
                case '3':
                    return Properties.Resources.three;
                case '4':
                    return Properties.Resources.four;
                case '5':
                    return Properties.Resources.five;
                case '6':
                    return Properties.Resources.six;
                case '7':
                    return Properties.Resources.seven;
                case '8':
                    return Properties.Resources.eight;
                case '9':
                    return Properties.Resources.nine;
                case ' ':
                    return Properties.Resources._null;
                case '.':
                    return Properties.Resources.periodSmal;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
