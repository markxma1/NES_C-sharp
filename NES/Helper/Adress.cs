using System;
using System.Collections;

namespace NES
{
    public class Adress
    {
        int ID = 0;
        public delegate void func();
        public delegate void funcOut(byte value);
        public func beforGet = delegate () { };
        public func afterGet = delegate () { };
        public func beforSet = delegate () { };
        public funcOut afterSet = delegate (byte value) { };

        public bool busy = false;

        public byte value;
        public byte Value
        {
            get { beforGet(); byte temp = value; afterGet(); return temp; }
            set { beforSet(); this.value = value; afterSet(value); }
        }

        /// <summary>
        /// Create an empty Adress and Gives an ID to it. 
        /// </summary>
        /// <param name="ID">Adress ID</param>
        public Adress(int ID) { this.ID = ID; }

        /// <summary>
        /// Create an Adress with new value and Gives an ID to it. 
        /// </summary>
        /// <param name="value">Value that was saved on that Adress</param>
        /// <param name="ID">Adress ID</param>
        public Adress(byte value, int ID)
        {
            this.value = value;
            this.ID = ID;
        }

        public override string ToString()
        {
            string HEXValue = "0x" + value.ToString("X").PadLeft(2, '0');
            string HEXID = ID.ToString("X").PadLeft(4, '0');
            return HEXValue + ": (" + HEXID + ")";
        }

        public static explicit operator Adress(ArrayList v)
        {
            throw new NotImplementedException();
        }
    }
}
