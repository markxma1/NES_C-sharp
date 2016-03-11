using NES_PPU;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

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
            System.TimeSpan t1 = new System.TimeSpan(0);
            System.TimeSpan t2 = new System.TimeSpan(0);
            System.TimeSpan t3 = new System.TimeSpan(0);
            System.TimeSpan t4 = new System.TimeSpan(0);

            if (NES_PPU_Register.PPUCTRL.V)
            {
                NES_PPU_Register.PPUCTRL.V = false;
                Interrupt.NMI = true;

                Stopwatch t = new Stopwatch();
                t.Start();


                Draw = true;
                Picture Display = new Picture(TempDisplay.Size.Width, TempDisplay.Size.Height);

                t1 = t.Elapsed;
                Display.DrawImage(InsetObect(false), 0, 0);
                t2 = t.Elapsed;
                DrawBackground(Display, NameTabele());
                t3 = t.Elapsed;
                Display.DrawImage(InsetObect(true), 0, 0);
                t.Stop();
                t4 = t.Elapsed;
                t4 -= t3;
                t3 -= t2;
                t2 -= t1;
                NES_PPU_Palette.setAllPaletesAsOld();
                TempDisplay = Display;
                Draw = false;
                NES_PPU_Register.PPUSTATUS.V = true;
            }

            return TempDisplay;
        }

        private static void DrawBackground(Picture Display, Picture NameTabeleT)
        {
            System.TimeSpan t1 = new System.TimeSpan(0);
            System.TimeSpan t2 = new System.TimeSpan(0);
            System.TimeSpan t3 = new System.TimeSpan(0);
            System.TimeSpan t4 = new System.TimeSpan(0);
            System.TimeSpan t5 = new System.TimeSpan(0);

            Stopwatch t = new Stopwatch();
            t.Start();
            Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, YScroll, 256, 240));
            t1 = t.Elapsed;
            if (XScroll > 240)
                Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll - (256 * 2), YScroll, 256, 240));
            t2 = t.Elapsed;
            if (YScroll > 240)
                Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, YScroll - (240 * 2), 256, 240));
            t3 = t.Elapsed;
            if (XScroll < 0)
                Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle((256 * 2) - XScroll, YScroll, 256, 240));
            t4 = t.Elapsed;
            if (YScroll < 0)
                Display.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), new Rectangle(XScroll, (240 * 2) - YScroll, 256, 240));
            t5 = t.Elapsed;
            t.Stop();
            t5 -= t5;
            t4 -= t3;
            t3 -= t2;
            t2 -= t1;
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
