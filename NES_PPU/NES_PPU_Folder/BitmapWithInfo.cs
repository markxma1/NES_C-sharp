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
using NES_PPU;

namespace NES
{
    class BitmapWithInfo
    {
        Picture bitmap;
        byte[,] pattern;
        public bool isNew;
        public byte[] cID;

        public BitmapWithInfo(Picture bitmap, byte[,] pattern, byte[] cID, bool isNew = true)
        {
            this.bitmap = new Picture(bitmap);
            this.pattern = pattern;
            this.isNew = isNew;
            this.cID = cID;
        }

        public Picture Image
        {
            get
            {
                return new Picture(bitmap);
            }

            set
            {
                bitmap = new Picture(value);
            }
        }

        public byte[,] Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                pattern = value;
            }
        }
    }
}
