using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TienIchLich
{
    /// <summary>
    /// View model of a calendar event.
    /// </summary>
    public class CalendarCategoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string name = "";
        string displayColor = "#000000";

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Category display color.
        /// </summary>
        public string DisplayColor
        {
            get
            {
                return displayColor;
            }
            set
            {
                displayColor = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Raises property change event.
        /// </summary>
        /// <param name="propertyName"></param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
