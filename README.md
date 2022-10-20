# Caffeine
A simple .NET program that keeps your computer awake

## Features
 - Control using a Tray Icon
 - Can play silence, to keep your audio devices from going into stand-by
 - Can simulate pressing F15 to prevent screensavers and such
 
## How to use
You can toggle each option by right-clicking the tray icon and selecting the features you want.

By default, no options are enabled, when you add the following arguments, the corresponding features will be enabled:
 - `k` turns on the keyboard feature which presses F15 every 2 seconds
 - `s` will play a silence through the active audio device, preventing some models from going into stand-by.
 
The command "caffeine.exe -ks" will enable both the keyboard and silence feature; "caffeine.exe -s" will only enable silence.
 
## Planned features
 - Pinging a web adress, for phoning home or keeping connections alive
 - Error messaging through the windows notifications
 