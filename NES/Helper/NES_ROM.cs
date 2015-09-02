using System.IO;
using System.Linq;

namespace NES
{
    class NES_ROM
    {
        /// <summary>
        /// http://wiki.nesdev.com/w/index.php/INES
        /// 
        /// Load .nes ins ROM
        /// </summary>
        /// <param name="filePath"></param>
        public static void LoadRom(string filePath)
        {
            byte[] b = File.ReadAllBytes(filePath);

            #region Header
            INES.PRGROMSize = 16384 * b[4];//Size of PRG ROM in 16 KB units
            INES.CHRROMSize = 8192 * b[5];//Size of CHR ROM in 8 KB units (Value 0 means the board uses CHR RAM)
            INES.PRGRAMSize = 8192 * b[8];//Size of PRG RAM in 8 KB units (Value 0 infers 8 KB for compatibility; see PRG RAM circuit)

            Flag6(b[6]);
            Flag7(b[7]);
            Flag9(b[9]);
            Flag10(b[10]);
            #endregion

            int begin = 0x10;
            int end = begin + INES.PRGROMSize + ((INES.trainer) ? (512) : (0)) - 1;

            for (int i = begin; i <= end; i++)
            {
                var test = i - begin;
                ((Address)NES_Memory.PRGROM[i - begin]).Value = b[i];
                if (INES.PRGROMSize <= 16384)
                {
                    NES_Memory.PRGROM[16384 + i - begin] = NES_Memory.PRGROM[i - begin];
                    NES_Memory.Memory[0xC000 + i - begin]= NES_Memory.PRGROM[i - begin];
                }
            }
            begin = end + 1;
            end += INES.CHRROMSize;

            for (int i = begin; i <= end; i++)
            {
                ((Address)NES_PPU_Memory.PatternTable[i - begin]).Value = b[i];
            }

            byte[] byteArray = (byte[])(b.Skip(end).ToArray());
            INES.title = System.Text.Encoding.UTF8.GetString(byteArray);
        }

        #region Flags

        /// <summary>
        /// In the iNES format, cartridge boards are divided into classes called "mappers" 
        /// based on similar board hardware and behavior, and each mapper has a number from 0 to 255.
        ///Some mappers, such as MMC1, MMC3, and AxROM, can control nametable mirroring. 
        ///They ignore bit 0. On the other hand, if bit 3 is true, the cart has 4 KiB of RAM at PPU $2000-$2FFF,
        ///and it ignores the mapper's CIRAM A10 output (as in Rad Racer 2). One exception is #218, which uses 
        ///the four-screen bit to switch between one-screen and V/H mirroring.
        ///Unlike the PowerPak, Famicom copiers did not implement even the most popular 
        ///Famicom mappers. Instead, games for the copiers were patched to use the mappers 
        ///that the copier supported, and some of these patches relied on extra code called a "trainer" 
        ///that the copier loaded into PRG RAM before starting the game. One of these copiers was made 
        ///by Front Fareast Industrial; hence the nickname "FFE" for these mapper hacks. It is probably
        ///not worth the effort to support FFE hacks in future emulators or copiers because substantially all 
        ///games available with an FFE hack also have a good dump without a trainer. 
        /// </summary>
        /// <param name="b"></param>
        private static void Flag6(byte b)
        {
            //7654321:
            //(3+0): 0xx0: vertical arrangement/horizontal mirroring (CIRAM A10 = PPU A11)
            //(3+0): 0xx1: horizontal arrangement/vertical mirroring (CIRAM A10 = PPU A10)
            //(3+0): 1xxx: four-screen VRAM
            switch (b & 0x9)
            {
                case 0: INES.arrangement = INES.Mirror.vertical; break;
                case 1: INES.arrangement = INES.Mirror.horisontal; break;
                default: INES.arrangement = INES.Mirror.four_screen; break;
            }

            //(1): 1: Cartridge contains battery-backed PRG RAM ($6000-7FFF) or other persistent memory
            if ((b & 0x2) > 0)
                INES.battery_backed = true;
            //(2): 1: 512-byte trainer at $7000-$71FF (stored before PRG data)
            if ((b & 0x4) > 0)
                INES.trainer = true;
            //(7-4): Lower nybble of mapper number
            INES.Lmapper = (b & 0xE0) >> 5;

        }

        /// <summary>
        /// The PlayChoice-10 bit is not part of the official specification, and most emulators simply ignore the extra 8KB of data.
        /// PlayChoice games are designed to look good with the 2C03 RGB PPU, 
        /// which handles color emphasis differently from a standard NES PPU.
        /// Vs. games have a coin slot and different palettes. The detection of which palette a particular game uses is left unspecified.
        /// NES 2.0 is a more recent extension to the format that allows more flexibility in ROM and RAM size, among other things. 
        /// </summary>
        /// <param name="b"></param>
        private static void Flag7(byte b)
        {
            if ((b & 0x1) > 0)
                INES.VSUnisystem = true;//VS Unisystem

            if ((b & 0x2) > 0)
                INES.PlayChoice = true;//PlayChoice-10 (8KB of Hint Screen data stored after CHR data)

            if ((b & 0xC) == 0x4)
                INES.NES2 = true;//If equal to 2, flags 8-15 are in NES 2.0 format

            INES.Hmapper = (b & 0xE0) >> 5; //Upper nybble of mapper number
        }

        /// <summary>
        /// Though in the official specification, very few emulators honor this bit as virtually no 
        /// ROM images in circulation make use of it. 
        /// </summary>
        /// <param name="b"></param>
        private static void Flag9(byte b)
        {
            if ((b & 0x1) > 0)
                INES.TVsystem = INES.TV.PAL;//TV system
            switch (b & 0x1)
            {
                case 0: INES.TVsystem = INES.TV.NTS; break;
                case 1: INES.TVsystem = INES.TV.PAL; break;
            }
        }

        /// <summary>
        /// This byte is not part of the official specification, and relatively few emulators honor it.
        /// The PRG RAM Size value (stored in byte 8) was recently added to the official specification; 
        /// as such, virtually no ROM images in circulation make use of it.
        /// Older versions of the iNES emulator ignored bytes 7-15, and several ROM management tools wrote messages in there. 
        /// Commonly, these will be filled with "DiskDude!", which results in 64 being added to the mapper number.
        /// A general rule of thumb: if the last 4 bytes are not all zero, and the header is not marked for NES 2.0 format, 
        /// an emulator should either mask off the upper 4 bits of the mapper number or simply refuse to load the ROM. 
        /// </summary>
        /// <param name="b"></param>
        private static void Flag10(byte b)
        {
            //TV system (0: NTSC; 2: PAL; 1/3: dual compatible)
            switch (b & 0x3)
            {
                case 0: INES.TVsystem = INES.TV.NTS; break;
                case 2: INES.TVsystem = INES.TV.PAL; break;
                default: INES.TVsystem = INES.TV.DUAL; break;
            }

            if ((b & 0xA) > 0)
                INES.present = true;//PRG RAM ($6000-$7FFF) (0: present; 1: not present)

            if ((b & 0xC) > 0)
                INES.Boardconflicts = true;//0: Board has no bus conflicts; 1: Board has bus conflicts
        }
        #endregion
    }
}
