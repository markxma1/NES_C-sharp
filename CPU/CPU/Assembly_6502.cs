﻿///   Copyright 2016 Xma1
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
///https://en.wikibooks.org/wiki/6502_Assembly And http://nesdev.com/6502.txt
namespace NES
{
    class Assembly_6502
    {
        #region Load and Store

        #region Load

        #region LDA

        /// <summary>
        /// Load Accumulator with Memory: LDA
        /// M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void LDA_AD(ushort a)
        {
            NES_Register.A = ((AddressSetup)NES_Memory.Memory[a]).Value;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Load Accumulator with Memory: LDA
        /// M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void LDA_BD(ushort ax)
        {
            NES_Register.A = ((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Load Accumulator with Memory: LDA
        /// M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void LDA_B9(ushort ay)
        {
            NES_Register.A = ((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Load Accumulator with Memory: LDA
        /// M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void LDA_A9(byte v)
        {
            NES_Register.A = v;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Load Accumulator with Memory: LDA
        /// M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void LDA_A5(byte zp)
        {
            NES_Register.A = ((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Load Accumulator with Memory: LDA
        /// M -> A
        /// Flags: N, Z
        /// </summary>
        /// <param name="zpx">
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </param>
        public static void LDA_A1(byte zpx)
        {

            NES_Register.A = ((AddressSetup)NES_Memory.Memory[Parameter.zpx1(zpx)]).Value;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Load Accumulator with Memory: LDA
        /// M -> A
        /// Flags: N, Z
        /// </summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void LDA_B5(byte zpx)
        {
            NES_Register.A = ((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Load Accumulator with Memory: LDA
        /// M -> A
        /// Flags: N, Z
        /// </summary>
        /// <param name="zpy">
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </param>
        public static void LDA_B1(byte zpy)
        {
            NES_Register.A = ((AddressSetup)NES_Memory.Memory[Parameter.zpy1(zpy)]).Value;
            Status.NZ(NES_Register.A);
        }
        #endregion

        #region LDX

        /// <summary>
        /// Load Index X with Memory: LDX
        /// M -> X
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void LDX_AE(ushort a)
        {
            NES_Register.X = ((AddressSetup)NES_Memory.Memory[a]).Value;
            Status.NZ(NES_Register.X);
        }

        /// <summary>
        /// Load Index X with Memory: LDX
        /// M -> X
        /// Flags: N, Z
        ///</summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void LDX_BE(ushort ay)
        {
            NES_Register.X = ((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value;
            Status.NZ(NES_Register.X);
        }

        /// <summary>
        /// Load Index X with Memory: LDX
        /// M -> X
        /// Flags: N, Z
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void LDX_A2(byte v)
        {
            NES_Register.X = v;
            Status.NZ(NES_Register.X);
        }

        /// <summary>
        /// Load Accumulator with Memory: LDX
        /// M -> X
        /// Flags: N, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void LDX_A6(byte zp)
        {
            NES_Register.X = ((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value;
            Status.NZ(NES_Register.X);
        }

        /// <summary>
        /// Load Index X with Memory: LDX
        /// M -> X
        /// Flags: N, Z
        /// </summary>
        /// <param name="zpy">
        /// The value in Y is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $03 in Y is added to $01 for a sum of $04. The value $E3 at address $0004 is loaded into the Accumulator.
        /// LDA $01,Y
        /// </param>
        public static void LDX_B6(byte zpy)
        {
            NES_Register.X = ((AddressSetup)NES_Memory.Memory[Parameter.zpy2(zpy)]).Value;
            Status.NZ(NES_Register.X);
        }
        #endregion

        #region LDY

        /// <summary>
        /// Load Index Y with Memory: LDY
        /// M -> Y
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the Y register.
        /// LDX $D010
        /// </param>
        public static void LDY_AC(ushort a)
        {
            NES_Register.Y = ((AddressSetup)NES_Memory.Memory[a]).Value;
            Status.NZ(NES_Register.Y);
        }

        /// <summary>
        /// Load Index Y with Memory: LDY
        /// M -> Y
        /// Flags: N, Z
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void LDY_BC(ushort ax)
        {
            NES_Register.Y = ((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value;
            Status.NZ(NES_Register.Y);
        }

        /// <summary>
        /// Load Index Y with Memory: LDY
        /// M -> Y
        /// Flags: N, Z
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void LDY_A0(byte v)
        {
            NES_Register.Y = v;
            Status.NZ(NES_Register.Y);
        }

        /// <summary>
        /// Load Index Y with Memory: LDY
        /// M -> Y
        /// Flags: N, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void LDY_A4(byte zp)
        {
            NES_Register.Y = ((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value;
            Status.NZ(NES_Register.Y);
        }

        /// <summary>
        /// Load Index Y with Memory: LDY
        /// M -> Y
        /// Flags: N, Z
        /// </summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void LDY_B4(byte zpx)
        {
            NES_Register.Y = ((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value;
            Status.NZ(NES_Register.Y);
        }
        #endregion
        #endregion

        #region Store

        #region STA

        /// <summary>
        /// Store Accumulator in Memory: STA
        /// A -> M
        /// </summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void STA_8D(ushort a)
        {
            ((AddressSetup)NES_Memory.Memory[a]).Value = NES_Register.A;
        }

        /// <summary>
        /// Store Accumulator in Memory: STA
        /// A -> M
        /// </summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void STA_9D(ushort ax)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value = NES_Register.A;
        }

        /// <summary>
        /// Store Accumulator in Memory: STA
        /// A -> M
        /// </summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void STA_99(ushort ay)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value = NES_Register.A;
        }

        /// <summary>
        /// Store Accumulator in Memory: STA
        /// A -> M
        /// </summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void STA_85(byte zp)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value = NES_Register.A;
        }

        /// <summary>
        /// Store Accumulator in Memory: STA
        /// A -> M
        /// </summary>
        /// <param name="zpx">
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </param>
        public static void STA_81(byte zpx)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.zpx1(zpx)]).Value = NES_Register.A;
        }

        /// <summary>
        /// Store Accumulator in Memory: STA
        /// A -> M
        /// </summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void STA_95(byte zpx)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value = NES_Register.A;
        }

        /// <summary>
        /// Store Accumulator in Memory: STA
        /// A -> M
        /// </summary>
        /// <param name="zpy">
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </param>
        public static void STA_91(byte zpy)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.zpy1(zpy)]).Value = NES_Register.A;
        }
        #endregion

        #region STX

        /// <summary>
        /// Store Index X in Memory: STX
        /// X -> M
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void STX_8E(ushort a)
        {
            ((AddressSetup)NES_Memory.Memory[a]).Value = NES_Register.X;
        }

        /// <summary>
        /// Store Index X in Memory: STX
        /// X -> M
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void STX_86(byte zp)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value = NES_Register.X;
        }

        /// <summary>
        /// Store Index X in Memory: STX
        /// X -> M
        ///</summary>
        /// <param name="zpy">
        /// The value in Y is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $03 in Y is added to $01 for a sum of $04. The value $E3 at address $0004 is loaded into the Accumulator.
        /// LDA $01,Y
        /// </param>
        public static void STX_96(byte zpy)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.zpy2(zpy)]).Value = NES_Register.X;
        }
        #endregion

        #region STY

        /// <summary>
        /// Store Index Y in Memory: STY
        /// Y -> M
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the Y register.
        /// LDX $D010
        /// </param>
        public static void STY_8C(ushort a)
        {
            ((AddressSetup)NES_Memory.Memory[a]).Value = NES_Register.Y;
        }

        /// <summary>
        /// Store Index Y in Memory: STY
        /// Y -> M
        ///</summary>
        /// <param name="zp">
        /// Immediate: #
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void STY_84(byte zp)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value = NES_Register.Y;
        }

        /// <summary>
        /// Store Index Y in Memory: STY
        /// Y -> M
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void STY_94(byte zpx)
        {
            ((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value = NES_Register.Y;
        }
        #endregion
        #endregion
        #endregion

        #region Arithmetic

        #region ADC

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void ADC_6D(ushort a)
        {
            NES_Register.A = Math.ADC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void ADC_7D(ushort ax)
        {
            NES_Register.A = Math.ADC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value);
        }

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void ADC_79(ushort ay)
        {
            NES_Register.A = Math.ADC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value);
        }

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void ADC_69(byte v)
        {
            NES_Register.A = Math.ADC(NES_Register.A, v);
        }

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void ADC_65(byte zp)
        {
            NES_Register.A = Math.ADC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[zp]).Value);
        }

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </param>
        public static void ADC_61(byte zpx)
        {
            NES_Register.A = Math.ADC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.zpx1(zpx)]).Value);
        }

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void ADC_75(byte zpx)
        {
            NES_Register.A = Math.ADC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.zpy2(zpx)]).Value);
        }

        /// <summary>
        /// Add Memory to Accumulator with Carry: ADC
        /// A + M + C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zpy">
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </param>
        public static void ADC_71(byte zpy)
        {
            NES_Register.A = Math.ADC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.zpy1(zpy)]).Value);
        }
        #endregion

