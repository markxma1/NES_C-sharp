using System;
using System.Collections.Generic;

namespace NES
{
    class AssemblyList
    {
        #region Init

        static private List<Action> Assembly = new List<Action>();
        static public List<string> debug = new List<string>();

        public List<Action> assembly { get { return Assembly; } }

        public AssemblyList()
        {
            CreateAdressMemory();
            NoImputToList();
            U8BitToList();
            BranchToList();
            U16ByteToList();
        } 

        private static void CreateAdressMemory()
        {
            for (int i = 0; i < 0xFF; i++)
            {
                Assembly.Add(delegate () { throw new NoAssemby(); });                
            }
        }

        #endregion

        private static void Debug(ushort PC, int b)
        {
            debug.Add("0X"+PC.ToString("X") + ": 0X" + b.ToString("X"));
            if (debug.Count > 20)
                debug.RemoveAt(0);
        }

        #region Delegates
        public delegate void Func();
        private delegate void Func8(byte b);
        private delegate void Funcs8(sbyte b);
        private delegate void Func16(ushort b);

        private void NoInput(Func func)
        {
            var PC = NES_Register.PC;
            NES_Register.PC++;
            Debug(PC, 0);
            func();
        }

        private void Input8Byte(Func8 func)
        {
            var PC = NES_Register.PC;
            byte b = ((AddressSetup)NES_Memory.Memory[++NES_Register.PC]).Value;
            NES_Register.PC++;
            Debug(PC, b);
            func(b);
        }

        private void Inputs8Byte(Funcs8 func)
        {
            var PC = NES_Register.PC;
            sbyte b = (sbyte)((AddressSetup)NES_Memory.Memory[++NES_Register.PC]).Value;
            NES_Register.PC++;
            Debug(PC, b);
            func(b);
        }

        private void Input16Byte(Func16 func)
        {
            var PC = NES_Register.PC;
            ushort b = ((AddressSetup)NES_Memory.Memory[++NES_Register.PC]).Value;
            b = (ushort)((((AddressSetup)NES_Memory.Memory[++NES_Register.PC]).Value << 8) | (b & 0xFF));
            NES_Register.PC++;
            Debug(PC, b);
            func(b);
        }
        #endregion

        #region Branch

        /// <summary>
        /// Functions with r (+-127) byte imput. 
        /// 
        /// <example>
        /// {---------
        ///                 | -r
        ///                 |
        /// PC-------
        ///                 |
        ///                 |+r
        /// {---------
        /// </example>
        /// </summary>
        private void BranchToList()
        {
            Assembly[0x90] = delegate () { Inputs8Byte(Assembly_6502.BCC_90); };
            Assembly[0xB0] = delegate () { Inputs8Byte(Assembly_6502.BCS_B0); };
            Assembly[0xF0] = delegate () { Inputs8Byte(Assembly_6502.BEQ_F0); };
            Assembly[0x30] = delegate () { Inputs8Byte(Assembly_6502.BMI_30); };
            Assembly[0xD0] = delegate () { Inputs8Byte(Assembly_6502.BNE_D0); };
            Assembly[0x10] = delegate () { Inputs8Byte(Assembly_6502.BPL_10); };
            Assembly[0x50] = delegate () { Inputs8Byte(Assembly_6502.BVC_50); };
            Assembly[0x70] = delegate () { Inputs8Byte(Assembly_6502.BVS_70); };
        }
        #endregion

        #region U8

        /// <summary>
        /// 8 Bit input it can be a Number or a Zero address like (0x0022)
        /// </summary>
        private void U8BitToList()
        {
            LoadU8();
            StoreU8();
            ArithmeticU8();
            ShiftRotateU8();
            Logic();
            CompareTestBit();
        }

        private void CompareTestBit()
        {
            CMPU8();
            CPXU8();
            CPYU8();
            BITU8();
        }

        private void Logic()
        {
            ANDU8();
            ORAU8();
            EORU8();
        }

        private void ShiftRotateU8()
        {
            ASLU8();
            LSRU8();
            ROLU8();
            RORU8();
        }

        private void ArithmeticU8()
        {
            ADCU8();
            SBCU8();
            INCU8();
            DECU8();
        }

        private void StoreU8()
        {
            STAU8();
            STXU8();
            STYU8();
        }

        private void LoadU8()
        {
            LDAU8();
            LDXU8();
            LDYU8();
        }

