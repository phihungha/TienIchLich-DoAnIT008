using System.Windows;
using System.Windows.Controls;

namespace TienIchLich.ViewModels
{
    public class CategoryColorOptionDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is CategoryDisplayColorOption)
            {
                var optionItem = (CategoryDisplayColorOption)item;

                if (optionItem.Name == "Tùy chọn")
                    return
                        element.FindResource("CustomCategoryColorOptionTemplate") as DataTemplate;
                else
                    return
                        element.FindResource("StandardCategoryColorOptionTemplate") as DataTemplate;
            }

            return null;
        }
    }
}
