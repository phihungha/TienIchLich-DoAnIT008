using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Identifiers for category display color option.
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
    /// Info of each category display color option.
    /// </summary>
    public struct CategoryDisplayColorOption
    {
        /// <summary>
        /// Identifier of this option.
        /// </summary>
        public CategoryDisplayColorOptionId Id { get; set; }

        /// <summary>
        /// Color hex code of this option.
        /// </summary>
        public string HexCode { get; set; }
    }

    /// <summary>
    /// View model of a calendar category for displaying in ItemsControl.
    /// Provides support for editing in a DataGrid.
    /// </summary>
    public class CalendarCategoryVM : ViewModelBase, IEditableObject, IDataErrorInfo
    {
        private CalendarCategoryVMManager calendarCategoryVMManager;
        private DialogService dialogService;

        /// <summary>
        /// Data values of this category.
        /// </summary>
        private struct Data
        {
            public int id;
            public string name;
            public string displayColor;
            public bool isDisplayed;
        }

        // Current data values
        private Data data;

        // Backup data values to recover when cancel editing
        private Data backupData;

        // Can this category be deleted.
        private bool canDelete = true;

        /// <summary>
        /// Command to delete this category.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

        /// <summary>
        /// Id of this category in database for searching.
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
        /// Name of this category.
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
        /// Display color of this category.
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

        /// <summary>
        /// True if events belong to this category is displayed.
        /// </summary>
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

        // Data validation
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

        /// <summary>
        /// Options for display color.
        /// </summary>
        public static CategoryDisplayColorOption[] DisplayColorOptions => displayColorOptions;

        private CategoryDisplayColorOption selectedDisplayColorOption;

        /// <summary>
        /// Selected display color option.
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

        private string customDisplayColorOption = "#ffffff";

        /// <summary>
        /// Selected custom display color.
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

        public CalendarCategoryVM(CalendarCategoryVMManager calendarCategoryVMManager, DialogService dialogService)
        {
            this.calendarCategoryVMManager = calendarCategoryVMManager;
            this.dialogService = dialogService;
            this.DeleteCommand = new RelayCommand(i => this.DeleteCategory(),
                                                  i => this.canDelete);

            // Default values for a new category created on a DataGrid.
            this.Id = -1;
            this.Name = "(Tên trống)";
            this.DisplayColor = DisplayColorOptions[0].HexCode;
            this.IsDisplayed = true;
        }

        /// <summary>
        /// Set DisplayColor value from the selected display color option.
        /// </summary>
        private void SetDisplayColorFromSelectedOption()
        {
            if (this.SelectedDisplayColorOption.Id == CategoryDisplayColorOptionId.Custom)
                this.DisplayColor = this.CustomDisplayColorOption;
            else
                this.DisplayColor = this.SelectedDisplayColorOption.HexCode;
        }

        /// <summary>
        /// Set selected display color option from DisplayColor value.
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

        /// <summary>
        /// Delete this category.
        /// </summary>
        private void DeleteCategory()
        {
            if (this.dialogService.ShowConfirmation("Bạn có muốn xóa loại lịch này?"))
                this.calendarCategoryVMManager.DeleteCalendarCategory(this);
        }

        // DataGrid editing helper methods.
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