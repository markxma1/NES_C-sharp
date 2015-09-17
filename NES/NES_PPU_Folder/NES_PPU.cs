using System.Collections;
using System.Drawing;
using System.Threading.Tasks;

namespace NES
{
    class NES_PPU
    {
        private static bool ScrollXoY = true;//false y ture x
        private static bool draw = false;
        private static int xScrollTemp = 0;
        private static int yScrollTemp = 0;
        public static int xScroll = 0;
        public static int yScroll = 0;
        private static Bitmap TempDisplay = new Bitmap(256, 240);
        private static Bitmap TempNameTable = new Bitmap(64 * 8, 60 * 8);
        private static Bitmap TempPatternTable = new Bitmap(128 * 2, 128);
        private static Bitmap TempPaletteTable = new Bitmap(16 * 20, 2 * 20);

        public static byte Scroll
        {
            get { return 0; }

            set
            {
                if (ScrollXoY)
                {
                    XScroll = value;
                }
                else
                {
                    if (value > 239)
                        value -= 255;
                    YScroll = value;
                }
                ScrollXoY = !ScrollXoY;
            }
        }

        private static int XScroll
        {
            get
            {
                return xScrollTemp;
            }

            set
            {
                AddxScroll(value);
                xScroll = value;
                if (!Draw)
                    xScrollTemp = value;
            }
        }

        private static int YScroll
        {
            get
            {
                return yScrollTemp;
            }

            set
            {
                value = AddyScroll(value);
                yScroll = value;
                if (!Draw)
                    yScrollTemp = value;
            }
        }

        private static int AddyScroll(int value)
        {
            int add = ((NES_PPU_Register.PPUCTRL.N & 2) == 0) ? (0) : (240);
            value += add;
            return value;
        }

        private static int AddxScroll(int value)
        {
            int add = ((NES_PPU_Register.PPUCTRL.N & 1) == 0) ? (0) : (256);
            value += add;
            return value;
        }

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
                    var a = (((Address)NES_PPU_Memory.Memory[i + startAdress]).Value >> j) & (0x01);
                    var b = ((((Address)NES_PPU_Memory.Memory[startAdress + i + 8]).Value >> j) & (0x01)) << 1;
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
            var PatternTable = NES_PPU_Memory.PatternTableN[0];
            if (NES_PPU_Register.PPUCTRL.B)
                PatternTable = NES_PPU_Memory.PatternTableN[1];
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var a = (((Address)PatternTable[startAdress + i]).Value >> j) & (0x01);
                    var b = ((((Address)PatternTable[startAdress + i + 8]).Value >> j) & (0x01)) << 1;
                    pattern[7 - j, i] = (byte)(a | b);
                    bitmap.SetPixel(7 - j, i, color[pattern[7 - j, i]]);
                }
            }
            return bitmap;
        }

        public static Bitmap Tile(ushort spriteID, int pallete, int bankID)
        {
            int startAdress = spriteID * 16;
            Bitmap bitmap = new Bitmap(8, 8);
            byte[,] pattern = new byte[8, 8];
            Color[] color = NES_PPU_Palette.getSpriteColorPalette(pallete);
            var PatternTable = NES_PPU_Memory.PatternTableN[bankID];
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    var a = (((Address)PatternTable[startAdress + i]).Value >> j) & (0x01);
                    var b = ((((Address)PatternTable[startAdress + i + 8]).Value >> j) & (0x01)) << 1;
                    pattern[7 - j, i] = (byte)(a | b);
                    bitmap.SetPixel(7 - j, i, color[pattern[7 - j, i]]);
                }
            }
            return bitmap;
        }

        public static Bitmap PatternTable(int PN)
        {
            Bitmap bitmap = new Bitmap(TempPatternTable);
            if (NES_PPU_Register.PPUCTRL.V)
            {
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
                TempPatternTable = bitmap;
            }
            return bitmap;
        }

        public static Bitmap NameTabele(bool display = true)
        {
            Bitmap bitmap = new Bitmap(TempNameTable);

            if (display)
                if (NES_PPU_Register.PPUCTRL.V)
                {
                    bitmap = new Bitmap(TempNameTable.Size.Width, TempNameTable.Size.Height);
                    Graphics g = Graphics.FromImage(bitmap);
                    DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(0), 0, 0, 0);
                    DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(1), 1, 0, 32);
                    DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(2), 2, 30, 0);
                    DrowOneNameTable(g, NES_PPU_AttributeTable.AttributeTable(3), 3, 30, 32);
                    g.DrawRectangle(Pens.Red, XScroll, YScroll, 256, 240);
                    g.DrawRectangle(Pens.Green, 0, 0, 64 * 8 - 1, 60 * 8 - 1);
                    TempNameTable = bitmap;
                }
            return bitmap;
        }

        private static void DrowOneNameTable(Graphics g, ArrayList Attribute, int Nr, ushort X, ushort Y)
        {
            Parallel.For(X, X + 30 - 1, i =>
                {
                    int k = 0;
                    try
                    {
                        for (ushort j = Y; j < Y + 32; j++)
                        {
                            k = (i - X) + (j - Y) * 30;
                            int c = (int)Attribute[k];
                            int t = ((Address)NES_PPU_Memory.NameTableN[Nr][k]).value;
                            g.DrawImage(Tile((ushort)(t), c), j * 8, i * 8);
                        }
                    }
                    catch (System.Exception ex)
                    { k = k + 1 - 1; }
                });
        }

        public static Bitmap Display()
        {
            Bitmap Display = new Bitmap(TempDisplay);

            if (NES_PPU_Register.PPUCTRL.V)
            {
                Draw = true;
                Display = new Bitmap(TempDisplay.Size.Width, TempDisplay.Size.Height);
                Interrupt.NMI = true;
                Rectangle cropRect = new Rectangle(XScroll, YScroll, 257, 241);
                Graphics g = Graphics.FromImage(Display);
                Bitmap NameTabeleT = NameTabele();
                g.DrawImage(InsetObect(false), 0, 0);
                g.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), cropRect, GraphicsUnit.Pixel);
                g.DrawImage(InsetObect(true), 0, 0);
                TempDisplay = Display;
                Draw = false;
                NES_PPU_Register.PPUSTATUS.V = true;
            }
            return Display;
        }

        private static Bitmap InsetObect(bool Priory)
        {
            Bitmap Display = new Bitmap(TempDisplay.Width, TempDisplay.Height);
            Graphics g = Graphics.FromImage(Display);
            for (int i = 0; i < NES_PPU_OAM.SpriteTile.Count; i++)
            {
                if (Priory == ((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).Priority)
                {
                    Bitmap tile = Tile(((NES_PPU_OAM.Byte1)NES_PPU_OAM.SpriteTile[i]).Number,
                                                   ((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).Palette,
                                                   (((NES_PPU_OAM.Byte1)NES_PPU_OAM.SpriteTile[i]).Bank) ? (1) : (0));

                    if (((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).FlipH)
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    if (((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).FlipV)
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    g.DrawImage(tile, ((Address)NES_PPU_OAM.SpriteXc[i]).Value - 8,
                                                        ((Address)NES_PPU_OAM.SpriteYc[i]).Value - 1);
                }
            }
            return Display;
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
                            if (color[h] == Color.Transparent)
                                g.FillRectangle(new SolidBrush(NES_PPU_Palette.UniversalBackgroundColor()), o * 20, i * 20, ++o * 20, (i + 1) * 20);
                            else
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
                TempPaletteTable = bitmap;
            }
            return bitmap;
        }

    }
}
