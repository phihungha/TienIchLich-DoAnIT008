using System.Windows;
using TienIchLich.ViewModels;
using System.Globalization;
using TienIchLich.Services;

namespace TienIchLich
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private DialogService dialogService = new();
        private AlarmSoundService alarmSoundService = new();

        public MainWindow()
        {
            CultureInfo.CurrentCulture = new CultureInfo("vi-VN");
            DataContext = new MainWindowVM(dialogService, alarmSoundService);
            InitializeComponent();
        }
    }
}
