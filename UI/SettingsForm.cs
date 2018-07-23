using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PACEBuzz
{
    public partial class SettingsForm : Form
    {
        private List<string> keyList = new List<string>();

        private SoundPlayer soundPlayer;

        private SettingsWrapper settings;

        public event SettingsChangedEventHandler OnSettingsChanged;

        public event FormClosedEventHandler MainFormShow;

        public const string FunctionKeyType = "FUNCTION";
        public const string DigitKeyType = "DIGIT";

        public SettingsForm(SettingsWrapper settings, SettingsChangedEventHandler onSettingsChanged, FormClosedEventHandler onFormClosed)
        {
            this.settings = settings;
            this.OnSettingsChanged = onSettingsChanged;
            this.MainFormShow += onFormClosed;

            InitializeComponent();
            keyList.Add(SettingsWrapper.NoneShortcut);
            keyList.Add("F1");
            keyList.Add("F2");
            keyList.Add("F3");
            keyList.Add("F4");
            keyList.Add("F5");
            keyList.Add("F6");
            keyList.Add("F7");
            keyList.Add("F8");
            keyList.Add("F9");
            keyList.Add("F10");
            keyList.Add("F11");
            keyList.Add("F12");
            keyList.Add("D1");
            keyList.Add("D2");
            keyList.Add("D3");
            keyList.Add("D4");
            keyList.Add("D5");
            keyList.Add("D6");
            keyList.Add("D7");
            keyList.Add("D8");
            keyList.Add("D9");
            keyList.Add("D0");
            keyList.Add("A");
            keyList.Add("B");
            keyList.Add("C");
            keyList.Add("D");
            keyList.Add("E");
            keyList.Add("F");
            keyList.Add("G");
            keyList.Add("H");
            keyList.Add("I");
            keyList.Add("J");
            keyList.Add("K");
            keyList.Add("L");
            keyList.Add("M");
            keyList.Add("N");
            keyList.Add("O");
            keyList.Add("P");
            keyList.Add("Q");
            keyList.Add("R");
            keyList.Add("S");
            keyList.Add("T");
            keyList.Add("U");
            keyList.Add("V");
            keyList.Add("W");
            keyList.Add("X");
            keyList.Add("Y");
            keyList.Add("Z");

            foreach (string value in keyList)
            {
                this.cmbResetKey.Items.Add(value);
                this.comboNextBuzzShortcutKey.Items.Add(value);
                this.cmbCountdownShortcutKey.Items.Add(value);
                this.cmbLightcheckShortcutKey.Items.Add(value);
                this.cmbPreviousBuzzShortcutKey.Items.Add(value);
            }

            for (int i = 0; i < this.cmbResetKey.Items.Count; i++)
            {
                if (this.cmbResetKey.Items[i].ToString().Equals(this.settings.ResetShortcutKey))
                {
                    this.cmbResetKey.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < this.comboNextBuzzShortcutKey.Items.Count; i++)
            {
                if (this.comboNextBuzzShortcutKey.Items[i].ToString().Equals(this.settings.NextBuzzShortcutKey))
                {
                    this.comboNextBuzzShortcutKey.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < this.cmbCountdownShortcutKey.Items.Count; i++)
            {
                if (this.cmbCountdownShortcutKey.Items[i].ToString().Equals(this.settings.CountdownShortcutKey))
                {
                    this.cmbCountdownShortcutKey.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < this.cmbLightcheckShortcutKey.Items.Count; i++)
            {
                if (this.cmbLightcheckShortcutKey.Items[i].ToString().Equals(this.settings.LightCheckShortcutKey))
                {
                    this.cmbLightcheckShortcutKey.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < this.cmbPreviousBuzzShortcutKey.Items.Count; i++)
            {
                if (this.cmbPreviousBuzzShortcutKey.Items[i].ToString().Equals(this.settings.PreviousBuzzShortcutKey))
                {
                    this.cmbPreviousBuzzShortcutKey.SelectedIndex = i;
                    break;
                }
            }

            this.chkResetShift.Checked = this.settings.ResetShortcutShift;
            this.chkResetControl.Checked = this.settings.ResetShortcutControl;
            this.chkNextBuzzShift.Checked = this.settings.NextBuzzShortcutShift;
            this.chkNextBuzzControl.Checked = this.settings.NextBuzzShortcutControl;
            this.chkCountdownShift.Checked = this.settings.CountdownShortcutShift;
            this.chkCountdownControl.Checked = this.settings.CountdownShortcutControl;
            this.chkLightCheckShift.Checked = this.settings.LightCheckShortcutShift;
            this.chkLightCheckControl.Checked = this.settings.LightCheckShortcutControl;
            this.chkPreviousBuzzShift.Checked = this.settings.PreviousBuzzShortcutShift;
            this.chkPreviousBuzzControl.Checked = this.settings.PreviousBuzzShortcutControl;
            this.chkBuzzerCancel.Checked = this.settings.AllowBuzzerCancels;
            this.chkShowInTaskbar.Checked = this.settings.ShowInTaskBar;
            this.chkFirstPlayerCanCancel.Enabled = this.settings.AllowBuzzerCancels;
            this.chkFirstPlayerCanCancel.Checked = this.settings.FirstPlayerCanCancel;
            this.chkQuitPrompt.Checked = this.settings.QuitPrompt;
            this.settings.Blink = this.chkBlink.Checked;
            this.settings.AlwaysOnTop = this.chkAlwaysOnTop.Checked;

            this.txtCountdownLength.Text = this.settings.CountdownLengthInSeconds.ToString();

            this.cmbSound.Items.Clear();
            for (int i = 1; i <= 8; i++)
            {
                this.cmbSound.Items.Add("beep" + i + ".wav");
            }

            this.cmbBuzzerGroup.Items.Clear();
            for (int i = 1; i <= 8; i++)
            {
                this.cmbBuzzerGroup.Items.Add(i);
            }

            this.cmbBuzzerGroup.SelectedIndex = 0;
            this.soundPlayer = new SoundPlayer();
        }

        private void SettingsForm_FormClosed(Object sender, FormClosedEventArgs e)
        {
            if (this.MainFormShow != null)
            {
                this.MainFormShow.Invoke(sender, e);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            int temp;
            if (Int32.TryParse(this.txtCountdownLength.Text, out temp) && temp > 0)
            {
                this.settings.CountdownLengthInSeconds = temp;
            }
            else
            {
                MessageBox.Show("Countdown Length must be a positive integer!");
                return;
            }

            // Verify that none of the shortcut keys overlap
            Dictionary<string, string> shortcutKeys = new Dictionary<string, string>();
            if (!AddKeyToShortcutDictionary(shortcutKeys, "Reset", settings.ResetShortcutKey, settings.ResetShortcutShift, settings.ResetShortcutControl)
                || !AddKeyToShortcutDictionary(shortcutKeys, "Next Buzz", settings.NextBuzzShortcutKey, settings.NextBuzzShortcutShift, settings.NextBuzzShortcutControl)
                || !AddKeyToShortcutDictionary(shortcutKeys, "Countdown", settings.CountdownShortcutKey, settings.CountdownShortcutShift, settings.CountdownShortcutControl)
                || !AddKeyToShortcutDictionary(shortcutKeys, "Light Check", settings.LightCheckShortcutKey, settings.LightCheckShortcutShift, settings.LightCheckShortcutControl)
                || !AddKeyToShortcutDictionary(shortcutKeys, "Previous Buzz", settings.PreviousBuzzShortcutKey, settings.PreviousBuzzShortcutShift, settings.PreviousBuzzShortcutControl))
            {
                return;
            }

            this.settings.WriteToAppConfigFile();

            if (this.OnSettingsChanged != null)
            {
                this.OnSettingsChanged(this, new SettingsCallbackEventArgs(this.settings));
            }

            this.Close();
        }

        private bool AddKeyToShortcutDictionary(Dictionary<string, string> dictionary, string name, string key, bool shift, bool control)
        {
            if (key == SettingsWrapper.NoneShortcut)
            {
                return true;
            }

            string formattedKey = "";
            if (shift)
            {
                formattedKey = "Shift + ";
            }

            if (control)
            {
                formattedKey += "Control + ";
            }

            formattedKey += key;

            if (dictionary.ContainsKey(formattedKey))
            {
                string existingKey = dictionary[formattedKey];
                MessageBox.Show(string.Format("Error: {0} and {1} shortcuts are both mapped to {2}.", existingKey, name, formattedKey));
                return false;
            }
            else
            {
                dictionary.Add(formattedKey, name);
                return true;
            }
        }

        private void chkResetShift_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.ResetShortcutShift = this.chkResetShift.Checked;
        }

        private void chkResetControl_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.ResetShortcutControl = this.chkResetControl.Checked;
        }

        private void chkNextBuzzShift_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.NextBuzzShortcutShift = this.chkNextBuzzShift.Checked;
        }

        private void chkNextBuzzControl_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.NextBuzzShortcutControl = this.chkNextBuzzControl.Checked;
        }

        private void chkCountdownShift_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.CountdownShortcutShift = this.chkCountdownShift.Checked;
        }

        private void chkCountdownControl_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.CountdownShortcutControl = this.chkCountdownControl.Checked;
        }

        private void chkLightCheckShift_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.LightCheckShortcutShift = this.chkLightCheckShift.Checked;
        }

        private void chkLightCheckControl_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.LightCheckShortcutControl = this.chkLightCheckControl.Checked;
        }

        private void cmbResetKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.ResetShortcutKey = cmbResetKey.Text;
        }

        private void comboNextBuzzShortcutKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.NextBuzzShortcutKey = comboNextBuzzShortcutKey.Text;
        }

        private void cmbCountdownShortcutKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.CountdownShortcutKey = cmbCountdownShortcutKey.Text;
        }

        private void cmbLightcheckShortcutKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.LightCheckShortcutKey = cmbLightcheckShortcutKey.Text;
        }

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.AlwaysOnTop = this.chkAlwaysOnTop.Checked;
        }

        private void chkBlink_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.Blink = this.chkBlink.Checked;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkBuzzerCancel_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.AllowBuzzerCancels = this.chkBuzzerCancel.Checked;
            this.chkFirstPlayerCanCancel.Enabled = this.chkBuzzerCancel.Checked;
        }

        private void chkShowInTaskbar_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.ShowInTaskBar = this.chkShowInTaskbar.Checked;
        }

        private void imgPlaySound_Click(object sender, EventArgs e)
        {
            try
            {
                this.soundPlayer.SoundLocation = Path.Combine("Sounds", this.cmbSound.Text);
                this.soundPlayer.Load();
                this.soundPlayer.Play();
            }
            catch (Exception)
            {
                MessageBox.Show("Error loading sound: " + this.cmbSound.Text);
            }
        }

        private void cmbBuzzerGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbSound.Text = this.settings.BuzzSounds[this.cmbBuzzerGroup.SelectedIndex];
        }

        private void cmbSound_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.BuzzSounds[this.cmbBuzzerGroup.SelectedIndex] = this.cmbSound.Text;
        }

        private void chkFirstPlayerCanCancel_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.FirstPlayerCanCancel = this.chkFirstPlayerCanCancel.Checked;
        }

        private void chkQuitPrompt_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.QuitPrompt = this.chkQuitPrompt.Checked;
        }

        private void chkPreviousBuzzShift_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.PreviousBuzzShortcutShift = this.chkPreviousBuzzShift.Checked;
        }

        private void chkPreviousBuzzControl_CheckedChanged(object sender, EventArgs e)
        {
            this.settings.PreviousBuzzShortcutControl = this.chkPreviousBuzzControl.Checked;
        }

        private void cmbPreviousBuzzShortcutKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.settings.PreviousBuzzShortcutKey = cmbPreviousBuzzShortcutKey.Text;
        }
    }

    public delegate void SettingsChangedEventHandler(object sender, SettingsCallbackEventArgs args);

    public class SettingsCallbackEventArgs : EventArgs
    {
        public SettingsWrapper ModifiedSettings
        {
            get;
            private set;
        }

        public SettingsCallbackEventArgs(SettingsWrapper settings)
        {
            this.ModifiedSettings = settings;
        }
    }
}
