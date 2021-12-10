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

        /// <summary>
        /// Id of this category in database for searching.
        /// </summary>
        public int Id
        {
            get
            {
                return data.id;
            }
            set
            {
                data.id = value;
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
                return data.name;
            }
            set
            {
                data.name = value;
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
                return data.displayColor;
            }
            set
            {
                data.displayColor = value;
                SetSelectedOptionFromDisplayColor();
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
                return data.isDisplayed;
            }
            set
            {
                data.isDisplayed = value;
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
                    if (Name == "")
                    {
                        result = "Tên không được rỗng!";
                        canDelete = false;
                    }
                    else
                        canDelete = true;
                }
                return result;
            }
        }

        // Can this category be deleted.
        private bool canDelete = true;

        /// <summary>
        /// Command to delete this category.
        /// </summary>
        public ICommand DeleteCommand { get; private set; }

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
            DeleteCommand = new RelayCommand(i => DeleteCategory(),
                                                  i => canDelete);

            // Default values for a new category created on a DataGrid.
            Id = -1;
            Name = "(Tên trống)";
            DisplayColor = DisplayColorOptions[0].HexCode;
            IsDisplayed = true;
        }

        /// <summary>
        /// Set DisplayColor value from the selected display color option.
        /// </summary>
        private void SetDisplayColorFromSelectedOption()
        {
            if (SelectedDisplayColorOption.Id == CategoryDisplayColorOptionId.Custom)
                DisplayColor = CustomDisplayColorOption;
            else
                DisplayColor = SelectedDisplayColorOption.HexCode;
        }

        /// <summary>
        /// Set selected display color option from DisplayColor value.
        /// </summary>
        private void SetSelectedOptionFromDisplayColor()
        {
            SelectedDisplayColorOption = DisplayColorOptions
                .Where(i => i.HexCode == DisplayColor)
                .DefaultIfEmpty(DisplayColorOptions[7])
                .First();

            if (SelectedDisplayColorOption.Id == CategoryDisplayColorOptionId.Custom)
                CustomDisplayColorOption = DisplayColor;
        }

        /// <summary>
        /// Delete this category.
        /// </summary>
        private void DeleteCategory()
        {
            if (dialogService.ShowConfirmation("Bạn có muốn xóa loại lịch này?"))
                calendarCategoryVMManager.DeleteCalendarCategory(this);
        }

        // DataGrid editing helper methods.
        public void BeginEdit()
        {
            backupData = data;
        }

        public void CancelEdit()
        {
            Name = backupData.name;
            DisplayColor = backupData.displayColor;
        }

        public void EndEdit()
        {
            SetDisplayColorFromSelectedOption();
            calendarCategoryVMManager.EditCalendarCategory(this);
        }
    }
}