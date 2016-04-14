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
using System.IO;
using System.Linq;

namespace NES
{
    public class NES_ROM
    {
        private static byte[] b;
        private static int end;
      
        /// <summary>
        /// http://wiki.nesdev.com/w/index.php/INES
        /// 
        /// Load .nes ins ROM
        /// </summary>
        /// <param name="filePath"></param>
        public static void LoadRom(string filePath)
        {
            b = File.ReadAllBytes(filePath);
            INES.ReadeHeader(b);
            LoadPRGROM();
            LoadPatternTable();
            ReadeTitle();
        }

        private static void ReadeTitle()
        {
            byte[] byteArray = b.Skip(end).ToArray();
            INES.title = System.Text.Encoding.UTF8.GetString(byteArray);
        }

        private static void LoadPatternTable()
        {
            int begin = end;
            end += INES.CHRROMSize;

            for (int i = begin; i < end; i++)
            {
                ((AddressSetup)NES_PPU_Memory.PatternTable[i - begin]).Value = b[i];
            }
        }

        private static void LoadPRGROM()
        {
            PatternTableBegin();
            for (int i = 0x10; i < end; i++)
            {
                ((AddressSetup)NES_Memory.PRGROM[i - 0x10]).Value = b[i];
                PRGROMMirror(0x10, i);
            }
        }

        private static void PatternTableBegin()
        {
            end = 0x10 + INES.PRGROMSize + ((INES.trainer) ? (512) : (0));
        }

        private static void PRGROMMirror(int begin, int i)
        {
            if (INES.PRGROMSize <= 16384)
            {
                NES_Memory.PRGROM[16384 + i - begin] = NES_Memory.PRGROM[i - begin];
                NES_Memory.Memory[0xC000 + i - begin] = NES_Memory.PRGROM[i - begin];
            }
        }

    }
}
