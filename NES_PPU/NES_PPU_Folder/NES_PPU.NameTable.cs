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
                                            DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(1), 1, 0, 32);
                                            break;
                                        case 2:
                                            DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(2), 2, 30, 0);
                                            break;
                                        default:
                                            DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(3), 3, 30, 32);
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                { }
                            });
                    g.DrawRectangle(Pens.Red, XScroll, YScroll, 256, 240);
                    g.DrawRectangle(Pens.Green, 0, 0, 64 * 8 - 1, 60 * 8 - 1);
                    g.Dispose();
                    TempNameTable = bitmap;
                }
            return bitmap;
        }

        private static void DrowOneNameTable(Graphics g, ArrayList Attribute, int Nr, ushort X, ushort Y)
        {
            Parallel.For(X, X + 30 - 1, i =>
                {
                    for (int j = Y; j < Y + 32; j++)
                    {
                        int k = K(X, Y, i, j);
                        int c = (int)Attribute[k];
                        int t = ((Address)NES_PPU_Memory.NameTableN[Nr][k]).value;
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
