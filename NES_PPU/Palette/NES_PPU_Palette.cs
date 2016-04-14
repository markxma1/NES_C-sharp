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
        public static Color[] PPUpalettes = new Color[0x40];
        private static bool[] isNewColor=new bool[4];

        public NES_PPU_Palette()
        {
            InitPalletesFromBMP(@".\Palletes\2C03and2C05.bmp");
        }

        public static NES_PPU_Color getPalette(int Nr)
        {
            if (Nr < 4) { return getBGColorPalette(Nr); }
            if (Nr < 8) { return getSpriteColorPalette(Nr - 4); }
            return DefaultPalette();
        }

        private static NES_PPU_Color DefaultPalette()
        {
            Color[] color = {
             Color.Black,
             Color.Red,
             Color.Green,
             Color.Blue};
            isNewColor = new bool[4];
            isNewColor[0] = isNewColor[1] = isNewColor[2]=true;

            return new NES_PPU_Color(color, true, isNewColor);
        }

        public static NES_PPU_Color getBGColorPalette(int start)
        {
            Color[] color = {
                Color.Transparent,
                getBGColorAsRGB(start * 4 + 1),
                getBGColorAsRGB(start * 4 + 2),
                getBGColorAsRGB(start * 4 + 3) };

            return new NES_PPU_Color(color, BGIsNew(start), isNewColor);
        }

        public static NES_PPU_Color getSpriteColorPalette(int start)
        {
            Color[] color = {
             Color.Transparent,
             getSpriteColorAsRGB(start * 4 + 1),
             getSpriteColorAsRGB(start * 4 + 2),
             getSpriteColorAsRGB(start * 4 + 3)};

            return new NES_PPU_Color(color, SpriteIsNew(start), isNewColor);
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
