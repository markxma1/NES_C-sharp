using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES;

namespace NES_UnitTest
{
    [TestClass]
    public class NES_PPU_MEMORY_TEST
    {
        [TestMethod]
        public void DrowOneNameTable0()
        {
            int n = 0;
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    Assert.AreEqual(n++, NES_PPU.K(0, 0, i, j));
                }
            }
        }

        [TestMethod]
        public void DrowOneNameTable1()
        {
            int n = 0;
            for (int i = 0; i < 30; i++)
            {
                for (int j = 32; j < 32 + 32; j++)
                {
                    Assert.AreEqual(n++, NES_PPU.K(0, 32, i, j));
                }
            }
        }

        [TestMethod]
        public void DrowOneNameTable2()
        {
            int n = 0;
            for (int i = 30; i < 30 + 30; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    Assert.AreEqual(n++, NES_PPU.K(30, 0, i, j));
                }
            }
        }

        [TestMethod]
        public void DrowOneNameTable3()
        {
            int n = 0;
            for (int i = 30; i < 30 + 30; i++)
            {
                for (int j = 32; j < 32 + 32; j++)
                {
                    Assert.AreEqual(n++, NES_PPU.K(30, 32, i, j));
                }
            }
        }
    }
}
