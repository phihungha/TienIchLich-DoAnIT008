using System.Windows.Input;
using TienIchLich.Services;
using TienIchLich.Properties;
using System.Linq;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for settings view
    /// </summary>
    public class SettingsVM : ViewModelBase
    {
        private NavigationVM navigationVM;
        private AlarmSoundService alarmSoundService;
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

        private string reminderSoundFileName = reminderSoundOptions[0].FilePath;

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
        public ICommand ExitCommand { get; private set; }

        public SettingsVM(NavigationVM navigationVM, AlarmSoundService alarmSoundService, DialogService dialogService)
        {
            this.alarmSoundService = alarmSoundService;
            this.navigationVM = navigationVM;
            this.dialogService = dialogService;
            SaveCommand = new RelayCommand(i => SaveSettings());
            ExitCommand = new RelayCommand(i => ExitSettings());
            PlaySoundCommand = new RelayCommand(i => TestReminderSound());
            OpenFileDialogCommand = new RelayCommand(i => OpenFileDialog());

            LoadSettings();
        }

        /// <summary>
        /// Set properties of this view model to the settings file's values.
        /// </summary>
        private void LoadSettings()
        {
            ReminderSoundVolume = Settings.Default.ReminderSoundVolume;
            SelectedReminderSoundOption = ReminderSoundOptions
                .Where(i => i.FilePath == Settings.Default.ReminderSoundFileName)
                .DefaultIfEmpty(ReminderSoundOptions[3])
                .First();
            ReminderSoundFileName = Settings.Default.ReminderSoundFileName;
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

        private void TestReminderSound()
        {
            alarmSoundService.PlaySoundOnce(ReminderSoundFileName, ReminderSoundVolume);
        }

        private void SaveSettings()
        {
            Settings.Default.ReminderSoundVolume = ReminderSoundVolume;
            Settings.Default.ReminderSoundFileName = ReminderSoundFileName;
            Settings.Default.Save();
            ExitSettings();
        }

        private void ExitSettings()
        {
            alarmSoundService.StopSound();
            navigationVM.NavigateToMainWorkspaceView();
        }
    }
}