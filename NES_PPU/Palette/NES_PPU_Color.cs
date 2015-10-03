using System.Drawing;

namespace NES
{
    public class NES_PPU_Color
    {
        public Color[] color;
        public bool isNew = false;
        public NES_PPU_Color(Color[] color, bool isNew)
        {
            this.color = color;
            this.isNew = isNew;
        }
    }
}
