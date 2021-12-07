using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Category display color option identifiers.
    /// </summary>
    public enum CategoryDisplayColorOptionId
    {
        Black,
        Red,
        Yellow,
        Green,
        Cyan,
        Blue,
        Purple,
        Custom
    }

    /// <summary>
    /// Info of each category display option.
    /// </summary>
    public struct CategoryDisplayColorOption
    {
        public CategoryDisplayColorOptionId Id { get; set; }
        public string HexCode { get; set; }
    }

    /// <summary>
    /// View model of a calendar category for
    /// displaying in ItemsControl and passing around.
    /// Provides support for editing directly on this object in the view.
    /// </summary>
    public class CalendarCategoryVM : ViewModelBase, IEditableObject
    {
        private CalendarCategoryVMManager calendarCategoryVMManager;

        private CategoryDisplayColorOption selectedDisplayColorOption;
        private static CategoryDisplayColorOption[] displayColorOptions =
        {
            new CategoryDisplayColorOption() 
            {   
                Id = CategoryDisplayColorOptionId.Black, 
                HexCode = Colors.Black.ToString() 
            },
            new CategoryDisplayColorOption()
            {
                Id = CategoryDisplayColorOptionId.Red,
                HexCode = Colors.Red.ToString()
            },
            new CategoryDisplayColorOption()
            {
                Id = CategoryDisplayColorOptionId.Yellow,
                HexCode = Colors.Yellow.ToString()
            },
            new CategoryDisplayColorOption()
            {
                Id = CategoryDisplayColorOptionId.Green,
                HexCode = Colors.Green.ToString()
            },
            new CategoryDisplayColorOption()
            {
                Id = CategoryDisplayColorOptionId.Cyan,
                HexCode = Colors.Cyan.ToString()
            },
            new CategoryDisplayColorOption()
            {
                Id = CategoryDisplayColorOptionId.Blue,
                HexCode = Colors.Blue.ToString()
            },
            new CategoryDisplayColorOption()
            {
                Id = CategoryDisplayColorOptionId.Purple,
                HexCode = Colors.Purple.ToString()
            },
            new CategoryDisplayColorOption()
            {
                Id = CategoryDisplayColorOptionId.Custom,
                HexCode = "#ffffff"
            },
        };
        private string customDisplayColorOption = "#ffffff";

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

        /// <summary>
        /// Command to delete this calendar category.
        /// </summary>
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

        public CalendarCategoryVM(CalendarCategoryVMManager calendarCategoryVMs)
        {
            this.calendarCategoryVMManager = calendarCategoryVMs;
            this.deleteCommand = new RelayCommand(i => calendarCategoryVMs.DeleteCalendarCategory(this));

            this.Id = -1;
            this.Name = "(Tên trống)";
            this.DisplayColor = DisplayColorOptions[0].HexCode;
            this.IsDisplayed = true;
        }

        /// <summary>
        /// Set category view model's DisplayColor with the selected option.
        /// </summary>
        private void SetDisplayColorFromSelectedOption()
        {
            if (this.SelectedDisplayColorOption.Id == CategoryDisplayColorOptionId.Custom)
                this.DisplayColor = this.CustomDisplayColorOption;
            else
                this.DisplayColor = this.SelectedDisplayColorOption.HexCode;
        }

        /// <summary>
        /// Set currently selected option from DisplayColor.
        /// </summary>
        private void SetSelectedOptionFromDisplayColor()
        {
            this.SelectedDisplayColorOption = DisplayColorOptions
                .Where(i => i.HexCode == this.DisplayColor)
                .DefaultIfEmpty(DisplayColorOptions[7])
                .First();

            if (this.SelectedDisplayColorOption.Id == CategoryDisplayColorOptionId.Custom)
                this.CustomDisplayColorOption = this.DisplayColor;
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
            this.calendarCategoryVMManager.EditCalendarCategory(this);
        }
    }
}