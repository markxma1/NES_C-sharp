using System.Runtime.CompilerServices;
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
[assembly: InternalsVisibleTo("CPUTest")]
namespace NES
{
     class Math
    {
        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>A + M + C</returns>
        public static byte ADC(int A, int B)
        {
            var temp = A + B + Status.Carry();
            Status.NVZC(temp);
            return (byte)temp;
        }

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A - M - !C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns>A - M - !C</returns>
        public static byte SBC(int A, int B)
        {
            var temp = A - B - Status.NotCarry();
            Status.NVZC(temp);
            return (byte)temp;
        }

        /// <summary>
        /// Arithmetic Shift Left One Bit: ASL
        /// C {- 7 6 5 4 3 2 1 0 {- 0
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void ASL(ushort a)
        {
            var temp = (AddressSetup)NES_Memory.Memory[a];
            temp.Value = ASL(temp.Value);
            Status.NZ(temp.Value);
        }

        /// <summary>
        /// Arithmetic Shift Left One Bit: ASL
        /// C {- 7 6 5 4 3 2 1 0 {- 0
        /// Flags: N, Z, C
        /// Accumulator: A
        /// The Accumulator is implied as the operand, so no address needs to be specified.
        /// Example
        /// Using the ASL (Arithmetic Shift Left) instruction with no operands, the Accumulator is always the value being shifted left.
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void ASL()
        {
            NES_Register.A = ASL(NES_Register.A);
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// C {- 7 6 5 4 3 2 1 0 {- 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static byte ASL(byte value)
        {
            getASLCarry(value);
            return (byte)(value << 1);
        }

        /// <summary>
        /// C {- 7 6 5 4 3 2 1 0 {- 0
        /// </summary>
        /// <param name="value"></param>
        private static void getASLCarry(byte value)
        {
            NES_Register.P.Carry = ((value & (0x80)) > 0);
        }

        /// <summary>
        /// Logical Shift Right One Bit: LSR
        /// 0 -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void LSR(ushort a)
        {
            var temp = (AddressSetup)NES_Memory.Memory[a];
            temp.Value = LSR(temp.Value);
            Status.NZ(temp.Value);
        }

        /// <summary>
        /// Logical Shift Right One Bit: LSR
        /// 0 -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///
        /// Accumulator: A
        /// The Accumulator is implied as the operand, so no address needs to be specified.
        /// Example
        /// Using the ASL (Arithmetic Shift Left) instruction with no operands, the Accumulator is always the value being shifted left.
        ///</summary>
        public static void LSR()
        {
            NES_Register.A = LSR(NES_Register.A);
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// 0 -> 7 6 5 4 3 2 1 0 -> C
        /// </summary>
        /// <param name="value"></param>
        private static void getLSRCarry(byte value)
        {
            NES_Register.P.Carry = ((value & (0x1)) > 0);
        }

        /// <summary>
        /// 0 -> 7 6 5 4 3 2 1 0 -> C
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static byte LSR(byte value)
        {
            getLSRCarry(value);
            return (byte)(value >> 1);
        }

        /// <summary>
        /// Rotate Left One Bit: ROL
        /// C {- 7 6 5 4 3 2 1 0 {- C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void ROL(ushort a)
        {
            var temp = ((AddressSetup)NES_Memory.Memory[a]);
            temp.Value = ASL(temp.Value);
            temp.Value = (byte)(temp.Value | Status.Carry());
            Status.NZ(temp.Value);
        }

        /// <summary>
        /// Rotate Left One Bit: ROL
        /// C {- 7 6 5 4 3 2 1 0 {- C
        /// Flags: N, Z, C
        ///
        /// Accumulator: A
        /// The Accumulator is implied as the operand, so no address needs to be specified.
        /// Example
        /// Using the ASL (Arithmetic Shift Left) instruction with no operands, the Accumulator is always the value being shifted left.
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void ROL()
        {
            NES_Register.A = ASL(NES_Register.A);
            NES_Register.A = (byte)(NES_Register.A | Status.Carry());
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Rotate Right One Bit: ROR
        /// C -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void ROR(ushort a)
        {
            var temp = ((AddressSetup)NES_Memory.Memory[a]);
            temp.Value = LSR(temp.Value);
            temp.Value = (byte)(temp.Value | (Status.Carry() << 8));
            Status.NZ(temp.Value);
        }

        /// <summary>
        /// Rotate Right One Bit: ROR
        /// C -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///
        /// Accumulator: A
        /// The Accumulator is implied as the operand, so no address needs to be specified.
        /// Example
        /// Using the ASL (Arithmetic Shift Left) instruction with no operands, the Accumulator is always the value being shifted left.
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void ROR()
        {
            NES_Register.A = LSR(NES_Register.A);
            NES_Register.A = (byte)(NES_Register.A | (Status.Carry() << 8));
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void AND(byte value)
        {
            NES_Register.A = (byte)(NES_Register.A & value);
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void ORA(byte value)
        {
            NES_Register.A = (byte)(NES_Register.A | value);
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Exclusive-OR Memory with Accumulator: EOR
        /// A ^ M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">Absolute: a</param>
        public static void EOR(byte value)
        {
            NES_Register.A = (byte)(NES_Register.A ^ value);
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="value"></param>
        public static void CMP(byte value)
        {
            if (NES_Register.A < value)
            { Status.MemoryIsBigger(); }
            else if (NES_Register.A == value)
            { Status.Equal(); }
            else
            { Status.RegisterIsBigger(); }
        }

        /// <summary>
        /// Compare Memory and Index X: CPX
        /// X - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="value"></param>
        public static void CPX(byte value)
        {
            if (NES_Register.X < value)
            { Status.MemoryIsBigger(); }
            else if (NES_Register.X == value)
            { Status.Equal(); }
            else
            { Status.RegisterIsBigger(); }
        }

        /// <summary>
        /// Compare Memory with Index Y: CPY
        /// Y - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="value"></param>
        public static void CPY(byte value)
        {
            if (NES_Register.Y < value)
            { Status.MemoryIsBigger(); }
            else if (NES_Register.Y == value)
            { Status.Equal(); }
            else
            { Status.RegisterIsBigger(); }
        }

        /// <summary>
        /// Test Bits in Memory with Accumulator: BIT
        /// A & M
        /// Flags: N = M7, V = M6, Z
        ///</summary>
        /// <param name="value"></param>
        public static void BIT(byte value)
        {
            var temp = value & NES_Register.A;

            NES_Register.P.Negative = (temp & 0x80) > 0;
            NES_Register.P.Zero = temp == 0;
            NES_Register.P.Overflow = (temp & 0x40) > 0;
        }

        public static void Branch(sbyte r)
        {
            NES_Register.PC = (ushort)(NES_Register.PC + r);
        }

    }
}
