using System;
using System.Windows.Forms;

namespace NES
{
    public partial class NameTable : Form
    {
        public NameTable()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = NES_Console.getNameTabele(false);
            BackColor = NES_Console.getUniversalBackgroundColor();
        }
    }
}
