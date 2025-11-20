using System.Globalization;
using Microsoft.Maui.Controls;

namespace HelpDesk.Mobile.Converters
{
    public class BoolToHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isUser && parameter is string paramString)
            {
                // Espera algo como "End|Start"
                var parameters = paramString.Split('|');
                if (parameters.Length == 2)
                {
                    // Se for Usuário (true), pega a primeira opção (ex: End)
                    // Se for Robô (false), pega a segunda opção (ex: Start)
                    string optionName = isUser ? parameters[0] : parameters[1];

                    return GetLayoutOption(optionName);
                }
            }
            return LayoutOptions.Start;
        }

        // Método auxiliar para converter String em LayoutOptions
        private LayoutOptions GetLayoutOption(string optionName)
        {
            return optionName?.ToLower().Trim() switch
            {
                "start" => LayoutOptions.Start,
                "center" => LayoutOptions.Center,
                "end" => LayoutOptions.End,
                "fill" => LayoutOptions.Fill,
                _ => LayoutOptions.Start // Padrão se não encontrar
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}