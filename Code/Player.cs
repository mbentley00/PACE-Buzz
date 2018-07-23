using System;
using System.Collections.Generic;
using System.Text;

namespace PACEBuzz
{
    public class Player
    {
        public const int NO_BUZZER_INDEX = -1;

        public int EasterEggSequence
        {
            get;
            set;
        }

        public DateTime EasterEggLastTime
        {
            get; set;
        }

        public int BuzzerIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Which of the four buzzers on this one USB device does
        /// this player correspond to?
        /// </summary>
        public int SubBuzzerIndex
        {
            get;
            set;
        }

        public Player(int buzzerIndex, int subBuzzerIndex)
        {
            this.BuzzerIndex = buzzerIndex;
            this.SubBuzzerIndex = subBuzzerIndex;
            this.EasterEggLastTime = DateTime.MinValue;
        }

        public bool DidEasterEggTimeEllapse()
        {
            TimeSpan diff = DateTime.Now.Subtract(this.EasterEggLastTime);
            if (diff.TotalMilliseconds >= 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void IncrementEasterEggValue()
        {
            this.EasterEggLastTime = DateTime.Now;
            this.EasterEggSequence++;
        }
    }
}
