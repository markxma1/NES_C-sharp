using System.Drawing;

namespace NES
{
    class BitmapWithInfo
    {
        Bitmap bitmap;
        byte[,] pattern;
        public bool isNew;
        public byte[] cID;

        public BitmapWithInfo(Bitmap bitmap, byte[,] pattern, byte[] cID, bool isNew = true)
        {
            this.bitmap = new Bitmap(bitmap);
            this.pattern = pattern;
            this.isNew = isNew;
            this.cID = cID;
        }

        public Bitmap Image
        {
            get
            {
                return new Bitmap(bitmap);
            }

            set
            {
                bitmap = new Bitmap(value);
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
