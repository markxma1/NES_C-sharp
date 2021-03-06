// <copyright file="NES_PPU_AttributeTableTest.cs" company="Home">Copyright Mark Tamaev ©  2015</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES;

namespace NES.Tests
{
    /// <summary>This class contains parameterized unit tests for NES_PPU_AttributeTable</summary>
    [TestClass]
    [PexClass(typeof(NES_PPU_AttributeTable))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class NES_PPU_AttributeTableTest
    {

        /// <summary>Test stub for SplitAttribute(Int32, Byte)</summary>
        [PexMethod]
        internal int SplitAttributeTest(int shift1, byte value)
        {
            int result = NES_PPU_AttributeTable.SplitAttribute(shift1, value);
            result = NES_PPU_AttributeTable.SplitAttribute(0, 0xE4);
            PexAssert.AreEqual(result, 0);
            result = NES_PPU_AttributeTable.SplitAttribute(1, 0xE4);
            PexAssert.AreEqual(result, 1);
            result = NES_PPU_AttributeTable.SplitAttribute(2, 0xE4);
            PexAssert.AreEqual(result, 2);
            result = NES_PPU_AttributeTable.SplitAttribute(3, 0xE4);
            PexAssert.AreEqual(result, 3);
            return result;
        }
    }
}
