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
