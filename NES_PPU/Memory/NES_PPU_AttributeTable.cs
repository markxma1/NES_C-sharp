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
using System.Collections;

namespace NES
{
    /// <summary>
    /// http://wiki.nesdev.com/w/index.php/PPU_attribute_tables
    /// </summary>
    /// <param name="NR">Attribute tabellenummer</param>
    /// <returns>decoded table</returns>
    public class NES_PPU_AttributeTable
    {

        public static ArrayList AttributeTable(int NR)
        {

            ArrayList AttributeTable = getTable(NR);
            return CreateAL(AttributeTable);
        }

        private static ArrayList CreateAL(ArrayList AttributeTable)
        {
            ArrayList AL = new ArrayList();
            for (int i = 0; i < 0x8; i++)
            {
                Repeate(AL, AttributeTable, i, 0, 1);
                Repeate(AL, AttributeTable, i, 2, 3);
            }
            return AL;
        }

        private static void Repeate(ArrayList AL, ArrayList AttributeTable, int i, int shift1, int shift2)
        {
            for (int r = 0; r < 2; r++)
            {
                for (int j = 0; j < 0x8; j++)
                {
                    Block(AL, AttributeTable, i, j, shift1, shift2);
                }
            }
        }

        private static void Block(ArrayList AL, ArrayList AttributeTable, int i, int j, int shift1, int shift2)
        {
            SubBlock(AL, AttributeTable, i, j, shift1);
            SubBlock(AL, AttributeTable, i, j, shift2);
        }

        private static void SubBlock(ArrayList AL, ArrayList AttributeTable, int i, int j, int shift1)
        {
            for (int r = 0; r < 2; r++)
            {
                AL.Add(SplitAttribute(shift1, ((AddressSetup)AttributeTable[j + i * 8]).value));
            }
        }

        private static int SplitAttribute(int shift1, byte value)
        {
            return value >> shift1*2 & 3;
        }

        private static ArrayList getTable(int NR)
        {
            return NES_PPU_Memory.AttributeTableN[NR];
        }
    }
}
