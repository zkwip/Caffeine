using Cocona;
using System;
using System.Media;
using System.Threading;

namespace Caffeine
{
    public class CaffeineManager
    {
        private const int TIME_INTERVAL = 1000;
        private const string SOUND_LOCATION = "sound.wav";
        private const KeyboardHandler.KeyCode WAKE_KEY = KeyboardHandler.KeyCode.F15;

        private readonly SoundPlayer _player;

        public CaffeineManager([Option("k")] bool keyboard = true, [Option("s")] bool sound = true, [Option("v")] bool visible = true)
        {
            _player = new SoundPlayer(SOUND_LOCATION);

            KeyboardEnabled = keyboard;
            SoundEnabled = sound;
            Visible = visible;

            Active = false;
        }

        public void Run()
        {
            Active = true;

            while(Active)
            {
                HandleWakingActions();
                Thread.Sleep(TIME_INTERVAL);
            }
        }

        private void HandleWakingActions()
        {
            try
            {
                if (KeyboardEnabled) 
                    KeyboardHandler.Simulate(WAKE_KEY);

                if (SoundEnabled) 
                    _player.Play();
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

        static void Main(string[] args) => CoconaApp.Run<CaffeineManager>(args);
    }
}
