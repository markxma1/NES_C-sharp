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
using System.Data;


namespace NES
{
    public class SaveLoadMemory
    {
        public static void SaveTo(string path)
        {
            lock (NES_Memory.Memory)
            {
                lock (NES_PPU_Memory.Memory)
                {
                    DataSet memory = new DataSet();
                    DataTable dataTable = memory.Tables.Add("CPU");
                    dataTable.Columns.Add();
                    Array.ForEach(NES_Memory.Memory.ToArray(), c => dataTable.Rows.Add()[0] = ((AddressSetup)c).Value);

                    dataTable = memory.Tables.Add("PPU");
                    dataTable.Columns.Add();
                    Array.ForEach(NES_PPU_Memory.Memory.ToArray(), c => dataTable.Rows.Add()[0] = ((AddressSetup)c).Value);

                    dataTable = memory.Tables.Add("Register");
                    dataTable.Columns.Add();
                    dataTable.Rows.Add()[0] = NES_Register.A;
                    dataTable.Rows.Add()[0] = NES_Register.P.P;
                    dataTable.Rows.Add()[0] = NES_Register.PC;
                    dataTable.Rows.Add()[0] = NES_Register.S;
                    dataTable.Rows.Add()[0] = NES_Register.X;
                    dataTable.Rows.Add()[0] = NES_Register.Y;


                    memory.WriteXml(path);
                }
            }
        }

        public static void LoadFrom(string path)
        {
            lock (NES_Memory.Memory)
            {
                lock (NES_PPU_Memory.Memory)
                {
                    DataSet memory = new DataSet();
                    memory.ReadXml(path);
                    DataTable dataTable = memory.Tables["CPU"];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ((AddressSetup)(NES_Memory.Memory[dataTable.Rows.IndexOf(row)])).Value = byte.Parse(row[0].ToString());
                    }

                    dataTable = memory.Tables["PPU"];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        ((AddressSetup)(NES_PPU_Memory.Memory[dataTable.Rows.IndexOf(row)])).Value = byte.Parse(row[0].ToString());
                    }

                    dataTable = memory.Tables["Register"];
                    NES_Register.A = byte.Parse(dataTable.Rows[0][0].ToString());
                    NES_Register.P.P = byte.Parse(dataTable.Rows[1][0].ToString());
                    NES_Register.PC = ushort.Parse(dataTable.Rows[2][0].ToString());
                    NES_Register.S = byte.Parse(dataTable.Rows[3][0].ToString());
                    NES_Register.X = byte.Parse(dataTable.Rows[4][0].ToString());
                    NES_Register.Y = byte.Parse(dataTable.Rows[5][0].ToString());
                }
            }
        }
    }
}
