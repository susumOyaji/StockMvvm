using System;
using System.Globalization;
using Xamarin.Forms;

namespace StockMvvm
{
   
       
        public class NullToDefaultConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
            if (value == null)
            {
                value = "Gray";
            }
            return value;//return (int)value != 0;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }
        }


}

