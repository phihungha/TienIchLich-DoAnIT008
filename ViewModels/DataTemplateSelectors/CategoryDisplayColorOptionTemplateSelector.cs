﻿using System.Windows;
using System.Windows.Controls;

namespace TienIchLich.ViewModels
{
    public class CategoryDisplayColorOptionTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is CategoryDisplayColorOption)
            {
                var optionItem = (CategoryDisplayColorOption)item;

                if (optionItem.HexCode == "")
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