using System.Runtime.CompilerServices;
///   Copyright 2016 Xma1
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
[assembly: InternalsVisibleTo("CPUTest")]
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
