using System;
using System.Windows.Media;

namespace TienIchLich.Services
{
    /// <summary>
    /// Provides alarm sound playback for event reminder.
    /// </summary>
    public class AlarmSoundService
    {
        private MediaPlayer soundPlayer = new();
        private bool repeat = true;

        public AlarmSoundService()
        {
            soundPlayer.MediaEnded += SoundPlayer_MediaEnded;
        }

        /// <summary>
        /// Repeat playback.
        /// </summary>
        private void SoundPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (repeat)
            {
                soundPlayer.Position = TimeSpan.Zero;
                soundPlayer.Play();
            }
            else
                repeat = true;
        }

        /// <summary>
        /// Play reminder alarm sound once.
        /// </summary>
        /// <param name="filePath">Sound file's path</param>
        /// <param name="volume">Audio volume</param>
        public void PlaySoundOnce(string filePath, int volume)
        {
            repeat = false;
            PlaySound(filePath, volume);
        }

        /// <summary>
        /// Play reminder alarm sound.
        /// </summary>
        /// <param name="filePath">Sound file's path</param>
        /// <param name="volumne">Audio volume</param>
        public void PlaySound(string filePath, int volumne)
        {
            soundPlayer.Dispatcher.Invoke(() =>
            {
                soundPlayer.Open(new Uri(filePath, UriKind.RelativeOrAbsolute));
                soundPlayer.Volume = (double)volumne / 100;
                soundPlayer.Play();
            });
        }

        /// <summary>
        /// Stop reminder alarm sound.
        /// </summary>
        public void StopSound()
        {
            soundPlayer.Stop();
        }
    }
}
