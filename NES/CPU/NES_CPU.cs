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
        static public AssemblyList Assembly = new AssemblyList();

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
            NES_Register.POWER = true;

            NES_PPU.InitialAtPower();
            DateTime t2 = DateTime.Now;

            while (NES_Register.POWER)
            {
                cpuspeed = Sleep((mod == Mod.PAL) ? (PAL) : (NTSC), delegate
                {

                    if (NES_Register.PC == 0xC070)
                    { }

                    //if (NES_Register.PPUPCADDR == 0x2212)
                    //{ }

                    //if (NES_Register.PPUPCADDR == 0x2612)
                    //{ }

                    //if (NES_Register.PPUPCADDR == 0x3212)
                    //{ }

                    //if (NES_Register.PPUPCADDR == 0x3612)
                    //{ }
                    try { Assembly.assembly[((Adress)NES_Memory.Memory[NES_Register.PC]).Value](); }
                    catch (Exception ex) { System.Windows.Forms.MessageBox.Show("No Assembly: " + ((Adress)NES_Memory.Memory[NES_Register.PC]).Value.ToString("X")); }
                    Interrupt();
                });
            }
        }

        private static void Interrupt()
        {

            // $FFFA–$FFFB 	2 bytes 	Address of Non Maskable Interrupt (NMI) handler routine
            // $FFFC–$FFFD 	2 bytes 	Address of Power on reset handler routine
            // $FFFE–$FFFF 	2 bytes 	Address of Break (BRK instruction) handler routine
            if (NES_Register.IRQ && !NES_Register.P.Interrupt)
            {
                NES_Register.P.Interrupt = true;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = (byte)NES_Register.PC;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = (byte)(NES_Register.PC >> 8);
                var PStack = NES_Register.P;
                PStack.B = false;
                PStack.U = true;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = PStack.P;
                NES_Register.PC = (ushort)(((Adress)NES_Memory.Stack[0xfffe]).Value | (((Adress)NES_Memory.Stack[0xffff]).Value << 8));
                NES_Register.IRQ = false;
            }
            if (NES_Register.BRK)
            {
                NES_Register.P.Interrupt = true;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = (byte)NES_Register.PC;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = (byte)(NES_Register.PC >> 8);
                var PStack = NES_Register.P;
                PStack.B = true;
                PStack.U = true;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = PStack.P;
                NES_Register.PC = (ushort)(((Adress)NES_Memory.Memory[0xfffe]).Value | (((Adress)NES_Memory.Memory[0xffff]).Value << 8));
                NES_Register.BRK = false;
            }
            if (NES_Register.NMI)
            {
                NES_Register.P.Interrupt = true;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = (byte)NES_Register.PC;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = (byte)(NES_Register.PC >> 8);
                var PStack = NES_Register.P;
                PStack.B = false;
                PStack.U = true;
                ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = PStack.P;
                NES_Register.PC = (ushort)(((Adress)NES_Memory.Memory[0xfffa]).Value | (((Adress)NES_Memory.Memory[0xfffb]).Value << 8));
                NES_Register.NMI = false;
            }
            if (NES_Register.RESET)
            {
                NES_Register.PC = (ushort)(((Adress)NES_Memory.Memory[0xfffc]).Value | (((Adress)NES_Memory.Memory[0xfffd]).Value << 8));
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

        public static void Stop() { NES_Register.POWER = false; }

    }
}
