using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;

namespace TienIchLich.ViewModels
{
    public class TimelineVM : ViewModelBase
    {
        private ObservableCollection<CalendarEventVM> eventVMs;
        private ChartValues<CalendarEventVM> timelineChartValues = new();

        public SeriesCollection TimelineChartSeriesCollection { get; private set; }

        private int lastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

        /// <summary>
        /// Last day of the selected month,
        /// </summary>
        public int LastDayOfMonth
        {
            get
            {
                return lastDayOfMonth;
            }
            set
            {
                lastDayOfMonth = value;
                NotifyPropertyChanged();
            }
        }


        public TimelineVM(ObservableCollection<CalendarEventVM> eventVMs)
        {
            this.eventVMs = eventVMs;
            this.eventVMs.CollectionChanged += EventVMs_CollectionChanged;

            var timelineMapper = Mappers.Gantt<CalendarEventVM>()
                .XStart(i => i.StartTime.Day + 1)
                .X(i => i.EndTime.Day + 1)
                .Fill(i => new SolidColorBrush((Color)ColorConverter.ConvertFromString(i.CategoryVM.DisplayColor)));
            TimelineChartSeriesCollection = new SeriesCollection()
            {
                new RowSeries(timelineMapper)
                {
                    Values = timelineChartValues,
                    DataLabels = true,
                    LabelPoint = i => GetLineName((CalendarEventVM)i.Instance)
                }
            };

            LoadAllEventsIntoTimelineChart();
        }

        private string GetLineName(CalendarEventVM eventVM)
        {
            return $"{eventVM.Subject}\n({eventVM.StartTime:g} - {eventVM.EndTime:g})";
        }

        /// <summary>
        /// Load all calendar event view models into timeline chart.
        /// </summary>
        private void LoadAllEventsIntoTimelineChart()
        {
            timelineChartValues.Clear();
            foreach (CalendarEventVM eventVM in eventVMs)
                if (eventVM.CategoryVM.IsDisplayed && eventVM.StatusVM.IsDisplayed)
                    timelineChartValues.Add(eventVM);
        }

        private void EventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                timelineChartValues.Add((CalendarEventVM)e.NewItems[0]);
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                timelineChartValues.Remove((CalendarEventVM)e.OldItems[0]);
        }

    }
}
