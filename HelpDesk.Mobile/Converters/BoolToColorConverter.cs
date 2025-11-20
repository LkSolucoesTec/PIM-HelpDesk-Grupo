using System.Globalization;
using System.Reflection.Emit;
using Microsoft.Maui.Controls;

namespace HelpDesk.Mobile.Converters;

// Converte 'true' para a primeira cor e 'false' para a segunda
public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isTrue)
        {
            var parameters = parameter.ToString().Split('|');
            var trueColorKey = parameters[0];
            var falseColorKey = parameters[1];

            return Application.Current.Resources[isTrue ? trueColorKey : falseColorKey];
        }
        return Application.Current.Resources["BackgroundColor"];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
