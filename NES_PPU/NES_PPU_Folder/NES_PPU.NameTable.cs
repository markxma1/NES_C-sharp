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
                        catch (Exception ex)
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
                    int k = K(X, Y, i, j);
                    int c = (int)Attribute[k];
                    int t = ((Address)NES_PPU_Memory.NameTableN[Nr][k]).Value;
                    Picture temp = Tile((ushort)(t), c);
                    lock (image)
                    {
                        image.DrawNewImage(temp, j * 8, i * 8);
                    }
                }
            });
        }

        private static int K(ushort X, ushort Y, int i, int j)
        {
            return ((i - X) * 32) + (j - Y);
        }
    }
}
