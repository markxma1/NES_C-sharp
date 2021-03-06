﻿using System.Runtime.CompilerServices;
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
///https://en.wikibooks.org/wiki/NES_Programming/Introduction 
///http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior
///http://wiki.nesdev.com/w/index.php/PPU_registers
[assembly: InternalsVisibleTo("CPUTest")]
namespace NES
{

    public struct PFlags
    {
        public bool Carry { get { return (P & 0x1) > 0; } set { P = (byte)(P & ~0x1); if (value) P = (byte)(P | 0x1); } }

        public bool Zero { get { return (P & 0x2) > 0; } set { P = (byte)(P & ~0x2); if (value) P = (byte)(P | 0x2); } }

        public bool Interrupt { get { return (P & 0x4) > 0; } set { P = (byte)(P & ~0x4); if (value) P = (byte)(P | 0x4); } }

        public bool Decimal { get { return (P & 0x8) > 0; } set { P = (byte)(P & ~0x8); if (value) P = (byte)(P | 0x8); } }

        public bool B { get { return (P & 0x10) > 0; } set { P = (byte)(P & ~0x10); if (value) P = (byte)(P | 0x10); } }

        public bool U { get { return (P & 0x20) > 0; } set { P = (byte)(P & ~0x20); if (value) P = (byte)(P | 0x20); } }

        public bool Overflow { get { return (P & 0x40) > 0; } set { P = (byte)(P & ~0x40); if (value) P = (byte)(P | 0x40); } }

        public bool Negative { get { return (P & 0x80) > 0; } set { P = (byte)(P & ~0x80); if (value) P = (byte)(P | 0x80); } }

        public byte P;

        public override string ToString()
        {
            return P + ": "
                + ((Negative) ? ("N") : (""))
                + ((Overflow) ? ("V") : (""))
                + ((U) ? ("U") : (""))
                + ((B) ? ("B") : (""))
                + ((Decimal) ? ("D") : (""))
                + ((Interrupt) ? ("I") : (""))
                + ((Zero) ? ("Z") : (""))
                + ((Carry) ? ("C") : (""));
        }
    }

    public class NES_Register
    {
        #region General_Purpose_Registers

        /// <summary>
        /// Accumulator: Handles all arithmetic and logic. The essential core of the 65816.
        /// </summary>
        public static byte A;

        /// <summary>
        ///Index: Index Registers with limited Capabilities.
        /// </summary>
        public static byte X;
        public static byte Y;
        #endregion

        #region Special Purpose Registers

        /// <summary>
        ///Processor Status: Processor Flags, holds the results of tests and 65816 processing states.
        /// </summary>
        public static PFlags P = new PFlags();

        /// <summary>
        ///Stack : Stack Pointer
        /// </summary>
        public static byte S = 0xFD;

        /// <summary>
        ///Program Counter: Holds the memory address of the current CPU instruction
        /// </summary>
        public static ushort PC;

        #endregion

        public static void RessetPointer()
        {
            PC = (ushort)(((Address)NES_Memory.POR[0]).Value | (((Address)NES_Memory.POR[1]).Value << 8));
        }
    }
}