        private void BITU8()
        {
            Assembly[0x89] = delegate () { Input8Byte(Assembly_6502.BIT_89); };
            Assembly[0x24] = delegate () { Input8Byte(Assembly_6502.BIT_24); };
        }

        private void CPYU8()
        {
            Assembly[0xC0] = delegate () { Input8Byte(Assembly_6502.CPY_C0); };
            Assembly[0xC4] = delegate () { Input8Byte(Assembly_6502.CPY_C4); };
        }

        private void CPXU8()
        {
            Assembly[0xE0] = delegate () { Input8Byte(Assembly_6502.CPX_E0); };
            Assembly[0xE4] = delegate () { Input8Byte(Assembly_6502.CPX_E4); };
        }

        private void CMPU8()
        {
            Assembly[0xC5] = delegate () { Input8Byte(Assembly_6502.CMP_C5); };
            Assembly[0xC1] = delegate () { Input8Byte(Assembly_6502.CMP_C1); };
            Assembly[0xC9] = delegate () { Input8Byte(Assembly_6502.CMP_C9); };
            Assembly[0xD5] = delegate () { Input8Byte(Assembly_6502.CMP_D5); };
            Assembly[0xD1] = delegate () { Input8Byte(Assembly_6502.CMP_D1); };
        }

        private void EORU8()
        {
            Assembly[0x45] = delegate () { Input8Byte(Assembly_6502.EOR_45); };
            Assembly[0x41] = delegate () { Input8Byte(Assembly_6502.EOR_41); };
            Assembly[0x49] = delegate () { Input8Byte(Assembly_6502.EOR_49); }; 
            Assembly[0x55] = delegate () { Input8Byte(Assembly_6502.EOR_55); };
            Assembly[0x51] = delegate () { Input8Byte(Assembly_6502.EOR_51); };
        }

        private void ORAU8()
        {
            Assembly[0x09] = delegate () { Input8Byte(Assembly_6502.ORA_09); };
            Assembly[0x05] = delegate () { Input8Byte(Assembly_6502.ORA_05); }; 
             Assembly[0x21] = delegate () { Input8Byte(Assembly_6502.ORA_01); };
            Assembly[0x35] = delegate () { Input8Byte(Assembly_6502.ORA_15); };
            Assembly[0x31] = delegate () { Input8Byte(Assembly_6502.ORA_11); };
        }

        private void ANDU8()
        {
            Assembly[0x25] = delegate () { Input8Byte(Assembly_6502.AND_25); };
            Assembly[0x21] = delegate () { Input8Byte(Assembly_6502.AND_21); };
            Assembly[0x29] = delegate () { Input8Byte(Assembly_6502.AND_29); };
            Assembly[0x35] = delegate () { Input8Byte(Assembly_6502.AND_35); };
            Assembly[0x31] = delegate () { Input8Byte(Assembly_6502.AND_31); };
        }

        private void RORU8()
        {
            Assembly[0x66] = delegate () { Input8Byte(Assembly_6502.ROR_66); };
            Assembly[0x76] = delegate () { Input8Byte(Assembly_6502.ROR_76); };
        }

        private void ROLU8()
        {
            Assembly[0x26] = delegate () { Input8Byte(Assembly_6502.ROL_26); };
            Assembly[0x36] = delegate () { Input8Byte(Assembly_6502.ROL_36); };
        }

        private void LSRU8()
        {
            Assembly[0x46] = delegate () { Input8Byte(Assembly_6502.LSR_46); };
            Assembly[0x56] = delegate () { Input8Byte(Assembly_6502.LSR_56); };
        }

        private void ASLU8()
        {
            Assembly[0x06] = delegate () { Input8Byte(Assembly_6502.ASL_06); };
            Assembly[0x16] = delegate () { Input8Byte(Assembly_6502.ASL_16); };
        }

        private void DECU8()
        {
            Assembly[0xC6] = delegate () { Input8Byte(Assembly_6502.DEC_C6); };
            Assembly[0xD6] = delegate () { Input8Byte(Assembly_6502.DEC_D6); };
        }

        private void INCU8()
        {
            Assembly[0xE6] = delegate () { Input8Byte(Assembly_6502.INC_E6); };
            Assembly[0xF6] = delegate () { Input8Byte(Assembly_6502.INC_F6); };
        }

