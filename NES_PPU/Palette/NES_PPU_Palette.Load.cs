///   Copyright 2016 Xma1
///
///   This file is part of NES-C#.
///
///   Foobar is free software: you can redistribute it and/or modify
///   it under the terms of the GNU General Public License as published by
///   the Free Software Foundation, either version 3 of the License, or
///   (at your option) any later version.
///
///   Foobar is distributed in the hope that it will be useful,
///   but WITHOUT ANY WARRANTY; without even the implied warranty of
///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
///   See the GNU General Public License for more details.
///
///   You should have received a copy of the GNU General Public License
///   along with Foobar. If not, see http://www.gnu.org/licenses/.
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
