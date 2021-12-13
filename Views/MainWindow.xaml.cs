using System.Globalization;
using System.Windows;
using TienIchLich.Services;
using TienIchLich.ViewModels;

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