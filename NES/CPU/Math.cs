namespace NES
{
    class Math
    {
        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        public static byte ADC(int A, int B)
        {
            var temp = A + B + ((NES_Register.P.Carry) ? (1) : (0));
            Status.NVZC(temp);
            return (byte)temp;
        }


        public static void CMP(byte A, byte B)
        {
            if (A < B)
            {
                NES_Register.P.Negative = true;
                NES_Register.P.Zero = false;
                NES_Register.P.Carry = false;
            }
            if (A == B)
            {
                NES_Register.P.Negative = false;
                NES_Register.P.Zero = true;
                NES_Register.P.Carry = true;
            }
            if (A > B)
            {
                NES_Register.P.Negative = false;
                NES_Register.P.Zero = false;
                NES_Register.P.Carry = true;
            }
        }
    }
}
