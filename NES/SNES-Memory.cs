using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace NES
{
    class Adress
    {
        public byte Value;
    }

    //In byte $15 in the SNES header (see below for more details), the ROM makeup byte is stored.
    //Value |	Bitmask 	    |Definition   	                |Example ROM 	Example           |ROM size
    //$20    |0010 0000 |LoROM 	                    |Final Fantasy 4 	                      |1048576 bytes / 1 MB
    //$21     |0010 0001  |HiROM 	                    |Final Fantasy 5 	                      |2097152 bytes / 2 MB
    //$30    |0011 0000 	|LoROM + FastROM 	|Ultima VII 	                              |1572864 bytes / 1.5 MB
    //$31     |0011 0001 	|HiROM + FastROM 	|Final Fantasy 6 	                      |3145728 bytes / 3 MB
    //$32    |0011 0010 	|ExLoROM 	                |Star Ocean 	                              |6291456 bytes / 6 MB
    //$35    |0011 0101 	|ExHiROM 	                |Tales Of Phantasia 	                  |6291456 bytes / 6 MB
    //
    //The bitmask to use is 001A0BCD, the basic value is $20:
    //
    //- A == 0 means SlowROM (+ $0), A == 1 means FastROM (+ $10).
    //- B == 1 means ExHiROM (+ $4)
    //- C == 1 means ExLoROM (+ $2)
    //- D == 0 means LoROM (+ $0), D == 1 means HiROM (+ $1), is used with B and C in case of extended ROMs.
    //

    class LowRAM_Memory
    {
        ArrayList Memory = new ArrayList();
        ArrayList Bank = new ArrayList();
        ArrayList LoROM = new ArrayList();//LoROM section (program memory)
        ArrayList LowRAM = new ArrayList();//(WRAM)
        ArrayList HighRAM = new ArrayList();//(WRAM)
        ArrayList ERAM = new ArrayList();//Extended RAM (WRAM) 
        ArrayList SRAM = new ArrayList();//Cartridge SRAM - 448+64 Kilobytes (512 KB total)

        public LowRAM_Memory()
        {
            #region LoROM
            for (int i = 0x00; i <= 0xFF; i++)
            {
                var ad = new Adress[0xFFFF + 1];
                for (int j = 0; j <= 0xFFFF; j++)
                {
                    ad[j] = new Adress();
                    Memory.Add(ad[j]);
                }
                Bank.Add(ad);
            }

            for (int j = 0x80; j <= 0xBF; j++)
            {
                    Bank[j]=Bank[j-0x80];
            }
            for (int j = 0xC0; j <= 0xEF; j++)
            {
                Bank[j] = Bank[j - 0x80];
            }
            for (int j = 0xF0; j <= 0xFD; j++)
            {
                Bank[j] = Bank[j - 0x80];
            }


            for (int j = 0x40; j <= 0x6F; j++)
            {
                for (int i = 0; i <= 0x7FFF; i++)
                {
                    ((Adress[])Bank[j])[i] = ((Adress[])Bank[j])[i+0x8000];
                }
            }

            for (int j = 0; j <= 0x7D; j++)
            {
                for (int i = 0x8000; i <= 0xFFFF; i++)
                {
                    LoROM.Add(((Adress[])Bank[j])[i]);
                }
            }

            for (int j = 0xFE; j <= 0xFF; j++)
            {
                for (int i = 0x8000; i <= 0xFFFF; i++)
                {
                    LoROM.Add(((Adress[])Bank[j])[i]);
                }
            }

            for (int i = 0x0000; i <= 0x1FFF; i++)
            {
                LowRAM.Add(((Adress[])Bank[0x7E])[i]);
            }
            for (int i = 0x2000; i <= 0x7FFF; i++)
            {
                HighRAM.Add(((Adress[])Bank[0x7E])[i]);
            }
            for (int i = 0x8000; i <= 0xFFFF; i++)
            {
                ERAM.Add(((Adress[])Bank[0x7E])[i]);
            }
            for (int i = 0x0000; i <= 0xFFFF; i++)
            {
                ERAM.Add(((Adress[])Bank[0x7F])[i]);
            }
            for (int j = 0x70; j <= 0x7D; j++)
            {
                for (int i = 0x0000; i <= 0x7FFF; i++)
                {
                    SRAM.Add(((Adress[])Bank[j])[i]);
                }
            }
            for (int j = 0xFE; j <= 0xFF; j++)
            {
                for (int i = 0x0000; i <= 0x7FFF; i++)
                {
                    SRAM.Add(((Adress[])Bank[j])[i]);
                }
            }
            #endregion
        }
    }
}
