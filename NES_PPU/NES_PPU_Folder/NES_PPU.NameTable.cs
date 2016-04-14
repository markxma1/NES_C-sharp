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
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace NES
{
    public partial class NES_PPU
    {
        private static Picture TempNameTable = new Picture(64 * 8, 60 * 8);

        public static Picture NameTabele(bool display = true)
        {
            TimeSpan t1 = new TimeSpan(0);
            TimeSpan t2 = new TimeSpan(0);
            TimeSpan t3 = new TimeSpan(0);
            TimeSpan t4 = new TimeSpan(0);

            if (display)
                if (NES_PPU_Register.PPUCTRL.V)
                {
                    Stopwatch t = new Stopwatch();
                    t.Start();

                    Picture bitmap = new Picture(TempNameTable.Size.Width, TempNameTable.Size.Height);
                    t1 = t.Elapsed;
                    Parallel.For(0, 4, i =>
                    {
                        try
                        {
                            switch (i)
                            {
                                case 0:
                                    DrowOneNameTable(bitmap, NES_PPU_AttributeTable.AttributeTable(0), 0, 0, 0);
                                    break;
                                case 1:
                                    if (INES.arrangement != INES.Mirror.vertical)
                                        DrowOneNameTable(bitmap, NES_PPU_AttributeTable.AttributeTable(1), 1, 0, 32);
                                    break;
                                case 2:
                                    if (INES.arrangement != INES.Mirror.horisontal)
                                        DrowOneNameTable(bitmap, NES_PPU_AttributeTable.AttributeTable(2), 2, 30, 0);
                                    break;
                                default:
                                    if (INES.arrangement == INES.Mirror.four_screen)
                                        DrowOneNameTable(bitmap, NES_PPU_AttributeTable.AttributeTable(3), 3, 30, 32);
                                    break;
                            }
                        }
                        catch (Exception)
                        { throw; }
                    });

                    t2 = t.Elapsed;
                    DrawMirror(bitmap);
                    DrawDisplayFrame(bitmap);
                    t3 = t.Elapsed;
                    bitmap.DrawInfoRectangle(Color.Green, 0, 0, 64 * 8 - 1, 60 * 8 - 1);
                    TempNameTable = bitmap;
                    t.Stop();
                    t4 = t.Elapsed;
                    t4 -= t3;
                    t3 -= t2;
                    t2 -= t1;
                }
            return TempNameTable;
        }

        private static void DrawDisplayFrame(Picture bitmap)
        {
            bitmap.DrawInfoRectangle(Color.Red, XScroll, YScroll, 256, 240);

            if (XScroll > 240)
                bitmap.DrawInfoRectangle(Color.Red, XScroll - (256 * 2), YScroll, 256, 240);
            if (YScroll > 240)
                bitmap.DrawInfoRectangle(Color.Red, XScroll, YScroll - (240 * 2), 256, 240);
            if (XScroll < 0)
                bitmap.DrawInfoRectangle(Color.Red, (256 * 2) - XScroll, YScroll, 256, 240);
            if (YScroll < 0)
                bitmap.DrawInfoRectangle(Color.Red, XScroll, (240 * 2) - YScroll, 256, 240);
        }

        private static void DrawMirror(Picture bitmap)
        {
            if (INES.arrangement == INES.Mirror.vertical)
                bitmap.DrawMirror(TempNameTable.Size.Width / 2, 0);
            if (INES.arrangement == INES.Mirror.horisontal)
                bitmap.DrawMirror(0, TempNameTable.Size.Height / 2);
        }

        private static void DrowOneNameTable(Picture image, ArrayList Attribute, int Nr, ushort X, ushort Y)
        {
            Parallel.For(X, X + 30 - 1, i =>
            {
                for (int j = Y; j < Y + 32; j++)
                {
                    TimeSpan t1 = new TimeSpan(0);
                    TimeSpan t2 = new TimeSpan(0);
                    TimeSpan t3 = new TimeSpan(0);
                    Stopwatch ti = new Stopwatch();
                    ti.Start();
                    int k = K(X, Y, i, j);
                    int c = (int)Attribute[k];
                    int t = ((Address)NES_PPU_Memory.NameTableN[Nr][k]).Value;
                    t1 = ti.Elapsed;
                    Picture temp = Tile((ushort)(t), c);
                    t2 = ti.Elapsed;
                    lock (image)
                    {
                        image.DrawNewImage(temp, j * 8, i * 8);
                    }
                    t3 = ti.Elapsed;
                    t3 -= t2;
                    t2 -= t1;
                }
            });
        }

        private static int K(ushort X, ushort Y, int i, int j)
        {
            return ((i - X) * 32) + (j - Y);
        }
    }
}
