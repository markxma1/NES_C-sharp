using System;
using System.Windows.Forms;

namespace NES
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = NES_PPU.NameTabele();
            BackColor = NES_PPU_Palette.UniversalBackgroundColor();
        }
    }
}
