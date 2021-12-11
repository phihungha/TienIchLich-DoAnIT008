using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Convert start time filter options of upcoming event overview to strings for ComboBox.
    /// </summary>
    [ValueConversion(typeof(UpcomingOverviewVM.StartTimeFilterOptionId), typeof(string))]
    public class StartTimeFilterOptionToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UpcomingOverviewVM.StartTimeFilterOptionId optionId;
            if (value != null && value is UpcomingOverviewVM.StartTimeFilterOptionId)
                optionId = (UpcomingOverviewVM.StartTimeFilterOptionId)value;
            else
                return "";

            switch (optionId)
            {
                case UpcomingOverviewVM.StartTimeFilterOptionId.Week1:
                    return "1 tuần";

                case UpcomingOverviewVM.StartTimeFilterOptionId.Week2:
                    return "2 tuần";

                case UpcomingOverviewVM.StartTimeFilterOptionId.Month1:
                    return "1 tháng";

                case UpcomingOverviewVM.StartTimeFilterOptionId.Month6:
                    return "6 tháng";

                case UpcomingOverviewVM.StartTimeFilterOptionId.Year1:
                    return "1 năm";

                case UpcomingOverviewVM.StartTimeFilterOptionId.All:
                    return "Tất cả";

                case UpcomingOverviewVM.StartTimeFilterOptionId.Custom:
                    return "Tùy chọn";

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