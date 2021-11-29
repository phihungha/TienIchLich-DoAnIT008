using System.ComponentModel;
using System.Runtime.CompilerServices;
using TienIchLich.ViewModels;

namespace TienIchLich
{
    /// <summary>
    /// View model of a calendar event.
    /// </summary>
    public class CalendarCategoryViewModel : ViewModelBase
    {
        int id = 0;
        string name = "";
        string displayColor = "#000000";

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Category name.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
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
    }
}
