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

        private CollectionViewSource eventCollectionViewSource;

        public ICollectionView EventCollectionView => eventCollectionViewSource.View;

        public UpcomingOverviewVM(ObservableCollection<CalendarEventVM> eventVMs, ObservableCollection<CalendarCategoryVM> categoryVMs)
        {
            this.eventCollectionViewSource = new CollectionViewSource() 
            {   Source = eventVMs, 
                IsLiveFilteringRequested = true, 
                IsLiveSortingRequested = true, 
                IsLiveGroupingRequested = true 
            };
            this.eventCollectionViewSource.Filter += EventCollectionViewSource_Filter;
            this.eventCollectionViewSource.GroupDescriptions.Add(new PropertyGroupDescription("StartTime", new DatetimeToDateConverter()));
            this.eventCollectionViewSource.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
            this.eventCollectionViewSource.LiveFilteringProperties.Add("CalendarCategoryVM.IsDisplayed");
            this.eventCollectionViewSource.LiveSortingProperties.Add("StartTime");
            this.eventCollectionViewSource.LiveGroupingProperties.Add("StartTime");
        }

        private void EventCollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            var eventVM = (CalendarEventVM)e.Item;
            e.Accepted = eventVM.CalendarCategoryVM.IsDisplayed;
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

        }

        private void DataVMChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
