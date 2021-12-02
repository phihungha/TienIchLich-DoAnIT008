using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace TienIchLich.ViewModels
{
    public struct CategoryDisplayColorOption
    {
        public string Name { get; set; }
        public string HexCode { get; set; }
    }

    /// <summary>
    /// View model of a calendar category for
    /// displaying in ItemsControl and passing around.
    /// </summary>
    public class CalendarCategoryVM : ViewModelBase, IEditableObject
    {
        private CalendarCategoryVMs calendarCategoryVMs;

        private CategoryDisplayColorOption selectedDisplayColorOption;
        private static CategoryDisplayColorOption[] displayColorOptions =
        {
            new CategoryDisplayColorOption() { Name = "Đen", HexCode = Colors.Black.ToString() },
            new CategoryDisplayColorOption() { Name = "Đỏ", HexCode = Colors.Red.ToString() },
            new CategoryDisplayColorOption() { Name = "Vàng", HexCode = Colors.Yellow.ToString() },
            new CategoryDisplayColorOption() { Name = "Xanh lá", HexCode = Colors.Green.ToString() },
            new CategoryDisplayColorOption() { Name = "Xanh nhạt", HexCode = Colors.Cyan.ToString() },
            new CategoryDisplayColorOption() { Name = "Xanh dương", HexCode = Colors.Blue.ToString() },
            new CategoryDisplayColorOption() { Name = "Tím", HexCode = Colors.Purple.ToString() },
            new CategoryDisplayColorOption() { Name = "Tùy chọn", HexCode = "" }
        };
        private string customDisplayColorOption;

        private ICommand deleteCommand;

        struct Data
        {
            public int id;
            public string name;
            public string displayColor;
            public bool isDisplayed;
        }

        private Data data; // Current data values
        private Data backupData; // Backup data values to recover when cancel editing

        /// <summary>
        /// Options for category display color.
        /// </summary>
        public static CategoryDisplayColorOption[] DisplayColorOptions => displayColorOptions;

        /// <summary>
        /// Selected category display color.
        /// </summary>
        public CategoryDisplayColorOption SelectedDisplayColorOption
        {
            get
            {
                return selectedDisplayColorOption;
            }
            set
            {
                selectedDisplayColorOption = value;
                NotifyPropertyChanged();
            }
        }
        
        /// <summary>
        /// Chosen custom display color.
        /// </summary>
        public string CustomDisplayColorOption
        {
            get
            {
                return customDisplayColorOption;
            }
            set
            {
                customDisplayColorOption = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand DeleteCommand => deleteCommand;

        /// <summary>
        /// Id of category in the model for searching.
        /// </summary>
        public int Id
        {
            get
            {
                return this.data.id;
            }
            set
            {
                this.data.id = value;
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
                return this.data.name;
            }
            set
            {
                this.data.name = value;
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
                return this.data.displayColor;
            }
            set
            {
                this.data.displayColor = value;
                this.SetSelectedOptionFromDisplayColor();
                NotifyPropertyChanged();
            }
        }

        public bool IsDisplayed
        {
            get
            {
                return this.data.isDisplayed;
            }
            set
            {
                this.data.isDisplayed = value;
                NotifyPropertyChanged();
            }
        }

        public CalendarCategoryVM(CalendarCategoryVMs calendarCategoryVMs)
        {
            this.calendarCategoryVMs = calendarCategoryVMs;
            this.deleteCommand = new RelayCommand(i => calendarCategoryVMs.DeleteCalendarCategory(this));

            this.Id = -1;
            this.Name = "(Tên trống)";
            this.DisplayColor = DisplayColorOptions[0].HexCode;
            this.IsDisplayed = true;
        }

        /// <summary>
        /// Set category view model's display color with the selected option.
        /// </summary>
        private void SetDisplayColorFromSelectedOption()
        {
            if (this.SelectedDisplayColorOption.Name == "Tùy chọn")
                this.DisplayColor = this.CustomDisplayColorOption;
            else
                this.DisplayColor = this.SelectedDisplayColorOption.HexCode;
        }

        private void SetSelectedOptionFromDisplayColor()
        {
            this.SelectedDisplayColorOption = DisplayColorOptions
                .Where(i => i.HexCode == this.DisplayColor)
                .DefaultIfEmpty(DisplayColorOptions[7])
                .First();
        }

        public void BeginEdit()
        {
            this.backupData = data;
        }

        public void CancelEdit()
        {
            this.data = this.backupData;
        }

        public void EndEdit()
        {
            this.SetDisplayColorFromSelectedOption();
            this.calendarCategoryVMs.EditCalendarCategory(this);
        }
    }
}