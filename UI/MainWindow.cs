using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace PACEBuzz
{
    public partial class MainWindow : Form
    {
        #region Buzzer COM variables

        /// <summary>HID Guid--don't think this is used anymore</summary>
        private Guid deviceClassGuid;

        /// <summary>Handle returned by RegisterForUsbEvents - need it when we unregister</summary>
        private IntPtr usbEventHandle;

        /// <summary>Event called when a new device is detected</summary>
        public event EventHandler DeviceArrived;
        /// <summary>Event called when a device is removed</summary>
        public event EventHandler DeviceRemoved;

        #endregion

        enum ShortcutKeys
        {
            Reset = 0,
            NextBuzz = 1,
            Countdown = 2,
            LightCheck = 3,
            PreviousBuzz = 4
        }

        public Dictionary<string, SoundPlayer> soundFiles;
        public SoundPlayer easterEggSound;
        public SoundPlayer errorSound;

        private Random random = new Random();

        public readonly Color NewBuzzColor = Color.Red;
        public readonly Color NextBuzzColor = Color.Green;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private Stack<Player> previousBuzzStack;

        /// <summary>
        /// Whether a modal dialog is open.  When this is the case,
        /// ignore things like buzzes and hotkey presses.
        /// </summary>
        private bool isModalDialogOpen;

        private SettingsWrapper settings;

        private event SettingsChangedEventHandler onSettingsChanged;
        private event FormClosedEventHandler onFormClosed;

        private Player BuzzedInPlayer;
        private bool isFirstBuzzedInPlayer;

        private int bonusCountDownTime;
        private System.Timers.Timer bonusCountDownTimer;

        private System.Timers.Timer buzzLightTimer;

        private DateTime lastEasterEggSound = DateTime.Now;

        /// <summary>
        /// Accessor for device class guid
        /// </summary>
        public Guid DeviceClassGuid
        {
            get
            {
                return deviceClassGuid;
            }
        }

        private ToolTip resetToolTip;
        private ToolTip nextBuzzToolTip;
        private ToolTip lightCheckToolTip;
        private ToolTip bonusCountdownToolTip;
        private ToolTip settingsToolTip;
        private ToolTip helpToolTip;
        private ToolTip buzzersDetectedToolTip;
        private ToolTip closeToolTip;
        private ToolTip refreshToolTip;
        private ToolTip showLastBuzzToolTip;

        public List<Buzzer> Buzzers
        {
            get;
            set;
        }

        /// <summary>
        /// List of queued buzzes in singles mode
        /// </summary>
        public List<Player> QueuedPlayers
        {
            get;
            set;
        }

        public MainWindow()
        {
            InitializeComponent();
            this.QueuedPlayers = new List<Player>();
            this.CheckIfBuzzHandsetsPresent();
            this.settings = SettingsWrapper.LoadFromAppConfig();
            this.resetToolTip = new ToolTip();
            this.resetToolTip.SetToolTip(this.imgMinReset, "Reset Buzzer, Get Rid of All Buzzes in Queue");
            this.nextBuzzToolTip = new ToolTip();
            this.nextBuzzToolTip.SetToolTip(this.imgMinNextBuzz, "Next Buzz in Queue");
            this.settingsToolTip = new ToolTip();
            this.settingsToolTip.SetToolTip(this.imgMinSettings, "Settings");
            this.lightCheckToolTip = new ToolTip();
            this.lightCheckToolTip.SetToolTip(this.imgMinLightCheck, "Briefly light all of the buzzers to test that their lights are working.");
            this.bonusCountdownToolTip = new ToolTip();
            this.bonusCountdownToolTip.SetToolTip(this.imgMinBonusCountdown, "Start countdown");
            this.closeToolTip = new ToolTip();
            this.closeToolTip.SetToolTip(this.imgMinExit, "Exit");
            this.buzzersDetectedToolTip = new ToolTip();
            this.buzzersDetectedToolTip.SetToolTip(this.lblBuzzersDetected, "# of Buzzers Detected");
            this.helpToolTip = new ToolTip();
            this.helpToolTip.SetToolTip(this.imgMinHelp, "Help");
            this.lblMinBonusCountdown.Visible = false;
            this.refreshToolTip = new ToolTip();
            this.refreshToolTip.SetToolTip(this.imgMinRefresh, "Check for newly plugged in buzzers");
            this.showLastBuzzToolTip = new ToolTip();
            this.showLastBuzzToolTip.SetToolTip(this.imgMinForceLastLight, "Light up who last buzzed");
            this.previousBuzzStack = new Stack<Player>();

            this.soundFiles = new Dictionary<string, SoundPlayer>();
            for (int i = 1; i <= 8; i++)
            {
                SoundPlayer player = new SoundPlayer();
                player.SoundLocation = Path.Combine("Sounds", "beep" + i + ".wav");
                try
                {
                    player.Load();
                }
                catch (Exception)
                {
                }

                this.soundFiles.Add("beep" + i + ".wav", player);
            }

            this.easterEggSound = new SoundPlayer();
            this.easterEggSound.SoundLocation = Path.Combine("Sounds", "easteregg.wav");
            try
            {
                this.easterEggSound.Load();
            }
            catch (Exception)
            {
            }

            this.errorSound = new SoundPlayer();
            this.errorSound.SoundLocation = Path.Combine("Sounds", "error.wav");
            try
            {
                this.errorSound.Load();
            }
            catch (Exception)
            {
            }

            this.ApplySettings();
            this.ResetAllLights();
            this.onSettingsChanged += new SettingsChangedEventHandler(OnSettingsChanged);
            this.onFormClosed += new FormClosedEventHandler(OnFormClosed);
        }

        private void OnFormClosed(object sender, EventArgs args)
        {
            this.Show();
        }

        private void MainWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public void ApplySettings()
        {
            this.ShowInTaskbar = this.settings.ShowInTaskBar;
            this.TopMost = this.settings.AlwaysOnTop;

            WindowsShell.UnregisterHotKey(this, (int)ShortcutKeys.Reset);
            WindowsShell.UnregisterHotKey(this, (int)ShortcutKeys.NextBuzz);
            WindowsShell.UnregisterHotKey(this, (int)ShortcutKeys.Countdown);
            WindowsShell.UnregisterHotKey(this, (int)ShortcutKeys.LightCheck);
            WindowsShell.UnregisterHotKey(this, (int)ShortcutKeys.PreviousBuzz);

            string newToolTip = "Reset buzzer and clear all buzzes in queue";
            if (!string.IsNullOrWhiteSpace(this.settings.ResetShortcutKey) && this.settings.ResetShortcutKey != SettingsWrapper.NoneShortcut)
            {
                newToolTip += " (";
                Keys shortcutKey = (Keys)Enum.Parse(typeof(Keys), this.settings.ResetShortcutKey);

                if (this.settings.ResetShortcutShift)
                {
                    shortcutKey |= Keys.Shift;
                    newToolTip += "Shift + ";
                }

                if (this.settings.ResetShortcutControl)
                {
                    shortcutKey |= Keys.Control;
                    newToolTip += "Control + ";
                }

                newToolTip += this.settings.ResetShortcutKey + ")";
                WindowsShell.RegisterHotKey(this, shortcutKey, (int)ShortcutKeys.Reset);
            }

            this.resetToolTip.RemoveAll();
            this.resetToolTip.SetToolTip(this.imgMinReset, newToolTip);

            newToolTip = "Next buzz in queue";
            if (!string.IsNullOrWhiteSpace(this.settings.NextBuzzShortcutKey) && this.settings.NextBuzzShortcutKey != SettingsWrapper.NoneShortcut)
            {
                newToolTip += " (";
                Keys shortcutKey = (Keys)Enum.Parse(typeof(Keys), this.settings.NextBuzzShortcutKey);

                if (this.settings.NextBuzzShortcutShift)
                {
                    shortcutKey |= Keys.Shift;
                    newToolTip += "Shift + ";
                }

                if (this.settings.NextBuzzShortcutControl)
                {
                    shortcutKey |= Keys.Control;
                    newToolTip += "Control + ";
                }

                newToolTip += this.settings.NextBuzzShortcutKey + ")";
                WindowsShell.RegisterHotKey(this, shortcutKey, (int)ShortcutKeys.NextBuzz);
            }

            this.nextBuzzToolTip.RemoveAll();
            this.nextBuzzToolTip.SetToolTip(this.imgMinNextBuzz, newToolTip);

            newToolTip = "Start countdown";
            if (!string.IsNullOrWhiteSpace(this.settings.CountdownShortcutKey) && this.settings.CountdownShortcutKey != SettingsWrapper.NoneShortcut)
            {
                newToolTip += " (";
                Keys shortcutKey = (Keys)Enum.Parse(typeof(Keys), this.settings.CountdownShortcutKey);

                if (this.settings.CountdownShortcutShift)
                {
                    shortcutKey |= Keys.Shift;
                    newToolTip += "Shift + ";
                }

                if (this.settings.CountdownShortcutControl)
                {
                    shortcutKey |= Keys.Control;
                    newToolTip += "Control + ";
                }

                newToolTip += this.settings.CountdownShortcutKey + ")";
                WindowsShell.RegisterHotKey(this, shortcutKey, (int)ShortcutKeys.Countdown);
            }

            this.bonusCountdownToolTip.RemoveAll();
            this.bonusCountdownToolTip.SetToolTip(this.imgMinBonusCountdown, newToolTip);

            newToolTip = "Briefly light all of the buzzers to test that their lights are working";
            if (!string.IsNullOrWhiteSpace(this.settings.LightCheckShortcutKey) && this.settings.LightCheckShortcutKey != SettingsWrapper.NoneShortcut)
            {
                newToolTip += " (";
                Keys shortcutKey = (Keys)Enum.Parse(typeof(Keys), this.settings.LightCheckShortcutKey);

                if (this.settings.LightCheckShortcutShift)
                {
                    shortcutKey |= Keys.Shift;
                    newToolTip += "Shift + ";
                }

                if (this.settings.LightCheckShortcutControl)
                {
                    shortcutKey |= Keys.Control;
                    newToolTip += "Control + ";
                }

                newToolTip += this.settings.LightCheckShortcutKey + ")";
                WindowsShell.RegisterHotKey(this, shortcutKey, (int)ShortcutKeys.LightCheck);
            }

            this.lightCheckToolTip.RemoveAll();
            this.lightCheckToolTip.SetToolTip(this.imgMinLightCheck, newToolTip);

            newToolTip = "Light up who last buzzed";
            if (!string.IsNullOrWhiteSpace(this.settings.PreviousBuzzShortcutKey) && this.settings.PreviousBuzzShortcutKey != SettingsWrapper.NoneShortcut)
            {
                newToolTip += " (";
                Keys shortcutKey = (Keys)Enum.Parse(typeof(Keys), this.settings.PreviousBuzzShortcutKey);

                if (this.settings.PreviousBuzzShortcutShift)
                {
                    shortcutKey |= Keys.Shift;
                    newToolTip += "Shift + ";
                }

                if (this.settings.PreviousBuzzShortcutControl)
                {
                    shortcutKey |= Keys.Control;
                    newToolTip += "Control + ";
                }

                newToolTip += this.settings.PreviousBuzzShortcutKey + ")";
                WindowsShell.RegisterHotKey(this, shortcutKey, (int)ShortcutKeys.PreviousBuzz);
            }

            this.showLastBuzzToolTip.RemoveAll();
            this.showLastBuzzToolTip.SetToolTip(this.imgMinForceLastLight, newToolTip);
        }

        /// <summary>
        /// Override WndProc to handle incoming Windows messages.
        /// </summary>
        /// <param name="m">Message from Windows</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Win32Usb.WM_DEVICECHANGE)	// we got a device change message! A USB device was inserted or removed
            {
                switch (m.WParam.ToInt32())	// Check the W parameter to see if a device was inserted or removed
                {
                    case Win32Usb.DEVICE_ARRIVAL:	// inserted
                        OnDeviceArrived(new EventArgs());
                        break;
                    case Win32Usb.DEVICE_REMOVECOMPLETE:	// removed
                        OnDeviceRemoved(new EventArgs());
                        break;
                }
            }
            else if (m.Msg == WindowsShell.WM_HOTKEY)
            {
                if (!this.isModalDialogOpen)
                {
                    int id = m.WParam.ToInt32();
                    ShortcutKeys key = (ShortcutKeys)id;
                    switch (key)
                    {
                        case ShortcutKeys.Reset:
                            this.Reset();
                            break;
                        case ShortcutKeys.NextBuzz:
                            this.NextBuzz();
                            break;
                        case ShortcutKeys.LightCheck:
                            this.LightCheck();
                            break;
                        case ShortcutKeys.Countdown:
                            this.StartBonusCountdown();
                            break;
                        case ShortcutKeys.PreviousBuzz:
                            this.LightPreviousBuzz();
                            break;
                    }
                }
            }

            base.WndProc(ref m);	// pass message on to base form
        }

        /// <summary>
        /// Override called when the window handle has been created.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            usbEventHandle = Win32Usb.RegisterForUsbEvents(Handle, DeviceClassGuid);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ResetAllLights();

                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }


        /// <summary>
        /// Advances to the next buzz when buzz queueing is being used (i.e. for a
        /// tossup-only game).
        /// </summary>
        public void NextBuzz()
        {
            if (this.QueuedPlayers.Count > 0)
            {
                this.QueuedPlayers.RemoveAt(0);
                this.UpdateLightsForNextInQueue();
            }
            else
            {
                this.ResetAllLights();
            }
        }

        /// <summary>
        /// Called when a new device is detected
        /// </summary>
        protected void OnDeviceArrived(EventArgs args)
        {
            if (DeviceArrived != null)
            {
                DeviceArrived(this, args);
            }

            if (this.Buzzers == null || this.Buzzers.Count == 0)
            {
                CheckIfBuzzHandsetsPresent();
            }

            this.SafePlayErrorSound();
        }

        /// <summary>
        /// Overridable 'On' method called when a device is removed
        /// </summary>
        protected void OnDeviceRemoved(EventArgs args)
        {
            if (DeviceRemoved != null)
            {
                DeviceRemoved(this, args);
            }

            this.SafePlayErrorSound();
        }

        public void Reset(bool resetLights = true)
        {
            this.ResetAllLights();
        }

        private void ResetAllLights()
        {
            this.BuzzedInPlayer = null;
            this.QueuedPlayers.Clear();
            this.lblMinQuestionController.Text = "Clear";
            this.BackColor = Color.White;

            foreach (Buzzer buzzer in this.Buzzers)
            {
                buzzer.SetLights(false, false, false, false);
            }
        }

        private void FlashScreen(Color color)
        {
            if (this.settings.Blink)
            {
                FlashWindow flashWindow = new FlashWindow();
                flashWindow.BackColor = color;
                flashWindow.TopMost = true;
                flashWindow.Show();
                flashWindow.WindowState = FormWindowState.Maximized;
                System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(50));
                flashWindow.Close();
            }
        }

        private void AddPlayerToBuzzerQueue(Player player)
        {
            if (QueuedPlayers.Contains(player))
            {
                return;
            }
            else
            {
                QueuedPlayers.Add(player);

                // Play a sound and light up the lights if this is the first player to buzz
                if (this.QueuedPlayers.Count == 1)
                {
                    this.FlashScreen(this.NewBuzzColor);
                    this.SafePlaySound(player.BuzzerIndex);
                    this.BuzzedInPlayer = player;
                    this.isFirstBuzzedInPlayer = true;
                    this.previousBuzzStack.Push(this.BuzzedInPlayer);
                    this.lblMinQuestionController.Text = "Buzz";
                    this.BackColor = Color.Red;
                    this.LightUpActivePlayer();
                }
            }
        }

        private void BuzzerCancel(Player player)
        {
            if (this.settings.AllowBuzzerCancels)
            {
                if (!QueuedPlayers.Contains(player))
                {
                    return;
                }

                if (this.BuzzedInPlayer == player)
                {
                    if (!this.isFirstBuzzedInPlayer || this.settings.FirstPlayerCanCancel)
                    {
                        this.NextBuzz();
                    }
                }
                else
                {
                    this.QueuedPlayers.Remove(player);
                }
            }
        }

        private void HandleRegularGameBuzzerInput(Player player, ButtonStates button)
        {
            if (!this.isModalDialogOpen)
            {
                // Did the player hit the buzz in button?
                if (button.Red)
                {
                    this.AddPlayerToBuzzerQueue(player);
                }
                else if (player.EasterEggSequence == 0 && player.DidEasterEggTimeEllapse())
                {
                    if (button.Blue && !button.Yellow && !button.Orange && !button.Green)
                    {
                        player.IncrementEasterEggValue();
                    }
                    else
                    {
                        player.EasterEggSequence = 0;
                    }
                }
                else if (player.EasterEggSequence == 1 && player.DidEasterEggTimeEllapse())
                {
                    if (button.Yellow && !button.Blue && !button.Orange && !button.Green)
                    {
                        player.IncrementEasterEggValue();
                    }
                    else
                    {
                        player.EasterEggSequence = 0;
                    }
                }
                else if (player.EasterEggSequence == 2 && player.DidEasterEggTimeEllapse())
                {
                    if (button.Blue && !button.Yellow && !button.Orange && !button.Green)
                    {
                        player.IncrementEasterEggValue();
                    }
                    else
                    {
                        player.EasterEggSequence = 0;
                    }
                }
                else if (player.EasterEggSequence == 3 && player.DidEasterEggTimeEllapse())
                {
                    if (button.Orange && !button.Blue && !button.Yellow && !button.Green)
                    {
                        player.IncrementEasterEggValue();
                    }
                    else
                    {
                        player.EasterEggSequence = 0;
                    }
                }
                else if (player.EasterEggSequence == 1 && player.DidEasterEggTimeEllapse())
                {
                    if (button.Green && !button.Blue && !button.Orange && !button.Yellow)
                    {
                        player.IncrementEasterEggValue();
                        this.SafePlayEasterEggSound();
                        player.EasterEggSequence = 0;
                    }
                    else
                    {
                        player.EasterEggSequence = 0;
                    }
                }

                if (button.Blue || button.Orange || button.Green || button.Yellow)
                {
                    this.BuzzerCancel(player);
                }
            }
        }

        /// <summary>
        /// Lights up the first player in the queue for tossup-only games.
        /// </summary>
        private void UpdateLightsForNextInQueue()
        {
            // Turn off the lights for everyone except for the active player
            int activeBuzzerIndex = -1;
            int activeSubBuzzerIndex = -1;
            if (this.QueuedPlayers.Count > 0)
            {
                activeBuzzerIndex = this.QueuedPlayers[0].BuzzerIndex;
                activeSubBuzzerIndex = this.QueuedPlayers[0].SubBuzzerIndex;
                this.BuzzedInPlayer = this.QueuedPlayers[0];
                this.isFirstBuzzedInPlayer = false;
                this.previousBuzzStack.Push(this.BuzzedInPlayer);
                this.FlashScreen(NextBuzzColor);
                this.SafePlaySound(activeBuzzerIndex);
                this.LightUpActivePlayer();
            }
            else
            {
                this.ResetAllLights();
            }
        }

        private void LightUpActivePlayer()
        {
            if (this.BuzzedInPlayer != null)
            {
                this.lblMinQuestionController.Text = "Buzz";
                this.BackColor = Color.Red;
                foreach (Buzzer buzzer in this.Buzzers)
                {
                    bool[] lights = new bool[4];
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i] = false;
                    }

                    if (buzzer.BuzzerIndex == this.BuzzedInPlayer.BuzzerIndex)
                    {
                        lights[this.BuzzedInPlayer.SubBuzzerIndex] = true;
                    }

                    buzzer.SetLights(lights[0], lights[1], lights[2], lights[3]);
                }

                if (this.buzzLightTimer == null)
                {
                    this.buzzLightTimer = new System.Timers.Timer();
                    this.buzzLightTimer.Elapsed += new ElapsedEventHandler(this.OnKeepLightActive);
                    this.buzzLightTimer.Interval = 2000;
                    this.buzzLightTimer.Start();
                }
            }
        }

        /// <summary>
        /// Buzz device button pressed or released : Called when read terminates normally on the background
        /// thread from the async read. Must use standard invokerequired/invoke practice.
        /// </summary>
        private void BuzzDevice_OnButtonChanged(object sender, BuzzButtonChangedEventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(new BuzzButtonChangedEventHandler(BuzzDevice_OnButtonChanged), new object[] { sender, args });
            }
            else
            {
                Buzzer buzzer = (Buzzer)sender;
                int subBuzzerIndex = 0;
                List<int> indexes = new List<int>();
                indexes.Add(0);
                indexes.Add(1);
                indexes.Add(2);
                indexes.Add(3);

                while (indexes.Count > 0)
                {
                    // Randomly go through players to ensure that Player 1 doesn't have the advantage in outright ties
                    subBuzzerIndex = indexes[random.Next(indexes.Count)];
                    indexes.Remove(subBuzzerIndex);
                    ButtonStates button = args.Buttons[subBuzzerIndex];
                    Player player = buzzer.Players[subBuzzerIndex];
                    HandleRegularGameBuzzerInput(player, button);
                }
            }
        }

        /// <summary>
        /// Buzz device has been removed : Called when read terminates with an exception on the background
        /// thread from the async read. Must use standard invokerequired/invoke practice.
        /// </summary>
        private void BuzzDevice_OnDeviceRemoved(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(BuzzDevice_OnDeviceRemoved), new object[] { sender, e });
            }

            // Reload the buzzers that remain
            CheckIfBuzzHandsetsPresent();

            this.SafePlayErrorSound();
        }

        /// <summary>
        /// Checks to see if there the device has been plugged in
        /// </summary>
        private void CheckIfBuzzHandsetsPresent()
        {
            if (this.Buzzers != null)
            {
                foreach (Buzzer buzzer in this.Buzzers)
                {
                    if (buzzer.OutputFile != null)
                    {
                        buzzer.Dispose();
                    }
                }
            }

            try
            {
                List<Buzzer> newBuzzers = Buzzer.FindBuzzHandsets();

                int index = 0;
                foreach (Buzzer buzzer in newBuzzers)
                {
                    buzzer.OnDeviceRemoved += new EventHandler(BuzzDevice_OnDeviceRemoved);
                    buzzer.OnButtonChanged += new BuzzButtonChangedEventHandler(BuzzDevice_OnButtonChanged);
                    buzzer.BuzzerIndex = index;
                    index++;
                }

                this.lblBuzzersDetected.Text = (index * 4).ToString();
                this.Buzzers = newBuzzers;
                this.previousBuzzStack = new Stack<Player>();
                this.Reset();
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong finding the buzzers.  Try plugging the buzzers back in and/or restarting the program in administrator mode.  Refer to the help screen for more troubleshooting tips.");
                this.Buzzers = new List<Buzzer>();
                this.previousBuzzStack = new Stack<Player>();
            }
        }

        private void SafePlayEasterEggSound()
        {
            if (DateTime.Now.Subtract(this.lastEasterEggSound).TotalSeconds > 10)
            {
                try
                {
                    this.lastEasterEggSound = DateTime.Now;
                    this.easterEggSound.Play();
                }
                catch (Exception)
                {
                    Console.WriteLine("error");
                }
            }
        }

        private void SafePlayErrorSound()
        {
            try
            {
                this.errorSound.Play();
            }
            catch (Exception)
            {
                Console.WriteLine("error");
            }
        }

        private void SafePlaySound(int buzzerGroup)
        {
            try
            {
                this.soundFiles[this.settings.BuzzSounds[buzzerGroup]].Play();
            }
            catch (Exception)
            {
                // It's possible that playing the sound will fail
            }
        }

        private void OnSettingsChanged(object sender, SettingsCallbackEventArgs args)
        {
            this.isModalDialogOpen = false;
            this.settings = args.ModifiedSettings;
            this.ApplySettings();
        }

        private void StartBonusCountdown()
        {
            if (this.lblMinBonusCountdown.Visible)
            {
                // Stop the countdown
                this.lblMinBonusCountdown.Visible = false;
                this.bonusCountDownTimer.Stop();
            }
            else
            {
                // Start the countdown
                this.lblMinBonusCountdown.Visible = true;
                this.lblMinBonusCountdown.Text = this.settings.CountdownLengthInSeconds.ToString();
                this.bonusCountDownTime = this.settings.CountdownLengthInSeconds;
                this.bonusCountDownTimer = new System.Timers.Timer();
                this.bonusCountDownTimer.Elapsed += new ElapsedEventHandler(this.OnBonusTimerElapsed);
                this.bonusCountDownTimer.Interval = 1000;
                this.bonusCountDownTimer.Start();
            }
        }

        private void OnBonusTimerElapsed(object source, ElapsedEventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(new ElapsedEventHandler(this.OnBonusTimerElapsed), new object[] { source, args });
            }
            else
            {
                this.bonusCountDownTime--;
                this.lblMinBonusCountdown.Text = this.bonusCountDownTime.ToString();
                if (this.bonusCountDownTime == 0)
                {
                    this.lblMinBonusCountdown.Visible = false;
                    this.bonusCountDownTimer.Stop();
                }
            }
        }

        private void imgMinHelp_Click(object sender, EventArgs e)
        {
            this.isModalDialogOpen = true;
            About about = new About(this.onFormClosed);
            this.Hide();
            about.ShowDialog();
        }

        private void imgMinNextBuzz_Click(object sender, EventArgs e)
        {
            this.NextBuzz();
        }

        private void imgMinReset_Click(object sender, EventArgs e)
        {
            this.Reset();
        }

        private void imgMinExit_Click(object sender, EventArgs e)
        {
            if (!this.settings.QuitPrompt || MessageBox.Show("Are you sure you want to quit?", "PACEBuzz", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Reset();
                Environment.Exit(0);
            }
        }

        private void imgMinBonusCountdown_Click(object sender, EventArgs e)
        {
            this.StartBonusCountdown();
        }

        private void imgMinSettings_Click(object sender, EventArgs e)
        {
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            this.isModalDialogOpen = true;
            SettingsForm settings = new SettingsForm(this.settings, this.onSettingsChanged, this.onFormClosed);
            this.Hide();
            settings.ShowDialog();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Reset();
            this.Dispose();
        }

        private void imgMinLightCheck_Click(object sender, EventArgs e)
        {
            this.LightCheck();
        }

        private void LightCheck()
        {
            this.Reset();

            foreach (Buzzer buzzer in this.Buzzers)
            {
                for (int i = 0; i < 4; i++)
                {
                    bool[] lights = new bool[4];
                    lights[i] = true;
                    buzzer.SetLights(lights[0], lights[1], lights[2], lights[3]);
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(500));
                }

                this.Reset();
            }
        }

        private void imgMinForceLastLight_Click(object sender, EventArgs e)
        {
            this.LightPreviousBuzz();
        }

        private void LightPreviousBuzz()
        {
            if (this.previousBuzzStack.Count > 0)
            {
                this.Reset();
                this.BuzzedInPlayer = previousBuzzStack.Pop();
                this.isFirstBuzzedInPlayer = false;
                this.FlashScreen(NextBuzzColor);
                this.SafePlaySound(this.BuzzedInPlayer.BuzzerIndex);
                this.LightUpActivePlayer();
            }
        }

        private void imgMinRefresh_Click(object sender, EventArgs e)
        {
            this.CheckIfBuzzHandsetsPresent();
        }

        private void OnKeepLightActive(object source, ElapsedEventArgs args)
        {
            if (this.BuzzedInPlayer != null)
            {
                this.LightUpActivePlayer();
            }
            else
            {
                this.buzzLightTimer.Stop();
                this.buzzLightTimer = null;
            }
        }
    }

    public delegate void FormClosedEventHandler(object sender, EventArgs args);

    public struct ShortcutKeyTuple
    {
        public Keys ShortcutKey
        {
            get;
            set;
        }

        public int ID
        {
            get;
            set;
        }

        public Button Button
        {
            get;
            set;
        }
    }
}
