using System.Windows;
using TienIchLich.ViewModels;

namespace TienIchLich
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CalendarViewModel();
        }
    }
}
