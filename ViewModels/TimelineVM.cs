using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.Windows.Media;

namespace TienIchLich.ViewModels
{
    public class TimelineVM : ViewModelBase
    {
        private ObservableCollection<CalendarEventVM> eventVMs;
        private ChartValues<CalendarEventVM> timelineChartValues = new();

        public SeriesCollection TimelineChartSeriesCollection { get; private set; }

        private DateTime selectedDisplayMonth = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

        /// <summary>
        /// Current month displayed on the event count line chart.
        /// </summary>
        public DateTime SelectedDisplayMonth
        {
            get
            {
                return selectedDisplayMonth;
            }
            set
            {
                selectedDisplayMonth = new DateTime(value.Year,
                    value.Month,
                    DateTime.DaysInMonth(value.Year, value.Month));
                FirstDateOfMonth = new DateTime(selectedDisplayMonth.Year, selectedDisplayMonth.Month, 1);
                ResetDisplayDayRange();
                LoadAllEventsIntoTimelineChart();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// First date of selected month.
        /// </summary>
        private DateTime FirstDateOfMonth { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        /// <summary>
        /// Last date of selected month.
        /// </summary>
        private DateTime LastDateOfMonth => SelectedDisplayMonth;

        private int minDisplayDay = 1;

        /// <summary>
        /// Minimum day number to display on the chart
        /// </summary>
        public int MinDisplayDay
        {
            get
            {
                return minDisplayDay;
            }
            set
            {
                minDisplayDay = value;
                NotifyPropertyChanged();
            }
        }

        private int maxDisplayDay;

        /// <summary>
        /// Minimum day number to display on the chart
        /// </summary>
        public int MaxDisplayDay
        {
            get
            {
                return maxDisplayDay;
            }
            set
            {
                maxDisplayDay = value;
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Display previous month on the chart.
        /// </summary>
        public ICommand PrevMonthCommand { get; private set; }

        /// <summary>
        /// Display next month on the chart.
        /// </summary>
        public ICommand NextMonthCommand { get; private set; }

        /// <summary>
        /// Reset chart's zoom and pan.
        /// </summary>
        public ICommand ResetDisplayDayRangeCommand { get; private set; }


        public TimelineVM(ObservableCollection<CalendarEventVM> eventVMs)
        {
            this.eventVMs = eventVMs;
            this.eventVMs.CollectionChanged += EventVMs_CollectionChanged;
            ResetDisplayDayRange();

            PrevMonthCommand = new RelayCommand(i => GoToPrevDisplayMonth());
            NextMonthCommand = new RelayCommand(i => GoToNextDisplayMonth());
            ResetDisplayDayRangeCommand = new RelayCommand(i => ResetDisplayDayRange());

            var timelineMapper = Mappers.Gantt<CalendarEventVM>()
                .XStart(i => GetDayNumSinceStartOfMonth(i.StartTime) + 1)
                .X(i => GetDayNumSinceStartOfMonth(i.EndTime) + 1)
                .Fill(i => new SolidColorBrush((Color)ColorConverter.ConvertFromString(i.CategoryVM.DisplayColor)));

            TimelineChartSeriesCollection = new SeriesCollection()
            {
                new RowSeries(timelineMapper)
                {
                    Values = timelineChartValues,
                    DataLabels = true,
                    Foreground = Brushes.White,
                    LabelPoint = i => GetLineName((CalendarEventVM)i.Instance)
                }
            };

            LoadAllEventsIntoTimelineChart();
        }

        private double GetDayNumSinceStartOfMonth(DateTime time)
        {
            if (time > LastDateOfMonth)
                return LastDateOfMonth.Day - 1;
            if (time < FirstDateOfMonth)
                return 0;
            return (time - FirstDateOfMonth).TotalDays;
        }

        private void ResetDisplayDayRange()
        {
            MinDisplayDay = 1;
            MaxDisplayDay = SelectedDisplayMonth.Day;
        }

        private void GoToPrevDisplayMonth()
        {
            SelectedDisplayMonth = SelectedDisplayMonth.AddMonths(-1);
        }

        private void GoToNextDisplayMonth()
        {
            SelectedDisplayMonth = SelectedDisplayMonth.AddMonths(1);
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
                if (eventVM.CategoryVM.IsDisplayed 
                    && eventVM.StatusVM.IsDisplayed
                    && eventVM.StartTime <= LastDateOfMonth
                    && eventVM.EndTime >= FirstDateOfMonth)
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
