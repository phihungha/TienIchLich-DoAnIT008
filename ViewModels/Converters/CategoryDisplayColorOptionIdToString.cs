using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels.Converters
{
    /// <summary>
    /// Convert calendar category display color option identifier to user-readable text.
    /// </summary>
    [ValueConversion(typeof(CategoryDisplayColorOptionId), typeof(string))]
    public class CategoryDisplayColorOptionIdToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CategoryDisplayColorOptionId optionId;
            try
            {
                optionId = (CategoryDisplayColorOptionId)value;
            }
            catch (InvalidCastException)
            {
                return "";
            }

            switch (optionId)
            {
                case CategoryDisplayColorOptionId.Black:
                    return "Đen";

                case CategoryDisplayColorOptionId.Red:
                    return "Đỏ";

                case CategoryDisplayColorOptionId.Yellow:
                    return "Vàng";

                case CategoryDisplayColorOptionId.Green:
                    return "Xanh lá";

                case CategoryDisplayColorOptionId.Blue:
                    return "Xanh nước biển";

                case CategoryDisplayColorOptionId.Cyan:
                    return "Xanh nhạt";

                case CategoryDisplayColorOptionId.Purple:
                    return "Tím";

                default:
                    return optionId.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}