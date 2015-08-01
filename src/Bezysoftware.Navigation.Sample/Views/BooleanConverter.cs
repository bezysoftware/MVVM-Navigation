namespace Bezysoftware.Navigation.Sample.Views
{
    using System;
    using Windows.UI.Xaml.Data;

    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }
    }
}
