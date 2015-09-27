using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NES
{
    public class NES_Console
    {

        public static void INIT()
        {
            new NES_Memory();
            new NES_Register();
            new NES_CPU();
            new NES_PPU();
            new NES_GamePad();
        }

        public static void Run()
        {
            NES_Register.RessetPointer();
            NES_Register.P.Interrupt = false;
            Interrupt.POWER = true;
            NES_PPU_Register.InitialAtPower();
            NES_CPU.Run();
        }

        public static void Restart()
        {
            Interrupt.POWER = false;
            NES_Register.RessetPointer();
            NES_Register.P.Interrupt = false;
            Interrupt.POWER = true;
            NES_PPU_Register.InitialOnReset();
            NES_CPU.Run();
        }

        public static void Stop()
        {
            Interrupt.POWER = false;
        }

        public static Bitmap getPaletteTable()
        {
            return NES_PPU.PaletteTable();
        }

        public static Bitmap getPatternTable(int PN)
        {
            return NES_PPU.PatternTable(PN);
        }

        public static Bitmap getNameTabele(bool display = true)
        {
            return NES_PPU.NameTabele(display);
        }

        public static Bitmap getDisplay(bool display = true)
        {
            return NES_PPU.Display();
        }

        public static Color getUniversalBackgroundColor()
        {
            return NES_PPU_Palette.UniversalBackgroundColor();
        }
    }
}
