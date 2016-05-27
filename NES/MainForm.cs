///   Copyright 2016 Xma1
///
///   This file is part of NES-C#.
///
///   NES-C# is free software: you can redistribute it and/or modify
///   it under the terms of the GNU General Public License as published by
///   the Free Software Foundation, either version 3 of the License, or
///   (at your option) any later version.
///
///   NES-C# is distributed in the hope that it will be useful,
///   but WITHOUT ANY WARRANTY; without even the implied warranty of
///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
///   See the GNU General Public License for more details.
///
///   You should have received a copy of the GNU General Public License
///   along with NES-C#. If not, see http://www.gnu.org/licenses/.
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
            NES_Console.LoadRom("./Galaga.nes");
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

        private void button2_Click(object sender, EventArgs e)
        {
            NES_Console.SaveGame("memory.xml");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NES_Console.LoadGame("memory.xml");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CPUSpeedForm c = new CPUSpeedForm();
            c.Show();
        }
    }
}
