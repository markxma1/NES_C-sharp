using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bitmapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Bitmap bm = new Bitmap(255,255,PixelFormat.Format16bppArgb1555);
            for (int i = 0; i < 255; i++)
            {
                for (int j = 0;j < 255; j++)
                {
                    bm.SetPixel(i, j, Color.FromArgb(i,j,(i+j)/2));
                }
            }

            for (int i = 0; i < 255; i++)
            {
                for (int j = 0; j < 255; j++)
                {
                    if (i % 2 == j % 3)
                    {
                        Color c = bm.GetPixel(i, j);
                        bm.SetPixel(i, j, Color.FromArgb(255-c.G,255- c.B,255- c.R));
                    }
                }
            }

            pictureBox1.Image=bm;
        }
    }
}
