using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NES.Tests
{
    [TestClass()]
    public class NES_PPU_AttributeTableTests
    {
        [TestMethod()]
        public void SplitAttributeTest()
        {
            int result;
            result = NES_PPU_AttributeTable.SplitAttribute(0, 0xE4);
            Assert.AreEqual(result, 0);
            result = NES_PPU_AttributeTable.SplitAttribute(1, 0xE4);
            Assert.AreEqual(result, 1);
            result = NES_PPU_AttributeTable.SplitAttribute(2, 0xE4);
            Assert.AreEqual(result, 2);
            result = NES_PPU_AttributeTable.SplitAttribute(3, 0xE4);
            Assert.AreEqual(result, 3);
        }
    }
}