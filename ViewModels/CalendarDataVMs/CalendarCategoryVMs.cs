using System.Collections.ObjectModel;
using System.Linq;
using TienIchLich.Models;

namespace TienIchLich.ViewModels
{
    public class CalendarCategoryVMs : ViewModelBase
    {
        private ObservableCollection<CalendarCategoryVM> categoryVMs = new();

        /// <summary>
        /// Calendar category view models.
        /// </summary>
        public ObservableCollection<CalendarCategoryVM> CategoryVMs { get => categoryVMs; }

        public CalendarCategoryVMs()
        {
            // Build view models from all calendar categories in database
            using (var db = new CalendarDbContext())
            {
                foreach (CalendarCategory categoryModel in db.CalendarCategories)
                    this.categoryVMs.Add(GetVMFromCalendarCategoryModel(categoryModel));
            }
        }

        /// <summary>
        /// Build view model of calendar category.
        /// </summary>
        /// <param name="calendarCategory">Calendar category model object.</param>
        /// <returns></returns>
        private CalendarCategoryVM GetVMFromCalendarCategoryModel(CalendarCategory calendarCategory)
        {
            return new CalendarCategoryVM(this)
            {
                Id = calendarCategory.CalendarCategoryId,
                Name = calendarCategory.Name,
                DisplayColor = calendarCategory.DisplayColor
            };
        }

        /// <summary>
        /// Add new calendar category into database from provided calendar category view model.
        /// </summary>
        /// <param name="categoryVM">View model of calendar category to add.</param>
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

            // Add to view model collection to update displayed ItemControls
            this.CategoryVMs.Add(categoryVM);
        }

        /// <summary>
        /// Edit calendar category of provided calendar category view model in database.
        /// </summary>
        /// <param name="categoryVM">View model of calendar category to edit.</param>
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
        /// Delete calendar category of provided calendar category view model from database.
        /// </summary>
        /// <param name="categoryVM">View model of calendar category to delete.</param>
        public void DeleteCalendarCategory(CalendarCategoryVM categoryVM)
        {
            using (var db = new CalendarDbContext())
            {
                CalendarCategory categoryToEdit = db.CalendarCategories.Single(i => i.CalendarCategoryId == categoryVM.Id);
                db.CalendarCategories.Remove(categoryToEdit);
                db.SaveChanges();
            }

            // Remove from view model collection to update displayed ItemControls
            this.CategoryVMs.Remove(categoryVM);
        }
    }
}
