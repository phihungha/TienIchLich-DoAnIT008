using LiveCharts.Wpf;
using System.Windows.Controls;

namespace TienIchLich.Views
{
    /// <summary>
    /// Interaction logic for StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();

            // Set axises for event count line chart.
            EventCountLineChart.AxisX = new AxesCollection()
            {
                new Axis()
                {
                    Title= "Ngày",
                    FontSize = 14,
                    MinValue = 1,
                    Separator = new LiveCharts.Wpf.Separator()
                    {
                        Step = 1.0,
                        IsEnabled = false
                    },
                }
            };

            EventCountLineChart.AxisY = new AxesCollection()
            {
                new Axis()
                {
                    Title= "Số sự kiện",
                    FontSize = 14,
                    MinValue = 0,
                    Separator = new LiveCharts.Wpf.Separator()
                    {
                        Step = 1.0,
                        IsEnabled = false
                    }
                }
            };
        }
    }
}