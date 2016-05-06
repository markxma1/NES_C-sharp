///   Copyright 2016 Xma1
///
///   This file is part of NES-C#.
///
///   NES-C# is free software: you can redistribute it and/or modify
///   it under the terms of the GNU General Public License as published by
///   the Free Software Foundation, either version 3 of the License, or
///   (at your option) any later version.
///
///   NES-C# is distributed in the hope that it will be useful,
///   but WITHOUT ANY WARRANTY; without even the implied warranty of
///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
///   See the GNU General Public License for more details.
///
///   You should have received a copy of the GNU General Public License
///   along with NES-C#. If not, see http://www.gnu.org/licenses/.
using NES_PPU;
using System.Drawing;

namespace NES
{
    public class NES_Console
    {
        public static bool DrawRefresh
        {
            get { return NES_PPU.DrawRefresh; }
            set { NES_PPU.DrawRefresh = value; }
        }

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
            return NES_PPU.PaletteTable().Image;
        }

        public static Bitmap getPatternTable(int PN)
        {
            return NES_PPU.PatternTable(PN).Image;
        }

        public static Bitmap getNameTabele(bool display = true)
        {
            return NES_PPU.NameTabele(display).Image;
        }

        public static Bitmap getDisplay(bool display = true)
        {
            return NES_PPU.Display().Image;
        }

        public static Color getUniversalBackgroundColor()
        {
            return NES_PPU_Palette.UniversalBackgroundColor();
        }

        public static void LoadRom(string path)
        {
            NES_ROM.LoadRom(path);
        }

        public static void SaveGame(string path)
        {
           SaveLoadMemory.SaveTo(path);
        }

        public static void LoadGame(string path)
        {
            SaveLoadMemory.LoadFrom(path);
        }
    }
}
