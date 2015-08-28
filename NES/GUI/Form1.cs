using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace NES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            new NES_CPU();
            //NES_ROM.LoadRom(@"F:\roms\thwaite.nes");
            NES_ROM.LoadRom(@"F:\roms\Dendy\galaga.nes");
            //NES_ROM.LoadRom(@"F:\roms\Dendy\ICE_HOCK.nes");
            //NES_ROM.LoadRom(@"F:\roms\Dendy\FCEUX\test.nes");
            backgroundWorker1.RunWorkerAsync();
            Form2 f = new Form2();
            f.Show();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            NES_CPU.Run();
        }

        int PN = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (PN < 10)
                PN++;
            else
                PN = 0; 
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (NES_Register.POWER)
                NES_Register.POWER = false;
            else
                backgroundWorker1.RunWorkerAsync();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = NES_PPU.Patterntable(PN);
            pictureBox2.Image = NES_PPU.PaletteTable();
            BackColor = NES_PPU.UniversalBackgroundColor();
        }
    }
}
