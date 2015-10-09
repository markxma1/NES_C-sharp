using System;
using System.Collections;
using System.Drawing;
using System.Threading.Tasks;

namespace NES
{
    public partial class NES_PPU
    {
        private static Bitmap TempNameTable = new Bitmap(64 * 8, 60 * 8);

        public static Bitmap NameTabele(bool display = true)
        {
            Bitmap bitmap = new Bitmap(TempNameTable);

            if (display)
                if (NES_PPU_Register.PPUCTRL.V)
                {
                    bitmap = new Bitmap(TempNameTable.Size.Width, TempNameTable.Size.Height);
                    Graphics g = Graphics.FromImage(bitmap);
                    Parallel.For(0, 4, i =>
                    {
                        try
                        {
                            switch (i)
                            {
                                case 0:
                                    DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(0), 0, 0, 0);
                                    break;
                                case 1:
                                    if (INES.arrangement != INES.Mirror.vertical)
                                        DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(1), 1, 0, 32);
                                    break;
                                case 2:
                                    if (INES.arrangement != INES.Mirror.horisontal)
                                        DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(2), 2, 30, 0);
                                    break;
                                default:
                                    if (INES.arrangement == INES.Mirror.four_screen)
                                        DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(3), 3, 30, 32);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        { throw; }
                    });

                    DrawMirror(bitmap, g);

                    DrawDisplayFrame(g);

                    g.DrawRectangle(Pens.Green, 0, 0, 64 * 8 - 1, 60 * 8 - 1);
                    g.Dispose();
                    TempNameTable = bitmap;
                }
            return bitmap;
        }

        private static void DrawDisplayFrame(Graphics g)
        {
            g.DrawRectangle(Pens.Red, XScroll, YScroll, 256, 240);

            if (XScroll > 240)
                g.DrawRectangle(Pens.Red, XScroll - (256 * 2), YScroll, 256, 240);
            if (YScroll > 240)
                g.DrawRectangle(Pens.Red, XScroll, YScroll - (240 * 2), 256, 240);
            if (XScroll < 0)
                g.DrawRectangle(Pens.Red, (256 * 2) - XScroll, YScroll, 256, 240);
            if (YScroll < 0)
                g.DrawRectangle(Pens.Red, XScroll, (240 * 2) - YScroll, 256, 240);
        }

        private static void DrawMirror(Bitmap bitmap, Graphics g)
        {
            if (INES.arrangement == INES.Mirror.vertical)
                g.DrawImage(bitmap, TempNameTable.Size.Width / 2, 0);
            if (INES.arrangement == INES.Mirror.horisontal)
                g.DrawImage(bitmap, 0, TempNameTable.Size.Height / 2);
        }

        private static void DrowOneNameTable(Graphics g, ArrayList Attribute, int Nr, ushort X, ushort Y)
        {
            Parallel.For(X, X + 30 - 1, i =>
                {
                    for (int j = Y; j < Y + 32; j++)
                    {
                        int k = K(X, Y, i, j);
                        int c = (int)Attribute[k];
                        int t = ((Address)NES_PPU_Memory.NameTableN[Nr][k]).Value;
                        Bitmap temp = Tile((ushort)(t), c);
                        lock (g)
                        {
                            g.DrawImage(temp, j * 8, i * 8);
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
