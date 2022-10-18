using Cocona;
using System;
using System.Media;
using System.Threading;
using H.NotifyIcon.Core;
using System.Windows;

namespace Caffeine
{
    public sealed class CaffeineManager
    {
        private const int TIME_INTERVAL = 2000;
        private const string SOUND_LOCATION = "sound.wav";
        private const KeyboardHandler.KeyCode WAKE_KEY = KeyboardHandler.KeyCode.F15;

        private readonly SoundPlayer _player;
        private readonly TrayIcon _icon;

        public CaffeineManager([Option("k")] bool keyboard = true, [Option("s")] bool sound = true, [Option("v")] bool visible = true)
        {
            _player = new SoundPlayer(SOUND_LOCATION);

            KeyboardEnabled = keyboard;
            SoundEnabled = sound;
            Visible = visible;

            Active = false;
            _icon = new IconHandler(this).CreateTrayIcon();

            Run();
        }

        public void Run()
        {
            Active = true;

            while(Active)
            {
                Thread.Sleep(TIME_INTERVAL);
                HandleWakingActions();

                if (Visible) 
                    SpawnWindow();
            }
        }

        private void SpawnWindow()
        {
            var thread = new Thread(ShowWindow);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        private void ShowWindow()
        {
            var window = new MainWindow(this);
            window.ShowDialog();

            Visible = false;
            KeyboardEnabled = window.EnableKeys;
            SoundEnabled = window.EnableSound;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Active = false;
            }
        }

        public bool KeyboardEnabled { get; set; }

        public bool SoundEnabled { get; set; }

        public bool Active { get; set; }

        public Exception LastException { get; private set; }

        public bool Visible { get; set; }

        [STAThread]
        static void Main(string[] args) => CoconaApp.Run<CaffeineManager>(args);
    }
}
