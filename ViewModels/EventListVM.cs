using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Holds filter values for a text property.
        /// </summary>
        public class TextFilter : ViewModelBase
        {
            // Method to refresh the CollectionView on property update.
            private Action Refresh;

            private string filterText = "";

            /// <summary>
            /// Text value to filter.
            /// </summary>
            public string FilterText
            {
                get
                {
                    return filterText;
                }
                set
                {
                    filterText = value;
                    Refresh();
                    NotifyPropertyChanged();
                }
            }

            private bool matchCase = false;

            /// <summary>
            /// True if case matching is required.
            /// </summary>
            public bool MatchCase
            {
                get
                {
                    return matchCase;
                }
                set
                {
                    matchCase = value;
                    Refresh();
                    NotifyPropertyChanged();
                }
            }

            private bool matchWholeWord = false;

            /// <summary>
            /// True if whole word matching is required.
            /// </summary>
            public bool MatchWholeWord
            {
                get
                {
                    return matchWholeWord;
                }
                set
                {
                    matchWholeWord = value;
                    Refresh();
                    NotifyPropertyChanged();
                }
            }

            public TextFilter(Action refresh)
            {
                Refresh = refresh;
            }

            /// <summary>
            /// Get filter result.
            /// </summary>
            /// <param name="timeValue">Time value to filter</param>
            /// <returns></returns>
            public bool Filter(string text)
            {
                bool matchFound;
                if (MatchCase)
                    matchFound = text.Contains(FilterText);
                else
                    matchFound = text.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
                if (!matchFound)
                    return false;

                if (MatchWholeWord)
                    return Regex.Match(text, $"\\b{Regex.Escape(FilterText)}\\b", RegexOptions.IgnoreCase).Success;
                return true;
            }
        }

        private CollectionViewSource eventCollectionViewSource;

        /// <summary>
        /// Collection view for the event list DataGrid.
        /// </summary>
        public ICollectionView EventCollectionView => eventCollectionViewSource.View;

        /// <summary>
        /// Event's subject filter values.
        /// </summary>
        public TextFilter SubjectFilter { get; private set; }

        /// <summary>
        /// Event's description filter values.
        /// </summary>
        public TextFilter DescriptionFilter { get; private set; }

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
            SubjectFilter = new TextFilter(EventCollectionView.Refresh);
            DescriptionFilter = new TextFilter(EventCollectionView.Refresh);
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
            e.Accepted = eventVM.CategoryVM.IsDisplayed
                         && SubjectFilter.Filter(eventVM.Subject)
                         && DescriptionFilter.Filter(eventVM.Description)
                         && StartTimeFilter.Filter(eventVM.StartTime)
                         && EndTimeFilter.Filter(eventVM.EndTime);
        }
    }
}