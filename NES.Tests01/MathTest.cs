// <copyright file="MathTest.cs">Copyright ©  2015</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES;

namespace NES.Tests
{
    /// <summary>This class contains parameterized unit tests for Math</summary>
    [PexClass(typeof(Math))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class MathTest
    {
        /// <summary>Test stub for ADC(Int32, Int32)</summary>
        [PexMethod]
        internal byte ADCTest(int A, int B)
        {
            byte result = Math.ADC(A, B);
            return result;
            // TODO: add assertions to method MathTest.ADCTest(Int32, Int32)
        }

        /// <summary>Test stub for SBC(Int32, Int32)</summary>
        [PexMethod]
        internal byte SBCTest(int A, int B)
        {
            byte result = Math.SBC(A, B);
            return result;
            // TODO: add assertions to method MathTest.SBCTest(Int32, Int32)
        }

        /// <summary>Test stub for ASL(UInt16)</summary>
        [PexMethod]
        internal void ASLTest(ushort a)
        {
            Math.ASL(a);
            // TODO: add assertions to method MathTest.ASLTest(UInt16)
        }

        /// <summary>Test stub for ASL()</summary>
        [PexMethod]
        internal void ASLTest()
        {
            Math.ASL();
            // TODO: add assertions to method MathTest.ASLTest()
        }
    }
}