        private void SBCU8()
        {
            Assembly[0xE9] = delegate () { Input8Byte(Assembly_6502.SBC_E9); };
            Assembly[0xE5] = delegate () { Input8Byte(Assembly_6502.SBC_E5); };
            Assembly[0xE1] = delegate () { Input8Byte(Assembly_6502.SBC_E1); };
            Assembly[0xF5] = delegate () { Input8Byte(Assembly_6502.SBC_F5); };
            Assembly[0xF1] = delegate () { Input8Byte(Assembly_6502.SBC_F1); };
        }

        private void ADCU8()
        {
            Assembly[0x69] = delegate () { Input8Byte(Assembly_6502.ADC_69); };
            Assembly[0x65] = delegate () { Input8Byte(Assembly_6502.ADC_65); };
            Assembly[0x61] = delegate () { Input8Byte(Assembly_6502.ADC_61); };
            Assembly[0x75] = delegate () { Input8Byte(Assembly_6502.ADC_75); };
            Assembly[0x71] = delegate () { Input8Byte(Assembly_6502.ADC_71); };
        }

        private void STYU8()
        {
            Assembly[0x84] = delegate () { Input8Byte(Assembly_6502.STY_84); };
            Assembly[0x94] = delegate () { Input8Byte(Assembly_6502.STY_94); };
        }

        private void STXU8()
        {
            Assembly[0x86] = delegate () { Input8Byte(Assembly_6502.STX_86); };
            Assembly[0x96] = delegate () { Input8Byte(Assembly_6502.STX_96); };
        }

        private void STAU8()
        {
            Assembly[0x85] = delegate () { Input8Byte(Assembly_6502.STA_85); };
            Assembly[0x81] = delegate () { Input8Byte(Assembly_6502.STA_81); };
            Assembly[0x95] = delegate () { Input8Byte(Assembly_6502.STA_95); };
            Assembly[0x91] = delegate () { Input8Byte(Assembly_6502.STA_91); };
        }

        private void LDYU8()
        {
            Assembly[0xA0] = delegate () { Input8Byte(Assembly_6502.LDY_A0); };
            Assembly[0xA4] = delegate () { Input8Byte(Assembly_6502.LDY_A4); };
            Assembly[0xB4] = delegate () { Input8Byte(Assembly_6502.LDY_B4); };
        }

        private void LDXU8()
        {
            Assembly[0xA2] = delegate () { Input8Byte(Assembly_6502.LDX_A2); };
            Assembly[0xA6] = delegate () { Input8Byte(Assembly_6502.LDX_A6); };
            Assembly[0xB6] = delegate () { Input8Byte(Assembly_6502.LDX_B6); };
        }

        private void LDAU8()
        {
            Assembly[0xA9] = delegate () { Input8Byte(Assembly_6502.LDA_A9); };
            Assembly[0xA5] = delegate () { Input8Byte(Assembly_6502.LDA_A5); };
            Assembly[0xA1] = delegate () { Input8Byte(Assembly_6502.LDA_A1); };
            Assembly[0xB5] = delegate () { Input8Byte(Assembly_6502.LDA_B5); };
            Assembly[0xB1] = delegate () { Input8Byte(Assembly_6502.LDA_B1); };
        }
        #endregion

        #region U16

        /// <summary>
        /// 16 Bit input mostly addresses
        /// </summary>
        private void U16ByteToList()
        {
            LoadU16();
            StoreU16();
            ArithmeticU16();
            ShiftRotateU16();
            LogicU16();
            CMPU16();
            CompareTestBitU16();
            JumpU16();
        }

        private void LogicU16()
        {
            ANDU16();
            ORAU16();
            EORU16();
        }

        private void ShiftRotateU16()
        {
            ASLU16();
            LSRU16();
            ROLU16();
            RORU16();
        }

        private void ArithmeticU16()
        {
            ADCU16();
            SBCU16();
            INCU16();
            DECU16();
        }

        private void StoreU16()
        {
            STAU16();
            STXYU16();
        }

        private void LoadU16()
        {
            LDAU16();
            LDXU16();
            LDYU16();
        }

        private void JumpU16()
        {
            Assembly[0x4C] = delegate () { Input16Byte(Assembly_6502.JMP_4C); };
            Assembly[0x6C] = delegate () { Input16Byte(Assembly_6502.JMP_6C); };
            Assembly[0x20] = delegate () { Input16Byte(Assembly_6502.JSR_20); };
        }

