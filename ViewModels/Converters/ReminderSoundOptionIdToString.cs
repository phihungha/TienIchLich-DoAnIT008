using System;
using System.Globalization;
using System.Windows.Data;

namespace TienIchLich.ViewModels.Converters
{
    [ValueConversion(typeof(SettingsVM.ReminderSoundOption.OptionId), typeof(string))]
    public class ReminderSoundOptionIdToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SettingsVM.ReminderSoundOption.OptionId optionId;
            if (value != null && value is SettingsVM.ReminderSoundOption.OptionId)
                optionId = (SettingsVM.ReminderSoundOption.OptionId)value;
            else
                return "";

            switch (optionId)
            {
                case SettingsVM.ReminderSoundOption.OptionId.Sound1:
                    return "Âm báo 1";

                case SettingsVM.ReminderSoundOption.OptionId.Sound2:
                    return "Âm báo 2";

                case SettingsVM.ReminderSoundOption.OptionId.Sound3:
                    return "Âm báo 3";

                case SettingsVM.ReminderSoundOption.OptionId.Custom:
                    return "Tùy thích";
            }
            return optionId.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}