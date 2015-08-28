namespace NES
{
    class Status
    {
        public static void NZ(int number)
        {
            NES_Register.P.Negative = (number & 0x80) != 0;
            NES_Register.P.Zero = number == 0;
        }

        public static void OC(int number)
        {
            NES_Register.P.Overflow = ((number & ~(0xFF)) > 0);
            NES_Register.P.Carry = ((number & (0x0100)) > 0);
        }

        public static void NVZC(int number)
        {
            OC(number);
            NZ(number);
        }
    }
}
