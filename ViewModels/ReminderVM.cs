using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for the reminder view.
    /// </summary>
    public class ReminderVM : ViewModelBase
    {
        private NavigationVM navigationVM;
        private MediaPlayer soundPlayer = new();

        private CalendarEventVM eventVM;

        /// <summary>
        /// View model of the calendar event being reminded.
        /// </summary>
        public CalendarEventVM EventVM
        {
            get
            {
                return eventVM;
            }
            set
            {
                eventVM = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Command to acknowledge the reminder and cancel it.
        /// </summary>
        public ICommand AckCommand { get; private set; }

        /// <summary>
        /// Command to remind again later.
        /// </summary>
        public ICommand RemindLaterCommand { get; private set; }

        public ReminderVM(NavigationVM navigationVM)
        {
            this.navigationVM = navigationVM;
            AckCommand = new RelayCommand(i => Acknowledge());

            // PlaySound();
        }

        private void PlaySound()
        {
            soundPlayer.Open(new Uri(@"D:\\test.flac"));
            soundPlayer.Play();
        }

        private void Acknowledge()
        {
            soundPlayer.Stop();
            navigationVM.NavigateToMainWorkspaceView();
        }
    }
}