        private void CompareTestBitU16()
        {
            Assembly[0xEC] = delegate () { Input16Byte(Assembly_6502.CPX_EC); };
            Assembly[0xCC] = delegate () { Input16Byte(Assembly_6502.CPY_CC); };
            Assembly[0x2C] = delegate () { Input16Byte(Assembly_6502.BIT_2C); };
        }

        private void CMPU16()
        {
            Assembly[0xCD] = delegate () { Input16Byte(Assembly_6502.CMP_CD); };
            Assembly[0xDD] = delegate () { Input16Byte(Assembly_6502.CMP_DD); };
            Assembly[0xD9] = delegate () { Input16Byte(Assembly_6502.CMP_D9); };
        }

        private void EORU16()
        {
            Assembly[0x4D] = delegate () { Input16Byte(Assembly_6502.EOR_4D); };
            Assembly[0x5D] = delegate () { Input16Byte(Assembly_6502.EOR_5D); };
            Assembly[0x59] = delegate () { Input16Byte(Assembly_6502.EOR_59); };
        }

        private void ORAU16()
        {
            Assembly[0x0D] = delegate () { Input16Byte(Assembly_6502.ORA_0D); };
            Assembly[0x1D] = delegate () { Input16Byte(Assembly_6502.ORA_1D); };
            Assembly[0x19] = delegate () { Input16Byte(Assembly_6502.ORA_19); };
        }

        private void ANDU16()
        {
            Assembly[0x2D] = delegate () { Input16Byte(Assembly_6502.AND_2D); };
            Assembly[0x3D] = delegate () { Input16Byte(Assembly_6502.AND_3D); };
            Assembly[0x39] = delegate () { Input16Byte(Assembly_6502.AND_39); };
        }

        private void RORU16()
        {
            Assembly[0x6E] = delegate () { Input16Byte(Assembly_6502.ROR_6E); };
            Assembly[0x7E] = delegate () { Input16Byte(Assembly_6502.ROR_7E); };
        }

        private void ROLU16()
        {
            Assembly[0x2E] = delegate () { Input16Byte(Assembly_6502.ROL_2E); };
            Assembly[0x3E] = delegate () { Input16Byte(Assembly_6502.ROL_3E); };
        }

        private void LSRU16()
        {
            Assembly[0x4E] = delegate () { Input16Byte(Assembly_6502.LSR_4E); };
            Assembly[0x5E] = delegate () { Input16Byte(Assembly_6502.LSR_5E); };
        }

        private void ASLU16()
        {
            Assembly[0x0E] = delegate () { Input16Byte(Assembly_6502.ASL_0E); };
            Assembly[0x1E] = delegate () { Input16Byte(Assembly_6502.ASL_1E); };
        }

        private void DECU16()
        {
            Assembly[0xCE] = delegate () { Input16Byte(Assembly_6502.DEC_CE); };
            Assembly[0xDE] = delegate () { Input16Byte(Assembly_6502.DEC_DE); };
        }

        private void INCU16()
        {
            Assembly[0xEE] = delegate () { Input16Byte(Assembly_6502.INC_EE); };
            Assembly[0xFE] = delegate () { Input16Byte(Assembly_6502.INC_FE); };
        }

        private void SBCU16()
        {
            Assembly[0xED] = delegate () { Input16Byte(Assembly_6502.SBC_ED); };
            Assembly[0xFD] = delegate () { Input16Byte(Assembly_6502.SBC_FD); };
            Assembly[0xF9] = delegate () { Input16Byte(Assembly_6502.SBC_F9); };
        }

        private void ADCU16()
        {
            Assembly[0x6D] = delegate () { Input16Byte(Assembly_6502.ADC_6D); };
            Assembly[0x7D] = delegate () { Input16Byte(Assembly_6502.ADC_7D); };
            Assembly[0x79] = delegate () { Input16Byte(Assembly_6502.ADC_79); };
        }

        private void STXYU16()
        {
            Assembly[0x8E] = delegate () { Input16Byte(Assembly_6502.STX_8E); };
            Assembly[0x8C] = delegate () { Input16Byte(Assembly_6502.STY_8C); };
        }

        private void STAU16()
        {
            Assembly[0x8D] = delegate () { Input16Byte(Assembly_6502.STA_8D); };
            Assembly[0x9D] = delegate () { Input16Byte(Assembly_6502.STA_9D); };
            Assembly[0x99] = delegate () { Input16Byte(Assembly_6502.STA_99); };
        }

