namespace NES
{
    class INES
    {
        public enum TV
        {
            NTS,
            PAL,
            DUAL
        }
     
        public enum Mirror
        {
            vertical,
            horisontal,
            four_screen
        }
      
        /// <summary>
        /// 0xx0: vertical arrangement/horizontal mirroring (CIRAM A10 = PPU A11)
        /// 0xx1: horizontal arrangement/vertical mirroring (CIRAM A10 = PPU A10)
        /// 1xxx: four-screen VRAM
        /// </summary>
        public static Mirror arrangement;

        /// <summary>
        /// Size of PRG ROM in 16 KB units
        /// </summary>
        public static int PRGROMSize;

        /// <summary>
        /// Size of CHR ROM in 8 KB units (Value 0 means the board uses CHR RAM)
        /// </summary>
        public static int CHRROMSize;

        /// <summary>
        /// Size of PRG RAM in 8 KB units (Value 0 infers 8 KB for compatibility; see PRG RAM circuit)
        /// </summary>
        public static int PRGRAMSize;

        /// <summary>
        /// Some ROM-Images additionally contain a 128-byte (or sometimes 127-byte) title at the end of the file. 
        /// </summary>
        public static string title;
       
        /// <summary>
        /// Cartridge contains battery-backed PRG RAM ($6000-7FFF) or other persistent memory
        /// </summary>
        public static bool battery_backed;

        /// <summary>
        /// 512-byte trainer at $7000-$71FF (stored before PRG data)
        /// </summary>
        public static bool trainer;

        /// <summary>
        /// Lower nybble of mapper number
        /// </summary>
        public static int Lmapper;

        /// <summary>
        /// Upper nybble of mapper number
        /// </summary>
        public static int Hmapper;

        /// <summary>
        /// VS Unisystem
        /// </summary>
        public static bool VSUnisystem;

        /// <summary>
        /// PlayChoice-10 (8KB of Hint Screen data stored after CHR data)
        /// </summary>
        public static bool PlayChoice;

        /// <summary>
        /// If equal to 2, flags 8-15 are in NES 2.0 format
        /// </summary>
        public static bool NES2;

        /// <summary>
        /// TV system (0: NTSC; 2: PAL; 1/3: dual compatible)
        /// </summary>
        public static TV TVsystem;

        /// <summary>
        /// PRG RAM ($6000-$7FFF) (0: present; 1: not present)
        /// </summary>
        public static bool present;

        /// <summary>
        /// 0: Board has no bus conflicts; 1: Board has bus conflicts
        /// </summary>
        public static bool Boardconflicts;
    }
}
