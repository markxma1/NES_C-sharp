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
namespace NES
{
    public partial class NES_PPU
    {
        private static bool ScrollXoY = true;//false y ture x
        private static int xScrollTemp = 0;
        private static int yScrollTemp = 0;
        public static int xScroll = 0;
        public static int yScroll = 0;

        public static byte Scroll
        {
            get { return 0; }

            set
            {
                if (ScrollXoY)
                {
                    XScroll = value;
                }
                else
                {
                    if (value > 239)
                        value -= 255;
                    YScroll = value;
                }
                ScrollXoY = !ScrollXoY;
            }
        }

        private static int XScroll
        {
            get
            {
                return xScrollTemp;
            }

            set
            {
                AddxScroll(value);
                xScroll = value;
                if (!Draw)
                    xScrollTemp = value;
            }
        }

        private static int YScroll
        {
            get
            {
                return yScrollTemp;
            }

            set
            {
                value = AddyScroll(value);
                yScroll = value;
                if (!Draw)
                    yScrollTemp = value;
            }
        }

        private static int AddyScroll(int value)
        {
            int add = ((NES_PPU_Register.PPUCTRL.N & 2) == 0) ? (0) : (240);
            value += add;
            return value;
        }

        private static int AddxScroll(int value)
        {
            int add = ((NES_PPU_Register.PPUCTRL.N & 1) == 0) ? (0) : (256);
            value += add;
            return value;
        }
    }
}
