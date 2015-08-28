using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NES
{
    class Stack
    {

        public static void ProcessorstatusToStack(bool b, bool u)
        {
            var PStack = NES_Register.P;
            PStack.B = b;
            PStack.U = u;
            ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = PStack.P;
        }

        public static void StackToProcessorstatus()
        {
            NES_Register.P.P = (byte)(((Adress)NES_Memory.Stack[++NES_Register.S]).Value & 0xCF);
        }

        public static void PcToStack()
        {
            NES_Register.PC--;
            ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = (byte)(NES_Register.PC >> 8);
            ((Adress)NES_Memory.Stack[NES_Register.S--]).Value = (byte)NES_Register.PC;
        }

        public static void StackToPc()
        {
            NES_Register.PC = (ushort)((((Adress)NES_Memory.Stack[++NES_Register.S]).Value) | ((Adress)NES_Memory.Stack[++NES_Register.S]).Value << 8);
            NES_Register.PC++;
        }
    }
}
