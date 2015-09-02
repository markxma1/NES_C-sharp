using NES;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Linq;

namespace NES.Tests
{
    [TestClass()]
    public class NES_PPU_MemoryTests
    {
        public NES_PPU_MemoryTests()
        {
            new NES_PPU_Memory();
        }

        [TestMethod()]
        public void CreateUpdateMemoryTest()
        {

            Assert.AreEqual(NES_PPU_Memory.Memory.Count, 0x4000);
            for (int i = 0x3000; i < 0x3F00; i++)
            {
                Assert.AreSame(NES_PPU_Memory.Memory[i],
                                                NES_PPU_Memory.Memory[i - 0x1000]);
            }
            for (int i = 0x3F20; i < 0x3F20 + 0xE0; i++)
            {
                Assert.AreSame(NES_PPU_Memory.Memory[i],
                                                NES_PPU_Memory.Memory[i - 0x20]);
            }
        }

        [TestMethod()]
        public void InitPatternTableTest()
        {
            for (int i = 0; i < 0x1000; i++)
            {
                Assert.AreSame(NES_PPU_Memory.PatternTable[i],
                                                NES_PPU_Memory.PatternTableN[0][i]);
            }

            for (int i = 0; i < 0x1000; i++)
            {
                Assert.AreSame(NES_PPU_Memory.PatternTable[0x1000 + i],
                                                NES_PPU_Memory.PatternTableN[1][i]);
            }
            DoubleTest(NES_PPU_Memory.PatternTable);
        }

        private static void DoubleTest(ArrayList Table)
        {
            var temp2 = Table.Cast<Address>().GroupBy(s => s.ToString()).Where(gr => gr.Count() > 1).ToArray();
            var temp = temp2.Count();
            Assert.AreEqual(temp, 0);
        }

        [TestMethod()]
        public void InitNameTableTest()
        {
            for (int i = 0; i < 0x3C0; i++)
            {
                Assert.AreSame(NES_PPU_Memory.NameTable[i],
                                                NES_PPU_Memory.NameTableN[0][i]);
                Assert.AreSame(NES_PPU_Memory.NameTable[i + 0x3C0],
                                NES_PPU_Memory.NameTableN[1][i]);
                Assert.AreSame(NES_PPU_Memory.NameTable[i + 0x3C0 * 2],
                                NES_PPU_Memory.NameTableN[2][i]);
                Assert.AreSame(NES_PPU_Memory.NameTable[i + 0x3C0 * 3],
                                NES_PPU_Memory.NameTableN[3][i]);
            }

            DoubleTest(NES_PPU_Memory.NameTableN[0]);
            DoubleTest(NES_PPU_Memory.NameTableN[1]);
            DoubleTest(NES_PPU_Memory.NameTableN[2]);
            DoubleTest(NES_PPU_Memory.NameTableN[3]);
        }

        [TestMethod()]
        public void InitAttributeTableTest()
        {
            for (int i = 0; i < 0x40; i++)
            {
                Assert.AreSame(NES_PPU_Memory.AttributeTable[i],
                                                NES_PPU_Memory.AttributeTableN[0][i]);
                Assert.AreSame(NES_PPU_Memory.AttributeTable[i + 0x40],
                                NES_PPU_Memory.AttributeTableN[1][i]);
                Assert.AreSame(NES_PPU_Memory.AttributeTable[i + 0x40 * 2],
                                NES_PPU_Memory.AttributeTableN[2][i]);
                Assert.AreSame(NES_PPU_Memory.AttributeTable[i + 0x40 * 3],
                                NES_PPU_Memory.AttributeTableN[3][i]);
            }

            DoubleTest(NES_PPU_Memory.AttributeTableN[0]);
            DoubleTest(NES_PPU_Memory.AttributeTableN[1]);
            DoubleTest(NES_PPU_Memory.AttributeTableN[2]);
            DoubleTest(NES_PPU_Memory.AttributeTableN[3]);
        }

        [TestMethod()]
        public void InitPaletteRAMIndexesTest()
        {
            for (int i = 0; i < 0x10; i++)
            {
                Assert.AreNotSame(NES_PPU_Memory.BGPalette[i],
                                                NES_PPU_Memory.SpritePalette[i]);
            }
            DoubleTest(NES_PPU_Memory.BGPalette);
            DoubleTest(NES_PPU_Memory.SpritePalette);
        }
    }
}