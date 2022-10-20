﻿using System;
using System.Media;
using System.Threading;
using H.NotifyIcon.Core;
using System.Drawing;

namespace Caffeine
{
    public sealed class CaffeineManager
    {
        private const int TIME_INTERVAL = 2000;
        private const string ICON_LOCATION = "TrayIcon.png";
        private const string SOUND_LOCATION = "silence.wav";
        private const KeyboardHandler.KeyCode WAKE_KEY = KeyboardHandler.KeyCode.F15;

        private readonly SoundPlayer _player;
        private readonly TrayIconWithContextMenu _icon;

        public CaffeineManager(bool keyboard = true, bool sound = true)
        {
            KeyboardEnabled = keyboard;
            SoundEnabled = sound;
            Active = false;

            _player = new SoundPlayer(SOUND_LOCATION);
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
            return Icon.FromHandle(new Bitmap(ICON_LOCATION).GetHicon());
        }

        private PopupMenu GenerateContextMenu()
        {
            return new PopupMenu()
            {
                Items =
                {
                    new PopupMenuItem("Caffeine", (_,_)=>{ })
                    {
                        Enabled = false,
                    },
                    new PopupMenuSeparator(),
                    new PopupMenuItem("Enable Silence",(_,_) => ToggleSilence())
                    {
                        Checked = SoundEnabled
                    },
                    new PopupMenuItem("Enable Keyboard",(_,_) => ToggleKeyboard())
                    {
                        Checked = KeyboardEnabled
                    },
                    new PopupMenuSeparator(),
                    new PopupMenuItem("Quit",(_,_) => PressQuit())
                }
            };
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
