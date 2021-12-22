using System.Globalization;
using System.Windows;
using TienIchLich.Models;
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
            CreateDb();
            DataContext = new MainWindowVM(dialogService, alarmSoundService);
            InitializeComponent();
        }

        private void CreateDb()
        {
            using (var db = new CalendarDbContext())
            {
                db.Database.EnsureCreated();
            }
        }
    }
}