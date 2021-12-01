using System.Windows;
using TienIchLich.ViewModels;
using System.Globalization;

namespace TienIchLich
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CultureInfo.CurrentCulture = new CultureInfo("vi-VN");
            DataContext = new MainWindowVM();
            InitializeComponent();
        }
    }
}
