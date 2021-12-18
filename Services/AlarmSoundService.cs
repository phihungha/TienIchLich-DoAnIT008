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
        private bool isPlaying = false;
        private string currentPath = "";

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
        public void PlayStopSound(string filePath, int volume)
        {
            repeat = false;
            if (isPlaying && filePath == currentPath)
                StopSound();
            else
                PlaySound(filePath, volume);
        }

        /// <summary>
        /// Play reminder alarm sound.
        /// </summary>
        /// <param name="filePath">Sound file's path</param>
        /// <param name="volume">Audio volume</param>
        public void PlaySound(string filePath, int volume)
        {
            soundPlayer.Dispatcher.Invoke(() =>
            {
                soundPlayer.Open(new Uri(filePath, UriKind.RelativeOrAbsolute));
                soundPlayer.Volume = (double)volume / 100;
                soundPlayer.Play();
            });
            currentPath = filePath;
            isPlaying = true;
        }

        /// <summary>
        /// Set reminder alarm volume.
        /// </summary>
        /// <param name="volume">Audio volume</param>
        public void SetVolume(int volume)
        {
            soundPlayer.Dispatcher.Invoke(() =>
            {
                soundPlayer.Volume = (double)volume / 100;
            });
        }

        /// <summary>
        /// Stop reminder alarm sound.
        /// </summary>
        public void StopSound()
        {
            soundPlayer.Stop();
            isPlaying = false;
        }
    }
}