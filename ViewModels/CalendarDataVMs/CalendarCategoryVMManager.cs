using System.Collections.ObjectModel;
using System.Linq;
using TienIchLich.Models;
using TienIchLich.Services;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Manager for calendar category view models.
    /// Used to access database.
    /// </summary>
    public class CalendarCategoryVMManager : ViewModelBase
    {
        private DialogService dialogService;

        /// <summary>
        /// Calendar category view models.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CalendarCategoryVMs { get; private set; }

        public CalendarCategoryVMManager(DialogService dialogService)
        {
            this.dialogService = dialogService;
            CalendarCategoryVMs = new ObservableCollection<CalendarCategoryVM>();

            // Build view models for all calendar categories in database
            using (var db = new CalendarDbContext())
            {
                foreach (CalendarCategory categoryModel in db.CalendarCategories)
                    CalendarCategoryVMs.Add(GetVMFromCalendarCategoryModel(categoryModel));
            }
        }

        /// <summary>
        /// Get view model of a calendar category from model data.
        /// </summary>
        /// <param name="calendarCategory">Calendar category model object</param>
        /// <returns></returns>
        private CalendarCategoryVM GetVMFromCalendarCategoryModel(CalendarCategory calendarCategory)
        {
            return new CalendarCategoryVM(this, dialogService)
            {
                Id = calendarCategory.CalendarCategoryId,
                Name = calendarCategory.Name,
                DisplayColor = calendarCategory.DisplayColor
            };
        }

        /// <summary>
        /// Add a new calendar category into database.
        /// </summary>
        /// <param name="categoryVM">View model of calendar category to add</param>
        public void AddCalendarCategory(CalendarCategoryVM categoryVM)
        {
            using (var db = new CalendarDbContext())
            {
                var newCategory = new CalendarCategory()
                {
                    Name = categoryVM.Name,
                    DisplayColor = categoryVM.DisplayColor
                };

                db.CalendarCategories.Add(newCategory);
                db.SaveChanges();
                categoryVM.Id = newCategory.CalendarCategoryId;
            }

            CalendarCategoryVMs.Add(categoryVM);
        }

        /// <summary>
        /// Edit a calendar category in database.
        /// </summary>
        /// <param name="categoryVM">View model of calendar category to edit</param>
        public void EditCalendarCategory(CalendarCategoryVM categoryVM)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarCategory categoryToEdit = db.CalendarCategories.Single(i => i.CalendarCategoryId == categoryVM.Id);
                categoryToEdit.Name = categoryVM.Name;
                categoryToEdit.DisplayColor = categoryVM.DisplayColor;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Delete a calendar category from database.
        /// </summary>
        /// <param name="categoryVM">View model of calendar category to delete</param>
        public void DeleteCalendarCategory(CalendarCategoryVM categoryVM)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarCategory categoryToEdit = db.CalendarCategories.Single(i => i.CalendarCategoryId == categoryVM.Id);
                db.CalendarCategories.Remove(categoryToEdit);
                db.SaveChanges();
            }

            CalendarCategoryVMs.Remove(categoryVM);
        }
    }
}
