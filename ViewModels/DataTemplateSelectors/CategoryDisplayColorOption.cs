using System.Windows;
using System.Windows.Controls;

namespace TienIchLich.ViewModels.DataTemplateSelectors
{
    /// <summary>
    /// Select appropriate category display color option display (text or color picker).
    /// </summary>
    public class CategoryDisplayColorOption : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (element != null && item != null && item is ViewModels.CategoryDisplayColorOption)
            {
                var optionItem = (ViewModels.CategoryDisplayColorOption)item;

                if (optionItem.Id == CategoryDisplayColorOptionId.Custom)
                    return
                        element.FindResource("CustomCategoryDisplayColorOptionTemplate") as DataTemplate;
                else
                    return
                        element.FindResource("StandardCategoryDisplayColorOptionTemplate") as DataTemplate;
            }

            return null;
        }
    }
}