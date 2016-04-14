///   Copyright 2016 Xma1
///
///   This file is part of NES-C#.
///
///   Foobar is free software: you can redistribute it and/or modify
///   it under the terms of the GNU General Public License as published by
///   the Free Software Foundation, either version 3 of the License, or
///   (at your option) any later version.
///
///   Foobar is distributed in the hope that it will be useful,
///   but WITHOUT ANY WARRANTY; without even the implied warranty of
///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
///   See the GNU General Public License for more details.
///
///   You should have received a copy of the GNU General Public License
///   along with Foobar. If not, see http://www.gnu.org/licenses/.
namespace NES
{
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
        public bool V { get { return (adress.value & 0x80) > 0; } set { adress.value = (byte)(adress.value & ~0x80); if (value) adress.value = (byte)(adress.value | 0x80); Interrupt.NMI = value; } }

        public AddressSetup adress;
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

        public AddressSetup adress;
    }

    /// <summary>
    /// VSO- ---- 
    /// read resets write pair for $2005/2006 
    ///
    /// Least significant bits previously written into a PPU register
    /// (due to register not being updated for this address)
    /// </summary>
    public struct PPUSTATUSFlags
    {

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
        public bool V { get { return (adress.value & 0x80) > 0; }
            set { adress.value = (byte)(adress.value & ~0x80); if (value) adress.value = (byte)(adress.value | 0x80); } }

        public AddressSetup adress;
    }

    /// <summary>
    ///Stored to Mamory $2000–$2007 and $4014
    /// </summary>
    public class NES_PPU_Register
    {

        /// <summary>
        /// Controller ($2000) > write
        ///    Common name: PPUCTRL
        ///    Description: PPU control register
        ///    Access: write 
        /// Various flags controlling PPU operation 
        /// </summary>
        public static PPUCTRLFlags PPUCTRL;

        /// <summary>
        /// Mask ($2001) > write
        ///  Common name: PPUMASK
        ///  Description: PPU mask register
        ///  Access: write
        /// This register controls the rendering of sprites and backgrounds, as well as colour effects. 
        /// </summary>
        public static PPUMASKFlags PPUMASK;

        /// <summary>
        /// Status ($2002) { read
        /// Common name: PPUSTATUS
        /// Description: PPU status register
        /// Access: read
        /// read resets write pair for $2005/2006 
        /// This register reflects the state of various functions inside the PPU. It is often used for determining timing. To determine when the PPU has reached a given pixel of the screen, put an opaque pixel of sprite 0 there.
        /// </summary>
        public static PPUSTATUSFlags PPUSTATUS;

        /// <summary>
        /// aaaa aaaa 	
        /// OAM read/write address 
        /// </summary>
        public static AddressSetup OAMADDR = (AddressSetup)NES_Memory.Memory[0x2003];

        /// <summary>
        ///  	dddd dddd 	
        ///  	OAM data read/write 
        /// </summary>
        public static AddressSetup OAMDATA = (AddressSetup)NES_Memory.Memory[0x2004];

        /// <summary>
        /// xxxx xxxx 	
        /// fine scroll position (two writes: X, Y) 
        /// </summary>
        public static AddressSetup PPUSCROLL = (AddressSetup)NES_Memory.Memory[0x2005];

        /// <summary>
        ///  	aaaa aaaa 	
        ///  	PPU read/write address (two writes: MSB, LSB) 
        /// </summary>
        public static AddressSetup PPUADDR = (AddressSetup)NES_Memory.Memory[0x2006];

        /// <summary>
        /// dddd dddd 	
        /// PPU data read/write 
        /// </summary>
        public static AddressSetup PPUDATA = (AddressSetup)NES_Memory.Memory[0x2007];

        /// <summary>
        /// aaaa aaaa 	
        /// OAM DMA high address 
        /// </summary>
        public static AddressSetup OAMDMA = (AddressSetup)NES_Memory.Memory[0x4014];

        /// <summary>
        /// Adress Of  PPU Program Counter
        /// </summary>
        public static ushort PPUPCADDR = 0x0000;

        public NES_PPU_Register()
        {
            INITPPUCTRL();
            INITPPUMASK();
            INITPPUSTATUS();
            INITPPUADDR();
            INITPPUDATA();
            INITOAMDMA();
            INITPPUSCROLL();
        }

        private static void INITPPUSCROLL()
        {
            PPUSCROLL.AfterSet = delegate (byte value)
            {
                NES_PPU.Scroll = value;
            };
        }

        private static void INITPPUMASK()
        {
            PPUMASK = new PPUMASKFlags();
            PPUMASK.adress = (AddressSetup)NES_Memory.Memory[0x2001];
        }

        private static void INITPPUADDR()
        {
            PPUADDR.AfterSet = delegate (byte value)
            {
                PPUPCADDR = (ushort)((PPUPCADDR << 8) | (PPUADDR.Value));
            };
        }

        private static void INITOAMDMA()
        {
            OAMDMA.AfterSet = delegate (byte value)
            {
                NES_PPU_OAM.OAMDMA(value);
            };
        }

        private static void INITPPUDATA()
        {
            PPUDATA.AfterSet = delegate (byte value)
            {
                ((AddressSetup)NES_PPU_Memory.Memory[PPUPCADDR]).Value = value;
                PPUPCADDR += (ushort)((PPUCTRL.I) ? (32) : (1));
                if (PPUPCADDR == 0x4000) PPUPCADDR = 0;
            };
            PPUDATA.BeforGet = delegate ()
            {
                PPUDATA.value = ((AddressSetup)NES_PPU_Memory.Memory[PPUPCADDR]).value;
            };
        }

        private static void INITPPUCTRL()
        {
            PPUCTRL = new PPUCTRLFlags();
            PPUCTRL.adress = (AddressSetup)NES_Memory.Memory[0x2000];
            PPUCTRL.adress.AfterSet = delegate (byte value)
            {
                PPUCTRL.V = PPUCTRL.V;
            };
        }

        private static void INITPPUSTATUS()
        {
            PPUSTATUS = new PPUSTATUSFlags();
            PPUSTATUS.adress = (AddressSetup)NES_Memory.Memory[0x2002];
            PPUSTATUS.adress.AfterGet = delegate ()
            {
                PPUSTATUS.adress.value = (byte)(PPUSTATUS.adress.value & 0x7F);
                PPUSCROLLRESSET();
                PPUADDR.Value = 0;
            };
        }

        public static void InitialAtPower()
        {
            PPUCTRL.adress.Value = 0;
            PPUMASK.adress.Value = 0;
            PPUSTATUS.adress.Value = 0xA0;
            OAMADDR.Value = 0;
            PPUSCROLLRESSET();
            PPUADDR.Value = 0;
            PPUDATA.Value = 0;
        }

        public static void InitialOnReset()
        {
            PPUCTRL.adress.Value = 0;
            PPUMASK.adress.Value = 0;
            PPUSTATUS.adress.Value = (byte)(PPUSTATUS.adress.value & 0x80);
            PPUSCROLLRESSET();
            PPUDATA.Value = 0;
        }

        private static void PPUSCROLLRESSET()
        {
            PPUSCROLL.Value = 0;
            PPUSCROLL.Value = 0;
        }
    }
}
