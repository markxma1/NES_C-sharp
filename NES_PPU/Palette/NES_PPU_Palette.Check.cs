///   Copyright 2016 Xma1
///
///   This file is part of NES-C#.
///
///   NES-C# is free software: you can redistribute it and/or modify
///   it under the terms of the GNU General Public License as published by
///   the Free Software Foundation, either version 3 of the License, or
///   (at your option) any later version.
///
///   NES-C# is distributed in the hope that it will be useful,
///   but WITHOUT ANY WARRANTY; without even the implied warranty of
///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
///   See the GNU General Public License for more details.
///
///   You should have received a copy of the GNU General Public License
///   along with NES-C#. If not, see http://www.gnu.org/licenses/.
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
