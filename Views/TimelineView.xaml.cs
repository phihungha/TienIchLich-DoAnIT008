using LiveCharts.Events;
using System.Windows.Controls;
using TienIchLich.ViewModels;

namespace TienIchLich.Views
{
    /// <summary>
    /// Interaction logic for TimelineView.xaml
    /// </summary>
    public partial class TimelineView : UserControl
    {
        public TimelineView()
        {
            InitializeComponent();
        }

        private void Axis_PreviewRangeChanged(PreviewRangeChangedEventArgs e)
        {
            TimelineVM timelineVM = (TimelineVM)DataContext;
            if (e.PreviewMaxValue > timelineVM.SelectedDisplayMonth.Day)
                e.Cancel = true;
            if (e.PreviewMinValue < 1)
                e.Cancel = true;
        }
    }
}
