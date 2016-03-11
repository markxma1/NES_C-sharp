﻿using System;
using System.Diagnostics;
using System.Drawing;

namespace NES_PPU
{
    public class Picture
    {
        #region Init
        class Mirror
        {
            public bool HaveMirror = false;
            public int x;
            public int y;
        }

        private Mirror mirror = new Mirror();
        private Color[,] img;
        private Color[,] infoLayer;
        private Size size;
        private int width { get { return size.Width; } set { size.Width = value; } }
        private int height { get { return size.Height; } set { size.Height = value; } }
        #endregion

        #region Constructor

        public Picture(int width, int height)
        {
            size.Width = width;
            size.Height = height;
            img = new Color[width, height];
            infoLayer = new Color[Size.Width, Size.Height];
        }

        public Picture(Picture image)
        {
            width = image.width;
            height = image.height;
            img = /*(Color[,])image.img.Clone();//*/new Color[image.width, image.height];
            infoLayer = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetPixel(image.GetPixel(x, y), x, y);
                }
            }
        }

        public Picture(Bitmap image)
        {
            width = image.Width;
            height = image.Height;
            img = new Color[width, height];
            infoLayer = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetPixel(image.GetPixel(x, y), x, y);
                }
            }
        }

        public Picture(Color[,] image, int width, int height)
        {
            this.width = width;
            this.height = height;
            img = (Color[,])image.Clone();
            infoLayer = new Color[width, height];
        }

        #endregion

        #region Get/Set

        public bool HaveMirror { get { return mirror.HaveMirror; } set { mirror.HaveMirror = value; } }


        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public Color GetPixel(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                Color info = getInfLayerPixel(x, y);
                if (info != new Color())
                    return info;

                if (HaveMirror && (((x >= mirror.x) && (mirror.x != 0)) || ((y >= mirror.y) && (mirror.y != 0))))
                    return GetPixel(x - mirror.x, y - mirror.y);

                return img[x, y];
            }
            return Color.Transparent;
        }

        private Color getInfLayerPixel(int x, int y)
        {
            return infoLayer[x, y];
        }

        public void SetInfLayerPixel(Color color, int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                infoLayer[x, y] = color;
        }

        public void SetPixel(Color color, int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                img[x, y] = color;
        }

        public void SetPixel(int x, int y, Color color)
        {
            SetPixel(color, x, y);
        }

        public void DrawPixel(Color color, int x, int y)
        {
            SetPixel(add(GetPixel(x, y), color), x, y);
        }

        private Color[,] getMatrix()
        {
            return img;
        }

        public Size Size { get { return size; } }

        public Bitmap Image
        {
            get
            {
                Bitmap temp = new Bitmap(width, height);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        temp.SetPixel(x, y, GetPixel(x, y));
                    }
                }
                return temp;
            }
        }
        #endregion

        #region Draw

        internal void FillRectangle(Color color, int x, int y, int width, int height)
        {
            for (int i = x; i < width; i++)
            {
                for (int j = y; j < height; j++)
                {
                    SetPixel(color, i, j);
                }
            }
        }

        public void DrawImage(Picture bitmap, int x, int y)
        {
            for (int i = x; i < x + bitmap.width; i++)
            {
                for (int j = y; j < y + bitmap.height; j++)
                {
                    DrawPixel(bitmap.GetPixel(i - x, j - y), i, j);
                }
            }
        }

        public void DrawNewImage(Picture bitmap, int x, int y)
        {
            for (int i = x; i < x + bitmap.width; i++)
            {
                for (int j = y; j < y + bitmap.height; j++)
                {
                    SetPixel(bitmap.GetPixel(i - x, j - y), i, j);
                }
            }
        }

        public void DrawMirror(int x, int y)
        {
            mirror.x = x;
            mirror.y = y;
            HaveMirror = true;
        }

        internal void DrawRectangle(Color color, int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (j == y || i == x || j == y + height - 1 || i == x + width - 1)
                        SetPixel(color, i, j);
                }
            }
        }

        internal void DrawInfoRectangle(Color color, int x, int y, int width, int height)
        {
            for (int i = x; i < x + width; i++)
            {
                for (int j = y; j < y + height; j++)
                {
                    if (j == y || i == x || j == y + height - 1 || i == x + width - 1)
                        SetInfLayerPixel(color, i, j);
                }
            }
        }

        public void DrawImage(Picture bitmap, Rectangle destRec, Rectangle srcRec)
        {
            if (srcRec.Width == destRec.Width || srcRec.Height == destRec.Height)
                SameSize(bitmap, destRec, srcRec);
            else
                NotSameSize(bitmap, destRec, srcRec);
        }

        private void NotSameSize(Picture bitmap, Rectangle destRec, Rectangle srcRec)
        {
            Picture temp1 = new Picture(srcRec.Width, srcRec.Height);

            for (int i = srcRec.X; i < srcRec.Width + srcRec.X; i++)
            {
                for (int j = srcRec.Y; j < srcRec.Height + srcRec.Y; j++)
                {
                    temp1.SetPixel(bitmap.GetPixel(i, j), i - srcRec.X, j - srcRec.Y);
                }
            }

            int W2 = destRec.Width - destRec.X;
            int H2 = destRec.Height - destRec.Y;
            Picture temp2 = new Picture(ResizeArray(temp1.getMatrix(), W2, H2), W2, H2);

            for (int i = destRec.X; i < width; i++)
            {
                for (int j = destRec.Y; j < height; j++)
                {
                    DrawPixel(temp2.GetPixel(i - destRec.X, j - destRec.Y), i, j);
                }
            }
        }

        private void SameSize(Picture bitmap, Rectangle destRec, Rectangle srcRec)
        {
            for (int i = srcRec.X; i < srcRec.Width + srcRec.X; i++)
            {
                for (int j = srcRec.Y; j < srcRec.Height + srcRec.Y; j++)
                {
                    DrawPixel(bitmap.GetPixel(i, j), i - srcRec.X + destRec.X, j - srcRec.Y + destRec.Y);
                }
            }
        }

        private T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
        }

        private Color add(Color c1, Color c2)
        {
            Color temp = Color.FromArgb(Math.Max(c1.A, c2.A), AvarageColor(c1, c2, "R"), AvarageColor(c1, c2, "G"), AvarageColor(c1, c2, "B"));
            return temp;
        }

        private static int AvarageColor(Color c1, Color c2, string color)
        {
            if ((c1.A + c2.A) == 0)
                return 255;

            int A = Range(c1.A - c2.A);
            int B = (A + c2.A);
            switch (color)
            {
                case "R":
                    return (c1.R * A + c2.R * c2.A) / B;
                case "G":
                    return (c1.G * A + c2.G * c2.A) / B;
                case "B":
                    return (c1.B * A + c2.B * c2.A) / B;
                default:
                    return 255;
            }

        }

        /// <summary>
        /// set value range. min 0 max 255
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int Range(int value)
        {
            if (value < 0) value = 0;
            if (value > 255) value = 255;
            return value;
        }
        #endregion

        #region RotateFlip
        internal void RotateFlip(RotateFlipType rotateNoneFlipX)
        {
            switch (rotateNoneFlipX)
            {
                case RotateFlipType.RotateNoneFlipNone://Rotate180FlipXY
                    break;
                case RotateFlipType.Rotate90FlipNone://Rotate270FlipXY, Rotate270FlipXY
                    Rotate90FlipNone();
                    break;
                case RotateFlipType.RotateNoneFlipXY://Rotate180FlipNone
                    RotateNoneFlipX();
                    RotateNoneFlipY();
                    break;
                case RotateFlipType.Rotate270FlipNone://Rotate90FlipXY
                    Rotate90FlipNone();
                    RotateFlip(RotateFlipType.RotateNoneFlipXY);
                    break;
                case RotateFlipType.RotateNoneFlipX://Rotate180FlipY
                    RotateNoneFlipX();
                    break;
                case RotateFlipType.Rotate90FlipX:
                    Rotate90FlipNone();
                    RotateNoneFlipX();
                    break;
                case RotateFlipType.Rotate270FlipX://Rotate90FlipY
                    Rotate90FlipNone();
                    RotateNoneFlipY();
                    break;
                case RotateFlipType.RotateNoneFlipY: //Rotate180FlipX,
                    RotateNoneFlipY();
                    break;
                default:
                    break;
            }

        }

        private void Rotate90FlipNone()
        {
            Color[,] temp = img;
            img = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetPixel(temp[y, x], x, y);
                }
            }
        }

        private void RotateNoneFlipY()
        {
            Color[,] temp = img;
            img = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetPixel(temp[x, height - y], x, y);
                }
            }
        }

        private Color[,] RotateNoneFlipX()
        {
            Color[,] temp = img;
            img = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    SetPixel(temp[width - x, y], x, y);
                }
            }
            return temp;
        }
        #endregion

    }
}
