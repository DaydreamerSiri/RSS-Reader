using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RSS_Reader
{
    

    [ValueConversion(typeof(DateTime), typeof(String))]
    internal class StringToDateTimeConverter : IValueConverter
    {
        String[]dayArray = new String[7] { "Mon, ", "Tue, ", "Wed, ", "Thu, ", "Fri, ", "Sat, ", "Sun, " };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Formatiere das Datum eintrag und entfernt unnötige Ausgaben / Chars
            value = value.ToString().Replace(" Z", "");
            for(int i = 0; i < dayArray.Length; i++)
            {
                value = value.ToString().Replace(dayArray[i], "");
            }
           
            #pragma warning disable CS8604 // Mögliches Nullverweisargument.
            DateTime date = DateTime.ParseExact(value.ToString(), "dd MMM yyyy HH:mm:ss", culture);

            if((DateTime.Now - date).TotalDays <= 3 & (DateTime.Now - date).TotalDays >= 1)
            {
                return ($"Vor {((int)(DateTime.Now - date).TotalDays)} Tagen");
            }
            else if ((DateTime.Now - date).TotalDays <= 1)
            {
                return ($"Vor {((int)(DateTime.Now - date).Hours)} Stunden");
            }
            else {
                return date.ToString("dd MMM yyyy HH:mm:ss");
            }
            
        }

        public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              System.Globalization.CultureInfo culture)
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