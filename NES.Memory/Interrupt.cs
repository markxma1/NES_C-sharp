﻿///   Copyright 2016 Xma1
///
///   This file is part of NES-C#.
///
///   NES-C# is free software: you can redistribute it and/or modify
///   it under the terms of the GNU General Public License as published by
///   the Free Software Foundation, either version 3 of the License, or
///   (at your option) any later version.
///
///   NES-C# is distributed in the hope that it will be useful,
///   but WITHOUT ANY WARRANTY; without even the implied warranty of
///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
///   See the GNU General Public License for more details.
///
///   You should have received a copy of the GNU General Public License
///   along with NES-C#. If not, see http://www.gnu.org/licenses/.
namespace NES
{
    public class Interrupt
    {

        private static bool nmi = false;
        private static bool irq = false;
        private static bool brk = false;
        public static bool NMI { get { return nmi; } set { nmi = value; } }
        public static bool IRQ { get { return irq; } set { irq = value; } }
        public static bool BRK { get { return brk; } set { brk = value; } }

        public static bool POWER = true;
        public static bool RESET = false;

        private static int SevenClock = 7;

        /// <summary>
        /// $FFFA–$FFFB 	2 bytes 	Address of Non Maskable Interrupt (NMI) handler routine
        /// $FFFC–$FFFD 	2 bytes 	Address of Power on reset handler routine
        /// $FFFE–$FFFF 	2 bytes 	Address of Break (BRK instruction) handler routine
        /// </summary>
        public static void Check()
        {
            isIRQ();
            isBRK();
            isNMI();
            isRESET();
        }

        private static void isIRQ()
        {
            if (IRQ && !NES_Register.P.Interrupt)
            {
                NES_Register.P.Interrupt = true;
                if (SevenClock-- < 0)
                {
                    ReplacePC(0xfffe, false, true);
                    IRQ = false;
                    SevenClock = 7;
                }
            }
        }

        private static void isBRK()
        {
            if (BRK)
            {
                NES_Register.P.Interrupt = true;
                if (SevenClock-- < 0)
                {
                    ReplacePC(0xfffe, true, true);
                    BRK = false;
                    SevenClock = 7;
                }
            }
        }

        private static void isNMI()
        {
            if (NMI)
            {
                NES_Register.P.Interrupt = true;
                if (SevenClock-- < 0)
                {
                    ReplacePC(0xfffa, false, true);
                    NMI = false;
                    SevenClock = 7;
                }
            }
        }

        private static void isRESET()
        {
            if (RESET)
            {
                NES_Register.PC = (ushort)(((AddressSetup)NES_Memory.Memory[0xfffc]).Value | (((AddressSetup)NES_Memory.Memory[0xfffd]).Value << 8));
            }
        }

        private static void ReplacePC(int Address, bool b, bool u)
        {
            Stack.ProcessorstatusToStack(b, u);
            Stack.PcToStack();
            NES_Register.PC = (ushort)(((AddressSetup)NES_Memory.Memory[Address]).Value | (((AddressSetup)NES_Memory.Memory[Address + 1]).Value << 8));
        }

        public static void Stop() { POWER = false; }
    }
}