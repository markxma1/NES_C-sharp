using System.Collections;
using System.Linq;

namespace NES
{
    /// <summary>
    /// https://en.wikibooks.org/wiki/NES_Programming/Memory_Map 
    /// http://wiki.nesdev.com/w/index.php/PPU_memory_map
    /// </summary>
    class NES_PPU_Memory
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
        static public ArrayList PatternTable0 = new ArrayList();
        static public ArrayList PatternTable1 = new ArrayList();
        static public ArrayList NameTable = new ArrayList();
        static public ArrayList[] NameTableN;
        static public ArrayList AttributeTable = new ArrayList();
        static public ArrayList AttributeTable0 = new ArrayList();
        static public ArrayList AttributeTable1 = new ArrayList();
        static public ArrayList AttributeTable2 = new ArrayList();
        static public ArrayList AttributeTable3 = new ArrayList();
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
            InitBGPalette();
            InitSpritePalette();
        }

        private static void InitSpritePalette()
        {
            for (int i = 0x3F10; i < (0x3F10 + 0x10); i++)
            {
                SpritePalette.Add(Memory[i]);
            }
        }

        private static void InitBGPalette()
        {
            for (int i = 0x3F00; i < (0x3F00 + 0x10); i++)
            {
                BGPalette.Add(Memory[i]);
            }
        }

        private static void InitAttributeTable()
        {
            for (int i = 0x23C0; i <= (0x23C0 + 0x40 - 1); i++)
            {
                AttributeTable.Add(Memory[i]);
                AttributeTable0.Add(Memory[i]);
            }
            for (int i = 0x27C0; i <= (0x27C0 + 0x40 - 1); i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.vertical:
                        AttributeTable.Add(AttributeTable0[i - 0x27C0]);
                        AttributeTable1.Add(AttributeTable0[i - 0x27C0]);
                        Memory[i] = AttributeTable0[i - 0x27C0];
                        break;
                    default:
                        AttributeTable.Add(Memory[i]);
                        AttributeTable1.Add(Memory[i]);
                        break;
                }
            }
            for (int i = 0x2BC0; i <= (0x2BC0 + 0x40 - 1); i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.horisontal:
                        AttributeTable.Add(AttributeTable0[i - 0x2BC0]);
                        AttributeTable2.Add(AttributeTable0[i - 0x2BC0]);
                        Memory[i] = AttributeTable0[i - 0x2BC0];
                        break;
                    default:
                        AttributeTable.Add(Memory[i]);
                        AttributeTable2.Add(Memory[i]);
                        break;
                }

            }
            for (int i = 0x2FC0; i <= (0x2FC0 + 0x40 - 1); i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.vertical:
                        AttributeTable.Add(AttributeTable2[i - 0x2FC0]);
                        AttributeTable3.Add(AttributeTable2[i - 0x2FC0]);
                        Memory[i] = AttributeTable2[i - 0x2FC0];
                        break;
                    case INES.Mirror.horisontal:
                        AttributeTable.Add(AttributeTable1[i - 0x2FC0]);
                        AttributeTable3.Add(AttributeTable1[i - 0x2FC0]);
                        Memory[i] = AttributeTable1[i - 0x2FC0];
                        break;
                    default:
                        AttributeTable.Add(Memory[i]);
                        AttributeTable3.Add(Memory[i]);
                        break;
                }
            }
        }

        private static void InitNameTable()
        {
            InitNameTableN();
            InitNameTable0();
            InitNameTable1();
            InitNameTable2();
            InitNameTable3();
        }

        private static void InitNameTable3()
        {
            for (int i = 0x2C00; i <= (0x2C00 + 0x3C0 - 1); i++)
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
                        NameTable.Add(Memory[i]);
                        NameTableN[3].Add(Memory[i]);
                        break;
                }
            }
        }

        private static void NameTableMirror(int i, int MirrorTable, int OrigenalTable, int StartAddress)
        {
            NameTable.Add(NameTableN[OrigenalTable][i - StartAddress]);
            NameTableN[MirrorTable].Add(NameTableN[OrigenalTable][i - StartAddress]);
            Memory[i] = NameTableN[OrigenalTable][i - StartAddress];
        }

        private static void InitNameTable2()
        {
            for (int i = 0x2800; i <= (0x2800 + 0x3C0 - 1); i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.horisontal:
                        NameTableMirror(i, 2, 0, 0x2800);
                        break;
                    default:
                        NameTable.Add(Memory[i]);
                        NameTableN[2].Add(Memory[i]);
                        break;
                }
            }
        }

        private static void InitNameTable1()
        {
            for (int i = 0x2400; i <= (0x2400 + 0x3C0 - 1); i++)
            {
                switch (INES.arrangement)
                {
                    case INES.Mirror.vertical:
                        NameTableMirror(i, 1, 0, 0x2400);
                        break;
                    default:
                        NameTable.Add(Memory[i]);
                        NameTableN[1].Add(Memory[i]);
                        break;
                }
            }
        }

        private static void InitNameTable0()
        {
            for (int i = 0x2000; i <= (0x2000 + 0x3C0 - 1); i++)
            {
                NameTable.Add(Memory[i]);
                NameTableN[0].Add(Memory[i]);
            }
        }

        private static void InitNameTableN()
        {
            NameTableN = new ArrayList[4];
            for (int i = 0; i < 4; i++)
                NameTableN[i] = new ArrayList();
        }

        private static void InitPatternTable()
        {
            for (int i = 0; i <= 0x0FFF; i++)
            {
                PatternTable.Add(Memory[i]);
                PatternTable0.Add(Memory[i]);
            }
            for (int i = 0x1000; i <= 0x1FFF; i++)
            {
                PatternTable.Add(Memory[i]);
                PatternTable1.Add(Memory[i]);
            }
        }

        private static void CreateMemory()
        {
            for (int i = 0; i <= 0x3FFF; i++)
            {
                if (i >= 0x3000 && i < 0x3000 + 0xF00)
                    Memory.Add(Memory[i - 0x1000]);
                else
                    if (i >= 0x3F20 && i < 0x3F20 + 0xE0)
                    Memory.Add(Memory[i - 0x20]);
                else
                    Memory.Add(new Adress(i));
            }
        }

        private static void UpdateMemory()
        {
            for (int i = 0; i < 0x3FFF; i++)
            {
                if (i >= 0x3000 && i < 0x3000 + 0xF00)
                    Memory[i] = (Memory[i - 0x1000]);
                else
                    if (i >= 0x3F20 && i < 0x3F20 + 0xE0)
                    Memory[i] = (Memory[i - 0x20]);
            }
        }
    }
}
