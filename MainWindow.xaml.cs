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
            InitializeComponent();
            DataContext = new CalendarViewModel();
        }

        private void Frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            UpdateFrameDataContext(sender);
        }

        private void Frame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateFrameDataContext(sender);
        }

        private void UpdateFrameDataContext(object sender)
        {
            var content = mainCalendarViewFrame.Content as FrameworkElement;
            if (content == null)
                return;
            content.DataContext = mainCalendarViewFrame.DataContext;
        }
    }
}
