using System.Collections;

namespace NES
{
    /// <summary>
    /// http://wiki.nesdev.com/w/index.php/PPU_attribute_tables
    /// </summary>
    /// <param name="NR">Attribute tabellenummer</param>
    /// <returns>decoded table</returns>
    class NES_PPU_AttributeTable
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
                Repeate(AL, AttributeTable, i, 0, 2);
                Repeate(AL, AttributeTable, i, 4, 6);
            }
            return AL;
        }

        private static void Repeate(ArrayList AL, ArrayList AttributeTable, int i, int shift1, int shift2)
        {
            for (int r = 0; r < 2; r++)
            {
                for (int j = 0; j < 0x8; j++)
                {
                    Block(AL, AttributeTable, i, j, 0, 2);
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
                AL.Add(((Adress)AttributeTable[j + i * 8]).value >> shift1 & 3);
            }
        }

        private static ArrayList getTable(int NR)
        {
            switch (NR)
            {
                case 0: return NES_PPU_Memory.AttributeTable0;
                case 1: return NES_PPU_Memory.AttributeTable1;
                case 2: return NES_PPU_Memory.AttributeTable2;
                default: return NES_PPU_Memory.AttributeTable3;
            }
        }
    }
}
