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
using NES_PPU;
using System.Drawing;

namespace NES
{

    public partial class NES_PPU
    {

        private static Picture TempPaletteTable = new Picture(16 * 20, 2 * 20);

        public NES_PPU()
        {
            new NES_PPU_Register();
            new NES_PPU_OAM();
            new NES_PPU_Memory();
            new NES_PPU_Palette();
            new NES_PPU_AttributeTable();
        }

        public static Picture PaletteTable()
        {
            Picture bitmap = new Picture(TempPaletteTable);
            if (NES_PPU_Register.PPUCTRL.V)
            {
                bitmap.FillRectangle(NES_PPU_Palette.UniversalBackgroundColor(), 0, 0, bitmap.Size.Width, bitmap.Size.Height);
                int k = 0;
                for (ushort i = 0; i < 2; i++)
                {
                    int o = 0;
                    for (ushort j = 0; j < 4; j++)
                    {
                        var color = NES_PPU_Palette.getPalette(k++);
                        for (int h = 0; h < 4; h++)
                        {
                            if (color.color[h] == Color.Transparent)
                                bitmap.FillRectangle(NES_PPU_Palette.UniversalBackgroundColor(), o * 20, i * 20, ++o * 20, (i + 1) * 20);
                            else
                                bitmap.FillRectangle(color.color[h], o * 20, i * 20, ++o * 20, (i + 1) * 20);
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
                            bitmap.DrawInfoRectangle(Color.Black, o * 20, i * 20, ++o * 20, (i + 1) * 20);
                        }
                    }
                }
                TempPaletteTable = bitmap;
            }
            return bitmap;
        }
    }
}
