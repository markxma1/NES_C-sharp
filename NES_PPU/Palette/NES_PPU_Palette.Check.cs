namespace NES
{
    public partial class NES_PPU_Palette
    {
        private static bool BGIsNew(int start)
        {
            bool isNew = ((Address)NES_PPU_Memory.BGPalette[start * 4 + 1]).isNew();
            isNew &= ((Address)NES_PPU_Memory.BGPalette[start * 4 + 2]).isNew();
            isNew &= ((Address)NES_PPU_Memory.BGPalette[start * 4 + 3]).isNew();
            return isNew;
        }

        private static bool SpriteIsNew(int start)
        {
            bool isNew = ((Address)NES_PPU_Memory.SpritePalette[start * 4 + 1]).isNew();
            isNew &= ((Address)NES_PPU_Memory.SpritePalette[start * 4 + 2]).isNew();
            isNew &= ((Address)NES_PPU_Memory.SpritePalette[start * 4 + 3]).isNew();
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
