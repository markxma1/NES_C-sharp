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
    }
}
