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

        public static int Carry()
        {
            return ((NES_Register.P.Carry) ? (1) : (0));
        }

        public static int NotCarry()
        {
            return ((NES_Register.P.Carry) ? (0) : (1));
        }

        public static void RegisterIsBigger()
        {
            NES_Register.P.Negative = false;
            NES_Register.P.Zero = false;
            NES_Register.P.Carry = true;
        }

        public static void Equal()
        {
            NES_Register.P.Negative = false;
            NES_Register.P.Zero = true;
            NES_Register.P.Carry = true;
        }

        public static void MemoryIsBigger()
        {
            NES_Register.P.Negative = true;
            NES_Register.P.Zero = false;
            NES_Register.P.Carry = false;
        }
    }
}
