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


        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        public static void CMP(byte value)
        {
            if (NES_Register.A < value)
            {
                NES_Register.P.Negative = true;
                NES_Register.P.Zero = false;
                NES_Register.P.Carry = false;
            }
            if (NES_Register.A == value)
            {
                NES_Register.P.Negative = false;
                NES_Register.P.Zero = true;
                NES_Register.P.Carry = true;
            }
            if (NES_Register.A > value)
            {
                NES_Register.P.Negative = false;
                NES_Register.P.Zero = false;
                NES_Register.P.Carry = true;
            }
        }
    }
}
