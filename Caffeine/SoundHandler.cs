using System.Media;

namespace Caffeine
{
    internal class SoundHandler
    {
        internal static void PlaySound(string soundLocation)
        {
            SoundPlayer player = new SoundPlayer(soundLocation);
            player.Play();
        }
    }
}