using NES_PPU;
using System.Drawing;

namespace NES
{
    public partial class NES_PPU
    {
        private static Picture TempDisplay = new Picture(256, 240);
        private static bool draw = false;

        private static bool Draw
        {
            get
            {
                return draw;
            }

            set
            {
                if (value)
                    draw = value;
                else
                {
                    draw = value;
                    xScrollTemp = xScroll;
                    yScrollTemp = yScroll;
                }
            }
        }

        public static Picture Display()
        {
            Picture Display = new Picture(TempDisplay);

            if (NES_PPU_Register.PPUCTRL.V)
            {
                Draw = true;
                Display = new Picture(TempDisplay.Size.Width, TempDisplay.Size.Height);
                Interrupt.NMI = true;

                Display.DrawImage(InsetObect(false), 0, 0);
                DrawBackground(Display, NameTabele());
                Display.DrawImage(InsetObect(true), 0, 0);


                NES_PPU_Palette.setAllPaletesAsOld();
                TempDisplay = Display;
                Draw = false;
                NES_PPU_Register.PPUSTATUS.V = true;
            }
            return Display;
        }

        private static void DrawBackground(Picture Display, Picture NameTabeleT)
        {
            Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, YScroll, 257, 241));

                if (XScroll > 240)
                    Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll - (256 * 2), YScroll, 257, 241));
                if (YScroll > 240)
                    Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, YScroll - (240 * 2), 257, 241));
                if (XScroll < 0)
                    Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle((256 * 2) - XScroll, YScroll, 257, 241));
                if (YScroll < 0)
                    Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, (240 * 2) - YScroll, 257, 241));
        }

        private static Picture InsetObect(bool Priory)
        {
            Picture Display = new Picture(TempDisplay.Width, TempDisplay.Height);

            for (int i = 0; i < NES_PPU_OAM.SpriteTile.Count; i++)
            {
                if (Priory == ((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).Priority)
                {

                    Picture tile = Tile(((NES_PPU_OAM.Byte1)NES_PPU_OAM.SpriteTile[i]).Number,
                                                     ((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).Palette,
                                                   (((NES_PPU_OAM.Byte1)NES_PPU_OAM.SpriteTile[i]).Bank) ? (1) : (0));

                    tile = new Picture(tile);

                    if (((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).FlipH)
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    if (((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).FlipV)
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    Display.DrawImage(tile, ((Address)NES_PPU_OAM.SpriteXc[i]).Value - 8,
                                                        ((Address)NES_PPU_OAM.SpriteYc[i]).Value - 1);
                }
            }
            return Display;
        }
    }
}
