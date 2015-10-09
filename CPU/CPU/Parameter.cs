namespace NES
{
    class Parameter
    {

        /// <summary>
        ///Absolute Indexed with X: a,x
        ///The value in X is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in X is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,X
        /// </summary>
        /// <param name="ax">Absolute Indexed with X: a,x</param>
        /// <returns>Address from a,x </returns>
        public static ushort ax(ushort ax)
        {
            return (ushort)(ax + NES_Register.X);
        }

        /// <summary>
        ///Absolute Indexed with Y: a,y
        ///The value in Y is added to the specified address for a sum address. The value at the sum address is used to perform the computation.
        ///Example
        ///The value $02 in Y is added to $C001 for a sum of $C003. The value $5A at address $C003 is used to perform the add with carry (ADC) operation.
        ///ADC $C001,Y
        /// </summary>
        /// <param name="ay">Absolute Indexed with Y: a,y</param>
        /// <returns>Address from a,y </returns>
        public static ushort ay(ushort ay)
        {
            return (ushort)(ay + NES_Register.Y);
        }

        /// <summary>
        /// A single byte specifies an address in the first page of memory ($00xx), also known as the zero page, and the byte at that address is used to perform the computation.
        /// Example
        /// The value at address $0002 is loaded into the Y register.
        /// LDY $02
        /// </summary>
        /// <param name="zp">($00zp) address</param>
        /// <returns>Address from zp</returns>
        public static ushort zp(byte zp)
        {
            return zp;
        }

        /// <summary>
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </summary>
        /// <param name="zpx">(zp,x)</param>
        /// <returns>Address from (zp,x)</returns>
        public static ushort zpx1(int zpx)
        {
            short address = (short)(zpx + NES_Register.X);
            return (ushort)((((AddressSetup)NES_Memory.Memory[address]).Value) | ((AddressSetup)NES_Memory.Memory[address + 1]).Value << 8);
        }

        /// <summary>
        /// Zero Page Indexed with X: zp,x[edit]
        /// The value in X is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $01 for a sum of $03. The value $A5 at address $0003 is loaded into the Accumulator.
        /// LDA $01,X
        /// </summary>
        /// <param name="zpx">zp,x address</param>
        /// <returns>Address from zp,x </returns>
        public static ushort zpx2(byte zpx)
        {
            return (ushort)(zpx + NES_Register.X);
        }

        /// <summary>
        /// The value in Y is added to the address at the little-endian address stored at the two-byte pair of the specified address (LSB) and the specified address plus one (MSB). The value at the sum address is used to perform the computation. Indeed addressing mode actually repeats exactly the accumulator register's digits.
        /// Example
        /// The value $03 in Y is added to the address $C235 at addresses $002A and $002B for a sum of $C238. The value $2F at $C238 is shifted right (yielding $17) and written back to $C238.
        /// LSR ($2A),Y
        /// </summary>
        /// <param name="zpy">(zp),Y</param>
        /// <returns>Address from (zp),Y </returns>
        public static ushort zpy1(byte zpy)
        {
            var temp = ((AddressSetup)NES_Memory.Memory[zpy]).Value | ((AddressSetup)NES_Memory.Memory[zpy + 1]).Value << 8;
            return (ushort)(temp + NES_Register.Y);
        }

        /// <summary>
        /// The value in Y is added to the specified zero page address for a sum address. The value at the sum address is used to perform the computation.
        /// Example
        /// The value $03 in Y is added to $01 for a sum of $04. The value $E3 at address $0004 is loaded into the Accumulator.
        /// LDA $01,Y
        /// </summary>
        /// <param name="zpy">zp,Y</param>
        /// <returns>Address from zp,Y</returns>
        public static ushort zpy2(byte zpy)
        {
            return (ushort)(zpy + NES_Register.Y);
        }

        /// <summary>
        /// Puts 2 Memory Values together 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ushort MemoryValueToAdress(ushort a)
        {

            return (ushort)((((AddressSetup)NES_Memory.Memory[a]).Value) | (((AddressSetup)NES_Memory.Memory[a + 1]).Value << 8));
        }
    }
}
