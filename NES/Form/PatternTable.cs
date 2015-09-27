using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NES
{
    public partial class PatternTable : Form
    {
        public PatternTable()
        {
            InitializeComponent();
        }

        int PN = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (PN < 10)
                PN++;
            else
                PN = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = NES_Console.getPatternTable(PN);
            pictureBox2.Image = NES_Console.getPaletteTable();
            BackColor = NES_Console.getUniversalBackgroundColor();
        }
    }
}
