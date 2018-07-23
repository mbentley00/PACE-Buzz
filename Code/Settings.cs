using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace PACEBuzz
{
    public class SettingsWrapper : ApplicationSettingsBase
    {
        /// <summary>
        /// Maximum number of buzzer groups supported (i.e. 32 total)
        /// </summary>
        public const int BuzzerLimit = 8;

        /// <summary>
        /// Name for an unassigned shortcut
        /// </summary>
        public const string NoneShortcut = "NONE";

        public List<string> BuzzSounds
        {
            get; set;
        }

        public string ResetShortcutKey
        {
            get;
            set;
        }

        public bool ResetShortcutShift
        {
            get;
            set;
        }

        public bool ResetShortcutControl
        {
            get;
            set;
        }

        public string NextBuzzShortcutKey
        {
            get;
            set;
        }

        public bool NextBuzzShortcutShift
        {
            get;
            set;
        }

        public bool NextBuzzShortcutControl
        {
            get;
            set;
        }

        public string CountdownShortcutKey
        {
            get;
            set;
        }

        public bool CountdownShortcutShift
        {
            get;
            set;
        }

        public bool CountdownShortcutControl
        {
            get;
            set;
        }

        public string LightCheckShortcutKey
        {
            get;
            set;
        }

        public bool LightCheckShortcutShift
        {
            get;
            set;
        }

        public bool LightCheckShortcutControl
        {
            get;
            set;
        }

        public string PreviousBuzzShortcutKey
        {
            get;
            set;
        }

        public bool PreviousBuzzShortcutShift
        {
            get;
            set;
        }

        public bool PreviousBuzzShortcutControl
        {
            get;
            set;
        }

        public bool AlwaysOnTop
        {
            get;
            set;
        }

        public bool Blink
        {
            get;
            set;
        }

        public int CountdownLengthInSeconds
        {
            get;
            set;
        }

        public bool AllowBuzzerCancels
        {
            get;
            set;
        }

        public bool FirstPlayerCanCancel
        {
            get;
            set;
        }

        public bool ShowInTaskBar
        {
            get;
            set;
        }

        public bool QuitPrompt
        {
            get;
            set;
        }

        public SettingsWrapper()
        {
        }

        public void WriteToAppConfigFile()
        {
            StringCollection stringCollection = new StringCollection();
            stringCollection.AddRange(this.BuzzSounds.ToArray());

            PACEBuzz.Properties.Settings.Default.buzzSounds = stringCollection;
            PACEBuzz.Properties.Settings.Default.resetShortcutKey = this.ResetShortcutKey;
            PACEBuzz.Properties.Settings.Default.resetShortcutShift = this.ResetShortcutShift;
            PACEBuzz.Properties.Settings.Default.resetShortcutControl = this.ResetShortcutControl;
            PACEBuzz.Properties.Settings.Default.nextBuzzShortcutKey = this.NextBuzzShortcutKey;
            PACEBuzz.Properties.Settings.Default.nextBuzzShortcutShift = this.NextBuzzShortcutShift;
            PACEBuzz.Properties.Settings.Default.nextBuzzShortcutControl = this.NextBuzzShortcutControl;
            PACEBuzz.Properties.Settings.Default.countdownShortcutKey = this.CountdownShortcutKey;
            PACEBuzz.Properties.Settings.Default.countdownShortcutShift = this.CountdownShortcutShift;
            PACEBuzz.Properties.Settings.Default.countdownShortcutControl = this.CountdownShortcutControl;
            PACEBuzz.Properties.Settings.Default.lightCheckShortcutKey = this.LightCheckShortcutKey;
            PACEBuzz.Properties.Settings.Default.lightCheckShortcutShift = this.LightCheckShortcutShift;
            PACEBuzz.Properties.Settings.Default.lightCheckShortcutControl = this.LightCheckShortcutControl;
            PACEBuzz.Properties.Settings.Default.previousBuzzShortcutKey = this.PreviousBuzzShortcutKey;
            PACEBuzz.Properties.Settings.Default.previousBuzzShortcutShift = this.PreviousBuzzShortcutShift;
            PACEBuzz.Properties.Settings.Default.previousBuzzShortcutControl = this.PreviousBuzzShortcutControl;
            PACEBuzz.Properties.Settings.Default.alwaysOnTop = this.AlwaysOnTop;
            PACEBuzz.Properties.Settings.Default.blink = this.Blink;
            PACEBuzz.Properties.Settings.Default.allowBuzzerCancels = this.AllowBuzzerCancels;
            PACEBuzz.Properties.Settings.Default.showInTaskBar = this.ShowInTaskBar;
            PACEBuzz.Properties.Settings.Default.countdownLengthInSeconds = this.CountdownLengthInSeconds;
            PACEBuzz.Properties.Settings.Default.firstPlayerCanCancel = this.FirstPlayerCanCancel;
            PACEBuzz.Properties.Settings.Default.quitPrompt = this.QuitPrompt;

            PACEBuzz.Properties.Settings.Default.Save();
        }

        public static SettingsWrapper LoadFromAppConfig()
        {
            SettingsWrapper s = new SettingsWrapper();
            s.BuzzSounds = PACEBuzz.Properties.Settings.Default.buzzSounds.Cast<string>().ToList();
            s.ResetShortcutKey = PACEBuzz.Properties.Settings.Default.resetShortcutKey;
            s.ResetShortcutShift = PACEBuzz.Properties.Settings.Default.resetShortcutShift;
            s.ResetShortcutControl = PACEBuzz.Properties.Settings.Default.resetShortcutControl;
            s.NextBuzzShortcutKey = PACEBuzz.Properties.Settings.Default.nextBuzzShortcutKey;
            s.NextBuzzShortcutShift = PACEBuzz.Properties.Settings.Default.nextBuzzShortcutShift;
            s.NextBuzzShortcutControl = PACEBuzz.Properties.Settings.Default.nextBuzzShortcutControl;
            s.CountdownShortcutKey = PACEBuzz.Properties.Settings.Default.countdownShortcutKey;
            s.CountdownShortcutShift = PACEBuzz.Properties.Settings.Default.countdownShortcutShift;
            s.CountdownShortcutControl = PACEBuzz.Properties.Settings.Default.countdownShortcutControl;
            s.LightCheckShortcutKey = PACEBuzz.Properties.Settings.Default.lightCheckShortcutKey;
            s.LightCheckShortcutShift = PACEBuzz.Properties.Settings.Default.lightCheckShortcutShift;
            s.LightCheckShortcutControl = PACEBuzz.Properties.Settings.Default.lightCheckShortcutControl;
            s.PreviousBuzzShortcutKey = PACEBuzz.Properties.Settings.Default.previousBuzzShortcutKey;
            s.PreviousBuzzShortcutShift = PACEBuzz.Properties.Settings.Default.previousBuzzShortcutShift;
            s.PreviousBuzzShortcutControl = PACEBuzz.Properties.Settings.Default.previousBuzzShortcutControl;
            s.AlwaysOnTop = PACEBuzz.Properties.Settings.Default.alwaysOnTop;
            s.Blink = PACEBuzz.Properties.Settings.Default.blink;
            s.AllowBuzzerCancels = PACEBuzz.Properties.Settings.Default.allowBuzzerCancels;
            s.ShowInTaskBar = PACEBuzz.Properties.Settings.Default.showInTaskBar;
            s.CountdownLengthInSeconds = PACEBuzz.Properties.Settings.Default.countdownLengthInSeconds;
            s.QuitPrompt = PACEBuzz.Properties.Settings.Default.quitPrompt;
            s.FirstPlayerCanCancel = PACEBuzz.Properties.Settings.Default.firstPlayerCanCancel;

            return s;
        }
    }
}
