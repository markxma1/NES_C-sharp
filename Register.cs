using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
 
[StructLayout(LayoutKind.Explicit)]
    public static struct PFlags
    {
        ///Condition codes used for branch instructions.
        ///--------------------------------------------------------
        [FieldOffset(0)]
        public bool Negative;

        [FieldOffset(1)]
        public bool Overflow;

        [FieldOffset(6)]
        public bool Zero;

        [FieldOffset(7)]
        public bool Carry;
        ///---------------------------------------------------------

        [FieldOffset(4)]
        public bool Decimal;

        [FieldOffset(3)]
        public bool IRQ_disable;

        ///Index register size (native mode only) (0 = 16-bit, 1 = 8-bit)
        [FieldOffset(2)]
        public bool X;

        ///Accumulator register size (native mode only) (0 = 16-bit, 1 = 8-bit)
        [FieldOffset(7)]
        public bool M;

        ///E	not in P		6502 emulation mode
        /// (emulation mode only)
        [FieldOffset(3)]
        public bool Break;
        //-----------------------------------------------------------

        [FieldOffset(0)]
        public short P;
    }

namespace NES
{

    class nesRegister
    {
        #region General_Purpose_Registers
        //Accumulator: Handles all arithmetic and logic. The essential core of the 65816.
        static Int16 A;
        //Index: Index Registers with limited Capabilities.
        static Int16 X;
        static Int16 Y;
        #endregion

        #region Special Purpose Registers
        //Processor Status: Processor Flags, holds the results of tests and 65816 processing states.
        static PFlags P = new PFlags();
        //Stack : Stack Pointer
        static Int16 S;
        //Direct Page: Allows the 65816 to access memory in direct addressing modes
        static Int16 DP;
        //Program Bank: Holds the memory bank address of the current CPU instruction
        static Int16 PB;
        //Program Counter: Holds the memory address of the current CPU instruction
        static Int16 PC;
        //Data Bank: Holds the memory bank address of the data the CPU is accessing
        static Int16 DB;
        #endregion
    }
}