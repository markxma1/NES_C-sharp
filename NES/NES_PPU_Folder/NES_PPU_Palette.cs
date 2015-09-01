using System.Drawing;

namespace NES
{
    class NES_PPU_Palette
    {
        public static Color[] PPUpalettes = new Color[0x40];

        public NES_PPU_Palette()
        {
            InitPalletes2C03and2C05();
        }

        private static void InitPalletes2C03and2C05()
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

        public static Color[] getPalette(int Nr)
        {

            if (Nr < 4) { return getBGColorPalette(Nr); }
            if (Nr < 8) { return getSpriteColorPalette(Nr - 4); }
            return DefaultPalette();
        }

        private static Color[] DefaultPalette()
        {
            Color[] color = {
             Color.Black,
             Color.Red,
             Color.Green,
             Color.Blue};
            return color;
        }

        private static Color[] getBGColorPalette(int start)
        {
            Color[] color = {
                UniversalBackgroundColor(),
                getBGColorAsRGB(start * 4 + 1),
                getBGColorAsRGB(start * 4 + 2),
                getBGColorAsRGB(start * 4 + 3) };
            return color;
        }

        private static Color[] getSpriteColorPalette(int start)
        {
            Color[] color = {
             Color.LightGreen,
             getSpriteColorAsRGB(start * 4 + 1),
             getSpriteColorAsRGB(start * 4 + 2),
             getSpriteColorAsRGB(start * 4 + 3)};
            return color;
        }

        public static Color UniversalBackgroundColor()
        {
            return PPUpalettes[((Adress)NES_PPU_Memory.BGPalette[0]).Value];
        }

        private static Color getBGColorAsRGB(int BGAdress)
        {
            return PPUpalettes[((Adress)NES_PPU_Memory.BGPalette[BGAdress]).Value];
        }

        private static Color getSpriteColorAsRGB(int SpriteAdress)
        {
            return PPUpalettes[((Adress)NES_PPU_Memory.SpritePalette[SpriteAdress]).Value];
        }

        private static Color getColorAsRGB(int Adress)
        {
            return PPUpalettes[((Adress)NES_PPU_Memory.Memory[Adress]).Value];
        }

        private static int OctToHex(int a)
        {
            return (int)(((double)a / 7) * 255);
        }
    }
}
