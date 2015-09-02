using System.Collections;

namespace NES
{
    /// <summary>
    /// https://en.wikibooks.org/wiki/NES_Programming/Memory_Map 
    /// http://wiki.nesdev.com/w/index.php/PPU_memory_map
    /// </summary>
    public class NES_PPU_Memory
    {

        //Address 	Size 	Description
        //$0000 	$1000 	Pattern Table 0
        //$1000 	$1000 	Pattern Table 1
        //$2000 	$3C0 	Name Table 0
        //$23C0 	$40 	Attribute Table 0
        //$2400 	$3C0 	Name Table 1
        //$27C0 	$40 	Attribute Table 1
        //$2800 	$3C0 	Name Table 2
        //$2BC0 	$40 	Attribute Table 2
        //$2C00 	$3C0 	Name Table 3
        //$2FC0 	$40 	Attribute Table 3
        //$3000 	$F00 	Mirror of 2000h-2EFFh
        //$3F00 	$10 	BG Palette
        //$3F10 	    $10 	Sprite Palette
        //$3F20 	    $E0 	Mirror of 3F00h-3F1Fh

        static public ArrayList Memory = new ArrayList();
        static public ArrayList PatternTable = new ArrayList();
        static public ArrayList[] PatternTableN;
        static public ArrayList NameTable = new ArrayList();
        static public ArrayList[] NameTableN;
        static public ArrayList AttributeTable = new ArrayList();
        static public ArrayList[] AttributeTableN;
        static public ArrayList BGPalette = new ArrayList();
        static public ArrayList SpritePalette = new ArrayList();

        public NES_PPU_Memory()
        {
            CreateMemory();
            InitPatternTable();
            InitNameTable();
            InitAttributeTable();
            InitPaletteRAMIndexes();
            UpdateMemory();
        }

        private static void InitPaletteRAMIndexes()
        {
            for (int i = 0x3F00; i < 0x3F10; i++)
            {
                BGPalette.Add(Memory[i]);
                SpritePalette.Add(Memory[i + 0x10]);
            }
        }

        #region AttributeTable
        private static void InitAttributeTable()
        {
            InitTableN(ref AttributeTableN, 4);
            CreateAttributeTableN();
        }

        private static void CreateAttributeTableN()
        {
            InitAttributeTable0();
            InitAttributeTable1();
            InitAttributeTable2();
            InitAttributeTable3();
        }

        private static void InitAttributeTable3()
        {
            int Start = 0x2FC0;
            for (int i = Start; i < Start + 0x40; i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.vertical:
                        AttributeTableMirror(i, 3, 2, 0x2FC0);
                        break;
                    case INES.Mirror.horisontal:
                        AttributeTableMirror(i, 3, 1, 0x2FC0);
                        break;
                    default:
                        MemoryToAttributeTable(i, 3);
                        break;
                }
            }
        }

