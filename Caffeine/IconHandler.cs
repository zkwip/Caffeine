using H.NotifyIcon;
using H.NotifyIcon.Core;
using System;
using System.Drawing;

namespace Caffeine
{
    internal class IconHandler
    {
        CaffeineManager _manager;

        public IconHandler(CaffeineManager manager)
        {
            _manager = manager;
        }

        internal TrayIcon CreateTrayIcon() 
        {
            return new TrayIconWithContextMenu()
            {
                ContextMenu = GenerateContextMenu(),
                Icon = GenerateIconImage().Handle,
                ToolTip = "Caffeine",
            };
        }
        private static Icon GenerateIconImage()
        {
            return IconGenerator.Generate(Brushes.Black, Brushes.White, new Pen(Color.White), text: "C");
        }

        private PopupMenu GenerateContextMenu()
        {
            return new PopupMenu()
            {
                Items =
                {
                    new PopupMenuItem("Enable Silence",(_,_) => ToggleSilence())
                    { 
                        Checked = _manager.SoundEnabled
                    },
                    new PopupMenuItem("Enable Keyboard",(_,_) => ToggleKeyboard())
                    {
                        Checked = _manager.KeyboardEnabled
                    },
                    new PopupMenuItem("Quit",(_,_) => Quit())
                }
            };
        }

        private void Quit()
        {
            _manager.Active = false;
        }

        private void ToggleSilence()
        {
            _manager.SoundEnabled = !_manager.SoundEnabled;
        }

        private void ToggleKeyboard()
        {
            _manager.KeyboardEnabled = !_manager.KeyboardEnabled;
        }
    }
}