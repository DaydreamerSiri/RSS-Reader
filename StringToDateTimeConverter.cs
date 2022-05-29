using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Xml;

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


    internal class DateTimeComparer: IComparer
    {
        ListSortDirection SortDirection;
        public DateTimeComparer(ListSortDirection direction)
        {
            this.SortDirection = direction;
        }

        public int Compare(Object? x, Object? y)
        {
            XmlElement xmlElementA = (XmlElement)x;
            XmlElement xmlElementB = (XmlElement)y;
            string dateTimeValA = "";
            string dateTimeValB = "";

            foreach (XmlElement childElement in xmlElementA.ChildNodes)
            {
                if (childElement.Name == "pubDate")
                {
                    dateTimeValA = childElement.InnerText.ToString();
                }
            }

            foreach (XmlElement childElement in xmlElementB.ChildNodes)
            {
                if (childElement.Name == "pubDate")
                {
                    dateTimeValB = childElement.InnerText.ToString();
                }
            }
            if (String.Equals(dateTimeValA, "") || String.Equals(dateTimeValB, ""))
            {
                throw new Exception("No Date to Sort");
            }

            if (SortDirection == ListSortDirection.Ascending)
            {
                return DateTime.Compare(DateTime.Parse(dateTimeValA), DateTime.Parse(dateTimeValB));
            }
            else if (SortDirection == ListSortDirection.Descending)
            {
                var a = DateTime.Compare(DateTime.Parse(dateTimeValA), DateTime.Parse(dateTimeValB));
                if (DateTime.Compare(DateTime.Parse(dateTimeValA), DateTime.Parse(dateTimeValB)) > 0)
                {
                    return -1;
                }
                else if(DateTime.Compare(DateTime.Parse(dateTimeValA), DateTime.Parse(dateTimeValB)) < 0)
                {
                    return 1;
                }
            }
            return 0;
        
        }
    }


}