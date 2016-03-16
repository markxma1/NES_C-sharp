using System.Collections;
using System.Collections.Generic;
using System.Linq;

///https://en.wikibooks.org/wiki/NES_Programming
namespace NES
{
    public class NES_Memory
    {

        ////Address Range (Hexadecimal)	Size	Notes (Page size is 256 bytes)
        ////$0000–$00FF	256 bytes	Zero Page — Special Zero Page addressing modes give faster memory read/write access
        ////$0100–$01FF	256 bytes	Stack memory
        ////$0200–$07FF	1536 bytes	RAM
        ////$0800–$0FFF	2048 bytes	Mirror of $0000–$07FF	$0800–$08FF Zero Page
        ////$0900–$09FF Stack
        ////$0A00–$0FFF RAM
        ////$1000–$17FF	2048 bytes	Mirror of $0000–$07FF	$1000–$10FF Zero Page
        ////$1100–$11FF Stack
        ////$1200–$17FF RAM
        ////$1800–$1FFF	2048 bytes	Mirror of $0000–$07FF	$1800–$18FF Zero Page
        ////$1900–$19FF Stack
        ////$1A00–$1FFF RAM
        ////$2000–$2007	8 bytes	Input/Output registers
        ////$2008–$3FFF	8184 bytes	Mirror of $2000–$2007 (multiple times)
        ////$4000–$401F	32 bytes	Input/Output registers
        ////$4020–$5FFF	8160 bytes	Expansion ROM — Used with Nintendo's MMC5 to expand the capabilities of VRAM.
        ////$6000–$7FFF	8192 bytes	SRAM — Save Ram used to save data between game plays.
        ////$8000–$FFFF	32768 bytes	PRG-ROM
        ////$FFFA–$FFFB	2 bytes	Address of Non Maskable Interrupt (NMI) handler routine
        ////$FFFC–$FFFD	2 bytes	Address of Power on reset handler routine
        ////$FFFE–$FFFF	2 bytes	Address of Break (BRK instruction) handler routine

        static public ArrayList Memory = new ArrayList();
        static public ArrayList ZeroPage = new ArrayList();
        static public ArrayList Stack = new ArrayList();
        static public ArrayList RAM = new ArrayList();
        static public ArrayList PPU = new ArrayList();
        static public ArrayList APU = new ArrayList();
        static public ArrayList Joystick = new ArrayList();
        static public ArrayList IO = new ArrayList();
        static public ArrayList EROM = new ArrayList();
        static public ArrayList SRAM = new ArrayList();
        static public ArrayList PRGROM = new ArrayList();
        static public ArrayList NMI = new ArrayList();
        static public ArrayList POR = new ArrayList();
        static public ArrayList BRK = new ArrayList();

        public static List<byte> MemTest(ArrayList list)
        {
            return list.Cast<AddressSetup>().Select(a => a.value).ToList();
        }

        public NES_Memory()
        {
            InitMemory();
            ResetBlocks();
        }

        public static void ResetBlocks()
        {
            InitZeroPage();
            InitStack();
            InitRAM();
            InitPPU();
            InitAPU();
            InitJoystick();
            InitIO();
            InitEROM();
            InitSRAM();
            InitPRGROM();
            InitNMI();
            InitPOR();
            InitBRK();
        }

        private static void InitIO()
        {
            for (int i = 0x4018; i <= 0x401F; i++)
            {
                IO.Add(Memory[i]);
                ((AddressSetup)Memory[i]).value = 0xFF;
            }
        }

        private static void InitJoystick()
        {
            for (int i = 0x4016; i <= 0x4017; i++)
            {
                Joystick.Add(Memory[i]);
                ((AddressSetup)Memory[i]).value = 0xFF;
            }
        }

        private static void InitAPU()
        {
            for (int i = 0x4000; i <= 0x4015; i++)
            {
                APU.Add(Memory[i]);
                ((AddressSetup)Memory[i]).value = 0xFF;
            }
        }

        private static void InitBRK()
        {
            for (int i = 0xFFFE; i <= 0xFFFF; i++)
            {
                BRK.Add(Memory[i]);
            }
        }

        private static void InitPOR()
        {
            for (int i = 0xFFFC; i <= 0xFFFD; i++)
            {
                POR.Add(Memory[i]);
            }
        }

        private static void InitNMI()
        {
            for (int i = 0xFFFA; i <= 0xFFFB; i++)
            {
                NMI.Add(Memory[i]);
            }
        }

        private static void InitPRGROM()
        {
            for (int i = 0x8000; i <= 0xFFFF; i++)
            {
                PRGROM.Add(Memory[i]);
            }
        }

        private static void InitSRAM()
        {
            for (int i = 0x6000; i <= 0x7FFF; i++)
            {
                SRAM.Add(Memory[i]);
                ((AddressSetup)Memory[i]).value = 0;
            }
        }

        private static void InitEROM()
        {
            for (int i = 0x4020; i <= 0x5FFF; i++)
            {
                EROM.Add(Memory[i]);
                if (i < 0x5000)
                    ((AddressSetup)Memory[i]).value = 0xFF;
                else
                    ((AddressSetup)Memory[i]).value = 0;
            }
        }

        private static void InitPPU()
        {
            for (int i = 0x2000; i <= 0x2007; i++)
            {
                PPU.Add(Memory[i]);
                Memory[i + 0x08] = Memory[i];
                ((AddressSetup)Memory[i]).value = 0;
            }
        }

        private static void InitRAM()
        {
            for (int i = 0x0200; i <= 0x07FF; i++)
            {
                RAM.Add(Memory[i]);
            }
        }

        private static void InitStack()
        {
            for (int i = 0x0100; i <= 0x01FF; i++)
            {
                Stack.Add(Memory[i]);
            }
        }

        private static void InitZeroPage()
        {
            for (int i = 0x00; i <= 0x00FF; i++)
            {
                ZeroPage.Add(Memory[i]);
            }
        }

        private static void InitMemory()
        {
            for (int i = 0; i <= 0xFFFF; i++)
            {
                if (i >= 0x0800 && i <= 0x0FFF)
                    Memory.Add(Memory[i - 0x800]);
                else
                    if (i >= 0x1000 && i <= 0x17FF)
                        Memory.Add(Memory[i - 0x1000]);
                    else
                        if (i >= 0x1800 && i <= 0x1FFF)
                            Memory.Add(Memory[i - 0x1800]);
                        else
                            if ((i & 4) == 0)
                                Memory.Add(new AddressSetup(0x00,i));
                            else
                                Memory.Add(new AddressSetup(0xFF,i));
            }
        }
    }
}
