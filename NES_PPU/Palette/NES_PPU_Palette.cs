using System.Drawing;

namespace NES
{
    public class NES_PPU_Palette
    {
        public static Color[] PPUpalettes = new Color[0x40];

        public NES_PPU_Palette()
        {
            InitPalletesFromBMP(@".\Palleres\2C03and2C05.bmp");
        }

        private static void InitPalletesFromBMP(string Path)
        {
            Bitmap Pallete = LoadPalleteFromBMP(Path);
            LoadPallete(Pallete);
        }

        private static void LoadPallete(Bitmap Pallete)
        {
            for (int j = 0; j < 4; j++)
            {
                LoadRow(Pallete, j);
            }
        }

        private static void LoadRow(Bitmap Pallete, int j)
        {
            for (int i = 0; i < 16; i++)
            {
                PPUpalettes[i + j * 16] = Pallete.GetPixel(i, j);
            }
        }

        private static Bitmap LoadPalleteFromBMP(string Path)
        {
            Bitmap Pallete = null;
            using (var image = new Bitmap(Path))
            {
                Pallete = new Bitmap(image);
            }
            return Pallete;
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

        public static Color[] getBGColorPalette(int start)
        {
            Color[] color = {
                Color.Transparent,
                getBGColorAsRGB(start * 4 + 1),
                getBGColorAsRGB(start * 4 + 2),
                getBGColorAsRGB(start * 4 + 3) };
            return color;
        }

        public static Color[] getSpriteColorPalette(int start)
        {
            Color[] color = {
             Color.Transparent,
             getSpriteColorAsRGB(start * 4 + 1),
             getSpriteColorAsRGB(start * 4 + 2),
             getSpriteColorAsRGB(start * 4 + 3)};
            return color;
        }

        public static Color UniversalBackgroundColor()
        {
            return PPUpalettes[((Address)NES_PPU_Memory.BGPalette[0]).Value];
        }

        private static Color getBGColorAsRGB(int BGAdress)
        {
            return PPUpalettes[((Address)NES_PPU_Memory.BGPalette[BGAdress]).Value];
        }

        private static Color getSpriteColorAsRGB(int SpriteAdress)
        {
            return PPUpalettes[((Address)NES_PPU_Memory.SpritePalette[SpriteAdress]).Value];
        }

        private static Color getColorAsRGB(int Adress)
        {
            return PPUpalettes[((Address)NES_PPU_Memory.Memory[Adress]).Value];
        }

        private static int OctToHex(int a)
        {
            return (int)(((double)a / 7) * 255);
        }
    }
}
