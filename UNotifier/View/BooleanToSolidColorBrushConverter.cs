using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace UNotifier
{
    public class BooleanToSolidColorBrushConverter : IValueConverter
    {
        static SolidColorBrush New = Application.Current.FindResource("NewEntryBrush") as SolidColorBrush,
                               Watched = Application.Current.FindResource("WatchedEntryBrush") as SolidColorBrush;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Watched : New;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == Watched;
        }
    }
}
