using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES;

namespace CPUTest
{
    [TestClass]
    public class Math
    {
        [TestMethod]
        public void ADC1()
        {

            for (byte A = 0; A < 255; A++)
            {
                for (byte B = 0; B < 255; B++)
                {
                    int expect = A + B;
                    if (expect > 0 && expect < 255)
                    {
                        Assert.AreEqual(expect + Status.Carry(), NES.Math.ADC(A, B));
                        Assert.AreEqual(false, NES_Register.P.Carry);
                    }
                    if (expect > 255)
                    {
                        Assert.AreEqual((byte)expect + Status.Carry(), NES.Math.ADC(A, B));
                        Assert.AreEqual(((expect & (0x0100)) > 0), NES_Register.P.Carry);
                    }

                }
            }

            for (byte A = 0; A < 255; A++)
            {
                for (byte B = 0; B < 255; B++)
                {
                    Status.OC(0xff);
                    NES.Math.ADC(A, B);

                }
            }

        }
    }
}
