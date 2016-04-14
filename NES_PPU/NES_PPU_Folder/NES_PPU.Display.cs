///   Copyright 2016 Xma1
///
///   This file is part of NES-C#.
///
///   Foobar is free software: you can redistribute it and/or modify
///   it under the terms of the GNU General Public License as published by
///   the Free Software Foundation, either version 3 of the License, or
///   (at your option) any later version.
///
///   Foobar is distributed in the hope that it will be useful,
///   but WITHOUT ANY WARRANTY; without even the implied warranty of
///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
///   See the GNU General Public License for more details.
///
///   You should have received a copy of the GNU General Public License
///   along with Foobar. If not, see http://www.gnu.org/licenses/.
using NES_PPU;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace NES
{
    public partial class NES_PPU
    {
        private static Picture TempDisplay = new Picture(256, 240);
        private static bool draw = false;

        private static bool Draw
        {
            get
            {
                return draw;
            }

            set
            {
                if (value)
                    draw = value;
                else
                {
                    draw = value;
                    xScrollTemp = xScroll;
                    yScrollTemp = yScroll;
                }
            }
        }

        public static Picture Display()
        {
            TimeSpan t1 = new TimeSpan(0);
            TimeSpan t2 = new TimeSpan(0);
            TimeSpan t3 = new TimeSpan(0);
            TimeSpan t4 = new TimeSpan(0);

            if (NES_PPU_Register.PPUCTRL.V)
            {
                NES_PPU_Register.PPUCTRL.V = false;
                Interrupt.NMI = true;

                Stopwatch t = new Stopwatch();
                t.Start();


                Draw = true;
                Picture Display = new Picture(TempDisplay.Size.Width, TempDisplay.Size.Height);

                t1 = t.Elapsed;
                Display.DrawImage(InsertObect(true), 0, 0);
                t2 = t.Elapsed;
                DrawBackground(Display, NameTabele());
                t3 = t.Elapsed;
                Display.DrawImage(InsertObect(false), 0, 0);
                t.Stop();
                t4 = t.Elapsed;
                t4 -= t3;
                t3 -= t2;
                t2 -= t1;
                NES_PPU_Palette.setAllPaletesAsOld();
                TempDisplay = Display;
                Draw = false;
                NES_PPU_Register.PPUSTATUS.V = true;
            }

            return TempDisplay;
        }

        private static void DrawBackground(Picture Display, Picture NameTabeleT)
        {
            TimeSpan t1 = new TimeSpan(0);
            TimeSpan t2 = new TimeSpan(0);
            TimeSpan t3 = new TimeSpan(0);
            TimeSpan t4 = new TimeSpan(0);
            TimeSpan t5 = new TimeSpan(0);

            Stopwatch t = new Stopwatch();
            t.Start();
            Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, YScroll, 256, 240));
            t1 = t.Elapsed;
            if (XScroll > 240)
                Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll - (256 * 2), YScroll, 256, 240));
            t2 = t.Elapsed;
            if (YScroll > 240)
                Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, YScroll - (240 * 2), 256, 240));
            t3 = t.Elapsed;
            if (XScroll < 0)
                Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle((256 * 2) - XScroll, YScroll, 256, 240));
            t4 = t.Elapsed;
            if (YScroll < 0)
                Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, (240 * 2) - YScroll, 256, 240));
            t5 = t.Elapsed;
            t.Stop();
            t5 -= t5;
            t4 -= t3;
            t3 -= t2;
            t2 -= t1;
        }

        /// <summary>
        /// Inserts Tiles into Image. 
        /// </summary>
        /// <param name="Priority">false sets Tiles in front of Background, true in behind of background</param>
        /// <returns></returns>
        private static Picture InsertObect(bool Priority)
        {
            Picture Display = new Picture(TempDisplay.Width, TempDisplay.Height);

            for (int i = 0; i < NES_PPU_OAM.SpriteTile.Count; i++)
            {
                try
                {
                    NES_PPU_OAM.Byte2 Attribute = (NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i];
                    if (Priority == Attribute.Priority)
                    {
                        NES_PPU_OAM.Byte1 SpriteTile = (NES_PPU_OAM.Byte1)NES_PPU_OAM.SpriteTile[i];
                        Address SpriteXc = (Address)NES_PPU_OAM.SpriteXc[i];
                        Address SpriteYc = (Address)NES_PPU_OAM.SpriteYc[i];

                        Picture tile = Tile(SpriteTile.Number, Attribute.Palette, (SpriteTile.Bank) ? (1) : (0));

                        tile = new Picture(tile);

                        if (Attribute.FlipH)
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        if (Attribute.FlipV)
                            tile.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        // SpriteXc.Value - 8
                        Display.DrawImage(tile, SpriteXc.Value - 1, SpriteYc.Value - 1);

                    }
                }
                catch (Exception)
                { }

            }
            return Display;
        }
    }
}
