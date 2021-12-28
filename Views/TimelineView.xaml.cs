using LiveCharts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            if (e.PreviewMaxValue > 32)
                e.Cancel = true;
            if (e.PreviewMinValue < 0)
                e.Cancel = true;
        }
    }
}
