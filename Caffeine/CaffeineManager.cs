using System;
using System.Media;
using System.Threading;
using H.NotifyIcon.Core;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace Caffeine
{
    public sealed class CaffeineManager
    {
        private const int TIME_INTERVAL = 2000;
        private const KeyboardHandler.KeyCode WAKE_KEY = KeyboardHandler.KeyCode.F15;

        private readonly byte[] SILENCE_I16 = { 
            0x52, 0x49, 0x46, 0x46, 
            0x2C, 0x00, 0x00, 0x00, 
            0x57, 0x41, 0x56, 0x45, 
            0x66, 0x6D, 0x74, 0x20,
            0x10, 0x00, 0x00, 0x00, 
            0x01, 0x00, 0x02, 0x00, 
            0x44, 0xAC, 0x00, 0x00, 
            0x10, 0xB1, 0x02, 0x00,
            0x04, 0x00, 0x10, 0x00, 
            0x64, 0x61, 0x74, 0x61,
            0x80, 0x00, 0x00, 0x00, 
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00 };

        private readonly byte[] SILENCE_F32 = {
            0x52, 0x49, 0x46, 0x46,
            0x58, 0x00, 0x00, 0x00,
            0x57, 0x41, 0x56, 0x45,
            0x66, 0x6D, 0x74, 0x20,
            0x10, 0x00, 0x00, 0x00,
            0x03, 0x00, 0x02, 0x00,
            0x44, 0xAC, 0x00, 0x00,
            0x20, 0x62, 0x05, 0x00,
            0x08, 0x00, 0x20, 0x00,
            0x66, 0x61, 0x63, 0x74,
            0x04, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00,
            0x50, 0x45, 0x41, 0x4B,
            0x18, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00,
            0x4D, 0x92, 0x51, 0x63,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x64, 0x61, 0x74, 0x61,
            0x08, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        };


        private readonly SoundPlayer _player;
        private readonly TrayIconWithContextMenu _icon;

        public CaffeineManager(bool keyboard = true, bool sound = true)
        {
            KeyboardEnabled = keyboard;
            SoundEnabled = sound;
            Active = false;
            _player = new SoundPlayer(new MemoryStream(SILENCE_F32));
            _icon = CreateTrayIcon();
        }

        internal TrayIconWithContextMenu CreateTrayIcon()
        {
            var icon = new TrayIconWithContextMenu()
            {
                ContextMenu = GenerateContextMenu(),
                Icon = GenerateIconImage().Handle,
                ToolTip = "Caffeine",
            };

            icon.Create();
            return icon;
        }

        private static Icon GenerateIconImage()
        {
            return Icon.ExtractAssociatedIcon("Caffeine.exe");
        }

        private PopupMenu GenerateContextMenu()
        {
            return new PopupMenu()
            {
                Items =
                {
                    new PopupMenuItem("Enable Silence",(_,_) => ToggleSilence())
                    {
                        Checked = SoundEnabled
                    },
                    new PopupMenuItem("Enable Keyboard",(_,_) => ToggleKeyboard())
                    {
                        Checked = KeyboardEnabled
                    },
                    new PopupMenuSeparator(),
                    new PopupMenuItem("About Caffeine", (_,_) => { OpenUrl(@"https://github.com/zkwip/Caffeine#how-to-use"); }),
                    new PopupMenuSeparator(),
                    new PopupMenuItem("Quit",(_,_) => PressQuit()),
                }
            };
        }

        private static void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void PressQuit()
        {
            Active = false;
        }

        private void ToggleSilence()
        {
            SoundEnabled = !SoundEnabled;
            _icon.ContextMenu = GenerateContextMenu();
        }

        private void ToggleKeyboard()
        {
            KeyboardEnabled = !KeyboardEnabled;
            _icon.ContextMenu = GenerateContextMenu();
        }

        public void Run()
        {
            Active = true;

            while (Active)
            {
                Thread.Sleep(TIME_INTERVAL);
                HandleWakingActions();
            }

            //End

            Close();
        }

        private void Close()
        {
            _player.Stop();
            _player.Dispose();

            _icon.Remove();
            _icon.Dispose();
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
                throw;
            }
        }

        internal bool KeyboardEnabled { get; set; }

        internal bool SoundEnabled { get; set; }

        internal bool Active { get; set; }

        static void Main(string[] args)
        {
            bool key = false;
            bool sound = false;

            foreach (string arg in args)
            {
                if (arg.Contains('k')) 
                    key = true;

                if (arg.Contains('s'))
                    sound = true;
            }

            new CaffeineManager(key, sound).Run();
        }
    }
}
