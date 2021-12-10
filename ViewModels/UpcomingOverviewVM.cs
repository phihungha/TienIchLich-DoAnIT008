using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    public class UpcomingOverviewVM : ViewModelBase
    {
        public class DatetimeToDateConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var datetime = (DateTime)value;
                return datetime.Date;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public enum StartTimeFilterOptionId
        {
            Week1,
            Week2,
            Month1,
            Month6,
            Year1,
            All,
            Custom
        }

        public struct StartTimeFilterOption
        {
            public StartTimeFilterOptionId Id { get; set; }
            public TimeSpan Timespan { get; set; }
        }

        private CollectionViewSource eventCollectionViewSource;
        private static StartTimeFilterOption[] startTimeFilterOptions =
        {
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Week1, Timespan = new TimeSpan(7, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Week2, Timespan = new TimeSpan(14, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Month1, Timespan = new TimeSpan(31, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Month6, Timespan = new TimeSpan(186, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Year1, Timespan = new TimeSpan(366, 0, 0, 0) },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.All },
            new StartTimeFilterOption() { Id = StartTimeFilterOptionId.Custom, Timespan = new TimeSpan(1, 0, 0, 0) }
        };
        private bool useCustomStartTimeFilter;
        private StartTimeFilterOption selectedStartTimeFilterOption;
        private TimeSpan startTimeFilterValue;


        public ICollectionView EventCollectionView => eventCollectionViewSource.View;

        public StartTimeFilterOption[] StartTimeFilterOptions => startTimeFilterOptions;

        public bool UseCustomStartTimeFilter
        {
            get
            {
                return useCustomStartTimeFilter;
            }
            set
            {
                useCustomStartTimeFilter = value;
                NotifyPropertyChanged();
            }
        }

        public TimeSpan StartTimeFilterValue
        {
            get
            {
                return startTimeFilterValue;
            }
            set
            {
                startTimeFilterValue = value;
                this.EventCollectionView.Refresh();
                NotifyPropertyChanged();
            }
        }

        public StartTimeFilterOption SelectedStartTimeFilterOption
        {
            get
            {
                return selectedStartTimeFilterOption;
            }
            set
            {
                selectedStartTimeFilterOption = value;
                if (value.Id == StartTimeFilterOptionId.Custom)
                    UseCustomStartTimeFilter = true;
                else
                    UseCustomStartTimeFilter = false;
                StartTimeFilterValue = value.Timespan;
                NotifyPropertyChanged();
            }
        }

        public UpcomingOverviewVM(ObservableCollection<CalendarEventVM> eventVMs)
        {
            this.eventCollectionViewSource = new CollectionViewSource() 
            {   
                Source = eventVMs, 
                IsLiveFilteringRequested = true, 
                IsLiveSortingRequested = true, 
                IsLiveGroupingRequested = true 
            };
            AttachEventHandlersToCalendarEventVMs(eventVMs);
            this.eventCollectionViewSource.Filter += EventCollectionViewSource_Filter;
            this.eventCollectionViewSource.GroupDescriptions.Add(new PropertyGroupDescription("StartTime", new DatetimeToDateConverter()));
            this.eventCollectionViewSource.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
            this.eventCollectionViewSource.LiveFilteringProperties.Add("CalendarCategoryVM.IsDisplayed");
            this.eventCollectionViewSource.LiveSortingProperties.Add("StartTime");
            this.eventCollectionViewSource.LiveGroupingProperties.Add("StartTime");

            this.SelectedStartTimeFilterOption = this.StartTimeFilterOptions[0];
        }

        private void EventCollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            var eventVM = (CalendarEventVM)e.Item;
            bool startTimeAccepted = this.SelectedStartTimeFilterOption.Id == StartTimeFilterOptionId.All ||
                                     (eventVM.StartTime < (DateTime.Now + this.StartTimeFilterValue) 
                                      && eventVM.StartTime > DateTime.Now);
            e.Accepted = eventVM.CalendarCategoryVM.IsDisplayed && startTimeAccepted;
        }


        private void AttachEventHandlersToCalendarEventVMs(ObservableCollection<CalendarEventVM> eventVMs)
        {
            eventVMs.CollectionChanged += EventVMs_CollectionChanged;
            foreach (CalendarEventVM eventVM in eventVMs)
                eventVM.PropertyChanged += DataVMChanged;
        }

        private void EventVMs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                CalendarEventVM eventVM = (CalendarEventVM)e.NewItems[0];
                eventVM.PropertyChanged += DataVMChanged;
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                CalendarEventVM eventVM = (CalendarEventVM)e.OldItems[0];
                eventVM.PropertyChanged -= DataVMChanged;
            }

            this.EventCollectionView.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));

        }

        private void DataVMChanged(object sender, PropertyChangedEventArgs e)
        {
            this.eventCollectionViewSource.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
        }
    }
}
