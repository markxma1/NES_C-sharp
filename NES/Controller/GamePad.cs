using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NES.Controller
{
    /// <summary>
    /// Input ($4016 write)
    /// </summary>
    public struct InputFlags
    {
        /// <summary>
        /// While S (strobe) is high, the shift registers in the controllers 
        /// are continuously reloaded from the button states, 
        /// and reading $4016/$4017 will keep returning 
        /// the current state of the first button (A). 
        /// Once S goes low, this reloading will stop. 
        /// Hence a 1/0 write sequence is required to get the button states,
        ///  after which the buttons can be read back one at a time.
        /// (Note that bits 2-0 of $4016/write are stored in internal latches
        ///  in the 2A03/07.) 
        /// </summary>
        public bool strobe { get { return (address.value & 0x01) > 0; } set { address.value = (byte)(address.value & ~0x01); if (value) address.value = (byte)(address.value | 0x01); } }

        public Address address;
    }

    public class OutputFlags
    {

        public bool SerialControllerData { get { return (address.value & 0x01) > 0; } set { address.value = (byte)(address.value & ~0x01); if (value) address.value = (byte)(address.value | 0x01); } }

        public byte OpenBus { get { return (byte)(address.value & 0xE0); } set { address.value = (byte)(address.value & ~0xE0); address.value = (byte)(address.value | 0xE0); } }

        public Address address;
    }

    public class OutputFlags4016 : OutputFlags
    {


    }

    public class OutputFlags4017 : OutputFlags
    {


    }

    class GamePad
    {
        public class Controller
        {
            public Dictionary<string, bool> Button = new Dictionary<string, bool>();
            public Controller()
            {
                Button.Add("A", true);
                Button.Add("B", true);
                Button.Add("A", true);
                Button.Add("SELECT", true);
                Button.Add("START", true);
                Button.Add("L", true);
                Button.Add("R", true);
                Button.Add("U", true);
                Button.Add("D", true);
            }
        }

        public static Controller Player1 = new Controller();
        public static Controller Player2 = new Controller();
        public static Controller Player3 = new Controller();
        public static Controller Player4 = new Controller();

        private static InputFlags input4016 = new InputFlags();
        private static OutputFlags output4016 = new OutputFlags();
        private int P1BID=0;

        public static InputFlags Input4016
        {
            set
            {
                input4016 = value;
            }
        }

        public static OutputFlags Output4016
        {
            get
            {
                return output4016;
            }
        }

        public GamePad()
        {
            InitInput4016();
            InitOutput4016();
        }

        private static void InitInput4016()
        {
            input4016.address = (Address)NES_Memory.Memory[4016];
        }

        private void InitOutput4016()
        {
            output4016.address = (Address)NES_Memory.Memory[4016];
            output4016.address.beforGet = delegate { getButton(); };
        }

        private void getButton()
        {
            output4016.SerialControllerData = Player1.Button.ElementAt(P1BID).Value; if (P1BID++ > 7) P1BID = 0;
        }
    }
}
