namespace NES
{
    class Interrupt
    {
        private static int SevenClock = 7;

        public static void Check()
        {
            // $FFFA–$FFFB 	2 bytes 	Address of Non Maskable Interrupt (NMI) handler routine
            // $FFFC–$FFFD 	2 bytes 	Address of Power on reset handler routine
            // $FFFE–$FFFF 	2 bytes 	Address of Break (BRK instruction) handler routine
            if (NES_Register.IRQ && !NES_Register.P.Interrupt)
            {
                NES_Register.P.Interrupt = true;
                if (SevenClock-- < 0)
                {
                    Stack.ProcessorstatusToStack(false, true);
                    Stack.PcToStack();
                    NES_Register.PC = (ushort)(((Adress)NES_Memory.Stack[0xfffe]).Value | (((Adress)NES_Memory.Stack[0xffff]).Value << 8));
                    NES_Register.IRQ = false;
                    SevenClock = 7;
                }
            }
            if (NES_Register.BRK)
            {
                NES_Register.P.Interrupt = true;
                if (SevenClock-- < 0)
                {
                    Stack.ProcessorstatusToStack(true, true);
                    Stack.PcToStack();
                    NES_Register.PC = (ushort)(((Adress)NES_Memory.Memory[0xfffe]).Value | (((Adress)NES_Memory.Memory[0xffff]).Value << 8));
                    NES_Register.BRK = false;
                    SevenClock = 7;
                }
            }
            if (NES_Register.NMI)
            {
                NES_Register.P.Interrupt = true;
                if (SevenClock-- < 0)
                {
                    Stack.ProcessorstatusToStack(false, true);
                    Stack.PcToStack();
                    NES_Register.PC = (ushort)(((Adress)NES_Memory.Memory[0xfffa]).Value | (((Adress)NES_Memory.Memory[0xfffb]).Value << 8));
                    NES_Register.NMI = false;
                    SevenClock = 7;
                }
            }
            if (NES_Register.RESET)
            {
                NES_Register.PC = (ushort)(((Adress)NES_Memory.Memory[0xfffc]).Value | (((Adress)NES_Memory.Memory[0xfffd]).Value << 8));
            }
        }
    }
}