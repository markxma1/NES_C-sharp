using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NES_PPU
{
    public class Picture
    {
        // private Bitmap image;
        private Color[,] img;
        private Size size;
        private int width { get { return size.Width; } set { size.Width = value; } }
        private int height { get { return size.Height; } set { size.Height = value; } }

        public Picture(int width, int height)
        {
            size.Width = width;
            size.Height = height;
            img = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    img[x, y] = new Color();
                }
            }
        }

        public Picture(Picture image)
        {
            width = image.width;
            height = image.height;
            img = new Color[image.width, image.height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    img[x, y] = (Color)image.img[x, y];
                }
            }
        }

        public Picture(Bitmap image)
        {
            size.Width = image.Width;
            size.Height = image.Height;
            img = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    img[x, y] = image.GetPixel(x, y);
                }
            }
        }

        public Picture(Color[,] image, int width, int height)
        {
            this.width = width;
            this.height = height;
            img = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    img[x, y] = image[x, y];
                }
            }
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
                        temp.SetPixel(x, y, img[x, y]);
                    }
                }
                return temp;
            }
        }

        internal void FillRectangle(Color color, int x, int y, int width, int height)
        {
            for (int i = x; i < width; i++)
            {
                for (int j = y; j < height; j++)
                {
                    img[i, j] = color;
                }
            }
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public void DrawImage(Picture bitmap, int x, int y)
        {
            for (int i = x; i < x + bitmap.width; i++)
            {
                for (int j = y; j < y + bitmap.height; j++)
                {
                    if (i >= 0 && j >= 0 && i < width && j < height)
                        add(img[i, j], bitmap.getPixel(i - x, j - y));
                }
            }
        }

        private Color add(Color c1, Color c2)
        {
            Color temp = Color.FromArgb(Math.Max(c1.A, c2.A), AvarageColor(c1, c2, "R"), AvarageColor(c1, c2, "G"), AvarageColor(c1, c2, "B"));
            return temp;
        }

        private static int AvarageColor(Color c1, Color c2, string color)
        {
            int A = (c1.A + c2.A);
            if (A == 0)
                return 255;
            if (c1.B == 0)
                switch (color)
                {
                    case "R":
                        return c1.R;
                    case "G":
                        return c1.G;
                    case "B":
                        return c1.B;
                    default:
                        return 255;
                }
            if (c1.B == 255)
                switch (color)
                {
                    case "R":
                        return c2.R;
                    case "G":
                        return c2.G;
                    case "B":
                        return c2.B;
                    default:
                        return 255;
                }
            switch (color)
            {
                case "R":
                    return (c1.R * c1.A + c2.R * c2.A) / A;
                case "G":
                    return (c1.G * c1.A + c2.G * c2.A) / A;
                case "B":
                    return (c1.B * c1.B + c2.B * c2.A) / A;
                default:
                    return 255;
            }

        }

        private Color getPixel(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
                return img[x, y];
            return Color.Transparent;
        }
        //TODO Cut image
        public void DrawImage(Picture bitmap, Rectangle destRec, Rectangle srcRec)
        {
            Picture temp1 = new Picture(srcRec.Width, srcRec.Height);

            for (int i = srcRec.X; i < srcRec.Width + srcRec.X; i++)
            {
                for (int j = srcRec.Y; j < srcRec.Height + srcRec.Y; j++)
                {
                    if (i - srcRec.X >= 0 && j - srcRec.Y >= 0 && i - srcRec.X < srcRec.Width && j - srcRec.Y < srcRec.Height)
                        temp1.SetPixel(i - srcRec.X, j - srcRec.Y, bitmap.getPixel(i, j));
                }
            }

            int W2 = destRec.Width - destRec.X;
            int H2 = destRec.Height - destRec.Y;
            Picture temp2 = new Picture(ResizeArray<Color>(temp1.getMatrix(), W2, H2), W2, H2);

            for (int i = destRec.X; i < width; i++)
            {
                for (int j = destRec.Y; j < height; j++)
                {
                    if (i >= 0 && j >= 0 && i < width && j < height)
                        if (i - destRec.X >= 0 && j - destRec.Y >= 0 && i - destRec.X < W2 && j - srcRec.Y < H2)
                            img[i, j] = temp2.getPixel(i - destRec.X, j - destRec.Y);
                }
            }
        }

        private Color[,] getMatrix()
        {
            return img;
        }

        T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
        }

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
                    img[x, y] = temp[y, x];
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
                    img[x, y] = temp[x, height - y];
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
                    img[x, y] = temp[width - x, y];
                }
            }

            return temp;
        }

        public void SetPixel(int x, int y, Color color)
        {
            img[x, y] = color;
        }

        internal void DrawRectangle(Color color, int x, int y, int width, int height)
        {
            for (int i = x; i < width; i++)
            {
                for (int j = y; j < height; j++)
                {
                    if (i > 0 && j > 0 && i < width && j < height)
                        if (j == y || i == x || j == height || x == width)
                            img[i, j] = color;
                }
            }
        }
    }
}
