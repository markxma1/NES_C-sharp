using System.Drawing;

namespace NES
{

    public partial class NES_PPU
    {

        private static Bitmap TempPaletteTable = new Bitmap(16 * 20, 2 * 20);

        public NES_PPU()
        {
            new NES_PPU_Register();
            new NES_PPU_OAM();
            new NES_PPU_Memory();
            new NES_PPU_Palette();
            new NES_PPU_AttributeTable();
        }

        public static Bitmap PaletteTable()
        {
            Bitmap bitmap = new Bitmap(TempPaletteTable);
            if (NES_PPU_Register.PPUCTRL.V)
            {
                Graphics g = Graphics.FromImage(bitmap);
                g.FillRectangle(new SolidBrush(NES_PPU_Palette.UniversalBackgroundColor()), 0, 0, bitmap.Size.Width, bitmap.Size.Height);
                int k = 0;
                for (ushort i = 0; i < 2; i++)
                {
                    int o = 0;
                    for (ushort j = 0; j < 4; j++)
                    {
                        var color = NES_PPU_Palette.getPalette(k++);
                        for (int h = 0; h < 4; h++)
                        {
                            if (color.color[h] == Color.Transparent)
                                g.FillRectangle(new SolidBrush(NES_PPU_Palette.UniversalBackgroundColor()), o * 20, i * 20, ++o * 20, (i + 1) * 20);
                            else
                                g.FillRectangle(new SolidBrush(color.color[h]), o * 20, i * 20, ++o * 20, (i + 1) * 20);
                        }
                    }

                }
                for (ushort i = 0; i < 2; i++)
                {
                    int o = 0;
                    for (ushort j = 0; j < 4; j++)
                    {
                        for (int h = 0; h < 4; h++)
                        {
                            g.DrawRectangle(Pens.Black, o * 20, i * 20, ++o * 20, (i + 1) * 20);
                        }
                    }
                }
                TempPaletteTable = bitmap;
            }
            return bitmap;
        }
    }
}
