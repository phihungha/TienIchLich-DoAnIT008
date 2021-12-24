using System.Windows.Controls;
using System.Windows.Data;
using TienIchLich.ViewModels;

namespace TienIchLich.Views
{
    /// <summary>
    /// Interaction logic for UpcomingOverviewView.xaml
    /// </summary>
    public partial class UpcomingOverviewView : UserControl
    {
        public UpcomingOverviewView()
        {
            InitializeComponent();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            CalendarEventCardVM cardVM = e.Item as CalendarEventCardVM;
            if (cardVM != null && cardVM.CategoryVM.IsDisplayed)
                e.Accepted = true;
            else
                e.Accepted = false;
        }
    }
}