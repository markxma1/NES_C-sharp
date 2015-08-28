using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NES
{
    class Parameter
    {
        /// <summary>
        /// Zero Page Indexed Indirect: (zp,x)
        /// The value in X is added to the specified zero page address for a sum address. The little-endian address stored at the two-byte pair of sum address (LSB) and sum address plus one (MSB) is loaded and the value at that address is used to perform the computation.
        /// Example
        /// The value $02 in X is added to $15 for a sum of $17. The address $D010 at addresses $0017 and $0018 will be where the value $0F in the accumulator is stored.
        /// STA ($15,X)
        /// </summary>
        /// <param name="zpx">(zp,x)</param>
        public static ushort zpx1(int zpx)
        {
            short address = (short)(zpx + NES_Register.X);
            return (ushort)((((Adress)NES_Memory.Memory[address]).Value) | ((Adress)NES_Memory.Memory[address + 1]).Value << 8);             
        }
    }
}
