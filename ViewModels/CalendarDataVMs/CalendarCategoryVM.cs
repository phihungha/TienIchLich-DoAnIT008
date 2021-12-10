using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using TienIchLich.Services;

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
    public class CalendarCategoryVM : ViewModelBase, IEditableObject, IDataErrorInfo
    {
        private CalendarCategoryVMManager calendarCategoryVMManager;
        private DialogService dialogService;

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

        private bool canDelete = true; // Can this category be deleted.

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
        public ICommand DeleteCommand { get; private set; }

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

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Name")
                {
                    if (this.Name == "")
                    {
                        result = "Tên không được rỗng!";
                        this.canDelete = false;
                    }
                    else
                        this.canDelete = true;
                }
                return result;
            }
        }

        public CalendarCategoryVM(CalendarCategoryVMManager calendarCategoryVMManager, DialogService dialogService)
        {
            this.calendarCategoryVMManager = calendarCategoryVMManager;
            this.dialogService = dialogService;
            this.DeleteCommand = new RelayCommand(i => this.DeleteCategory(), 
                                                  i => this.canDelete);

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

        private void DeleteCategory()
        {
            if (this.dialogService.ShowConfirmation("Bạn có muốn xóa loại lịch này?"))
                this.calendarCategoryVMManager.DeleteCalendarCategory(this);
        }

        public void BeginEdit()
        {
            this.backupData = data;
        }

        public void CancelEdit()
        {
            this.Name = this.backupData.name;
            this.DisplayColor = this.backupData.displayColor;
        }

        public void EndEdit()
        {
           this.SetDisplayColorFromSelectedOption();
           this.calendarCategoryVMManager.EditCalendarCategory(this);
        }
    }
}