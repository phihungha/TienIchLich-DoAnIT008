﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.Specialized;

namespace TienIchLich.ViewModels
{
    public class EventListVM : ViewModelBase
    {
        /// <summary>
        /// Holds filter values for a time-related property.
        /// </summary>
        public class TimeFilter : ViewModelBase
        {
            private DateTime low = DateTime.Now;
            private DateTime high = DateTime.Now.AddDays(1);
            private bool enable = false;

            private Action Refresh;  // The function to refresh the CollectionView on property update.

            /// <summary>
            /// Lower bound of the filter.
            /// </summary>
            public DateTime Low
            {
                get
                {
                    return this.low;
                }
                set
                {
                    this.low = value;
                    this.Refresh();
                    NotifyPropertyChanged();
                }
            }

            /// <summary>
            /// Higher bound of the filter.
            /// </summary>
            public DateTime High
            {
                get
                {
                    return this.high;
                }
                set
                {
                    this.high = value;
                    this.Refresh();
                    NotifyPropertyChanged();
                }
            }

            /// <summary>
            /// Enable the filter or not.
            /// </summary>
            public bool Enable
            {
                get
                {
                    return this.enable;
                }
                set
                {
                    this.enable = value;
                    this.Refresh();
                    NotifyPropertyChanged();
                }
            }

            public TimeFilter(Action refresh)
            {
                this.Refresh = refresh;
            }

            public bool Filter(DateTime timeValue)
            {
                return !this.Enable ||  (timeValue > this.Low && timeValue < this.High);
            }
        }

        private CollectionViewSource eventCollectionViewSource;

        private string subjectFilter = "";
        private string descriptionFilter = "";
        private TimeFilter startTimeFilter;
        private TimeFilter endTimeFilter;

        /// <summary>
        /// Collection view for calendar event DataGrid.
        /// </summary>
        public ICollectionView EventCollectionView => eventCollectionViewSource.View;

        /// <summary>
        /// Event's subject filter words.
        /// </summary>
        public string SubjectFilter
        {
            get
            {
                return this.subjectFilter;
            }
            set
            {
                this.subjectFilter = value;
                this.EventCollectionView.Refresh();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event's description filter words.
        /// </summary>
        public string DescriptionFilter
        {
            get
            {
                return this.descriptionFilter;
            }
            set
            {
                this.descriptionFilter = value;
                this.EventCollectionView.Refresh();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Event's start time filter values.
        /// </summary>
        public TimeFilter StartTimeFilter => startTimeFilter;

        /// <summary>
        /// Event's end time filter values.
        /// </summary>
        public TimeFilter EndTimeFilter => endTimeFilter;


        public EventListVM(ObservableCollection<CalendarEventVM> eventVMs)
        {
            this.eventCollectionViewSource = new CollectionViewSource()
            {
                Source = eventVMs,
                IsLiveFilteringRequested = true
            };
            this.startTimeFilter = new TimeFilter(this.EventCollectionView.Refresh);
            this.endTimeFilter = new TimeFilter(this.EventCollectionView.Refresh);
            this.eventCollectionViewSource.Filter += EventCollectionViewSource_Filter;
            this.eventCollectionViewSource.LiveFilteringProperties.Add("CalendarCategoryVM.IsDisplayed");
        }

        private void EventCollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            var eventVM = (CalendarEventVM)e.Item;
            e.Accepted = eventVM.Subject.Contains(this.SubjectFilter)
                         && eventVM.Description.Contains(this.DescriptionFilter)
                         && this.StartTimeFilter.Filter(eventVM.StartTime)
                         && this.EndTimeFilter.Filter(eventVM.EndTime)
                         && eventVM.CalendarCategoryVM.IsDisplayed;
        }
    }
}