        private void LDYU16()
        {
            Assembly[0xAC] = delegate () { Input16Byte(Assembly_6502.LDY_AC); };
            Assembly[0xBC] = delegate () { Input16Byte(Assembly_6502.LDY_BC); };
        }

        private void LDXU16()
        {
            Assembly[0xAE] = delegate () { Input16Byte(Assembly_6502.LDX_AE); };
            Assembly[0xBE] = delegate () { Input16Byte(Assembly_6502.LDX_BE); };
        }

        private void LDAU16()
        {
            Assembly[0xAD] = delegate () { Input16Byte(Assembly_6502.LDA_AD); };
            Assembly[0xBD] = delegate () { Input16Byte(Assembly_6502.LDA_BD); };
            Assembly[0xB9] = delegate () { Input16Byte(Assembly_6502.LDA_B9); };
        }

        #endregion

        #region No Imput
        /// <summary>
        /// Functions without Input.
        /// </summary>
        private void NoImputToList()
        {
            Transfer();
            Miscellaneous();
            Stack();
            Clear();
            Set();
            Return();
            IncrementDecrement();
            Logical();

        }

        private void Logical()
        {
            Assembly[0x0A] = delegate () { NoInput(Assembly_6502.ASL_0A); };
            Assembly[0x4A] = delegate () { NoInput(Assembly_6502.LSR_4A); };
            Assembly[0x2A] = delegate () { NoInput(Assembly_6502.ROL_2A); };
            Assembly[0x6A] = delegate () { NoInput(Assembly_6502.ROR_6A); };
        }

        private void IncrementDecrement()
        {
            Assembly[0xCA] = delegate () { NoInput(Assembly_6502.DEX_CA); };
            Assembly[0x88] = delegate () { NoInput(Assembly_6502.DEY_88); };
            Assembly[0xE8] = delegate () { NoInput(Assembly_6502.INX_E8); };
            Assembly[0xC8] = delegate () { NoInput(Assembly_6502.INY_C8); };
        }

        private void Return()
        {
            Assembly[0x40] = delegate () { NoInput(Assembly_6502.RTI_40); };
            Assembly[0x60] = delegate () { NoInput(Assembly_6502.RTS_60); };
        }

        private void Set()
        {
            Assembly[0x38] = delegate () { NoInput(Assembly_6502.SEC_38); };
            Assembly[0x78] = delegate () { NoInput(Assembly_6502.SEI_78); };
            Assembly[0xF8] = delegate () { NoInput(Assembly_6502.SED_F8); };
        }

        private void Clear()
        {
            Assembly[0x18] = delegate () { NoInput(Assembly_6502.CLC_18); };
            Assembly[0x58] = delegate () { NoInput(Assembly_6502.CLI_58); };
            Assembly[0xD8] = delegate () { NoInput(Assembly_6502.CLD_D8); };
            Assembly[0xB8] = delegate () { NoInput(Assembly_6502.CLV_B8); };
        }

        private void Stack()
        {
            Assembly[0x28] = delegate () { NoInput(Assembly_6502.PLP_28); };
            Assembly[0x08] = delegate () { NoInput(Assembly_6502.PHP_08); };
            Assembly[0x48] = delegate () { NoInput(Assembly_6502.PHA_48); };
            Assembly[0x68] = delegate () { NoInput(Assembly_6502.PLA_68); };
        }

        private void Miscellaneous()
        {
            Assembly[0x00] = delegate () { NoInput(Assembly_6502.BRK_00); };
            Assembly[0xEA] = delegate () { NoInput(Assembly_6502.NOP_EA); };
        }

        private void Transfer()
        {
            Assembly[0x8A] = delegate () { NoInput(Assembly_6502.TXA_8A); };
            Assembly[0x98] = delegate () { NoInput(Assembly_6502.TYA_98); };
            Assembly[0x9A] = delegate () { NoInput(Assembly_6502.TXS_9A); };
            Assembly[0xA8] = delegate () { NoInput(Assembly_6502.TAY_A8); };
            Assembly[0xAA] = delegate () { NoInput(Assembly_6502.TAX_AA); };
            Assembly[0xBA] = delegate () { NoInput(Assembly_6502.TSX_BA); };
        } 
        #endregion

    }
}
