using System.Collections;
using System.Drawing;

namespace NES
{
    class NES_PPU
    {

        public static Color[] PPUpalettes = new Color[0x40];

        public NES_PPU()
        {
            new NES_PPU_OAM();
            new NES_PPU_Memory();
            InitPalletes2C03and2C05();
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
            Color[] color = getPalette(pallete);
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
            Color[] color = getPalette(pallete);
            var PatternTable = NES_PPU_Memory.PatternTable0;
            if (NES_Register.PPUCTRL.B)
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

        public static Bitmap Patterntable(int PN)
        {
            Bitmap bitmap = new Bitmap(128 * 2, 128);
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

        /// <summary>
        /// http://wiki.nesdev.com/w/index.php/PPU_attribute_tables
        /// </summary>
        /// <param name="NR">Attribute tabellenummer</param>
        /// <returns>decoded table</returns>
        private static ArrayList AttributeTable(int NR)
        {
            ArrayList AL = new ArrayList();
            ArrayList AttributeTable = NES_PPU_Memory.AttributeTable0;
            if (NR == 1)
                AttributeTable = NES_PPU_Memory.AttributeTable1;
            if (NR == 2)
                AttributeTable = NES_PPU_Memory.AttributeTable2;
            if (NR == 3)
                AttributeTable = NES_PPU_Memory.AttributeTable3;

            for (int i = 0; i < 0x8; i++)
            {
                for (int j = 0; j < 0x8; j++)
                {
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 2) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 2) & 3);
                }
                for (int j = 0; j < 0x8; j++)
                {
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 2) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 2) & 3);
                }
                for (int j = 0; j < 0x8; j++)
                {
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 4) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 4) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 6) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 6) & 3);
                }
                for (int j = 0; j < 0x8; j++)
                {
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 4) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 4) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 6) & 3);
                    AL.Add((int)(((Adress)AttributeTable[j + i * 8]).value >> 6) & 3);
                }
            }
            return AL;
        }

        public static Bitmap NameTabele()
        {
            Bitmap bitmap = new Bitmap(64 * 8, 60 * 8);
            Graphics g = Graphics.FromImage(bitmap);
            ArrayList Attribute = AttributeTable(0);
            int k = 0;
            for (ushort i = 0; i < 30; i++)
            {
                for (ushort j = 0; j < 32; j++)
                {
                    int c = (int)Attribute[k];
                    int t = ((Adress)NES_PPU_Memory.NameTable0[k++]).value;
                    g.DrawImage(Tile((ushort)(t), c), j * 8, i * 8);
                }
            }
            Attribute = AttributeTable(2);
            k = 0;
            for (ushort i = 30; i < 60; i++)
            {
                for (ushort j = 0; j < 32; j++)
                {
                    int c = (int)Attribute[k];
                    int t = ((Adress)NES_PPU_Memory.NameTable2[k++]).value;
                    g.DrawImage(Tile((ushort)(t), c), j * 8, i * 8);
                }
            }
            Attribute = AttributeTable(1);
            k = 0;
            for (ushort i = 0; i < 30; i++)
            {
                for (ushort j = 32; j < 64; j++)
                {
                    int c = (int)Attribute[k];
                    int t = ((Adress)NES_PPU_Memory.NameTable1[k++]).value;
                    g.DrawImage(Tile((ushort)(t), c), j * 8, i * 8);
                }
            }
            Attribute = AttributeTable(3);
            k = 0;
            for (ushort i = 30; i < 60; i++)
            {
                for (ushort j = 32; j < 64; j++)
                {
                    int c = (int)Attribute[k];
                    int t = ((Adress)NES_PPU_Memory.NameTable3[k++]).value;
                    g.DrawImage(Tile((ushort)(t), c), j * 8, i * 8);
                }
            }
            return bitmap;
        }

        public static Bitmap PaletteTable()
        {
            Bitmap bitmap = new Bitmap(16 * 20, 2 * 20);
            Graphics g = Graphics.FromImage(bitmap);
            int k = 0;
            for (ushort i = 0; i < 2; i++)
            {
                int o = 0;
                for (ushort j = 0; j < 4; j++)
                {
                    var color = getPalette(k++);
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

            return bitmap;
        }

        private static Color[] getPalette(int Nr)
        {
            Color[] color = new Color[4];
            switch (Nr)
            {
                case 0:
                    color[0] = UniversalBackgroundColor();
                    color[1] = getBGColorAsRGB(1);
                    color[2] = getBGColorAsRGB(2);
                    color[3] = getBGColorAsRGB(3);
                    return color;
                case 1:
                    color[0] = UniversalBackgroundColor();
                    color[1] = getBGColorAsRGB(5);
                    color[2] = getBGColorAsRGB(6);
                    color[3] = getBGColorAsRGB(7);
                    return color;
                case 2:
                    color[0] = UniversalBackgroundColor();
                    color[1] = getBGColorAsRGB(9);
                    color[2] = getBGColorAsRGB(10);
                    color[3] = getBGColorAsRGB(11);
                    return color;
                case 3:
                    color[0] = UniversalBackgroundColor();
                    color[1] = getBGColorAsRGB(13);
                    color[2] = getBGColorAsRGB(14);
                    color[3] = getBGColorAsRGB(15);
                    return color;
                case 4:
                    color[0] = UniversalBackgroundColor();
                    color[1] = getSpriteColorAsRGB(1);
                    color[2] = getSpriteColorAsRGB(2);
                    color[3] = getSpriteColorAsRGB(3);
                    return color;
                case 5:
                    color[0] = UniversalBackgroundColor();
                    color[1] = getSpriteColorAsRGB(5);
                    color[2] = getSpriteColorAsRGB(6);
                    color[3] = getSpriteColorAsRGB(7);
                    return color;
                case 6:
                    color[0] = UniversalBackgroundColor();
                    color[1] = getSpriteColorAsRGB(9);
                    color[2] = getSpriteColorAsRGB(10);
                    color[3] = getSpriteColorAsRGB(11);
                    return color;
                case 7:
                    color[0] = UniversalBackgroundColor();
                    color[1] = getSpriteColorAsRGB(13);
                    color[2] = getSpriteColorAsRGB(14);
                    color[3] = getSpriteColorAsRGB(15);
                    return color;
                default:
                    color[0] = UniversalBackgroundColor();
                    color[1] = Color.Red;
                    color[2] = Color.Green;
                    color[3] = Color.Blue;
                    return color;
            }
        }

        public static Color UniversalBackgroundColor()
        {
            var color = ((Adress)NES_PPU_Memory.BGPalette[0]).Value;
            return PPUpalettes[color];
        }

        private static Color getBGColorAsRGB(int BGAdress)
        {
            var color = ((Adress)NES_PPU_Memory.BGPalette[BGAdress]).Value;
            return PPUpalettes[color];
        }

        private static Color getSpriteColorAsRGB(int SpriteAdress)
        {
            var color = ((Adress)NES_PPU_Memory.SpritePalette[SpriteAdress]).Value;
            return PPUpalettes[color];
        }

        private static Color getColorAsRGB(int Adress)
        {
            var color = ((Adress)NES_PPU_Memory.Memory[Adress]).Value;
            return PPUpalettes[color];
        }

        public static void InitialAtPower()
        {
            NES_Register.PPUCTRL.adress.value = 0;
            NES_Register.PPUMASK.adress.value = 0;
            NES_Register.PPUSTATUS.adress.value = 0xA0;
            NES_Register.OAMADDR.value = 0;
            NES_Register.PPUSCROLL.value = 0;
            NES_Register.PPUADDR.value = 0;
            NES_Register.PPUDATA.value = 0;
        }

        public static void InitialOnReset()
        {
            NES_Register.PPUCTRL.adress.value = 0;
            NES_Register.PPUMASK.adress.value = 0;
            NES_Register.PPUSTATUS.adress.value = (byte)(NES_Register.PPUSTATUS.adress.value & 0x80);
            NES_Register.PPUSCROLL.value = 0;
            NES_Register.PPUDATA.value = 0;
        }

        public static void InitPalletes2C03and2C05()
        {
            PPUpalettes[0x00] = Color.FromArgb(OctToHex(3), OctToHex(3), OctToHex(3));
            PPUpalettes[0x01] = Color.FromArgb(OctToHex(0), OctToHex(1), OctToHex(4));
            PPUpalettes[0x02] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(6));
            PPUpalettes[0x03] = Color.FromArgb(OctToHex(3), OctToHex(2), OctToHex(6));
            PPUpalettes[0x04] = Color.FromArgb(OctToHex(4), OctToHex(0), OctToHex(3));
            PPUpalettes[0x05] = Color.FromArgb(OctToHex(5), OctToHex(0), OctToHex(3));
            PPUpalettes[0x06] = Color.FromArgb(OctToHex(5), OctToHex(1), OctToHex(0));
            PPUpalettes[0x07] = Color.FromArgb(OctToHex(4), OctToHex(2), OctToHex(0));
            PPUpalettes[0x08] = Color.FromArgb(OctToHex(3), OctToHex(2), OctToHex(0));
            PPUpalettes[0x09] = Color.FromArgb(OctToHex(1), OctToHex(2), OctToHex(0));
            PPUpalettes[0x0A] = Color.FromArgb(OctToHex(0), OctToHex(3), OctToHex(1));
            PPUpalettes[0x0B] = Color.FromArgb(OctToHex(0), OctToHex(4), OctToHex(0));
            PPUpalettes[0x0C] = Color.FromArgb(OctToHex(0), OctToHex(2), OctToHex(2));
            PPUpalettes[0x0D] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
            PPUpalettes[0x0E] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
            PPUpalettes[0x0F] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));

            PPUpalettes[0x10] = Color.FromArgb(OctToHex(5), OctToHex(5), OctToHex(5));
            PPUpalettes[0x11] = Color.FromArgb(OctToHex(0), OctToHex(3), OctToHex(6));
            PPUpalettes[0x12] = Color.FromArgb(OctToHex(0), OctToHex(2), OctToHex(7));
            PPUpalettes[0x13] = Color.FromArgb(OctToHex(4), OctToHex(0), OctToHex(7));
            PPUpalettes[0x14] = Color.FromArgb(OctToHex(5), OctToHex(0), OctToHex(7));
            PPUpalettes[0x15] = Color.FromArgb(OctToHex(7), OctToHex(0), OctToHex(4));
            PPUpalettes[0x16] = Color.FromArgb(OctToHex(7), OctToHex(0), OctToHex(0));
            PPUpalettes[0x17] = Color.FromArgb(OctToHex(6), OctToHex(3), OctToHex(0));
            PPUpalettes[0x18] = Color.FromArgb(OctToHex(4), OctToHex(3), OctToHex(0));
            PPUpalettes[0x19] = Color.FromArgb(OctToHex(1), OctToHex(4), OctToHex(0));
            PPUpalettes[0x1A] = Color.FromArgb(OctToHex(0), OctToHex(4), OctToHex(0));
            PPUpalettes[0x1B] = Color.FromArgb(OctToHex(0), OctToHex(5), OctToHex(3));
            PPUpalettes[0x1C] = Color.FromArgb(OctToHex(0), OctToHex(4), OctToHex(4));
            PPUpalettes[0x1D] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
            PPUpalettes[0x1E] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
            PPUpalettes[0x1F] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));

            PPUpalettes[0x20] = Color.FromArgb(OctToHex(7), OctToHex(7), OctToHex(7));
            PPUpalettes[0x21] = Color.FromArgb(OctToHex(3), OctToHex(5), OctToHex(7));
            PPUpalettes[0x22] = Color.FromArgb(OctToHex(4), OctToHex(4), OctToHex(7));
            PPUpalettes[0x23] = Color.FromArgb(OctToHex(6), OctToHex(3), OctToHex(7));
            PPUpalettes[0x24] = Color.FromArgb(OctToHex(7), OctToHex(0), OctToHex(7));
            PPUpalettes[0x25] = Color.FromArgb(OctToHex(7), OctToHex(3), OctToHex(7));
            PPUpalettes[0x26] = Color.FromArgb(OctToHex(7), OctToHex(4), OctToHex(0));
            PPUpalettes[0x27] = Color.FromArgb(OctToHex(7), OctToHex(5), OctToHex(0));
            PPUpalettes[0x28] = Color.FromArgb(OctToHex(6), OctToHex(6), OctToHex(0));
            PPUpalettes[0x29] = Color.FromArgb(OctToHex(3), OctToHex(6), OctToHex(0));
            PPUpalettes[0x2A] = Color.FromArgb(OctToHex(0), OctToHex(7), OctToHex(0));
            PPUpalettes[0x2B] = Color.FromArgb(OctToHex(2), OctToHex(7), OctToHex(6));
            PPUpalettes[0x2C] = Color.FromArgb(OctToHex(0), OctToHex(7), OctToHex(7));
            PPUpalettes[0x2D] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
            PPUpalettes[0x2E] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
            PPUpalettes[0x2F] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));

            PPUpalettes[0x30] = Color.FromArgb(OctToHex(7), OctToHex(7), OctToHex(7));
            PPUpalettes[0x31] = Color.FromArgb(OctToHex(5), OctToHex(6), OctToHex(7));
            PPUpalettes[0x32] = Color.FromArgb(OctToHex(6), OctToHex(5), OctToHex(7));
            PPUpalettes[0x33] = Color.FromArgb(OctToHex(7), OctToHex(5), OctToHex(7));
            PPUpalettes[0x34] = Color.FromArgb(OctToHex(7), OctToHex(4), OctToHex(7));
            PPUpalettes[0x35] = Color.FromArgb(OctToHex(7), OctToHex(5), OctToHex(5));
            PPUpalettes[0x36] = Color.FromArgb(OctToHex(7), OctToHex(6), OctToHex(4));
            PPUpalettes[0x37] = Color.FromArgb(OctToHex(7), OctToHex(7), OctToHex(2));
            PPUpalettes[0x38] = Color.FromArgb(OctToHex(7), OctToHex(7), OctToHex(3));
            PPUpalettes[0x39] = Color.FromArgb(OctToHex(5), OctToHex(7), OctToHex(2));
            PPUpalettes[0x3A] = Color.FromArgb(OctToHex(4), OctToHex(7), OctToHex(3));
            PPUpalettes[0x3B] = Color.FromArgb(OctToHex(2), OctToHex(7), OctToHex(6));
            PPUpalettes[0x3C] = Color.FromArgb(OctToHex(4), OctToHex(6), OctToHex(7));
            PPUpalettes[0x3D] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
            PPUpalettes[0x3E] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
            PPUpalettes[0x3F] = Color.FromArgb(OctToHex(0), OctToHex(0), OctToHex(0));
        }

        private static int OctToHex(int a)
        {
            return (int)(((double)a / 7) * 255);
        }

    }
}
