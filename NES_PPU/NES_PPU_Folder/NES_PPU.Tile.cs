﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace NES
{
    public partial class NES_PPU
    {
        private static Bitmap TempPatternTable = new Bitmap(128 * 2, 128);
        private static Dictionary<int, BitmapWithInfo> patternArray = new Dictionary<int, BitmapWithInfo>();
        public static bool DrawRefresh = false;


        /// <summary>
        /// converts Tiles from Memory to Bitmap. 
        /// </summary>
        /// <param name="startAdress"></param>
        /// <returns></returns>
        public static Bitmap Tile_StartAdress(int startAdress, int pallete)
        {
            byte[,] pattern = new byte[8, 8];
            NES_PPU_Color color = NES_PPU_Palette.getPalette(pallete);
            var PatternTable = NES_PPU_Memory.Memory;
            int ID = GetTileID(startAdress, pallete, PatternTable);

            return CreateTileBitmap(startAdress, color, PatternTable, ID);
        }

        /// <summary>
        /// converts Tiles from Memory to Bitmap. With ID and  Backgrount Tile Select.
        /// </summary>
        /// <param name="spriteID">ID of Sprite  </param>
        /// <returns></returns>
        public static Bitmap Tile(ushort spriteID, int pallete)
        {
            int startAdress = spriteID * 16;
            byte[,] pattern = new byte[8, 8];
            NES_PPU_Color color = NES_PPU_Palette.getPalette(pallete);
            var PatternTable = NES_PPU_Memory.PatternTableN[NES_PPU_Register.PPUCTRL.B ? 1 : 0];
            int ID = GetTileID(startAdress, pallete, PatternTable);

            return CreateTileBitmap(startAdress, color, PatternTable, ID);
        }

        /// <summary>
        /// converts Tiles from Memory to Bitmap. With ID, BankID and  Backgrount Tile Select.
        /// </summary>
        /// <param name="spriteID">ID of Sprite  </param>
        /// <returns></returns>
        public static Bitmap Tile(ushort spriteID, int pallete, int bankID)
        {
            int startAdress = spriteID * 16;
            NES_PPU_Color color = NES_PPU_Palette.getSpriteColorPalette(pallete);
            var PatternTable = NES_PPU_Memory.PatternTableN[bankID];
            int ID = GetTileID(startAdress, pallete, PatternTable);
            return CreateTileBitmap(startAdress, color, PatternTable, ID);
        }

        private static int GetTileID(int startAdress, int pallete, ArrayList PatternTable)
        {
            return pallete | (NES_PPU_Memory.Memory.IndexOf(PatternTable[startAdress])) << 8;
        }

        /// <summary>
        /// Creates an Bitmap from data saved in PPU Adresses. 
        /// </summary>
        /// <param name="startAdress"></param>
        /// <param name="bitmap"></param>
        /// <param name="pattern"></param>
        /// <param name="color"></param>
        /// <param name="PatternTable"></param>
        private static Bitmap CreateTileBitmap(int startAdress, NES_PPU_Color color, ArrayList PatternTable, int ID)
        {
            try
            {
                if (isNew(startAdress, PatternTable, color, ID))
                {
                    BitmapWithInfo bitmap;

                    if (isNewPattern(startAdress, PatternTable, ID))
                    {
                        bitmap = CreateNewTile(startAdress, color, PatternTable);

                        AddTileToPatternArray(ID, bitmap);
                        return DrawRefreshFrame(bitmap.Image, Pens.Red);
                    }
                    else
                    {
                        bitmap = UpdateTile(ID, color);

                        AddTileToPatternArray(ID, bitmap);

                        lock (bitmap)
                        {
                            return DrawRefreshFrame(bitmap.Image, Pens.Blue, bitmap.isNew);
                        }
                    }
                }
                else
                {
                    lock (patternArray)
                    {
                        return patternArray[ID].Image;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static BitmapWithInfo UpdateTile(int ID, NES_PPU_Color color)
        {
            lock (patternArray)
            {
                bool isNew = false;
                foreach (int cID in patternArray[ID].cID)
                {
                    isNew |= color.isNewColor[cID];
                }

                if (isNew)
                {
                    byte[,] pattern = patternArray[ID].Pattern;
                    Bitmap bitmap = new Bitmap(8, 8);

                    for (int j = 0; j < 8; j++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            bitmap.SetPixel(7 - j, i, color.color[pattern[7 - j, i]]);
                        }
                    }
                    return new BitmapWithInfo(bitmap, pattern, patternArray[ID].cID, true);
                }
                patternArray[ID].isNew = false;
                return patternArray[ID];
            }
        }

        private static Bitmap DrawRefreshFrame(Bitmap bitmap, Pen pen, bool isNew = true)
        {
            if (DrawRefresh && isNew)
            {
                Graphics g = Graphics.FromImage(bitmap);
                g.DrawRectangle(pen, 0, 0, 7, 7);
                g.Dispose();
            }
            return bitmap;
        }

        private static BitmapWithInfo CreateNewTile(int startAdress, NES_PPU_Color color, ArrayList PatternTable)
        {
            byte[,] pattern = new byte[8, 8];
            Bitmap bitmap = new Bitmap(8, 8);
            List<byte> cID = new List<byte>();
            Parallel.For(0, 8, j =>
            {
                Parallel.For(0, 8, i =>
                {
                    var a = (((Address)PatternTable[startAdress + i]).Value >> j) & (0x01);
                    var b = ((((Address)PatternTable[startAdress + i + 8]).Value >> j) & (0x01)) << 1;
                    pattern[7 - j, i] = (byte)(a | b);


                    lock (cID)
                    {
                        if (!cID.Contains(pattern[7 - j, i]))
                            cID.Add(pattern[7 - j, i]);
                    }

                    lock (bitmap)
                    {
                        bitmap.SetPixel(7 - j, i, color.color[pattern[7 - j, i]]);
                    }

                });
            });
            ((Address)PatternTable[startAdress]).setAsOld();
            return new BitmapWithInfo(bitmap, pattern, cID.ToArray());
        }

        private static bool isNew(int startAdress, ArrayList PatternTable, NES_PPU_Color color, int ID)
        {
            bool isnew = isNewPattern(startAdress, PatternTable, ID);
            isnew |= color.isNewPalette;
            return isnew;
        }

        private static bool isNewPattern(int startAdress, ArrayList PatternTable, int ID)
        {
            bool isnew = !patternArray.ContainsKey(ID);
            isnew |= ((Address)PatternTable[startAdress]).isNew();
            return isnew;
        }

        /// <summary>
        /// Putts tile to array buffer.
        /// </summary>
        /// <param name="startAdress"></param>
        /// <param name="bitmap"></param>
        private static void AddTileToPatternArray(int ID, BitmapWithInfo bitmap)
        {
            lock (patternArray)
            {
                if (patternArray.ContainsKey(ID))
                    patternArray[ID] = bitmap;
                else
                    patternArray.Add(ID, bitmap);
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