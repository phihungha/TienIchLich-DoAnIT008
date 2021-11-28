using System.Windows;
using System.Windows.Navigation;
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
            DataContext = new MainViewModel();
            InitializeComponent();
        }
    }
}
