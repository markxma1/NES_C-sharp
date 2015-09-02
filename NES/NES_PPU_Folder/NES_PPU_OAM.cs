using System.Collections;

namespace NES
{
    class NES_PPU_OAM
    {

        // $00-$0C (0 of 4)	$40	Sprite Y coordinate
        // $01-$0D (1 of 4)	$40	Sprite tile #
        // $02-$0E (2 of 4)	$40	Sprite attribute
        // $03-$0F (3 of 4)	$40	Sprite X coordinate
        public struct Byte1
        {
            //7-1 Tile number of top of sprite (0 to 254; bottom half gets the next tile)
            //0 Bank ($0000 or $1000) of tiles


            /// <summary>
            /// Bank ($0000 or $1000) of tiles
            /// </summary>
            public bool Bank { get { return (bool)((adress.Value & 0x01) > 0); } set { adress.Value = (byte)(adress.Value & ~0x01); if (value)adress.Value = (byte)(adress.Value | 0x01); } }

            /// <summary>
            /// Tile number of top of sprite (0 to 254; bottom half gets the next tile)
            /// </summary>
            public byte Number { get { return (byte)(adress.Value & 0xFE); } set { adress.Value = (byte)((adress.Value & ~0xFE) | value & 0xFE); } }

            public Address adress;
        }

        public struct Byte2
        {
            /// <summary>
            /// Palette (4 to 7) of sprite
            /// </summary>
            public byte Palette { get { return (byte)(adress.Value & 0x3); } set { adress.Value = (byte)(adress.Value & ~0x3); adress.Value = (byte)(adress.Value | (value & 0x3)); } }

            /// <summary>
            /// Priority (0: in front of background; 1: behind background)
            /// </summary>
            public bool Priority { get { return (bool)((adress.Value & 0x20) > 0); } set { adress.Value = (byte)(adress.Value & ~0x20); if (value)adress.Value = (byte)(adress.Value | 0x20); } }

            /// <summary>
            /// Flip sprite horizontally
            /// </summary>
            public bool FlipH { get { return (bool)((adress.Value & 0x40) > 0); } set { adress.Value = (byte)(adress.Value & ~0x40); if (value)adress.Value = (byte)(adress.Value | 0x40); } }

            /// <summary>
            /// Flip sprite vertically
            /// </summary>
            public bool FlipV { get { return (bool)((adress.Value & 0x80) > 0); } set { adress.Value = (byte)(adress.Value & ~0x80); if (value)adress.Value = (byte)(adress.Value | 0x80); } }

            public Address adress;
        }

        static public ArrayList Memory = new ArrayList();

        /// <summary>
        /// Y position of top of sprite
        /// Sprite data is delayed by one scanline; 
        /// you must subtract 1 from the sprite's Y coordinate before writing it here. 
        /// Hide a sprite by writing any values in $EF-$FF here. 
        /// Sprites are never displayed on the first line of the picture, 
        /// and it is impossible to place a sprite partially off the top of the screen. 
        /// Thus, the pattern table memory map for 8x16 sprites looks like this:
        /// $00: $0000-$001F
        /// $01: $1000-$101F
        /// $02: $0020-$003F
        /// $03: $1020-$103F
        /// $04: $0040-$005F
        /// [...]
        /// $FE: $0FE0-$0FFF
        /// $FF: $1FE0-$1FFF
        /// </summary>
        static public ArrayList SpriteYc = new ArrayList();

        /// <summary>
        /// Tile index number
        /// For 8x8 sprites, this is the tile number of this sprite within the 
        /// pattern table selected in bit 3 of PPUCTRL ($2000).
        /// For 8x16 sprites, the PPU ignores the pattern table selection and 
        /// selects a pattern table from bit 0 of this number. 
        /// </summary>
        static public ArrayList SpriteTile = new ArrayList();

        /// <summary>
        ///  Flipping does not change the position of the sprite's bounding box, 
        ///  just the position of pixels within the sprite. If, for example, 
        ///  a sprite covers (120, 130) through (127, 137), it'll still cover the same area 
        ///  when flipped. In 8x16 mode, vertical flip flips each of the subtiles and also 
        ///  exchanges their position; the odd-numbered tile of a vertically flipped sprite
        ///  is drawn on top. This behavior differs from the behavior of the unofficial 
        ///  16x32 and 32x64 pixel sprite sizes on the Super NES, which will only 
        ///  vertically flip each square sub-region.
        ///  
        ///   The three unimplemented bits of each sprite's byte 2 do not exist in the 
        ///   PPU and always read back as 0 on PPU revisions that allow reading 
        ///   PPU OAM through OAMDATA ($2004). This can be emulated by 
        ///   ANDing byte 2 with $E3 either when writing to or when reading from 
        ///   OAM. It has not been determined whether the PPU actually drives these
        ///   bits low or whether this is the effect of data bus capacitance from reading 
        ///   the last byte of the instruction (LDA $2004, which assembles to AD 04 20). 
        /// </summary>
        static public ArrayList SpriteAttribute = new ArrayList();

        /// <summary>
        /// X position of left side of sprite.
        /// X-scroll values of $F9-FF results in parts of the sprite to be past 
        /// the right edge of the screen, thus invisible. 
        /// It is not possible to have a sprite partially 
        /// visible on the left edge. Instead, left-clipping through PPUMASK ($2001) 
        /// can be used to simulate this effect. 
        /// </summary>
        static public ArrayList SpriteXc = new ArrayList();


        public NES_PPU_OAM()
        {
            for (int i = 0; i <= 0xFF; i++)
            {
                Memory.Add(new Address(i));
            }
            InitBytes();
        }

        private static void InitBytes()
        {
            SpriteYc = new ArrayList();
            SpriteTile = new ArrayList();
            SpriteAttribute = new ArrayList();
            SpriteXc = new ArrayList();
            for (int i = 0; i < 0xff; i += 4)
            {
                SpriteYc.Add(Memory[i]);
                {
                    var temp = new Byte1();
                    temp.adress = ((Address)Memory[i + 1]);
                    SpriteTile.Add(temp);
                }
                {
                    var temp = new Byte2();
                    temp.adress = ((Address)Memory[i + 2]);
                    SpriteAttribute.Add(temp);
                }
                SpriteXc.Add(Memory[i + 3]);
            }
        }

        /// <summary>
        /// Common name: OAMDMA
        /// Description: OAM DMA register (high byte)
        /// Access: write
        /// 
        /// This port is located on the CPU. Writing $XX will upload 256 bytes of data from CPU page $XX00-$XXFF to the 
        /// internal PPU OAM. This page is typically located in internal RAM, commonly $0200-$02FF, 
        /// but cartridge RAM or ROM can be used as well. 
        /// </summary>
        /// <param name="XX">OAM DMA register (high byte)</param>
        static public void OAMDMA(byte XX)
        {
            for (int i = 0; i < 0xFF; i++)
            {
                Memory[i]=NES_Memory.Memory[(XX << 8) + i]; 
            }
            InitBytes();
        }
    }
}
