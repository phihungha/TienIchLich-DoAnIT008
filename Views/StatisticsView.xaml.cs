using LiveCharts.Wpf;
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
