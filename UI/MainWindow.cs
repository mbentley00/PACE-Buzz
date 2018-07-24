using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using SharpDX.DirectInput;

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

        public List<string> soundFiles;
        public string errorSound;

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

        private System.Timers.Timer arcadeBuzzTimer;

        private System.Timers.Timer buzzLightTimer;

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

        // The current arcade joystick
        private Joystick arcadeJoystick = null;

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

        /// <summary>
        /// Text that will be displayed in lblMinQuestionController
        /// </summary>
        private string questionControllerText = "Clear";

        public MainWindow()
        {
            InitializeComponent();
            this.QueuedPlayers = new List<Player>();
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
            this.buzzersDetectedToolTip.SetToolTip(this.lblBuzzersDetected, "# of Buzzers Detected. In arcade mode, this may include more than are plugged into control board");
            this.helpToolTip = new ToolTip();
            this.helpToolTip.SetToolTip(this.imgMinHelp, "Help");
            this.lblMinBonusCountdown.Visible = false;
            this.refreshToolTip = new ToolTip();
            this.refreshToolTip.SetToolTip(this.imgMinRefresh, "Check for newly plugged in buzzers");
            this.showLastBuzzToolTip = new ToolTip();
            this.showLastBuzzToolTip.SetToolTip(this.imgMinForceLastLight, "Light up who last buzzed");
            this.previousBuzzStack = new Stack<Player>();

            this.ApplySettings();
            this.onSettingsChanged += new SettingsChangedEventHandler(OnSettingsChanged);
            this.onFormClosed += new FormClosedEventHandler(OnFormClosed);

            // Initialize sounds
            this.errorSound = Path.Combine("Sounds", "error.wav");

            this.soundFiles = new List<string>();
            for (int i = 1; i <= 8; i++)
            {
                this.soundFiles.Add(Path.Combine("Sounds", "beep" + i + ".wav"));
            }

            this.CheckIfBuzzHandsetsPresent();
            this.ResetAllLights();

            // Play the buzz on load to get the sound cached
            this.SafePlaySound(this.soundFiles[7]);
        }

        /// <summary>
        /// Set up the inputs for running in arcade board mode
        /// </summary>
        private void SetupArcadeInputs()
        {
            this.Buzzers = new List<Buzzer>();

            this.arcadeJoystick = null;

            // Initialize DirectInput
            var directInput = new DirectInput();

            // Find a Joystick Guid
            var joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                joystickGuid = deviceInstance.InstanceGuid;
            }

            if (joystickGuid != Guid.Empty)
            {
                this.arcadeJoystick = new Joystick(directInput, joystickGuid);
                this.arcadeJoystick.Properties.BufferSize = 128;
                this.arcadeJoystick.Acquire();

                int buzzerCount = 0;

                var buzzer = new Buzzer();

                // Figure out how many players there are
                foreach (var joystickObject in this.arcadeJoystick.GetObjects())
                {
                    if (joystickObject.Name != null && joystickObject.Name.Contains("Button"))
                    {
                        buzzerCount++;
                    }
                }

                this.lblBuzzersDetected.Text = buzzerCount.ToString();
                buzzer.BuzzerIndex = 0;
                buzzer.AddPlayers(buzzerCount);
                this.Buzzers.Add(buzzer);
            }

            if (this.arcadeBuzzTimer != null)
            {
                this.arcadeBuzzTimer.Stop();
                this.arcadeBuzzTimer.Dispose();
            }

            this.arcadeBuzzTimer = new System.Timers.Timer();
            this.arcadeBuzzTimer.Elapsed += new ElapsedEventHandler(this.HandleArcadeBuzzes);
            this.arcadeBuzzTimer.Interval = 1;
            this.arcadeBuzzTimer.Start();
        }

        private void OnFormClosed(object sender, EventArgs args)
        {
            this.Show();
        }

        /// <summary>
        /// Play a wav file via XAudio
        /// </summary>
        public static void PlayXAudioSound(object soundFile)
        {
            try
            {
                var xaudio2 = new XAudio2();
                var masteringVoice = new MasteringVoice(xaudio2);

                var stream = new SoundStream(File.OpenRead(soundFile as string));
                var waveFormat = stream.Format;
                var buffer = new AudioBuffer
                {
                    Stream = stream.ToDataStream(),
                    AudioBytes = (int)stream.Length,
                    Flags = BufferFlags.EndOfStream
                };
                stream.Close();

                var sourceVoice = new SourceVoice(xaudio2, waveFormat, true);
                sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
                sourceVoice.Start();

                while (sourceVoice.State.BuffersQueued > 0)
                {
                    Thread.Sleep(1);
                }

                sourceVoice.DestroyVoice();
                sourceVoice.Dispose();
                buffer.Stream.Dispose();

                xaudio2.Dispose();
                masteringVoice.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

            if (string.IsNullOrWhiteSpace(this.settings.BuzzerType))
            {
                this.settings.BuzzerType = "PS2";
            }

            if (this.settings.BuzzerType == "PS2")
            {
                this.imgMinLightCheck.Visible = true;
            }
            else
            {
                this.imgMinLightCheck.Visible = false;
            }
        }

        /// <summary>
        /// Override WndProc to handle incoming Windows messages.
        /// </summary>
        /// <param name="m">Message from Windows</param>
        protected override void WndProc(ref Message m)
        {
            this.lblMinQuestionController.Text = this.questionControllerText; // TODO

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
        /// Called when a new PS2 device is detected
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
        /// Overridable 'On' method called when a device is removed (for PS2 mode)
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
            this.questionControllerText = "Clear";
            this.BackColor = Color.White;

            if (this.settings.BuzzerType == "PS2")
            {
                foreach (Buzzer buzzer in this.Buzzers)
                {
                    buzzer.SetLights(false, false, false, false);
                }
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
                    this.BuzzedInPlayer = player;
                    this.LightUpActivePlayer();
                    this.SafePlaySound(this.soundFiles[player.BuzzerIndex]);
                    this.isFirstBuzzedInPlayer = true;
                    this.previousBuzzStack.Push(this.BuzzedInPlayer);

                    if (this.settings.BuzzerType == "PS2")
                    {
                        questionControllerText = "Buzz";
                    }
                    else
                    {
                        questionControllerText = "Buzz: " + (this.BuzzedInPlayer.SubBuzzerIndex + 1);
                    }

                    this.FlashScreen(this.NewBuzzColor);
                    this.BackColor = Color.Red;
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
                if (this.settings.BuzzerType == "PS2")
                {
                    // Did the player hit the buzz in button?
                    if (button.Red)
                    {
                        this.AddPlayerToBuzzerQueue(player);
                    }

                    if (button.Blue || button.Orange || button.Green || button.Yellow)
                    {
                        this.BuzzerCancel(player);
                    }
                }
                else
                {
                    // There's only one option for arcade mode
                    this.AddPlayerToBuzzerQueue(player);
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
                this.LightUpActivePlayer();
                this.SafePlaySound(this.soundFiles[activeBuzzerIndex]);
                this.FlashScreen(NextBuzzColor);
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
                this.BackColor = Color.Red;

                if (this.settings.BuzzerType == "PS2")
                {
                    questionControllerText = "Buzz";

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
                else
                {
                    questionControllerText = "Buzz: " + (this.BuzzedInPlayer.SubBuzzerIndex + 1);
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
                if (this.settings.BuzzerType == "PS2")
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
        }

        /// <summary>
        /// Looks for buzzes on the arcade controllers
        /// </summary>
        private void HandleArcadeBuzzes(object source, ElapsedEventArgs args)
        {
            try
            {
                this.arcadeJoystick.Poll();
                var datas = this.arcadeJoystick.GetBufferedData();
                List<int> indexes = new List<int>();

                foreach (var state in datas)
                {
                    // Get list of buzzes, if there are more than one of them, randomly pick one

                    string buzzerName = state.Offset.ToString();
                    if (buzzerName.Contains("Buttons"))
                    {
                        int buzzerIndex = Int32.Parse(buzzerName.Substring(7));
                        if (state.Value > 0)
                        {
                            indexes.Add(buzzerIndex);
                        }
                    }
                }

                // In arcade mode, there's only one buzzer - TODO: Maybe let you split them up so you get different sounds
                var buzzer = this.Buzzers[0];

                while (indexes.Count > 0)
                {
                    // Randomly go through players to ensure that Player 1 doesn't have the advantage in outright ties
                    var subBuzzerIndex = indexes[random.Next(indexes.Count)];
                    indexes.Remove(subBuzzerIndex);
                    Player player = buzzer.Players[subBuzzerIndex];
                    HandleRegularGameBuzzerInput(player, null);
                }
            }
            catch (Exception e)
            {
                // Device sometimes fails for some reason
                Console.WriteLine("buzzer exception: " + e);
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

            if (this.settings.BuzzerType == "PS2")
            {
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

                    // Get rid of any handles for the arcade buzzers
                    if (this.arcadeBuzzTimer != null)
                    {
                        this.arcadeBuzzTimer.Stop();
                        this.arcadeBuzzTimer.Dispose();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Something went wrong finding the buzzers.  Try plugging the buzzers back in and/or restarting the program in administrator mode.  Refer to the help screen for more troubleshooting tips.");
                    this.Buzzers = new List<Buzzer>();
                    this.previousBuzzStack = new Stack<Player>();
                }
            }
            else
            {
                this.SetupArcadeInputs();
                this.previousBuzzStack = new Stack<Player>();
                this.Reset();
            }
        }

        private void SafePlayErrorSound()
        {
            this.SafePlaySound(this.errorSound);
        }

        private void SafePlaySound(string soundFile)
        {
            try
            {
                Thread thread = new Thread(new ParameterizedThreadStart(PlayXAudioSound));
                thread.Start(soundFile);

                // TODO: Dispose the thread?
            }
            catch (Exception)
            {
                Console.WriteLine("Exception");
            }
        }

        private void OnSettingsChanged(object sender, SettingsCallbackEventArgs args)
        {
            this.isModalDialogOpen = false;
            this.settings = args.ModifiedSettings;
            this.ApplySettings();
            this.CheckIfBuzzHandsetsPresent();
            this.ResetAllLights();
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
            if (this.settings.BuzzerType == "PS2")
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
                this.SafePlaySound(this.soundFiles[this.BuzzedInPlayer.BuzzerIndex]);
                this.LightUpActivePlayer();
                this.FlashScreen(NextBuzzColor);
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
