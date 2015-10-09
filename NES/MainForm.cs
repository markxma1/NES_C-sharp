using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NES
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            NES_Console.INIT();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            NES_Console.Run();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            NES_Console.Stop();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void Load_Click(object sender, EventArgs e)
        {
            //NES_ROM.LoadRom(@"F:\roms\thwaite.nes");
            NES_Console.LoadRom(@".\Galaga.nes");
            //NES_ROM.LoadRom(@"F:\roms\Dendy\ICE_HOCK.nes");
            //NES_ROM.LoadRom(@"F:\roms\Dendy\FCEUX\test.nes");
        }

        private void Display_Click(object sender, EventArgs e)
        {
            Monitor m = new Monitor();
            m.Show();
        }

        private void NameTable_Click(object sender, EventArgs e)
        {
            NameTable f = new NameTable();
            f.Show();
        }

        private void PatternTable_Click(object sender, EventArgs e)
        {
            PatternTable p = new PatternTable();
            p.Show();
        }


        private void drawRefresh_Click(object sender, EventArgs e)
        {
            NES_Console.DrawRefresh = !NES_Console.DrawRefresh;
            if (NES_Console.DrawRefresh)
                drawRefresh.BackColor = Color.Green;
            else
                drawRefresh.BackColor = Color.Red;
        }
    }
}
