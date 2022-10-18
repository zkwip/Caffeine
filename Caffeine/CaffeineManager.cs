using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Caffeine
{
    public class CaffeineManager
    {
        private const int timeInterval = 1000;
        private const KeyboardHandler.KeyCode wakingKey = KeyboardHandler.KeyCode.F15;
        private const string SoundLocation = "silence.wav";

        public CaffeineManager(bool keyboard, bool sound, bool startVisible)
        {
            KeyboardEnabled = keyboard;
            SoundEnabled = sound;
            Visible = startVisible;

            Active = false;
        }

        public void Start()
        {
            Active = true;

            while(Active)
            {
                HandleWakingActions();
                Thread.Sleep(timeInterval);
            }
        }

        private void HandleWakingActions()
        {
            try
            {
                if (KeyboardEnabled) 
                    KeyboardHandler.Simulate(wakingKey);

                if (SoundEnabled) 
                    SoundHandler.PlaySound(SoundLocation);
            }
            catch (Exception)
            {
                Active = false;
            }
        }

        public bool KeyboardEnabled { get; set; }

        public bool SoundEnabled { get; set; }

        public bool Active { get; private set; }

        public Exception LastException { get; private set; }

        public bool Visible { get; private set; }
    }
}
