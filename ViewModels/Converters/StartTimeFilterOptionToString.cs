using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels.Converters
{
    /// <summary>
    /// Convert start time filter options of upcoming event overview to user-readable text.
    /// </summary>
    [ValueConversion(typeof(UpcomingOverviewStartTimeFilterOptionId), typeof(string))]
    public class StartTimeFilterOptionToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            UpcomingOverviewStartTimeFilterOptionId optionId;
            if (value != null && value is UpcomingOverviewStartTimeFilterOptionId)
                optionId = (UpcomingOverviewStartTimeFilterOptionId)value;
            else
                return "";

            switch (optionId)
            {
                case UpcomingOverviewStartTimeFilterOptionId.Week1:
                    return "1 tuần";

                case UpcomingOverviewStartTimeFilterOptionId.Week2:
                    return "2 tuần";

                case UpcomingOverviewStartTimeFilterOptionId.Month1:
                    return "1 tháng";

                case UpcomingOverviewStartTimeFilterOptionId.Month6:
                    return "6 tháng";

                case UpcomingOverviewStartTimeFilterOptionId.Year1:
                    return "1 năm";

                case UpcomingOverviewStartTimeFilterOptionId.All:
                    return "Tất cả";

                case UpcomingOverviewStartTimeFilterOptionId.Custom:
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