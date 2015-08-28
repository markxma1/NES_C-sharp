using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NES
{
    class Instructions
    {
        #region Flags
        private int AccSize(int value)
        {
            if (Register.P.M == true)
                return (byte)value;
            return (Int16)value;
        }

        private void Negative(int value)
        {
            Register.P.Negative = value < 0;
        }

        private void Overflow(int value)
        {
            Register.P.Overflow = (Math.Abs(value) > ((Register.P.M) ? (255) : (65535)));
        }

        private void Zero(int value)
        {
            Register.P.Zero = value == 0;
        }

        private void Carry(int value)
        {
            Register.P.Carry = (value & 256) > 0;
        }
        #endregion

        #region Arithmetic and Logical Instructions
        #region Arithmetic instructions
        /// <summary>
        ///  	Add A with something and carry bit. Result is put in A. 	A=A+value
        ///  	Flags: n,v,z,c
        /// </summary>
        /// <param name="value">Immediate value</param>
        public void ADC(Int16 value)
        {
            int temp = (Register.A + value);
            Register.A = (Int16)AccSize(temp);
            Negative(temp);
            Overflow(temp);
            Zero(temp);
            Carry(temp);
        }

        /// <summary>
        ///  	Add A with something and carry bit. Result is put in A. 	A=A+adress.value
        ///  	Flags: n,v,z,c
        /// </summary>
        /// <param name="adress">address</param>
        public void ADC(ref Int16 adress)
        {
            int temp = (Register.A + adress);
            Register.A = (Int16)AccSize(temp);
            Negative(temp);
            Overflow(temp);
            Zero(temp);
            Carry(temp);
        }

        /// <summary>
        /// 	Add adress.value with something and carry bit. Result is put in adress.value. adress.value=adress.value+value
        /// 	Flags: n,v,z,c
        /// </summary>
        /// <param name="adress">address where it will be saved </param>
        /// <param name="value">Immediate value</param>
        public void ADC(ref Int16 adress, Int16 value)
        {
            int temp = (adress + value);
            adress = (Int16)AccSize(temp);
            Negative(temp);
            Overflow(temp);
            Zero(temp);
            Carry(temp);
        }

        /// <summary>
        /// Subtract something and the carry bit.
        /// Flags: n,v,z,c
        /// </summary>
        /// <param name="value"></param>
        public void SBC(Int16 value)
        {
            int temp = (Register.A - value);
            Register.A = (Int16)AccSize(temp);
            Negative(temp);
            Overflow(temp);
            Zero(temp);
            Carry(temp);
        }

        /// <summary>
        /// Subtract something and the carry bit.
        /// Flags: n,v,z,c
        /// </summary>
        /// <param name="value"></param>
        public void SBC(ref Int16 value)
        {
            int temp = (Register.A - value);
            Register.A = (Int16)AccSize(temp);
            Negative(temp);
            Overflow(temp);
            Zero(temp);
            Carry(temp);
        }

        /// <summary>
        /// Subtract something and the carry bit.
        /// Flags: n,v,z,c
        /// </summary>
        /// <param name="value"></param>
        public void SBC(ref Int16 A, Int16 value)
        {
            int temp = (Register.A - value);
            Register.A = (Int16)AccSize(temp);
            Negative(temp);
            Overflow(temp);
            Zero(temp);
            Carry(temp);
        }
        #endregion

        #region Logical instructions

        /// <summary>
        /// AND A with memory, storing result in A.
        /// Falgs:n,z
        /// </summary>
        /// <param name="value"> Immediate value </param>
        public void AND(Int16 value)
        {
            var temp = Register.A & value;
            Register.A = (Int16)AccSize(temp);
            Negative(temp);
            Zero(temp);
        }

        /// <summary>
        ///  	Exclusive or
        ///  	Flags: n,z
        /// </summary>
        /// <param name="value"></param>
        public void EOR(Int16 value)
        {
            var temp = Register.A ^ value;
            Register.A = (Int16)AccSize(temp);
            Negative(temp);
            Zero(temp);
        }

        /// <summary>
        /// OR A with memory, storing result in A.
        /// Flags:  	n,z
        /// </summary>
        /// <param name="value"></param>
        public void ORA(Int16 value)
        {
            var temp = Register.A | value;
            Register.A = (Int16)AccSize(temp);
            Negative(temp);
            Zero(temp);
        }

        /// <summary>
        ///  	Test and set bits
        ///  	Flags: Z
        /// </summary>
        /// <param name="value"></param>
        public void TSB(Int16 value)
        {
        }

        /// <summary>
        /// Test and reset bits
        /// Flags: Z
        /// </summary>
        /// <param name="value"></param>
        public void TRB(Int16 value)
        {
        }
        #endregion

        #region Shift instructions

        /// <summary>
        /// Arithmetic shift left (Last bit not Zero)
        /// Flags: c
        /// </summary>
        /// <param name="value">A or address</param>
        public void ASL(ref Int16 value)
        {
            if (Register.P.M)
                value = (Int16)((value << 1) | (value & 1));
            else
                value = (Int16)((value << 1) | (value & 1));
        }

        /// <summary>
        /// Logical shift right (Feeld with Zero)
        /// </summary>
        /// <param name="value">A or address</param>
        public void LSR(ref Int16 value)
        {
            if (Register.P.M)
                value = (Int16)((value >> 1));
            else
                value = (Int16)((value >> 1));
        }

        /// <summary>
        ///  	Rotate left
        /// </summary>
        /// <param name="value">A or address</param>
        public void ROL(ref Int16 value)
        {
            if (Register.P.M)
                value = (Int16)((value << 1) | (value >> 8));
            else
                value = (Int16)((value << 1) | (value >> 16));
        }

        /// <summary>
        /// Rotate right
        /// </summary>
        /// <param name="value">A or address</param>
        public void ROR(ref Int16 value)
        {
            if (Register.P.M)
                value = (Int16)((value >> 1) | (value << 8));
            else
                value = (Int16)((value >> 1) | (value << 16));
        }
        #endregion

        #region Test instructions
        ///TODO: 
        /// <summary>
        ///  	test bits, setting
        ///  	Flags: n,v,z (only z if in immediate mode)
        /// </summary>
        /// <param name="value"> 	immediate value or address</param>
        public void BIT(Int16 value)
        {

        }

        /// <summary>
        ///  	Compare accumulator with memory
        ///  	Flags: n,z,c
        /// </summary>
        /// <param name="value"></param>
        public void CMP(Int16 value)
        {

        }

        /// <summary>
        ///  	Compare register X with memory
        ///  	Flags: n,z,c
        /// </summary>
        /// <param name="value"></param>
        public void CPX(Int16 value)
        {

        }

        /// <summary>
        ///  	Compare register Y with memory
        ///  	Flags: n,z,c
        /// </summary>
        /// <param name="value"></param>
        public void CPY(Int16 value)
        {

        }
        #endregion

        #region Increment/Decrement
        ///TODO
        /// <summary>
        /// Decrement Accumulator
        /// Flags: n,z
        /// </summary>
        public void DEA()
        {

        }

        /// <summary>
        /// Decrement, see INC
        /// Flags: n,z
        /// </summary>
        public void DEC()
        {

        }

        /// <summary>
        /// Decrement, see INC
        /// Flags: n,z
        /// </summary>
        public void DEX()
        {

        }

        /// <summary>
        /// Decrement, see INC
        /// Flags: n,z
        /// </summary>
        public void DEY()
        {

        }

        /// <summary>
        /// Decrement, see INC
        /// Flags: n,z
        /// </summary>
        public void INA()
        {

        }

        /// <summary>
        /// Decrement, see INC
        /// Flags: n,z
        /// </summary>
        public void INC()
        {

        }

        /// <summary>
        /// Decrement, see INC
        /// Flags: n,z
        /// </summary>
        public void INX()
        {

        }

        /// <summary>
        /// Decrement, see INC
        /// Flags: n,z
        /// </summary>
        public void INY()
        {

        }
        #endregion

        #region NOP/XBA

        /// <summary>
        ///  	No operation
        /// </summary>
        public void NOP()
        {

        }

        /// <summary>
        /// Exchange bytes of accumulator
        /// Flags: n,z
        /// </summary>
        public void XBA()
        {

        }
        #endregion
        #endregion

        #region Load/Store Instructions

        /// <summary>
        ///  	Load accumulator from memory
        /// </summary>
        public void LDA()
        {

        }

        /// <summary>
        /// Load register X from memory
        /// </summary>
        public void LDX()
        {

        }

        /// <summary>
        ///  	Load register Y from memory
        /// </summary>
        public void LDY()
        {

        }

        /// <summary>
        ///  	Store accumulator in memory
        /// </summary>
        public void STA()
        {

        }

        /// <summary>
        ///  	Store register X in memory
        /// </summary>
        public void STX()
        {

        }


        /// <summary>
        /// Store register Y in memory
        /// </summary>
        public void STY()
        {

        }

        /// <summary>
        /// Store zero in memory
        /// </summary>
        ///  <param name="value">adress</param>
        public void STZ(Int16 value)
        {

        }
        #endregion

        #region Transfer Instructions

        /// <summary>
        /// Transfer Accumulator to index register X
        /// Flags: n,z
        /// </summary>
        public void TAX()
        {

        }

        /// <summary>
        /// Transfer Accumulator to index register Y
        /// Flags: n,z
        /// </summary>
        public void TAY()
        {

        }

        /// <summary>
        /// Transfer 16-bit Accumulator to Direct Page register 
        /// Flags: n,z
        /// </summary>
        public void TCD()
        {

        }

        /// <summary>
        /// Transfer 16-bit Accumulator to Stack Pointer
        /// </summary>
        public void TCS()
        {

        }

        /// <summary>
        /// Transfer Direct Page register to 16-bit Accumulator 
        /// Flags: n,z
        /// </summary>
        public void TDC()
        {

        }

        /// <summary>
        /// Transfer Stack Pointer to 16-bit Accumulator
        /// Flags: n,z
        /// </summary>
        public void TSC()
        {

        }

        /// <summary>
        /// Transfer Stack Pointer to index register X
        /// Flags: n,z
        /// </summary>
        public void TSX()
        {

        }

        /// <summary>
        /// Transfer Stack Pointer to index register Y
        /// Flags: n,z
        /// </summary>
        public void TSY()
        {

        }

        /// <summary>
        /// Transfer index register X to Accumulator
        /// Flags: n,z
        /// </summary>
        public void TXA()
        {

        }

        /// <summary>
        /// Transfer index register X to Stack Pointer
        /// Flags: n,z
        /// </summary>
        public void TXS()
        {

        }

        /// <summary>
        /// Transfer index register X to index register Y
        /// Flags: n,z
        /// </summary>
        public void TXY()
        {

        }

        /// <summary>
        /// Transfer index register Y to Accumulator
        /// Flags: n,z
        /// </summary>
        public void TYA()
        {

        }

        /// <summary>
        /// Transfer index register Y to index register X
        /// Flags: n,z
        /// </summary>
        public void TYX()
        {

        }
        #endregion

        #region Branch Instructions

        /// <summary>
        /// Branch if Carry flag is clear (C=0)
        /// </summary>
        public void BCC()
        {

        }

        /// <summary>
        /// Branch if Carry flag is set (C=1)
        /// </summary>
        public void BCS()
        {

        }

        /// <summary>
        /// Branch if not equal (Z=0)
        /// </summary>
        public void BNE()
        {

        }

        /// <summary>
        /// Branch if equal (Z=1)
        /// </summary>
        public void BEQ()
        {

        }

        /// <summary>
        /// Branch if plus (N=0)
        /// </summary>
        public void BPL()
        {

        }

        /// <summary>
        /// Branch if minus (N=1)
        /// </summary>
        public void BMI()
        {

        }

        /// <summary>
        ///  Branch if overflow flag is clear (V=0)
        /// </summary>
        public void BVC()
        {

        }

        /// <summary>
        /// Branch if overflow flag is set (V=1)
        /// </summary>
        public void BVS()
        {

        }

        /// <summary>
        /// Branch Always (unconditional)
        /// </summary>
        public void BRA()
        {

        }

        /// <summary>
        /// Branch Always Long (unconditional)
        /// </summary>
        public void BRL()
        {

        }
        #endregion

        #region Jump and call instructions

        /// <summary>
        /// Jump
        /// </summary>
        public void JMP()
        {

        }

        /// <summary>
        /// Jump long
        /// </summary>
        public void JML()
        {

        }

        /// <summary>
        /// Jump and save return address
        /// </summary>
        public void JSR()
        {

        }

        /// <summary>
        /// Jump long and save return address
        /// </summary>
        public void JSL()
        {

        }

        /// <summary>
        /// Return from subroutine
        /// </summary>
        public void RTS()
        {

        }

        /// <summary>
        /// Return long from subroutine
        /// </summary>
        public void RTL()
        {

        }
        #endregion

        #region Interrupt instructions

        /// <summary>
        /// Generate software interrupt
        /// </summary>
        public void BRK()
        {

        }

        /// <summary>
        /// Generate coprocessor interrupt
        /// </summary>
        public void COP()
        {

        }

        /// <summary>
        /// Return from interrupt
        /// </summary>
        public void RTI()
        {

        }

        /// <summary>
        /// Stop processor until RESET
        /// </summary>
        public void STP()
        {

        }

        /// <summary>
        /// Wait for hardware interrupt
        /// </summary>
        public void WAI()
        {

        }
        #endregion

        #region P Flag instructions

        /// <summary>
        /// Clear carry flag
        /// </summary>
        public void CLC()
        {
            Register.P.P = 0;
        }

        /// <summary>
        /// Select binary arithmetic
        /// </summary>
        public void CLD()
        {

        }

        /// <summary>
        /// Enable interrupt requests
        /// </summary>
        public void CLI()
        {

        }

        /// <summary>
        /// Clear overflow flag
        /// </summary>
        public void CLV()
        {
            Register.P.Overflow = false;
        }

        /// <summary>
        /// Reset status bits (for example REP #%00100000 clears the M flag)
        /// </summary>
        /// <param name="value">(for example REP #%00100000 clears the M flag)</param>
        public void REP(Int16 value)
        {
            Register.P.P = (Int16)((~value) & Register.P.P);
        }

        /// <summary>
        /// Set carry flag
        /// </summary>
        public void SEC()
        {

        }

        /// <summary>
        /// Select decimal arithmetic
        /// </summary>
        public void SED()
        {

        }

        /// <summary>
        /// Set status bits (for example SEP #%00010000 sets the X flag)
        /// </summary>
        /// <param name="value"> (for example SEP #%00010000 sets the X flag)</param>
        public void SEP(Int16 value)
        {
            Register.P.P = (Int16)(value | Register.P.P);
        }

        /// <summary>
        /// Disable interrupt requests
        /// </summary>
        public void SEI()
        {

        }

        /// <summary>
        /// Exchange carry flag with emulation flag
        /// </summary>
        public void XCE()
        {

        }
        #endregion

        #region Stack Instructions

        #region Push instructions

        /// <summary>
        /// Push Accumulator
        /// </summary>
        public void PHA()
        {

        }

        /// <summary>
        /// Push index register X
        /// </summary>
        public void PHX()
        {

        }

        /// <summary>
        /// Push index register Y
        /// </summary>
        public void PHY()
        {

        }

        /// <summary>
        /// Push direct page register
        /// </summary>
        public void PHD()
        {

        }

        /// <summary>
        /// Push data bank register
        /// </summary>
        public void PHB()
        {

        }

        /// <summary>
        /// Push Program Bank Register
        /// </summary>
        public void PHK()
        {

        }

        /// <summary>
        /// Push processor status
        /// </summary>
        public void PHP()
        {

        }

        /// <summary>
        /// Push effective address
        /// </summary>
        public void PEA()
        {

        }

        /// <summary>
        /// Push effective indirect address
        /// </summary>
        public void PEI()
        {

        }

        /// <summary>
        /// Push effective relative address
        /// </summary>
        public void PER()
        {

        }
        #endregion

        #region Pull instructions

        /// <summary>
        /// Pull Accumulator
        /// </summary>
        public void PLA()
        {

        }

        /// <summary>
        /// Pull index register X
        /// </summary>
        public void PLX()
        {

        }

        /// <summary>
        /// Pull index register Y
        /// </summary>
        public void PLY()
        {

        }

        /// <summary>
        /// Pull processor status
        /// </summary>
        public void PLP()
        {

        }

        /// <summary>
        /// Pull direct page register
        /// </summary>
        public void PLD()
        {

        }

        /// <summary>
        /// Pull data bank register
        /// </summary>
        public void PLB()
        {

        }
        #endregion
       
        #endregion
   
    
    }
}
