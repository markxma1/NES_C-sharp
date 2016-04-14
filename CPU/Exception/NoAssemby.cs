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
using System.Runtime.Serialization;

namespace NES
{
    [Serializable]
    internal class NoAssemby : Exception
    {
        public NoAssemby()
        {
        }

        public NoAssemby(string message) : base(message)
        {
        }

        public NoAssemby(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoAssemby(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}