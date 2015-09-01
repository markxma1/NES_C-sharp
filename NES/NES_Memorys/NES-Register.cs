///https://en.wikibooks.org/wiki/NES_Programming/Introduction 
///http://wiki.nesdev.com/w/index.php/CPU_status_flag_behavior
///http://wiki.nesdev.com/w/index.php/PPU_registers
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

    class NES_Register
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
            PC = (ushort)(((Adress)NES_Memory.POR[0]).Value | (((Adress)NES_Memory.POR[1]).Value << 8));
        }
    }
}
