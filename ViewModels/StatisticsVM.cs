using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using System.Windows.Media;
using System.ComponentModel;
using System;
using System.Windows.Input;
using LiveCharts.Defaults;

namespace TienIchLich.ViewModels
{
    public class StatisticsVM : ViewModelBase
    {
        private ObservableCollection<CalendarEventVM> eventVMs;
        private ObservableCollection<CalendarCategoryVM> categoryVMs;

        /// <summary>
        /// Category pie chart series collection.
        /// </summary>
        public SeriesCollection CategoryPieChartSeriesCollection { get; private set; }

        /// <summary>
        /// Series collection for event count per day line chart.
        /// </summary>
        public SeriesCollection EventCountLineChartSeriesCollection { get; private set; }

        private ChartValues<ObservableValue> eventCountChartValues = new();

        /// <summary>
        /// Chart values for event count chart.
        /// </summary>
        public ChartValues<ObservableValue> EventCountChartValues
        {
            get
            {
                return eventCountChartValues;
            }
            set
            {
                eventCountChartValues = value;
                NotifyPropertyChanged();
            }
        }

        private int categoryNum = 0;

        /// <summary>
        /// Total number of categories we have.
        /// </summary>
        public int CategoryNum
        {
            get
            {
                return categoryNum;
            }
            set
            {
                categoryNum = value;
                NotifyPropertyChanged();
            }
        }

        private double categoryEventNumAverage = 0;

        /// <summary>
        /// Average event number of a category.
        /// </summary>
        public double CategoryEventNumAverage
        {
            get
            {
                return categoryEventNumAverage;
            }
            set
            {
                categoryEventNumAverage = value;
                NotifyPropertyChanged();
            }
        }

        private CalendarCategoryVM maxCategory;

        /// <summary>
        /// Category with the most events.
        /// </summary>
        public CalendarCategoryVM MaxCategory
        {
            get
            {
                return maxCategory;
            }
            set
            {
                maxCategory = value;
                NotifyPropertyChanged();
            }
        }

        private int eventNum = 0;

        /// <summary>
        /// Total number of events.
        /// </summary>
        public int EventNum
        {
            get
            {
                return eventNum;
            }
            set
            {
                eventNum = value;
                NotifyPropertyChanged();
            }
        }

        private double averageEventHours = 0;

        /// <summary>
        /// Average event duration hours.
        /// </summary>
        public double AverageEventHours
        {
            get
            {
                return averageEventHours;
            }
            set
            {
                averageEventHours = value;
                NotifyPropertyChanged();
            }
        }

        private DateTime currentMonthOfEventCountLineChart = 
            new DateTime(DateTime.Now.Year, 
                         DateTime.Now.Month, 
                         DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

        /// <summary>
        /// Current month displayed on the event count line chart.
        /// </summary>
        public DateTime CurrentMonthOfEventCountLineChart
        {
            get
            {
                return currentMonthOfEventCountLineChart;
            }
            set
            {
                currentMonthOfEventCountLineChart = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Go to previous month on event count line chart.
        /// </summary>
        public ICommand PrevMonthCommand { get; private set; }

        /// <summary>
        /// Go to next month on event count line chart.
        /// </summary>
        public ICommand NextMonthCommand { get; private set; }

        public StatisticsVM(ObservableCollection<CalendarEventVM> eventVMs, ObservableCollection<CalendarCategoryVM> categoryVMs)
        {
            this.eventVMs = eventVMs;
            this.categoryVMs = categoryVMs;

            var categoryMapper = Mappers.Xy<CalendarCategoryVM>().Y(i => i.EventNum);
            CategoryPieChartSeriesCollection = new SeriesCollection(categoryMapper);
            EventCountLineChartSeriesCollection = new SeriesCollection()
            {
                new LineSeries()
                {
                    Title = "Số sự kiện",
                    Configuration = Mappers.Xy<int>().X((value, index) => index + 1).Y((value, index) => value),
                    DataLabels = true,
                    Values = EventCountChartValues,
                }
            };

            categoryVMs.CollectionChanged += CategoryVMs_CollectionChanged;
            eventVMs.CollectionChanged += EventVMs_CollectionChanged;

            PrevMonthCommand = new RelayCommand(i => GoToPrevMonthOnEventCountLineChart());
            NextMonthCommand = new RelayCommand(i => GoToNextMonthOnEventCountLineChart());

            InitData();
            CalculateStats();
        }

        private void GoToPrevMonthOnEventCountLineChart()
        {
            CurrentMonthOfEventCountLineChart = CurrentMonthOfEventCountLineChart.AddMonths(-1);
            CalculateEventCountLineChartForAllEvents();
        }

        private void GoToNextMonthOnEventCountLineChart()
        {
            CurrentMonthOfEventCountLineChart = CurrentMonthOfEventCountLineChart.AddMonths(1);
            CalculateEventCountLineChartForAllEvents();
        }

        /// <summary>
        /// Populate the charts with all data when the app first runs.
        /// </summary>
        private void InitData()
        {
            foreach (CalendarCategoryVM categoryVM in categoryVMs)
            {
                LoadCategoryIntoCategoryPieChartSeriesCollection(categoryVM);
                categoryVM.PropertyChanged += CategoryVM_PropertyChanged;
            }

            foreach (CalendarEventVM eventVM in eventVMs)
                eventVM.PropertyChanged += EventVM_PropertyChanged;
            CalculateEventCountLineChartForAllEvents();
        }

        private void EventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateStats(); 
            CalculateEventCountLineChartForAllEvents();
            if (e.Action == NotifyCollectionChangedAction.Add)
                ((CalendarEventVM)e.NewItems[0]).PropertyChanged += EventVM_PropertyChanged;
        }

        private void EventVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CalculateStats(); 
            CalculateEventCountLineChartForAllEvents();
        }

        /// <summary>
        /// Update charts and statistics on category view model collection change.
        /// </summary>
        private void CategoryVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var categoryVM = (CalendarCategoryVM)e.NewItems[0];
                LoadCategoryIntoCategoryPieChartSeriesCollection(categoryVM);
                categoryVM.PropertyChanged += CategoryVM_PropertyChanged;

            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var categoryVM = (CalendarCategoryVM)e.OldItems[0];
                var pieSeries = CategoryPieChartSeriesCollection
                    .Where(i => ((CalendarCategoryVM)i.Values[0]).Id == categoryVM.Id)
                    .Single();
                CategoryPieChartSeriesCollection.Remove(pieSeries);
            }
            CalculateStats();
        }

