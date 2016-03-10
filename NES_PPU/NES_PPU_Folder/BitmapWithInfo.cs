using NES_PPU;
using System.Drawing;

namespace NES
{
    class BitmapWithInfo
    {
        Picture bitmap;
        byte[,] pattern;
        public bool isNew;
        public byte[] cID;

        public BitmapWithInfo(Picture bitmap, byte[,] pattern, byte[] cID, bool isNew = true)
        {
            this.bitmap = new Picture(bitmap);
            this.pattern = pattern;
            this.isNew = isNew;
            this.cID = cID;
        }

        public Picture Image
        {
            get
            {
                return new Picture(bitmap);
            }

            set
            {
                bitmap = new Picture(value);
            }
        }

        public byte[,] Pattern
        {
            get
            {
                return pattern;
            }
            set
            {
                pattern = value;
            }
        }
    }
}
