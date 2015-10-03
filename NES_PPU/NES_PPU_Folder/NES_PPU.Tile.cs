using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace NES
{
    public partial class NES_PPU
    {
        private static Bitmap TempPatternTable = new Bitmap(128 * 2, 128);
        private static Dictionary<int, Bitmap> patternArray = new Dictionary<int, Bitmap>();

        /// <summary>
        /// converts Tiles from Memory to Bitmap. 
        /// </summary>
        /// <param name="startAdress"></param>
        /// <returns></returns>
        public static Bitmap Tile_StartAdress(ushort startAdress, int pallete)
        {
            Bitmap bitmap = new Bitmap(8, 8);
            byte[,] pattern = new byte[8, 8];
            Color[] color = NES_PPU_Palette.getPalette(pallete);
            var PatternTable = NES_PPU_Memory.Memory;

            CreateTileBitmap(startAdress, bitmap, pattern, color, PatternTable);

            return bitmap;
        }

        /// <summary>
        /// converts Tiles from Memory to Bitmap. With ID and  Backgrount Tile Select.
        /// </summary>
        /// <param name="spriteID">ID of Sprite  </param>
        /// <returns></returns>
        public static Bitmap Tile(ushort spriteID, int pallete)
        {
            int startAdress = spriteID * 16;
            Bitmap bitmap = new Bitmap(8, 8);
            byte[,] pattern = new byte[8, 8];
            Color[] color = NES_PPU_Palette.getPalette(pallete);
            var PatternTable = NES_PPU_Memory.PatternTableN[NES_PPU_Register.PPUCTRL.B ? 1 : 0];

            CreateTileBitmap(startAdress, bitmap, pattern, color, PatternTable);

            return bitmap;
        }

        /// <summary>
        /// converts Tiles from Memory to Bitmap. With ID, BankID and  Backgrount Tile Select.
        /// </summary>
        /// <param name="spriteID">ID of Sprite  </param>
        /// <returns></returns>
        public static Bitmap Tile(ushort spriteID, int pallete, int bankID)
        {
            int startAdress = spriteID * 16;
            Bitmap bitmap = new Bitmap(8, 8);
            byte[,] pattern = new byte[8, 8];
            Color[] color = NES_PPU_Palette.getSpriteColorPalette(pallete);
            var PatternTable = NES_PPU_Memory.PatternTableN[bankID];

            CreateTileBitmap(startAdress, bitmap, pattern, color, PatternTable);

            return bitmap;
        }

        /// <summary>
        /// Creates an Bitmap from data saved in PPU Adresses. 
        /// </summary>
        /// <param name="startAdress"></param>
        /// <param name="bitmap"></param>
        /// <param name="pattern"></param>
        /// <param name="color"></param>
        /// <param name="PatternTable"></param>
        private static void CreateTileBitmap(int startAdress, Bitmap bitmap, byte[,] pattern, Color[] color, ArrayList PatternTable)
        {
            if (((Address)PatternTable[startAdress]).isNew() || !patternArray.ContainsKey(startAdress))
            {
                Parallel.For(0, 8, j =>
                {
                    Parallel.For(0, 8, i =>
                    {
                        var a = (((Address)PatternTable[startAdress + i]).Value >> j) & (0x01);
                        var b = ((((Address)PatternTable[startAdress + i + 8]).Value >> j) & (0x01)) << 1;
                        pattern[7 - j, i] = (byte)(a | b);
                        lock (bitmap)
                        {
                            bitmap.SetPixel(7 - j, i, color[pattern[7 - j, i]]);
                        }
                    });
                });
                AddTileToPatternArray(startAdress, bitmap);
            }
            else
            {
                bitmap = patternArray[startAdress];
            }
        }

        /// <summary>
        /// Putts tile to array buffer.
        /// </summary>
        /// <param name="startAdress"></param>
        /// <param name="bitmap"></param>
        private static void AddTileToPatternArray(int startAdress, Bitmap bitmap)
        {
            lock (bitmap)
            {
                lock (patternArray)
                {
                    if (patternArray.ContainsKey(startAdress))
                        patternArray[startAdress] = bitmap;
                    else
                        patternArray.Add(startAdress, bitmap);
                }
            }
        }

        /// <summary>
        /// Creats an Bitmap with Patterns used for Game. 
        /// </summary>
        /// <param name="PN">Palette Nummer</param>
        /// <returns></returns>
        public static Bitmap PatternTable(int PN)
        {
            Bitmap bitmap = new Bitmap(TempPatternTable);
            if (NES_PPU_Register.PPUCTRL.V)
            {
                Graphics g = Graphics.FromImage(bitmap);
                int k = 0;
                for (ushort i = 0; i < 16; i++)
                {
                    for (ushort j = 0; j < 16; j++)
                    {
                        int t = (k++) * 16;
                        g.DrawImage(Tile_StartAdress((ushort)(t), PN), j * 8, i * 8);
                        g.DrawImage(Tile_StartAdress((ushort)(t + 4096), PN), (j + 16) * 8, i * 8);
                    }
                }
                g.Dispose();
                TempPatternTable = bitmap;
            }
            return bitmap;
        }

    }
}
