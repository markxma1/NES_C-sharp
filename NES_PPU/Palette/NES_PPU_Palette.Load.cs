using System.Drawing;

namespace NES
{
    public partial class NES_PPU_Palette
    {
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
    }
}
