namespace NES
{
    public partial class NES_PPU
    {
        private static bool ScrollXoY = true;//false y ture x
        private static int xScrollTemp = 0;
        private static int yScrollTemp = 0;
        public static int xScroll = 0;
        public static int yScroll = 0;

        public static byte Scroll
        {
            get { return 0; }

            set
            {
                if (ScrollXoY)
                {
                    XScroll = value;
                }
                else
                {
                    if (value > 239)
                        value -= 255;
                    YScroll = value;
                }
                ScrollXoY = !ScrollXoY;
            }
        }

        private static int XScroll
        {
            get
            {
                return xScrollTemp;
            }

            set
            {
                AddxScroll(value);
                xScroll = value;
                if (!Draw)
                    xScrollTemp = value;
            }
        }

        private static int YScroll
        {
            get
            {
                return yScrollTemp;
            }

            set
            {
                value = AddyScroll(value);
                yScroll = value;
                if (!Draw)
                    yScrollTemp = value;
            }
        }

        private static int AddyScroll(int value)
        {
            int add = ((NES_PPU_Register.PPUCTRL.N & 2) == 0) ? (0) : (240);
            value += add;
            return value;
        }

        private static int AddxScroll(int value)
        {
            int add = ((NES_PPU_Register.PPUCTRL.N & 1) == 0) ? (0) : (256);
            value += add;
            return value;
        }
    }
}
