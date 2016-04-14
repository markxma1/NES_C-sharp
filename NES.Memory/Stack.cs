///   Copyright 2016 Xma1
///
///   This file is part of NES-C#.
///
///   Foobar is free software: you can redistribute it and/or modify
///   it under the terms of the GNU General Public License as published by
///   the Free Software Foundation, either version 3 of the License, or
///   (at your option) any later version.
///
///   Foobar is distributed in the hope that it will be useful,
///   but WITHOUT ANY WARRANTY; without even the implied warranty of
///   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
///   See the GNU General Public License for more details.
///
///   You should have received a copy of the GNU General Public License
///   along with Foobar. If not, see http://www.gnu.org/licenses/.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NES
{
    public class Stack
    {

        public static void ProcessorstatusToStack(bool b, bool u)
        {
            var PStack = NES_Register.P;
            PStack.B = b;
            PStack.U = u;
            PushToStack(PStack.P);
        }

        public static void StackToProcessorstatus()
        {
            NES_Register.P.P = PopFromStack();
        }

        public static void PcToStack()
        {
            NES_Register.PC--;
            PushToStack((byte)(NES_Register.PC >> 8));
            PushToStack((byte)NES_Register.PC);
        }

        public static void StackToPc()
        {
            NES_Register.PC = (ushort)(PopFromStack() | PopFromStack() << 8);
            NES_Register.PC++;
        }

        public static void PushToStack(byte value)
        {
            ((Address)NES_Memory.Stack[NES_Register.S--]).Value = value;
        }

        public static byte PopFromStack()
        {
            return ((Address)NES_Memory.Stack[++NES_Register.S]).Value;
        }
    }
}
