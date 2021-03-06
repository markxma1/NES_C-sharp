﻿///   Copyright 2016 Xma1
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
