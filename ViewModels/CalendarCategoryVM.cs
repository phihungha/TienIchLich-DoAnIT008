using System.ComponentModel;
using System.Runtime.CompilerServices;
using TienIchLich.ViewModels;

namespace TienIchLich
{
    /// <summary>
    /// View model of a calendar category for 
    /// displaying in ItemsControl and passing around.
    /// </summary>
    public class CalendarCategoryVM : ViewModelBase
    {
        private int id = 0;
        private string name = "(Không tên)";
        private string displayColor = "#000000";

        /// <summary>
        /// Id of category in the model for searching.
        /// </summary>
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
