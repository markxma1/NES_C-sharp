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

        public void setAsOld()
        {
                oldValue = value;
        }

        public static explicit operator AddressSetup(ArrayList v)
        {
            throw new NotImplementedException();
        }
    }
}
