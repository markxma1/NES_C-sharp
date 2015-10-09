namespace NES
{
    public partial class NES_PPU_Palette
    {
        private static bool BGIsNew(int start)
        {
            isNewColor = new bool[4];
            isNewColor[0] = false;
            bool isNew = isNewColor[1] = ((Address)NES_PPU_Memory.BGPalette[start * 4 + 1]).isNew();
            isNew |= isNewColor[2] = ((Address)NES_PPU_Memory.BGPalette[start * 4 + 2]).isNew();
            isNew |= isNewColor[3] = ((Address)NES_PPU_Memory.BGPalette[start * 4 + 3]).isNew();
            return isNew;
        }

        private static bool SpriteIsNew(int start)
        {
            isNewColor = new bool[4];
            isNewColor[0] = false;
            bool isNew = isNewColor[1] = ((Address)NES_PPU_Memory.SpritePalette[start * 4 + 1]).isNew();
            isNew |= isNewColor[2] = ((Address)NES_PPU_Memory.SpritePalette[start * 4 + 2]).isNew();
            isNew |= isNewColor[3] = ((Address)NES_PPU_Memory.SpritePalette[start * 4 + 3]).isNew();
            return isNew;
        }

        public static void setAllPaletesAsOld()
        {
            foreach (Address a in NES_PPU_Memory.BGPalette)
            {
                a.setAsOld();
            }

            foreach (Address a in NES_PPU_Memory.SpritePalette)
            {
                a.setAsOld();
            }
        }
    }
}
