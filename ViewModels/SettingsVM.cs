using System.Windows.Input;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for settings view
    /// </summary>
    public class SettingsVM : ViewModelBase
    {
        private NavigationVM navigationVM;
        private DialogService dialogService;

        /// <summary>
        /// Reminder sound option info.
        /// </summary>
        public struct ReminderSoundOption
        {
            public enum OptionId
            {
                Sound1,
                Sound2,
                Sound3,
                Custom
            }

            public OptionId Id { get; set; }
            public string FilePath { get; set; }
        }

        private static ReminderSoundOption[] reminderSoundOptions =
        {
            new ReminderSoundOption()
            {
                Id = ReminderSoundOption.OptionId.Sound1,
                FilePath = "../../../Sounds/ringtone1.mp3"
            },
            new ReminderSoundOption()
            {
                Id = ReminderSoundOption.OptionId.Sound2,
                FilePath = "../../../Sounds/ringtone2.mp3"
            },
            new ReminderSoundOption()
            {
                Id = ReminderSoundOption.OptionId.Sound3,
                FilePath = "../../../Sounds/ringtone3.mp3"
            },
            new ReminderSoundOption()
            {
                Id = ReminderSoundOption.OptionId.Custom,
                FilePath = ""
            }
        };

        public static ReminderSoundOption[] ReminderSoundOptions => reminderSoundOptions;

        private ReminderSoundOption selectedReminderSoundOption = reminderSoundOptions[0];

        public ReminderSoundOption SelectedReminderSoundOption
        {
            get
            {
                return selectedReminderSoundOption;
            }
            set
            {
                selectedReminderSoundOption = value;
                ReminderSoundFileName = value.FilePath;
                if (value.Id == ReminderSoundOption.OptionId.Custom)
                    UseCustomReminderSoundOption = true;
                else
                    UseCustomReminderSoundOption = false;
                NotifyPropertyChanged();
            }
        }

        private bool useCustomReminderSoundOption = false;

        public bool UseCustomReminderSoundOption
        {
            get
            {
                return useCustomReminderSoundOption;
            }
            set
            {
                useCustomReminderSoundOption = value;
                NotifyPropertyChanged();
            }
        }

        private string reminderSoundFileName;

        public string ReminderSoundFileName
        {
            get
            {
                return reminderSoundFileName;
            }
            set
            {
                reminderSoundFileName = value;
                NotifyPropertyChanged();
            }
        }

        private int reminderSoundVolume = 50;

        public int ReminderSoundVolume
        {
            get
            {
                return reminderSoundVolume;
            }
            set
            {
                reminderSoundVolume = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Preview alarm sound.
        /// </summary>
        public ICommand PlaySoundCommand { get; private set; }

        /// <summary>
        /// Open file dialog.
        /// </summary>
        public ICommand OpenFileDialogCommand { get; private set; }

        /// <summary>
        /// Save settings into file.
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Cancel settings change and go back to main workspace view.
        /// </summary>
        public ICommand CancelCommand { get; private set; }

        public SettingsVM(NavigationVM navigationVM, AlarmSoundService alarmSoundService, DialogService dialogService)
        {
            this.navigationVM = navigationVM;
            this.dialogService = dialogService;
            SaveCommand = new RelayCommand(i => SaveSettings());
            CancelCommand = new RelayCommand(i => navigationVM.NavigateToMainWorkspaceView());
            PlaySoundCommand = new RelayCommand(i => alarmSoundService.PlaySoundOnce(ReminderSoundFileName, ReminderSoundVolume));
            OpenFileDialogCommand = new RelayCommand(i => OpenFileDialog());
        }

        /// <summary>
        /// Open file dialog for custom reminder alarm sound.
        /// </summary>
        private void OpenFileDialog()
        {
            string fileName = dialogService.ShowOpenFile("Sound files (.mp3)|*.mp3");
            if (fileName != null)
                ReminderSoundFileName = fileName;
        }

        private void SaveSettings()
        {
        }
    }
}