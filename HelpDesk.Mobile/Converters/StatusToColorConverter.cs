using System.Globalization;

namespace HelpDesk.Mobile.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            switch (status)
            {
                case "Aberto":
                    return Application.Current.Resources["StatusOpenColor"];
                case "Em andamento":
                    return Application.Current.Resources["StatusInProgressColor"];
                case "Fechado":
                    return Application.Current.Resources["StatusClosedColor"];
                default:
                    return Application.Current.Resources["SecondaryTextColor"];
            }
        }
        return Application.Current.Resources["SecondaryTextColor"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
