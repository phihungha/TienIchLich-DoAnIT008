using System.Windows;
using System.Windows.Controls;
using TienIchLich.ViewModels;

namespace TienIchLich.MonthEventCalendarControl
{
    /// <summary>
    /// Select event card template to use depends on the status of the event.
    /// </summary>
    public class CalendarEventCardTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null && item is CalendarEventStatusVM)
            {
                var status = (CalendarEventStatusVM)item;

                if (status == CalendarEventStatuses.Upcoming)
                    return element.FindResource("UpcomingEventCardTemplate") as DataTemplate;
                else if (status == CalendarEventStatuses.Happening)
                    return element.FindResource("HappeningEventCardTemplate") as DataTemplate;
                else if (status == CalendarEventStatuses.Finished)
                    return element.FindResource("FinishedEventCardTemplate") as DataTemplate;
            }

            return null;
        }
    }
}