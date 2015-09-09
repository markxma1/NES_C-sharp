using System;
using System.Diagnostics;

namespace NES
{
    enum Mod
    {
        NTSC,
        PAL,
        PAL2
    };

    class NES_CPU
    {

        #region Init
        delegate void Func();
        public static AssemblyList Assembly = new AssemblyList();
        private static ushort lastPC = 0;
        private static ushort lastPC2 = 0;
        private static ushort lastPC3 = 0;
        private static ushort lastPC4 = 0;
        private static ushort lastPC5 = 0;
        private static byte changed = 0;
        //private static int count = 0;

        #region PAL/NTSC Speed
        public static double cpuspeed;
        private static int NTSC = 559;//1.789773 MHz (~559 ns per cycle)
        private static int PAL = 601;//1.662607 MHz (~601 ns per cycle)
        public static Mod mod = Mod.PAL;
        #endregion

        /// <summary>
        /// Init all variables. 
        /// </summary>
        public NES_CPU()
        {
            InitMemorys();
        }

        private static void InitMemorys()
        {
            new NES_Memory();
            new Assembly_6502();
            new NES_PPU();
            new NES_Register();
        }

        #endregion

        public static void Run()
        {
            NES_Register.RessetPointer();
            NES_Register.P.Interrupt = false;
            Interrupt.POWER = true;

            NES_PPU_Register.InitialAtPower();
            DateTime timer = DateTime.Now;

            while (Interrupt.POWER)
            {
                cpuspeed = Sleep((mod == Mod.PAL) ? (PAL) : (NTSC), delegate
                {
                    
                    if (NES_Register.PC == 0xC063)//0xC0c8
                    { }

                    if (NES_Register.PC == 0xCDB9)//0xC0c8
                    { }

                    if (NES_Register.PC == 0xCDCC)//0xC0c8
                    { }

                    if (NES_Register.PC == 0xEBDE)//0xC0c8
                    { }

                    if (NES_Register.PC == 0xC5D3)//0xC0c8
                    { }

                    if (NES_Register.PC == 0xCDCA)//0xC0c8
                    { }

                    if (NES_Register.PC == 0xC2ED)//0xC0c8
                    { }

                    //if (NES_Register.PPUPCADDR == 0x2212)
                    //{ }

                    if (((Address)NES_Memory.Memory[0x46]).value != changed)
                    { changed = ((Address)NES_Memory.Memory[0x46]).value; }

                    lastPC5 = lastPC4;
                    lastPC4 = lastPC3;
                    lastPC3 = lastPC2;
                    lastPC2 = lastPC;
                    lastPC = NES_Register.PC;
                    try { Assembly.assembly[((Address)NES_Memory.Memory[NES_Register.PC]).Value](); }
                    catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message + ((Address)NES_Memory.Memory[lastPC]).Value.ToString("X")); }
                    Interrupt.Check();
                });
            }
        }

        private static double Sleep(int sleep, Func function)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            function();
            while (sw.Elapsed.TotalMilliseconds * 1000 < sleep) { }
            sw.Stop();
            return sw.Elapsed.TotalMilliseconds * 1000;
        }
    }
}