        private static void InitAttributeTable2()
        {
            int Start = 0x2BC0;
            for (int i = Start; i < Start + 0x40; i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.horisontal:
                        AttributeTableMirror(i, 2, 0, 0x2BC0);
                        break;
                    default:
                        MemoryToAttributeTable(i, 2);
                        break;
                }
            }
        }

        private static void InitAttributeTable1()
        {
            int Start = 0x27C0;
            for (int i = Start; i < Start + 0x40; i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.vertical:
                        AttributeTableMirror(i, 1, 0, 0x27C0);
                        break;
                    default:
                        MemoryToAttributeTable(i, 1);
                        break;
                }
            }
        }

        private static void InitAttributeTable0()
        {
            int Start = 0x23C0;
            for (int i = Start; i < Start + 0x40; i++)
            {
                MemoryToAttributeTable(i, 0);
            }
        }

        private static void MemoryToAttributeTable(int i, int TableNr)
        {
            AttributeTable.Add(Memory[i]);
            AttributeTableN[TableNr].Add(Memory[i]);
        }

        private static void AttributeTableMirror(int i, int MirrorTable, int OrigenalTable, int StartAddress)
        {
            AttributeTable.Add(AttributeTableN[OrigenalTable][i - StartAddress]);
            AttributeTableN[MirrorTable].Add(AttributeTableN[OrigenalTable][i - StartAddress]);
            Memory[i] = AttributeTableN[OrigenalTable][i - StartAddress];
        }
        #endregion

        #region NameTable
        private static void InitNameTable()
        {
            InitTableN(ref NameTableN, 4);
            CreateNameTableN();
        }

        private static void CreateNameTableN()
        {
            InitNameTable0();
            InitNameTable1();
            InitNameTable2();
            InitNameTable3();
        }

        private static void InitNameTable3()
        {
            int Start = 0x2C00;
            for (int i = Start; i < Start + 0x3C0; i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.vertical:
                        NameTableMirror(i, 3, 2, 0x2C00);
                        break;
                    case INES.Mirror.horisontal:
                        NameTableMirror(i, 3, 1, 0x2C00);
                        break;
                    default:
                        MemoryToNameTable(i, 3);
                        break;
                }
            }
        }

        private static void InitNameTable2()
        {
            int Start = 0x2800;
            for (int i = Start; i < Start + 0x3C0; i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.horisontal:
                        NameTableMirror(i, 2, 0, 0x2800);
                        break;
                    default:
                        MemoryToNameTable(i, 2);
                        break;
                }
            }
        }

        private static void InitNameTable1()
        {
            int Start = 0x2400;
            for (int i = Start; i < Start + 0x3C0; i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.vertical:
                        NameTableMirror(i, 1, 0, 0x2400);
                        break;
                    default:
                        MemoryToNameTable(i, 1);
                        break;
                }
            }
        }

        private static void InitNameTable0()
        {
            int Start = 0x2000;
            for (int i = Start; i < Start + 0x3C0; i++)
            {
                MemoryToNameTable(i, 0);
            }
        }

        private static void MemoryToNameTable(int i, int TableNr)
        {
            NameTable.Add(Memory[i]);
            NameTableN[TableNr].Add(Memory[i]);
        }

        private static void NameTableMirror(int i, int MirrorTable, int OrigenalTable, int StartAddress)
        {
            NameTable.Add(NameTableN[OrigenalTable][i - StartAddress]);
            NameTableN[MirrorTable].Add(NameTableN[OrigenalTable][i - StartAddress]);
            Memory[i] = NameTableN[OrigenalTable][i - StartAddress];
        }
        #endregion

        #region PatternTable
        public static void InitPatternTable()
        {
            InitTableN(ref PatternTableN, 2);
            CreatePatternTable();
        }

        private static void CreatePatternTable()
        {
            for (int i = 0; i < 0x1000; i++)
            {
                MemoryToPatternTable(i, 0);
            }
            for (int i = 0; i < 0x1000; i++)
            {
                MemoryToPatternTable(0x1000 + i, 1);
            }
        }

        private static void MemoryToPatternTable(int i, int TableNr)
        {
            PatternTable.Add(Memory[i]);
            PatternTableN[TableNr].Add(Memory[i]);
        }
        #endregion

        #region Memory
        private static void CreateMemory()
        {
            for (int i = 0; i <= 0x3FFF; i++)
            {
                Memory.Add(new Address(i));
                MemoryMirror(i);
            }
        }

        private static void UpdateMemory()
        {
            for (int i = 0; i < 0x3FFF; i++)
            {
                MemoryMirror(i);
            }
        }

        /// <summary>
        /// Spiegelt Memory Addressen.
        /// </summary>
        /// <param name="i"></param>
        /// <returns>Werft True die Addresse ein Spiegel von einer anderer ist. </returns>
        private static void MemoryMirror(int i)
        {
            if (i >= 0x3000 && i < 0x3F00)
                Memory[i] = (Memory[i - 0x1000]);
            else
                if (i >= 0x3F20 && i < 0x3F20 + 0xE0)
                Memory[i] = (Memory[i - 0x20]);
        }
        #endregion

        private static void InitTableN(ref ArrayList[] Table, int Count)
        {
            Table = new ArrayList[Count];
            for (int i = 0; i < Count; i++)
                Table[i] = new ArrayList();
        }
    }
}
