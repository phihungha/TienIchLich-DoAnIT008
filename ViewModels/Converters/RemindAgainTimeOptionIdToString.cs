using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TienIchLich.ViewModels.Converters
{
    [ValueConversion(typeof(RemindAgainTimeOptionId), typeof(string))]
    public class RemindAgainTimeOptionIdToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RemindAgainTimeOptionId optionId;
            if (value != null && value is RemindAgainTimeOptionId)
                optionId = (RemindAgainTimeOptionId)value;
            else
                return "";

            switch (optionId)
            {
                case RemindAgainTimeOptionId.Minute5:
                    return "5 phút";

                case RemindAgainTimeOptionId.Minute10:
                    return "10 phút";

                case RemindAgainTimeOptionId.Minute15:
                    return "15 phút";
            }
            return optionId.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
