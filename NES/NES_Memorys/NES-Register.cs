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

    public struct PPUCTRLFlags
    {
        //VPHB SINN 	

        /// <summary>
        /// nametable select (NN) 
        /// Base nametable address:
        /// (0 = $2000; 1 = $2400; 2 = $2800; 3 = $2C00)
        /// </summary>
        public byte N { get { return (byte)(adress.value & 0x3); } set { adress.value = (byte)(adress.value & ~0x3); adress.value = (byte)(adress.value | (value & 0x3)); } }

        /// <summary>
        /// increment mode (I)
        /// VRAM address increment per CPU read/write of PPUDATA
        /// (0: add 1, going across; 1: add 32, going down)
        /// </summary>
        public bool I { get { return (adress.value & 0x4) > 0; } set { adress.value = (byte)(adress.value & ~0x4); if (value) adress.value = (byte)(adress.value | 0x4); } }

        /// <summary>
        /// sprite tile select (S)
        /// Sprite pattern table address for 8x8 sprites
        /// (0: $0000; 1: $1000; ignored in 8x16 mode)
        /// </summary>
        public bool S { get { return (adress.value & 0x8) > 0; } set { adress.value = (byte)(adress.value & ~0x8); if (value) adress.value = (byte)(adress.value | 0x8); } }

        /// <summary>
        /// background tile select (B)
        /// Background pattern table address 
        /// (0: $0000; 1: $1000)
        /// </summary>
        public bool B { get { return (adress.value & 0x10) > 0; } set { adress.value = (byte)(adress.value & ~0x10); if (value) adress.value = (byte)(adress.value | 0x10); } }

        /// <summary>
        /// sprite height (H)
        /// Sprite size 
        /// (0: 8x8; 1: 8x16)
        /// </summary>
        public bool H { get { return (adress.value & 0x20) > 0; } set { adress.value = (byte)(adress.value & ~0x20); if (value) adress.value = (byte)(adress.value | 0x20); } }

        /// <summary>
        /// PPU master/slave (P)
        /// PPU master/slave select
        /// (0: read backdrop from EXT pins; 1: output color on EXT pins)
        /// </summary>
        public bool P { get { return (adress.value & 0x40) > 0; } set { adress.value = (byte)(adress.value & ~0x40); if (value) adress.value = (byte)(adress.value | 0x40); } }

        /// <summary>
        /// NMI enable (V)
        /// Generate an NMI at the start of the vertical blanking interval
        /// (0: off; 1: on)
        /// </summary>
        public bool V { get { return (adress.value & 0x80) > 0; } set { adress.value = (byte)(adress.value & ~0x80); if (value) adress.value = (byte)(adress.value | 0x80); NES_Register.NMI = value; } }

        public Adress adress;
    }

    public struct PPUMASKFlags
    {
        //BGRs bMmG 	

        /// <summary>
        /// greyscale (G)
        /// Grayscale 
        /// (0: normal color, 1: produce a greyscale display)
        /// </summary>
        public bool N0 { get { return (adress.value & 0x1) > 0; } set { adress.value = (byte)(adress.value & ~0x1); if (value) adress.value = (byte)(adress.value | 0x1); } }

        /// <summary>
        /// background left column enable (m)
        /// 1: Show background in leftmost 8 pixels of screen, 0: Hide
        /// </summary>
        public bool m { get { return (adress.value & 0x2) > 0; } set { adress.value = (byte)(adress.value & ~0x2); if (value) adress.value = (byte)(adress.value | 0x2); } }

        /// <summary>
        /// sprite left column enable (M)
        /// 1: Show sprites in leftmost 8 pixels of screen, 0: Hide
        /// </summary>
        public bool M { get { return (adress.value & 0x4) > 0; } set { adress.value = (byte)(adress.value & ~0x4); if (value) adress.value = (byte)(adress.value | 0x4); } }

        /// <summary>
        /// background enable (b)
        /// 1: Show background
        /// </summary>
        public bool b { get { return (adress.value & 0x8) > 0; } set { adress.value = (byte)(adress.value & ~0x8); if (value) adress.value = (byte)(adress.value | 0x8); } }

        /// <summary>
        ///  sprite enable (s)
        ///  1: Show sprites
        /// </summary>
        public bool s { get { return (adress.value & 0x10) > 0; } set { adress.value = (byte)(adress.value & ~0x10); if (value) adress.value = (byte)(adress.value | 0x10); } }

        /// <summary>
        /// color emphasis (BGR)
        /// Emphasize red*
        /// Emphasize green*
        /// Emphasize blue*
        /// </summary>
        public byte BGR { get { return (byte)(adress.value & 0xE0); } set { adress.value = (byte)(adress.value & ~0xE0); adress.value = (byte)(adress.value | (value & 0xE0)); } }

        public Adress adress;
    }

    public struct PPUSTATUSFlags
    {
        //VSO- ---- 
        //read resets write pair for $2005/2006 

        //Least significant bits previously written into a PPU register
        //(due to register not being updated for this address)

        /// <summary>
        /// sprite overflow (O)
        /// Sprite overflow. The intent was for this flag to be set
        /// whenever more than eight sprites appear on a scanline, but a
        /// hardware bug causes the actual behavior to be more complicated
        /// and generate false positives as well as false negatives; see
        /// PPU sprite evaluation. This flag is set during sprite
        /// evaluation and cleared at dot 1 (the second dot) of the
        /// pre-render line.
        /// </summary>
        public bool O { get { return (adress.value & 0x20) > 0; } set { adress.value = (byte)(adress.value & ~0x20); if (value) adress.value = (byte)(adress.value | 0x20); } }

        /// <summary>
        /// sprite 0 hit (S)
        /// Sprite 0 Hit.  Set when a nonzero pixel of sprite 0 overlaps
        /// a nonzero background pixel; cleared at dot 1 of the pre-render
        /// line.  Used for raster timing.
        /// </summary>
        public bool S { get { return (adress.value & 0x40) > 0; } set { adress.value = (byte)(adress.value & ~0x40); if (value) adress.value = (byte)(adress.value | 0x40); } }

        /// <summary>
        /// vblank (V)
        /// Vertical blank has started (0: not in vblank; 1: in vblank).
        /// Set at dot 1 of line 241 (the line *after* the post-render
        /// line); cleared after reading $2002 and at dot 1 of the
        /// pre-render line.
        /// </summary>
        public bool V { get { return (adress.value & 0x80) > 0; } set { adress.value = (byte)(adress.value & ~0x80); if (value) adress.value = (byte)(adress.value | 0x80); } }

        public Adress adress;
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

        #region PPU Registers
        //Stored to Mamory $2000–$2007 and $4014

        /// <summary>
        /// Controller ($2000) > write
        ///    Common name: PPUCTRL
        ///    Description: PPU control register
        ///    Access: write 
        /// Various flags controlling PPU operation 
        /// </summary>
        public static PPUCTRLFlags PPUCTRL = new PPUCTRLFlags();

        /// <summary>
        /// Mask ($2001) > write
        ///  Common name: PPUMASK
        ///  Description: PPU mask register
        ///  Access: write
        /// This register controls the rendering of sprites and backgrounds, as well as colour effects. 
        /// </summary>
        public static PPUMASKFlags PPUMASK = new PPUMASKFlags();

        /// <summary>
        /// Status ($2002) { read
        /// Common name: PPUSTATUS
        /// Description: PPU status register
        /// Access: read
        /// read resets write pair for $2005/2006 
        /// This register reflects the state of various functions inside the PPU. It is often used for determining timing. To determine when the PPU has reached a given pixel of the screen, put an opaque pixel of sprite 0 there.
        /// </summary>
        public static PPUSTATUSFlags PPUSTATUS = new PPUSTATUSFlags();

        /// <summary>
        /// aaaa aaaa 	
        /// OAM read/write address 
        /// </summary>
        public static Adress OAMADDR = (Adress)NES_Memory.Memory[0x2003];

        /// <summary>
        ///  	dddd dddd 	
        ///  	OAM data read/write 
        /// </summary>
        public static Adress OAMDATA = (Adress)NES_Memory.Memory[0x2004];

        /// <summary>
        /// xxxx xxxx 	
        /// fine scroll position (two writes: X, Y) 
        /// </summary>
        public static Adress PPUSCROLL = (Adress)NES_Memory.Memory[0x2005];

        /// <summary>
        ///  	aaaa aaaa 	
        ///  	PPU read/write address (two writes: MSB, LSB) 
        /// </summary>
        public static Adress PPUADDR = (Adress)NES_Memory.Memory[0x2006];

        /// <summary>
        /// dddd dddd 	
        /// PPU data read/write 
        /// </summary>
        public static Adress PPUDATA = (Adress)NES_Memory.Memory[0x2007];

        /// <summary>
        /// aaaa aaaa 	
        /// OAM DMA high address 
        /// </summary>
        public static Adress OAMDMA = (Adress)NES_Memory.Memory[0x4014];

        /// <summary>
        /// Adress Of  PPU Program Counter
        /// </summary>
        public static ushort PPUPCADDR = 0x0000;
        #endregion

        #region Interrupts

        private static bool nmi = false;
        private static bool irq = false;
        private static bool brk = false;
        public static bool NMI { get { return nmi; } set { nmi = value; } }
        public static bool IRQ { get { return irq; } set { irq = value; } }
        public static bool BRK { get { return brk; } set { brk = value; } }

        public static bool POWER = true;
        public static bool RESET = false;
        #endregion

        public NES_Register()
        {
            PPUCTRL.adress = (Adress)NES_Memory.Memory[0x2000];
            PPUCTRL.adress.afterSet = delegate (byte value)
            {
                PPUCTRL.V = PPUCTRL.V;
            };
            PPUMASK.adress = (Adress)NES_Memory.Memory[0x2001];
            PPUSTATUS.adress = (Adress)NES_Memory.Memory[0x2002];
            PPUSTATUS.adress.afterGet = delegate () { PPUSTATUS.adress.value = (byte)(PPUSTATUS.adress.value & 0x7F); PPUSCROLL.Value = 0; PPUADDR.Value = 0; };
            PPUADDR.afterSet = delegate (byte value)
            {
                PPUPCADDR = (ushort)((PPUPCADDR << 8) | (PPUADDR.Value));
            };
            PPUDATA.afterSet = delegate (byte value)
            {
                ((Adress)NES_PPU_Memory.Memory[PPUPCADDR]).Value = value; PPUPCADDR += (ushort)((PPUCTRL.I) ? (32) : (1)); if (PPUPCADDR == 0x4000) PPUPCADDR = 0;
            };
            PPUDATA.beforGet = delegate ()
            {
                PPUDATA.value = ((Adress)NES_PPU_Memory.Memory[PPUPCADDR]).value;
            };

            OAMDMA.afterSet= delegate (byte value)
            {
                NES_PPU_OAM.OAMDMA(value);
            };
        }

        public static void RessetPointer()
        {
            PC = (ushort)(((Adress)NES_Memory.POR[0]).Value | (((Adress)NES_Memory.POR[1]).Value << 8));
        }
    }
}