        private void CategoryVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var categoryVM = (CalendarCategoryVM)sender;
            PieSeries pieSeries = CategoryPieChartSeriesCollection.Cast<PieSeries>()
                                    .Where(i => ((CalendarCategoryVM)i.Values[0]).Id == categoryVM.Id)
                                    .Single();
            pieSeries.Title = categoryVM.Name;
            pieSeries.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(categoryVM.DisplayColor));
        }

        /// <summary>
        /// Calculate all statistics that may change due to event and category change.
        /// </summary>
        private void CalculateStats()
        {
            CategoryNum = categoryVMs.Count;
            CategoryEventNumAverage = categoryVMs.Where(i => i.EventNum != 0).Select(i => i.EventNum).DefaultIfEmpty().Average();
            MaxCategory = categoryVMs.OrderByDescending(i => i.EventNum).DefaultIfEmpty(new CalendarCategoryVM(null, null) { Name = "N/A" }).First();
            double avgEventSeconds = (from e in eventVMs let i = e.EndTime - e.StartTime select i.TotalSeconds).DefaultIfEmpty().Average();
            AverageEventHours = avgEventSeconds / 3600;
            EventNum = eventVMs.Count();
        }

        /// <summary>
        /// Load the data in a calendar category view model into pie chart series collection.
        /// </summary>
        /// <param name="categoryVM">Calendar category view model</param>
        private void LoadCategoryIntoCategoryPieChartSeriesCollection(CalendarCategoryVM categoryVM)
        {
            var pieSeries = new PieSeries()
            {
                Title = categoryVM.Name,
                Values = new ChartValues<CalendarCategoryVM> { categoryVM },
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(categoryVM.DisplayColor))
            };
            CategoryPieChartSeriesCollection.Add(pieSeries);
        }

        private void CalculateEventCountLineChartForAllEvents()
        {
            PopulateDaysForEventCountLineChart();
            foreach (CalendarEventVM eventVM in eventVMs)
                CalculateEventCountLineChartForAnEvent(eventVM);
        }

        private void PopulateDaysForEventCountLineChart()
        {
            EventCountChartValues.Clear();
            for (int i = 0; i < CurrentMonthOfEventCountLineChart.Day; i++)
                EventCountChartValues.Add(new ObservableValue(0));
        }

        private void IncrementDayEventCount(int day)
        {
            EventCountChartValues[day - 1].Value++;
        }

        private void CalculateEventCountLineChartForAnEvent(CalendarEventVM eventVM)
        {
            DateTime startOfMonth = new DateTime(CurrentMonthOfEventCountLineChart.Year, CurrentMonthOfEventCountLineChart.Month, 1);
            DateTime endOfMonth = CurrentMonthOfEventCountLineChart;
            DateTime startTime = eventVM.StartTime;
            DateTime endTime = eventVM.EndTime;
            if ((startTime >= startOfMonth && startTime <= endOfMonth)
                || (endTime >= startOfMonth && endTime <= endOfMonth))
            {
                int startDay = startTime < startOfMonth ? 1 : startTime.Day;
                int endDay = endTime > endOfMonth ? endOfMonth.Day : endTime.Day;
                for (int day = startDay; day <= endDay; day++)
                    IncrementDayEventCount(day);
            }
        }

    }
}
