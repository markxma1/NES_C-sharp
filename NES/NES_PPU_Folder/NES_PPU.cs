using System.Collections;
using System.Drawing;

namespace NES
{
    class NES_PPU
    {

        public static int XScroll = 0;
        public static int YScroll = 0;
        private static Bitmap TempDisplay = new Bitmap(256, 240);
        private static Bitmap TempNameTable = new Bitmap(64 * 8, 60 * 8);
        private static Bitmap TempPatternTable = new Bitmap(128 * 2, 128);
        private static Bitmap TempPaletteTable = new Bitmap(16 * 20, 2 * 20);

        public NES_PPU()
        {
            new NES_PPU_Register();
            new NES_PPU_OAM();
            new NES_PPU_Memory();
            new NES_PPU_Palette();
            new NES_PPU_AttributeTable();
        }

        /// <summary>
        /// converts Tiles from Memory to Bitmap. 
        /// </summary>
        /// <param name="startAdress"></param>
        /// <returns></returns>
        public static Bitmap Tile_StartAdress(ushort startAdress, int pallete)
        {
            Bitmap bitmap = new Bitmap(8, 8);
            byte[,] pattern = new byte[8, 8];
            Color[] color = NES_PPU_Palette.getPalette(pallete);
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var a = (((Adress)NES_PPU_Memory.Memory[i + startAdress]).Value >> j) & (0x01);
                    var b = ((((Adress)NES_PPU_Memory.Memory[startAdress + i + 8]).Value >> j) & (0x01)) << 1;
                    pattern[7 - j, i] = (byte)(a | b);
                    bitmap.SetPixel(7 - j, i, color[pattern[7 - j, i]]);
                }
            }
            return bitmap;
        }

        /// <summary>
        /// converts Tiles from Memory to Bitmap. With ID and  Backgrount Tile Select.
        /// </summary>
        /// <param name="spriteID">ID of Sprite  </param>
        /// <returns></returns>
        public static Bitmap Tile(ushort spriteID, int pallete)
        {
            int startAdress = spriteID * 16;
            Bitmap bitmap = new Bitmap(8, 8);
            byte[,] pattern = new byte[8, 8];
            Color[] color = NES_PPU_Palette.getPalette(pallete);
            var PatternTable = NES_PPU_Memory.PatternTable0;
            if (NES_PPU_Register.PPUCTRL.B)
                PatternTable = NES_PPU_Memory.PatternTable1;
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var a = (((Adress)PatternTable[startAdress + i]).Value >> j) & (0x01);
                    var b = ((((Adress)PatternTable[startAdress + i + 8]).Value >> j) & (0x01)) << 1;
                    pattern[7 - j, i] = (byte)(a | b);
                    bitmap.SetPixel(7 - j, i, color[pattern[7 - j, i]]);
                }
            }
            return bitmap;
        }

        public static Bitmap PatternTable(int PN)
        {
            Bitmap bitmap = new Bitmap(TempPatternTable);

            Graphics g = Graphics.FromImage(bitmap);
            int k = 0;
            for (ushort i = 0; i < 16; i++)
            {
                for (ushort j = 0; j < 16; j++)
                {
                    int t = (k++) * 16;
                    g.DrawImage(Tile_StartAdress((ushort)(t), PN), j * 8, i * 8);
                }
            }

            for (ushort i = 0; i < 16; i++)
            {
                for (ushort j = 16; j < 32; j++)
                {
                    int t = (k++) * 16;
                    g.DrawImage(Tile_StartAdress((ushort)(t), PN), j * 8, i * 8);
                }
            }
            return bitmap;
        }

        public static Bitmap NameTabele()
        {
            Bitmap bitmap = new Bitmap(TempNameTable);
            if (NES_PPU_Register.PPUCTRL.V)
            {
                Graphics g = Graphics.FromImage(bitmap);
                DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(0), NES_PPU_Memory.NameTable0, 0, 30, 0, 32);
                DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(2), NES_PPU_Memory.NameTable2, 30, 60, 0, 32);
                DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(1), NES_PPU_Memory.NameTable1, 0, 30, 32, 64);
                DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(3), NES_PPU_Memory.NameTable3, 30, 60, 32, 64);
                g.DrawRectangle(Pens.Red, XScroll, YScroll, 256, 240);
            }
            return bitmap;
        }

        private static void DrowOneNameTable(Graphics g, ArrayList Attribute, ArrayList Table, ushort XStart, ushort XStop, ushort YStart, ushort YStop)
        {
            int k = 0;
            for (ushort i = XStart; i < XStop; i++)
            {
                for (ushort j = YStart; j < YStop; j++)
                {
                    int c = (int)Attribute[k];
                    int t = ((Adress)Table[k++]).value;
                    g.DrawImage(Tile((ushort)(t), c), j * 8, i * 8);
                }
            }
        }

        public static Bitmap Display()
        {
            Bitmap Display = new Bitmap(TempDisplay);
            if (NES_PPU_Register.PPUCTRL.V)
            {
                Interrupt.NMI = true;
                Rectangle cropRect = new Rectangle(XScroll, YScroll, 257, 241);
                Graphics g = Graphics.FromImage(Display);
                Bitmap NameTabeleT = NameTabele();
                g.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), cropRect, GraphicsUnit.Pixel);
                TempDisplay = Display;
            }
            return Display;
        }

        public static Bitmap PaletteTable()
        {
            Bitmap bitmap = new Bitmap(TempPaletteTable);
            if (NES_PPU_Register.PPUCTRL.V)
            {
                Graphics g = Graphics.FromImage(bitmap);
                int k = 0;
                for (ushort i = 0; i < 2; i++)
                {
                    int o = 0;
                    for (ushort j = 0; j < 4; j++)
                    {
                        var color = NES_PPU_Palette.getPalette(k++);
                        for (int h = 0; h < 4; h++)
                        {
                            g.FillRectangle(new SolidBrush(color[h]), o * 20, i * 20, ++o * 20, (i + 1) * 20);
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
            }
            return bitmap;
        }

    }
}
