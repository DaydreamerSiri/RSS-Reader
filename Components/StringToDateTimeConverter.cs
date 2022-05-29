using System;
using System.Globalization;
using System.Windows.Data;


namespace RSS_Reader.Components
{


    [ValueConversion(typeof(DateTime), typeof(string))]
    internal class StringToDateTimeConverter : IValueConverter
    {
        string[] dayArray = new string[7] { "Mon, ", "Tue, ", "Wed, ", "Thu, ", "Fri, ", "Sat, ", "Sun, " };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Formatiere das Datum eintrag und entfernt unnötige Ausgaben / Chars
            value = value.ToString().Replace(" Z", "");
            for (int i = 0; i < dayArray.Length; i++)
            {
                value = value.ToString().Replace(dayArray[i], "");
            }

#pragma warning disable CS8604 // Mögliches Nullverweisargument.
            DateTime date = DateTime.ParseExact(value.ToString(), "dd MMM yyyy HH:mm:ss", culture);

            if ((DateTime.Now - date).TotalDays <= 3 & (DateTime.Now - date).TotalDays >= 1)
            {
                return $"Vor {(int)(DateTime.Now - date).TotalDays} Tagen";
            }
            else if ((DateTime.Now - date).TotalDays <= 1)
            {
                return $"Vor {(DateTime.Now - date).Hours} Stunden";
            }
            else
            {
                return date.ToString("dd MMM yyyy HH:mm:ss");
            }

        }

        public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            double outValue = 0;
            if (value != null)
            {
                outValue = System.Convert.ToDouble(outValue);
            }
            return outValue;

        }
    }
}