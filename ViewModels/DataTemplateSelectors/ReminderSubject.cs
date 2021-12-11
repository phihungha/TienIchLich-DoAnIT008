using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TienIchLich.ViewModels.DataTemplateSelectors
{
    /// <summary>
    /// Select appropriate reminder title based on reminder time.
    /// </summary>
    public class ReminderSubject : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null && item is CalendarEventVM)
            {
                var eventVM = (CalendarEventVM)item;
                if (eventVM.ReminderTime.Ticks == 0)
                    return element.FindResource("SubjectWithNoReminderTimeTemplate") as DataTemplate;
                return element.FindResource("SubjectWithReminderTimeTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
