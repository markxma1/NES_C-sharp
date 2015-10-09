using System.Drawing;

namespace NES
{
    public class NES_PPU_Color
    {
        public Color[] color;
        public bool isNewPalette = false;
        public bool[] isNewColor;
        public NES_PPU_Color(Color[] color, bool isNewPalette, bool[] isNewColor)
        {
            this.color = color;
            this.isNewPalette = isNewPalette;
            this.isNewColor = isNewColor;
        }
    }
}
