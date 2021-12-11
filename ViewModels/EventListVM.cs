using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// View model for calendar event list view.
    /// </summary>
    public class EventListVM : ViewModelBase
    {
        /// <summary>
        /// Holds filter values for a time-related property.
        /// </summary>
        public class TimeFilter : ViewModelBase
        {
            private bool enable = false;

            // Method to refresh the CollectionView on property update.
            private Action Refresh;

            private DateTime low = DateTime.Now;

            /// <summary>
            /// Lower bound of the filter.
            /// </summary>
            public DateTime Low
            {
                get
                {
                    return low;
                }
                set
                {
                    low = value;
                    Refresh();
                    NotifyPropertyChanged();
                }
            }

            private DateTime high = DateTime.Now.AddDays(1);

            /// <summary>
            /// Higher bound of the filter.
            /// </summary>
            public DateTime High
            {
                get
                {
                    return high;
                }
                set
                {
                    high = value;
                    Refresh();
                    NotifyPropertyChanged();
                }
            }

            /// <summary>
            /// True if filter is enabled.
            /// </summary>
            public bool Enable
            {
                get
                {
                    return enable;
                }
                set
                {
                    enable = value;
                    Refresh();
                    NotifyPropertyChanged();
                }
            }

            public TimeFilter(Action refresh)
            {
                Refresh = refresh;
            }

            /// <summary>
            /// Get filter result.
            /// </summary>
            /// <param name="timeValue">Time value to filter</param>
            /// <returns></returns>
            public bool Filter(DateTime timeValue)
            {
                return !Enable || (timeValue > Low && timeValue < High);
            }
        }

        private CollectionViewSource eventCollectionViewSource;

        /// <summary>
        /// Collection view for the event list DataGrid.
        /// </summary>
        public ICollectionView EventCollectionView => eventCollectionViewSource.View;

        private string subjectFilter = "";

        /// <summary>
        /// Event's subject filter words.
        /// </summary>
        public string SubjectFilter
        {
            get
            {
                return subjectFilter;
            }
            set
            {
                subjectFilter = value;
                EventCollectionView.Refresh();
                NotifyPropertyChanged();
            }
        }

        private string descriptionFilter = "";

        /// <summary>
        /// Event's description filter words.
        /// </summary>
        public string DescriptionFilter
        {
            get
            {
                return descriptionFilter;
            }
            set
            {
                descriptionFilter = value;
                EventCollectionView.Refresh();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event's start time filter values.
        /// </summary>
        public TimeFilter StartTimeFilter { get; private set; }

        /// <summary>
        /// Event's end time filter values.
        /// </summary>
        public TimeFilter EndTimeFilter { get; private set; }

        public EventListVM(ObservableCollection<CalendarEventVM> eventVMs)
        {
            eventCollectionViewSource = new CollectionViewSource()
            {
                Source = eventVMs,
                IsLiveFilteringRequested = true
            };
            StartTimeFilter = new TimeFilter(EventCollectionView.Refresh);
            EndTimeFilter = new TimeFilter(EventCollectionView.Refresh);
            eventCollectionViewSource.Filter += EventCollectionViewSource_Filter;
            // Update collection view on calendar category display status changed.
            eventCollectionViewSource.LiveFilteringProperties.Add("CategoryVM.IsDisplayed");
        }

        /// <summary>
        /// Do the filtering of events.
        /// </summary>
        private void EventCollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            var eventVM = (CalendarEventVM)e.Item;
            e.Accepted = eventVM.Subject.Contains(SubjectFilter)
                         && eventVM.Description.Contains(DescriptionFilter)
                         && StartTimeFilter.Filter(eventVM.StartTime)
                         && EndTimeFilter.Filter(eventVM.EndTime)
                         && eventVM.CategoryVM.IsDisplayed;
        }
    }
}