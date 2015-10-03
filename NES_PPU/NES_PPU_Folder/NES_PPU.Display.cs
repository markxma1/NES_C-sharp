﻿using System.Drawing;

namespace NES
{
    public partial class NES_PPU
    {
        private static Bitmap TempDisplay = new Bitmap(256, 240);
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

        public static Bitmap Display()
        {
            Bitmap Display = new Bitmap(TempDisplay);

            if (NES_PPU_Register.PPUCTRL.V)
            {
                Draw = true;
                Display = new Bitmap(TempDisplay.Size.Width, TempDisplay.Size.Height);
                Interrupt.NMI = true;
                Rectangle cropRect = new Rectangle(XScroll, YScroll, 257, 241);
                Graphics g = Graphics.FromImage(Display);
                Bitmap NameTabeleT = NameTabele();
                g.DrawImage(InsetObect(false), 0, 0);
                g.DrawImage(NameTabeleT, new Rectangle(0, 0, Display.Width, Display.Height), cropRect, GraphicsUnit.Pixel);
                g.DrawImage(InsetObect(true), 0, 0);
                g.Dispose();
                TempDisplay = Display;
                Draw = false;
                NES_PPU_Register.PPUSTATUS.V = true;
            }
            return Display;
        }

        private static Bitmap InsetObect(bool Priory)
        {
            Bitmap Display = new Bitmap(TempDisplay.Width, TempDisplay.Height);
            Graphics g = Graphics.FromImage(Display);
            for (int i = 0; i < NES_PPU_OAM.SpriteTile.Count; i++)
            {
                if (Priory == ((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).Priority)
                {
                    Bitmap tile = Tile(((NES_PPU_OAM.Byte1)NES_PPU_OAM.SpriteTile[i]).Number,
                                                   ((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).Palette,
                                                   (((NES_PPU_OAM.Byte1)NES_PPU_OAM.SpriteTile[i]).Bank) ? (1) : (0));

                    if (((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).FlipH)
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    if (((NES_PPU_OAM.Byte2)NES_PPU_OAM.SpriteAttribute[i]).FlipV)
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipY);

                    g.DrawImage(tile, ((Address)NES_PPU_OAM.SpriteXc[i]).Value - 8,
                                                        ((Address)NES_PPU_OAM.SpriteYc[i]).Value - 1);
                }
            }
            return Display;
        }
    }
}
