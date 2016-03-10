using NES_PPU;
using System;
using System.Collections;
using System.Drawing;
using System.Threading.Tasks;

namespace NES
{
    public partial class NES_PPU
    {
        private static Picture TempNameTable = new Picture(64 * 8, 60 * 8);

        public static Picture NameTabele(bool display = true)
        {
            Picture bitmap = new Picture(TempNameTable);

            if (display)
                if (NES_PPU_Register.PPUCTRL.V)
                {
                    bitmap = new Picture(TempNameTable.Size.Width, TempNameTable.Size.Height);

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

                    DrawMirror(bitmap);
                    DrawDisplayFrame(bitmap);

                    bitmap.DrawRectangle(Color.Green, 0, 0, 64 * 8 - 1, 60 * 8 - 1);
                    TempNameTable = bitmap;
                }
            return bitmap;
        }

        private static void DrawDisplayFrame(Picture bitmap)
        {
            bitmap.DrawRectangle(Color.Red, XScroll, YScroll, 256, 240);

            if (XScroll > 240)
                bitmap.DrawRectangle(Color.Red, XScroll - (256 * 2), YScroll, 256, 240);
            if (YScroll > 240)
                bitmap.DrawRectangle(Color.Red, XScroll, YScroll - (240 * 2), 256, 240);
            if (XScroll < 0)
                bitmap.DrawRectangle(Color.Red, (256 * 2) - XScroll, YScroll, 256, 240);
            if (YScroll < 0)
                bitmap.DrawRectangle(Color.Red, XScroll, (240 * 2) - YScroll, 256, 240);
        }

        private static void DrawMirror(Picture bitmap)
        {
            if (INES.arrangement == INES.Mirror.vertical)
                bitmap.DrawImage(bitmap, TempNameTable.Size.Width / 2, 0);
            if (INES.arrangement == INES.Mirror.horisontal)
                bitmap.DrawImage(bitmap, 0, TempNameTable.Size.Height / 2);
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
                            image.DrawImage(temp, j * 8, i * 8);
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
