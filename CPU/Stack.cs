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
