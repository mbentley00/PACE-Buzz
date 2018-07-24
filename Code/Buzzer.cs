using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PACEBuzz
{
    /// <summary>
    /// Class which defines the state of the buttons
    /// </summary>
    public class ButtonStates
    {
        /// <summary>True if red pressed</summary>
        public bool Red;
        /// <summary>True if blue pressed</summary>
        public bool Blue;
        /// <summary>True if orange pressed</summary>
        public bool Orange;
        /// <summary>True if green pressed</summary>
        public bool Green;
        /// <summary>True if yellow pressed</summary>
        public bool Yellow;
    }

    #region Event definitions
    /// <summary>
    /// Arguments for button changed event
    /// </summary>
    public class BuzzButtonChangedEventArgs : EventArgs
    {
        /// <summary>Current states of the buttons</summary>
        public readonly ButtonStates[] Buttons;

        /// <summary>
        /// Index of the buzzer being buzzed.
        /// </summary>
        public readonly int BuzzerIndex;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arrStates">State of the buttons</param>
        /// <param name="buzzerIndex">Index of the buzzer being buzzed</param>
        public BuzzButtonChangedEventArgs(ButtonStates[] arrStates, int buzzerIndex)
        {
            this.Buttons = arrStates;
            this.BuzzerIndex = buzzerIndex;
        }
    }
    /// <summary>
    /// Delegate for button event
    /// </summary>
    public delegate void BuzzButtonChangedEventHandler(object sender, BuzzButtonChangedEventArgs args);
    #endregion

    /// <summary>
    /// Class that defines a controller of Buzz handsets : Representation of the USB hardware device
    /// </summary>
    public class Buzzer : HIDDevice
    {
        #region Public Variables

        public int BuzzerIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Event fired when one or more button state changes
        /// </summary>
        public event BuzzButtonChangedEventHandler OnButtonChanged;

        /// <summary>
        /// Creates an input report for use in the HID device framework
        /// </summary>
        /// <returns>A new input report for this device</returns>
        public override InputReport CreateInputReport()
        {
            return new BuzzInputReport(this);
        }

        public List<Player> Players
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public Buzzer()
        {
            this.Players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                this.Players.Add(new Player(this.BuzzerIndex, i));
            }
        }

        public Buzzer(Buzzer b)
        {
            this.OutputReportLength = b.OutputReportLength;
            this.InputReportLength = b.InputReportLength;
            this.OutputFile = b.OutputFile;
            this.BuzzerIndex = b.BuzzerIndex;
            this.m_hHandle = b.m_hHandle;
            this.Players = new List<Player>();
            for (int i = 0; i < 4; i++)
            {
                this.Players.Add(new Player(this.BuzzerIndex, i));
            }
        }

        public void AddPlayers(int count = 4)
        {
            this.Players = new List<Player>();
            for (int i = 0; i < count; i++)
            {
                this.Players.Add(new Player(this.BuzzerIndex, i));
            }
        }

        #endregion

        /// <summary>
        /// Sets the states of the handsets lamps on or off.
        /// </summary>
        /// <param name="bLight1">Turn light on handset 1 on/off</param>
        /// <param name="bLight2">Turn light on handset 2 on/off</param>
        /// <param name="bLight3">Turn light on handset 3 on/off</param>
        /// <param name="bLight3">Turn light on handset 4 on/off</param>
        public void SetLights(bool bLight1, bool bLight2, bool bLight3, bool bLight4)
        {
            BuzzOutputReport oRep = new BuzzOutputReport(this);	// create output report
            oRep.SetLightStates(bLight1, bLight2, bLight3, bLight4);	// set the lights states
            try
            {
                Write(oRep); // write the output report
            }
            catch (HIDDeviceException e)
            {
                // Device may have been removed!
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Finds all buzz handsets.
        /// </summary>
        /// <returns>A list of all new buzz handsets.</returns>
        public static List<Buzzer> FindBuzzHandsets()
        {
            // VID and PID for Buzz device are 0x054c and 0x1000 respectively
            List<HIDDevice> devices = FindDevices(0x054c, 0x1000, typeof(Buzzer));
            List<Buzzer> buzzers = new List<Buzzer>();
            int index = 0;
            foreach (HIDDevice device in devices)
            {
                Buzzer buzzer = (Buzzer)device;
                buzzer.BuzzerIndex = index;
                buzzer.AddPlayers();
                buzzers.Add(buzzer);
                index++;
            }

            return buzzers;
        }

        #region Overrides
        /// <summary>
        /// Fired when data has been received over USB
        /// </summary>
        /// <param name="oInRep">Input report received</param>
        protected override void HandleDataReceived(InputReport oInRep)
        {
            // Fire the event handler if assigned
            if (OnButtonChanged != null)
            {
                BuzzInputReport oBuzIn = (BuzzInputReport)oInRep;
                OnButtonChanged(this, new BuzzButtonChangedEventArgs(oBuzIn.Buttons, this.BuzzerIndex));
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="bDisposing">True if object is being disposed - else is being finalised</param>
        protected override void Dispose(bool bDisposing)
        {
            if (bDisposing)
            {
                // before we go, turn all lights off
                // SetLights(false, false, false, false);
            }
            base.Dispose(bDisposing);
        }

        #endregion
    }

    #region Device reports
    /// <summary>
    /// Output report for Buzz device
    /// </summary>
    public class BuzzOutputReport : OutputReport
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oDev">Device constructing this report</param>
        public BuzzOutputReport(HIDDevice oDev) : base(oDev) { }

        /// <summary>
        /// Sets the states of the lights
        /// </summary>
        /// <param name="bLight1">State of light on handset 1</param>
        /// <param name="bLight2">State of light on handset 2</param>
        /// <param name="bLight3">State of light on handset 3</param>
        /// <param name="bLight4">State of light on handset 4</param>
        public void SetLightStates(bool bLight1, bool bLight2, bool bLight3, bool bLight4)
        {
            byte[] arrBuff = Buffer;
            arrBuff[2] = (byte)(bLight1 ? 0xff : 0);
            arrBuff[3] = (byte)(bLight2 ? 0xff : 0);
            arrBuff[4] = (byte)(bLight3 ? 0xff : 0);
            arrBuff[5] = (byte)(bLight4 ? 0xff : 0);
        }
    }

    /// <summary>
    /// Input report for Buzz Handset device
    /// </summary>
    public class BuzzInputReport : InputReport
    {
        /// <summary>Number of handsets</summary>
        public const int NUM_HANDSETS = 4;
        /// <summary>Decoded button states</summary>
        private ButtonStates[] m_arrButtons;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="oDev">Constructing device</param>
        public BuzzInputReport(HIDDevice oDev)
            : base(oDev)
        {
            // Create the button states
            m_arrButtons = new ButtonStates[NUM_HANDSETS];
            for (int i = 0; i < NUM_HANDSETS; i++)
            {
                m_arrButtons[i] = new ButtonStates();
            }
        }

        /// <summary>
        /// Process the raw data from the actual input report bytes
        /// </summary>
        public override void ProcessData()
        {
            byte[] arrData = Buffer;
            m_arrButtons[0].Red = ((arrData[3] & 0x01) != 0);
            m_arrButtons[0].Yellow = ((arrData[3] & 0x02) != 0);
            m_arrButtons[0].Green = ((arrData[3] & 0x04) != 0);
            m_arrButtons[0].Orange = ((arrData[3] & 0x08) != 0);
            m_arrButtons[0].Blue = ((arrData[3] & 0x10) != 0);

            m_arrButtons[1].Red = ((arrData[3] & 0x20) != 0);
            m_arrButtons[1].Yellow = ((arrData[3] & 0x40) != 0);
            m_arrButtons[1].Green = ((arrData[3] & 0x80) != 0);
            m_arrButtons[1].Orange = ((arrData[4] & 0x01) != 0);
            m_arrButtons[1].Blue = ((arrData[4] & 0x02) != 0);

            m_arrButtons[2].Red = ((arrData[4] & 0x04) != 0);
            m_arrButtons[2].Yellow = ((arrData[4] & 0x08) != 0);
            m_arrButtons[2].Green = ((arrData[4] & 0x10) != 0);
            m_arrButtons[2].Orange = ((arrData[4] & 0x20) != 0);
            m_arrButtons[2].Blue = ((arrData[4] & 0x40) != 0);

            m_arrButtons[3].Red = ((arrData[4] & 0x80) != 0);
            m_arrButtons[3].Yellow = ((arrData[5] & 0x01) != 0);
            m_arrButtons[3].Green = ((arrData[5] & 0x02) != 0);
            m_arrButtons[3].Orange = ((arrData[5] & 0x04) != 0);
            m_arrButtons[3].Blue = ((arrData[5] & 0x08) != 0);
        }

        /// <summary>
        /// Accessor for the button states
        /// </summary>
        public ButtonStates[] Buttons
        {
            get
            {
                return m_arrButtons;
            }
        }
    }

    #endregion
}
