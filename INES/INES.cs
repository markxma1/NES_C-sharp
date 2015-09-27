namespace NES
{
    public class INES
    {
        #region INIT
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

        #endregion

        #region Header

        public static void ReadeHeader(byte[] b)
        {
            ReadeSize(b);
            Flags(b);
        }

        /// <summary>
        ///Size of PRG ROM in 16 KB units
        ///Size of CHR ROM in 8 KB units (Value 0 means the board uses CHR RAM)
        ///Size of PRG RAM in 8 KB units (Value 0 infers 8 KB for compatibility; see PRG RAM circuit)
        /// </summary>
        /// <param name="b"></param>
        private static void ReadeSize(byte[] b)
        {
            PRGROMSize = 16384 * b[4];
            CHRROMSize = 8192 * b[5];
            PRGRAMSize = 8192 * b[8];
        }

        private static void Flags(byte[] b)
        {
            Flag6(b[6]);
            Flag7(b[7]);
            Flag9(b[9]);
            Flag10(b[10]);
        }

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
            VRAMMirroring(b);
            BatteryBacked(b);
            Trainer(b);
            LMapper(b);
        }

        /// <summary>
        /// (7-4): Lower nybble of mapper number
        /// </summary>
        /// <param name="b"></param>
        private static void LMapper(byte b)
        {
            Lmapper = (b & 0xE0) >> 5;
        }

        /// <summary>
        /// (2): 1: 512-byte trainer at $7000-$71FF (stored before PRG data)
        /// </summary>
        /// <param name="b"></param>
        private static void Trainer(byte b)
        {

            if ((b & 0x4) > 0)
                trainer = true;
        }

        /// <summary>
        /// (1): 1: Cartridge contains battery-backed PRG RAM ($6000-7FFF) or other persistent memory
        /// </summary>
        /// <param name="b"></param>
        private static void BatteryBacked(byte b)
        {

            if ((b & 0x2) > 0)
                battery_backed = true;
        }

        /// <summary>
        ///76543210:
        ///(3+0): 0xx0: vertical arrangement/horizontal mirroring (CIRAM A10 = PPU A11)
        ///(3+0): 0xx1: horizontal arrangement/vertical mirroring (CIRAM A10 = PPU A10)
        ///(3+0): 1xxx: four-screen VRAM
        /// </summary>
        /// <param name="b"></param>
        private static void VRAMMirroring(byte b)
        {
            switch (b & 0x9)
            {
                case 0: arrangement = Mirror.vertical; break;
                case 1: arrangement = Mirror.horisontal; break;
                default: arrangement = Mirror.four_screen; break;
            }
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
            VS_Unisystem(b);
            PlayChoice_10(b);
            NES2Format(b);
            HMapper(b);
        }

        /// <summary>
        ///Upper nybble of mapper number
        /// </summary>
        /// <param name="b"></param>
        private static void HMapper(byte b)
        {
            Hmapper = (b & 0xE0) >> 5;
        }

        /// <summary>
        ///If equal to 2, flags 8-15 are in NES 2.0 format
        /// </summary>
        /// <param name="b"></param>
        private static void NES2Format(byte b)
        {
            if ((b & 0xC) == 0x4)
                NES2 = true;
        }

        /// <summary>
        ///PlayChoice-10 (8KB of Hint Screen data stored after CHR data)
        /// </summary>
        /// <param name="b"></param>
        private static void PlayChoice_10(byte b)
        {
            if ((b & 0x2) > 0)
                PlayChoice = true;
        }

        /// <summary>
        ///VS Unisystem
        /// </summary>
        /// <param name="b"></param>
        private static void VS_Unisystem(byte b)
        {
            if ((b & 0x1) > 0)
                VSUnisystem = true;
        }

        /// <summary>
        /// Though in the official specification, very few emulators honor this bit as virtually no 
        /// ROM images in circulation make use of it. 
        /// </summary>
        /// <param name="b"></param>
        private static void Flag9(byte b)
        {
            if ((b & 0x1) > 0)
                TVsystem = TV.PAL;//TV system
            switch (b & 0x1)
            {
                case 0: TVsystem = TV.NTS; break;
                case 1: TVsystem = TV.PAL; break;
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

            InitTVsystem(b);

            Present(b);

            BoardConflicts(b);
        }

        /// <summary>
        /// 0: Board has no bus conflicts; 1: Board has bus conflicts 
        /// </summary>
        /// <param name="b"></param>
        private static void BoardConflicts(byte b)
        {
            if ((b & 0xC) > 0)
                Boardconflicts = true;
        }

        /// <summary>
        /// PRG RAM ($6000-$7FFF) (0: present; 1: not present)
        /// </summary>
        private static void Present(byte b)
        {
            if ((b & 0xA) > 0)
                present = true;
        }

        /// <summary>
        ///TV system (0: NTSC; 2: PAL; 1/3: dual compatible) 
        /// </summary>
        /// <param name="b"></param>
        private static void InitTVsystem(byte b)
        {
            switch (b & 0x3)
            {
                case 0: TVsystem = TV.NTS; break;
                case 2: TVsystem = TV.PAL; break;
                default: TVsystem = TV.DUAL; break;
            }
        }
        #endregion
    }
}
