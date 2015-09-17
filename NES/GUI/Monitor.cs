using System;
using System.Windows.Forms;

namespace NES
{
    public partial class Monitor : Form
    {
        public Monitor()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
                pictureBox1.Image = NES_PPU.Display();
                BackColor = NES_PPU_Palette.UniversalBackgroundColor();
        }
    }
}
