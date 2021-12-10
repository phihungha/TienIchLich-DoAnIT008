using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    [ValueConversion(typeof(CategoryDisplayColorOptionId), typeof(string))]
    public class CategoryDisplayColorOptionIdToStringConverter : IValueConverter
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