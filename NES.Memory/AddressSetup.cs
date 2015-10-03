using System;
using System.Collections;

namespace NES
{
    public class AddressSetup : Address
    {
        private int id = 0;
        public delegate void func();
        public delegate void funcOut(byte value);
        private func beforGet = delegate () { };
        private func afterGet = delegate () { };
        private func beforSet = delegate () { };
        private funcOut afterSet = delegate (byte value) { };
        private byte oldValue;
        private byte valueCore;

        public bool isNew()
        {
            bool temp = !(oldValue == value);
            if (temp)
                oldValue = value;
            return temp;
        }

        /// <summary>
        /// pure Value without additional functions. 
        /// </summary>
        public byte value
        {
            get
            {
                return valueCore;
            }

            set
            {
                valueCore = value;
            }
        }

        /// <summary>
        /// Value with additional functions
        /// </summary>
        public byte Value
        {
            get { BeforGet(); byte temp = value; AfterGet(); return temp; }
            set { BeforSet(); this.value = value; AfterSet(value); }
        }

        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public func BeforGet
        {
            get
            {
                return beforGet;
            }

            set
            {
                beforGet = value;
            }
        }

        public func AfterGet
        {
            get
            {
                return afterGet;
            }

            set
            {
                afterGet = value;
            }
        }

        public func BeforSet
        {
            get
            {
                return beforSet;
            }

            set
            {
                beforSet = value;
            }
        }

        public funcOut AfterSet
        {
            get
            {
                return afterSet;
            }

            set
            {
                afterSet = value;
            }
        }

        /// <summary>
        /// Create an empty Adress and Gives an ID to it. 
        /// </summary>
        /// <param name="ID">Adress ID</param>
        public AddressSetup(int ID) { this.ID = ID; }

        /// <summary>
        /// Create an Adress with new value and Gives an ID to it. 
        /// </summary>
        /// <param name="value">Value that was saved on that Adress</param>
        /// <param name="ID">Adress ID</param>
        public AddressSetup(byte value, int ID)
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

        public static explicit operator AddressSetup(ArrayList v)
        {
            throw new NotImplementedException();
        }
    }
}
