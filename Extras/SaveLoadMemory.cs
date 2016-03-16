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