        #region SBC

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void SBC_ED(ushort a)
        {
            NES_Register.A = Math.SBC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void SBC_FD(ushort ax)
        {
            NES_Register.A = Math.SBC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.ay(ax)]).Value);
        }

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void SBC_F9(ushort ay)
        {
            NES_Register.A = Math.SBC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value);
        }

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void SBC_E9(byte v)
        {
            NES_Register.A = Math.SBC(NES_Register.A, v);
        }

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void SBC_E5(byte zp)
        {
            NES_Register.A = Math.SBC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </param>
        public static void SBC_E1(byte zpx)
        {
            NES_Register.A = Math.SBC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.zpx1(zpx)]).Value);
        }

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void SBC_F5(byte zpx)
        {
            NES_Register.A = Math.SBC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value);
        }

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zpy">
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </param>
        public static void SBC_F1(byte zpy)
        {
            NES_Register.A = Math.SBC(NES_Register.A, ((AddressSetup)NES_Memory.Memory[Parameter.zpy1(zpy)]).Value);
        }
        #endregion
        #endregion

        #region Increment and Decrement

        #region INC

        /// <summary>
        /// Increment Memory by One: INC
        /// M + 1 -> M
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void INC_EE(ushort a)
        {
            Status.NZ(++((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// Increment Memory by One: INC
        /// M + 1 -> M
        /// Flags: N, Z
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void INC_FE(ushort ax)
        {
            Status.NZ(++((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value);
        }

        /// <summary>
        /// Increment Memory by One: INC
        /// M + 1 -> M
        /// Flags: N, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void INC_E6(byte zp)
        {
            Status.NZ(++((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }

        /// <summary>
        /// Subtract Memory from Accumulator with Borrow: SBC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void INC_F6(byte zpx)
        {
            Status.NZ(++((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value);
        }
        #endregion

        /// <summary>
        /// Increment Memory by One: INX
        /// X + 1 -> X
        /// Flags: N, Z
        ///</summary>
        public static void INX_E8()
        {
            Status.NZ(++NES_Register.X);
        }

        /// <summary>
        /// Increment Memory by One: INY
        /// Y + 1 -> Y
        /// Flags: N, Z
        ///</summary>
        public static void INY_C8()
        {
            Status.NZ(++NES_Register.Y);
        }

        #region DEC

        /// <summary>
        /// Decrement Memory by One: DEC
        /// M - 1 -> M
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void DEC_CE(ushort a)
        {
            Status.NZ(--((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// Decrement Memory by One: DEC
        /// M - 1 -> M
        /// Flags: N, Z
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void DEC_DE(ushort ax)
        {
            Status.NZ(--((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value);
        }

        /// <summary>
        /// Decrement Memory by One: DEC
        /// M - 1 -> M
        /// Flags: N, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void DEC_C6(byte zp)
        {
            Status.NZ(--((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }

        /// <summary>
        /// Decrement Memory by One: DEC
        /// A - M - ~C -> A
        /// Flags: N, V, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void DEC_D6(byte zpx)
        {
            Status.NZ(--((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value);
        }
        #endregion

        /// <summary>
        /// Decrement Index Y by One: DEY
        /// X - 1 -> X
        /// Flags: N, Z
        ///</summary>
        public static void DEX_CA()
        {
            Status.NZ(--NES_Register.X);
        }

        /// <summary>
        /// Decrement Index Y by One: DEY
        /// Y - 1 -> Y
        /// Flags: N, Z
        ///</summary>
        public static void DEY_88()
        {
            Status.NZ(--NES_Register.Y);
        }
        #endregion

        #region Shift and Rotate

        #region Shift
        #region ASL

        /// <summary>
        /// Arithmetic Shift Left One Bit: ASL
        /// C {- 7 6 5 4 3 2 1 0 {- 0
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void ASL_0E(ushort a)
        {
            Math.ASL(a);
        }

        /// <summary>
        /// Arithmetic Shift Left One Bit: ASL
        /// C {- 7 6 5 4 3 2 1 0 {- 0
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void ASL_1E(ushort ax)
        {
            Math.ASL(Parameter.ax(ax));
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
        public static void ASL_0A()
        {
            Math.ASL();
        }

        /// <summary>
        /// Arithmetic Shift Left One Bit: ASL
        /// C {- 7 6 5 4 3 2 1 0 {- 0
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void ASL_06(byte zp)
        {
            Math.ASL(Parameter.zp(zp));
        }

        /// <summary>
        /// Arithmetic Shift Left One Bit: ASL
        /// C {- 7 6 5 4 3 2 1 0 {- 0
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void ASL_16(byte zpx)
        {
            Math.ASL(Parameter.zpx2(zpx));
        }
        #endregion

        #region LSR

        /// <summary>
        /// Logical Shift Right One Bit: LSR
        /// 0 -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void LSR_4E(ushort a)
        {
            Math.LSR(a);
        }

        /// <summary>
        /// Logical Shift Right One Bit: LSR
        /// 0 -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void LSR_5E(ushort ax)
        {
            Math.LSR(Parameter.ax(ax));
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
        public static void LSR_4A()
        {
            Math.LSR();
        }

        /// <summary>
        /// Logical Shift Right One Bit: LSR
        /// 0 -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void LSR_46(byte zp)
        {
            Math.LSR(Parameter.zp(zp));
        }

        /// <summary>
        /// Logical Shift Right One Bit: LSR
        /// 0 -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void LSR_56(byte zpx)
        {
            Math.LSR(Parameter.zpx2(zpx));
        }
        #endregion
        #endregion

        #region Rotate

        #region ROL

        /// <summary>
        /// Rotate Left One Bit: ROL
        /// C {- 7 6 5 4 3 2 1 0 {- C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void ROL_2E(ushort a)
        {
            Math.ROL(a);
        }

        /// <summary>
        /// Rotate Left One Bit: ROL
        /// C {- 7 6 5 4 3 2 1 0 {- C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void ROL_3E(ushort ax)
        {
            Math.ROL(Parameter.ax(ax));
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
        public static void ROL_2A()
        {
            Math.ROL();
        }

        /// <summary>
        /// Rotate Left One Bit: ROL
        /// C {- 7 6 5 4 3 2 1 0 {- C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void ROL_26(byte zp)
        {
            Math.ROL(Parameter.zp(zp));
        }

        /// <summary>
        /// Rotate Left One Bit: ROL
        /// C {- 7 6 5 4 3 2 1 0 {- C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void ROL_36(byte zpx)
        {
            Math.ROL(Parameter.zpx2(zpx));
        }
        #endregion

        #region ROR

        /// <summary>
        /// Rotate Right One Bit: ROR
        /// C -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void ROR_6E(ushort a)
        {
            Math.ROR(a);
        }

        /// <summary>
        /// Rotate Right One Bit: ROR
        /// C -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void ROR_7E(ushort ax)
        {
            Math.ROR(Parameter.ax(ax));
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
        public static void ROR_6A()
        {
            Math.ROR();
        }

        /// <summary>
        /// Rotate Right One Bit: ROR
        /// C -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void ROR_66(byte zp)
        {
            Math.ROR(zp);
        }

        /// <summary>
        /// Rotate Right One Bit: ROR
        /// C -> 7 6 5 4 3 2 1 0 -> C
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void ROR_76(byte zpx)
        {
            Math.ROR(Parameter.zpy2(zpx));
        }
        #endregion
        #endregion
        #endregion

        #region Logic

        #region AND

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void AND_2D(ushort a)
        {
            Math.AND(((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void AND_3D(ushort ax)
        {
            Math.AND(((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value);
        }

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void AND_39(ushort ay)
        {
            Math.AND(((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value);
        }

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void AND_29(byte v)
        {
            Math.AND(v);
        }

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void AND_25(byte zp)
        {
            Math.AND(((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </param>
        public static void AND_21(byte zpx)
        {
            Math.AND(((AddressSetup)NES_Memory.Memory[Parameter.zpx1(zpx)]).Value);
        }

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void AND_35(byte zpx)
        {
            Math.AND(((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value);
        }

        /// <summary>
        /// AND Memory with Accumulator: AND
        /// A & M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpy">
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </param>
        public static void AND_31(byte zpy)
        {
            Math.AND(((AddressSetup)NES_Memory.Memory[Parameter.zpy1(zpy)]).Value);
        }
        #endregion

        #region ORA

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void ORA_0D(ushort a)
        {
            Math.ORA(((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void ORA_1D(ushort ax)
        {
            Math.ORA(((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void ORA_19(ushort ay)
        {
            Math.ORA(((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void ORA_09(byte v)
        {
            Math.ORA(v);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void ORA_05(byte zp)
        {
            Math.ORA(((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </param>
        public static void ORA_01(byte zpx)
        {
            Math.ORA(((AddressSetup)NES_Memory.Memory[Parameter.zpx1(zpx)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void ORA_15(byte zpx)
        {
            Math.ORA(((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpy">
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </param>
        public static void ORA_11(byte zpy)
        {
            Math.ORA(((AddressSetup)NES_Memory.Memory[Parameter.zpy1(zpy)]).Value);
        }
        #endregion

        #region EOR

        /// <summary>
        /// Exclusive-OR Memory with Accumulator: EOR
        /// A ^ M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void EOR_4D(ushort a)
        {
            Math.EOR(((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void EOR_5D(ushort ax)
        {
            Math.EOR(((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void EOR_59(ushort ay)
        {
            Math.EOR(((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void EOR_49(byte v)
        {
            Math.EOR(v);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void EOR_45(byte zp)
        {
            Math.EOR(((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </param>
        public static void EOR_41(byte zpx)
        {
            Math.EOR(((AddressSetup)NES_Memory.Memory[Parameter.zpx1(zpx)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void EOR_55(byte zpx)
        {
            Math.EOR(((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value);
        }

        /// <summary>
        /// OR Memory with Accumulator: ORA
        /// A | M -> A
        /// Flags: N, Z
        ///</summary>
        /// <param name="zpy">
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </param>
        public static void EOR_51(byte zpy)
        {
            Math.EOR(((AddressSetup)NES_Memory.Memory[Parameter.zpy1(zpy)]).Value);
        }
        #endregion
        #endregion

        #region Compare and Test Bit

        //For all Compare instructions:
        //
        //Condition 	               | N | Z | C |
        //Register < Memory   | 1 | 0  | 0 |
        //Register = Memory   | 0 | 1  | 1 |
        //Register > Memory   | 0 | 0 | 1 |

        #region CMP

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void CMP_CD(ushort a)
        {
            Math.CMP(((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="ax">
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </param>
        public static void CMP_DD(ushort ax)
        {
            Math.CMP(((AddressSetup)NES_Memory.Memory[Parameter.ax(ax)]).Value);
        }

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="ay">
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </param>
        public static void CMP_D9(ushort ay)
        {
            Math.CMP(((AddressSetup)NES_Memory.Memory[Parameter.ay(ay)]).Value);
        }

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void CMP_C9(byte v)
        {
            Math.CMP(v);
        }

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void CMP_C5(byte zp)
        {
            Math.CMP(((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </param>
        public static void CMP_C1(byte zpx)
        {
            Math.CMP(((AddressSetup)NES_Memory.Memory[Parameter.zpx1(zpx)]).Value);
        }

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zpx">
        /// Zero Page Indexed with X: zp,x
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </param>
        public static void CMP_D5(byte zpx)
        {
            Math.CMP(((AddressSetup)NES_Memory.Memory[Parameter.zpx2(zpx)]).Value);
        }

        /// <summary>
        /// Compare Memory and Accumulator: CMP
        /// A - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zpy">
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </param>
        public static void CMP_D1(byte zpy)
        {
            Math.CMP(((AddressSetup)NES_Memory.Memory[Parameter.zpy1(zpy)]).Value);
        }
        #endregion

        #region CPX

        /// <summary>
        /// Compare Memory and Index X: CPX
        /// X - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void CPX_EC(ushort a)
        {
            Math.CPX(((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// Compare Memory and Index X: CPX
        /// X - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void CPX_E0(byte v)
        {
            Math.CPX(v);
        }

        /// <summary>
        /// Compare Memory and Index X: CPX
        /// X - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void CPX_E4(byte zp)
        {
            Math.CPX(((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }
        #endregion

        #region CPY

        /// <summary>
        /// Compare Memory with Index Y: CPY
        /// Y - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void CPY_CC(ushort a)
        {
            Math.CPY(((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// Compare Memory with Index Y: CPY
        /// Y - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void CPY_C0(byte v)
        {
            Math.CPY(v);
        }

        /// <summary>
        /// Compare Memory with Index Y: CPY
        /// Y - M
        /// Flags: N, Z, C
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void CPY_C4(byte zp)
        {
            Math.CPY(((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }
        #endregion

        #region BIT

        /// <summary>
        /// Test Bits in Memory with Accumulator: BIT
        /// A & M
        /// Flags: N = M7, V = M6, Z
        ///</summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void BIT_2C(ushort a)
        {
            Math.BIT(((AddressSetup)NES_Memory.Memory[a]).Value);
        }

        /// <summary>
        /// Test Bits in Memory with Accumulator: BIT
        /// A & M
        /// Flags: N = M7, V = M6, Z
        ///</summary>
        /// <param name="v">
        /// Immediate: #(v)
        /// The operand is used directly to perform the computation.
        /// Example
        /// The value $22 is loaded into the Accumulator.
        /// LDA #$22
        /// </param>
        public static void BIT_89(byte v)
        {
            Math.BIT(v);
        }

        /// <summary>
        /// Test Bits in Memory with Accumulator: BIT
        /// A & M
        /// Flags: N = M7, V = M6, Z
        ///</summary>
        /// <param name="zp">
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </param>
        public static void BIT_24(byte zp)
        {
            Math.BIT(((AddressSetup)NES_Memory.Memory[Parameter.zp(zp)]).Value);
        }
        #endregion
        #endregion

        #region Branch

        /// <summary>
        /// Branch on Carry Clear: BCC
        /// Branch if C = 0
        /// </summary>
        /// <param name="r">
        /// Relative: r
        /// The offset specified is added to the current address stored in the Program Counter (PC). Offsets can range from -128 to +127.
        /// Example
        /// The offset $2D is added to the address in the Program Counter (say $C100). The destination of the branch (if taken) will be $C12D.
        /// BPL $2D
        /// </param>
        public static void BCC_90(sbyte r)
        {
            if (!NES_Register.P.Carry)
                Math.Branch(r);
        }

        /// <summary>
        /// Branch on Carry Set: BCS
        /// Branch if C = 1
        /// </summary>
        /// <param name="r">
        /// Relative: r
        /// The offset specified is added to the current address stored in the Program Counter (PC). Offsets can range from -128 to +127.
        /// Example
        /// The offset $2D is added to the address in the Program Counter (say $C100). The destination of the branch (if taken) will be $C12D.
        /// BPL $2D
        /// </param>
        public static void BCS_B0(sbyte r)
        {
            if (NES_Register.P.Carry)
                Math.Branch(r);
        }

        /// <summary>
        /// Branch on Result Zero: BEQ
        /// Branch if Z = 1
        /// </summary>
        /// <param name="r">
        /// Relative: r
        /// The offset specified is added to the current address stored in the Program Counter (PC). Offsets can range from -128 to +127.
        /// Example
        /// The offset $2D is added to the address in the Program Counter (say $C100). The destination of the branch (if taken) will be $C12D.
        /// BPL $2D
        /// </param>
        public static void BEQ_F0(sbyte r)
        {
            if (NES_Register.P.Zero)
                Math.Branch(r);
        }

        /// <summary>
        /// Branch on Result Minus: BMI
        /// Branch if N = 1
        /// </summary>
        /// <param name="r">
        /// Relative: r
        /// The offset specified is added to the current address stored in the Program Counter (PC). Offsets can range from -128 to +127.
        /// Example
        /// The offset $2D is added to the address in the Program Counter (say $C100). The destination of the branch (if taken) will be $C12D.
        /// BPL $2D
        /// </param>
        public static void BMI_30(sbyte r)
        {
            if (NES_Register.P.Negative)
                Math.Branch(r);
        }

        /// <summary>
        /// Branch on Result not Zero: BNE
        /// Branch if Z = 0
        /// </summary>
        /// <param name="r">
        /// Relative: r
        /// The offset specified is added to the current address stored in the Program Counter (PC). Offsets can range from -128 to +127.
        /// Example
        /// The offset $2D is added to the address in the Program Counter (say $C100). The destination of the branch (if taken) will be $C12D.
        /// BPL $2D
        /// </param>
        public static void BNE_D0(sbyte r)
        {
            if (!NES_Register.P.Zero)
                Math.Branch(r);
        }

        /// <summary>
        /// Branch on Result Plus: BPL
        /// Branch if N = 0
        /// </summary>
        /// <param name="r">
        /// Relative: r
        /// The offset specified is added to the current address stored in the Program Counter (PC). Offsets can range from -128 to +127.
        /// Example
        /// The offset $2D is added to the address in the Program Counter (say $C100). The destination of the branch (if taken) will be $C12D.
        /// BPL $2D
        /// </param>
        public static void BPL_10(sbyte r)
        {
            if (!NES_Register.P.Negative)
                Math.Branch(r);
        }

        /// <summary>
        /// Branch on Overflow Clear: BVC
        /// Branch if V = 0
        /// </summary>
        /// <param name="r">
        /// Relative: r
        /// The offset specified is added to the current address stored in the Program Counter (PC). Offsets can range from -128 to +127.
        /// Example
        /// The offset $2D is added to the address in the Program Counter (say $C100). The destination of the branch (if taken) will be $C12D.
        /// BPL $2D
        /// </param>
        public static void BVC_50(sbyte r)
        {
            if (!NES_Register.P.Carry)
                Math.Branch(r);
        }

        /// <summary>
        /// Branch on Overflow Set: BVS
        /// Branch if V = 1
        /// </summary>
        /// <param name="r">
        /// Relative: r
        /// The offset specified is added to the current address stored in the Program Counter (PC). Offsets can range from -128 to +127.
        /// Example
        /// The offset $2D is added to the address in the Program Counter (say $C100). The destination of the branch (if taken) will be $C12D.
        /// BPL $2D
        /// </param>
        public static void BVS_70(sbyte r)
        {
            if (NES_Register.P.Carry)
                Math.Branch(r);
        }
        #endregion

        #region Transfer

        /// <summary>
        /// Transfer Accumulator to Index X: TAX
        /// A -> X
        /// Flags: N, Z
        /// </summary>
        public static void TAX_AA()
        {
            NES_Register.X = NES_Register.A;
            Status.NZ(NES_Register.X);
        }

        /// <summary>
        /// Transfer Index X to Accumulator: TXA
        /// X -> A
        /// Flags: N, Z
        /// </summary>
        public static void TXA_8A()
        {
            NES_Register.A = NES_Register.X;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Transfer Accumulator to Index Y: TAY
        /// A -> Y
        /// Flags: N, Z
        /// </summary>
        public static void TAY_A8()
        {
            NES_Register.Y = NES_Register.A;
            Status.NZ(NES_Register.A);
        }

        /// <summary>
        /// Pull Accumulator from Stack: TYA
        /// Y -> A
        /// Flags: N, Z
        /// </summary>
        public static void TYA_98()
        {
            NES_Register.A = NES_Register.Y;
            Status.NZ(NES_Register.Y);
        }

        /// <summary>
        /// Transfer Stack Pointer to Index X: TSX
        /// S -> X
        /// Flags: N, Z
        /// </summary>
        public static void TSX_BA()
        {
            NES_Register.X = NES_Register.S;
            Status.NZ(NES_Register.S);
        }

        /// <summary>
        /// Transfer Index X to Stack Pointer: TXS
        /// X -> S
        /// </summary>
        public static void TXS_9A()
        {
            NES_Register.S = NES_Register.X;
        }

        #endregion

        #region Stack

        /// <summary>
        /// Push Accumulator on Stack: PHA
        /// A -> S
        /// </summary>
        public static void PHA_48()
        {
             Stack.PushToStack(NES_Register.A);
        }

        /// <summary>
        /// Pull Accumulator from Stack: PLA
        /// S -> A
        /// Flags: N, Z
        /// </summary>
        public static void PLA_68()
        {
            NES_Register.A = Stack.PopFromStack();
        }

        /// <summary>
        /// Push Processor Status on Stack: PHP
        /// P -> S
        /// The processor status is stored as a single byte with the following flags bits from high to low: NV-BDIZC.
        /// </summary>
        public static void PHP_08()
        {
            Stack.PushToStack((byte)(NES_Register.P.P | 0x30));
        }

        /// <summary>
        /// Pull Processor Status from Stack: PLP
        /// S -> P
        /// Setting the processor status from the stack is the only way to clear the B (Break) flag.
        /// Flags: ALL
        /// </summary>
        public static void PLP_28()
        {
            Stack.StackToProcessorstatus();
        }
        #endregion

        #region Subroutines and Jump

        #region JMP

        /// <summary>
        /// Jump to New Location: JMP
        /// Jump to new location
        /// </summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void JMP_4C(ushort a)
        {
            NES_Register.PC = a;
        }

        /// <summary>
        /// Jump to New Location: JMP
        /// Jump to new location
        /// </summary>
        /// <param name="a">
        /// Zero Page Indexed Indirect: (a)
        /// The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// a = $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15)
        /// </param>
        public static void JMP_6C(ushort a)
        {
            NES_Register.PC = Parameter.MemoryValueToAdress(a);
        }
        #endregion

        /// <summary>
        /// Jump to New Location Saving Return Address: JSR
        /// Jump to Subroutine
        /// </summary>
        /// <param name="a">
        /// Absolute: a
        /// A full 16-bit address is specified and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $D010 is loaded into the X register.
        /// LDX $D010
        /// </param>
        public static void JSR_20(ushort a)
        {
            Stack.PcToStack();
            NES_Register.PC = a;
        }

        /// <summary>
        /// Return from Subroutine: RTS
        /// Return from Subroutine
        /// </summary>
        public static void RTS_60()
        {
            Stack.StackToPc();
        }

        /// <summary>
        /// Return from Interrupt: RTI
        /// Return from Interrupt
        /// Flags: all
        /// </summary>
        public static void RTI_40()
        {
            Stack.StackToPc();
            Stack.StackToProcessorstatus();
        }
        #endregion

        #region Set and Clear

        #region Set
        /// <summary>
        /// Set Carry Flag: SEC
        /// 1 -> C
        /// Flags: C = 1
        /// </summary>
        public static void SEC_38() { NES_Register.P.Carry = true; }

        /// <summary>
        /// Set Carry Flag: SEC
        /// 1 -> D
        /// Flags: D = 1
        /// </summary>
        public static void SED_F8() { NES_Register.P.Decimal = true; }

        /// <summary>
        /// Set Interrupt Disable Status: SEI
        /// 1 -> I
        /// Flags:  I = 1
        /// </summary>
        public static void SEI_78() { NES_Register.P.Interrupt = true; }
        #endregion

        #region Clear
        /// <summary>
        /// Clear Carry Flag: CLC
        /// 0 -> C
        /// Flags: C = 0
        /// </summary>
        public static void CLC_18() { NES_Register.P.Carry = false; }

        /// <summary>
        /// Clear Decimal Mode: CLD
        /// 0 -> D
        /// Flags: D = 0
        /// </summary>
        public static void CLD_D8() { NES_Register.P.Decimal = false; }

        /// <summary>
        /// Clear Interrupt Disable Status: CLI
        /// 0 -> I
        /// Flags:  I = 0
        /// </summary>
        public static void CLI_58() { NES_Register.P.Interrupt = false; }

        /// <summary>
        /// Clear Overflow Flag: CLV
        /// 0 -> V
        /// Flags:  V = 0
        /// </summary>
        public static void CLV_B8() { NES_Register.P.Overflow = false; }
        #endregion
        #endregion

        #region Miscellaneous

        /// <summary>
        /// No Operation: NOP
        /// No Operation
        /// </summary>
        public static void NOP_EA()
        { }

        /// <summary>
        /// Break: BRK
        /// Force an Interrupt
        /// Flags: B = 1, I = 1
        /// </summary>
        public static void BRK_00()
        {
            Interrupt.BRK = true;
        }
        #endregion
    }
}
