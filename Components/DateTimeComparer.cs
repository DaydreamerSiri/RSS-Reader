using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;
namespace RSS_Reader.Components
{
    internal class DateTimeComparer : IComparer
    {
        ListSortDirection SortDirection;
        public DateTimeComparer(ListSortDirection direction)
        {
            SortDirection = direction;
        }

        public int Compare(object? x, object? y)
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
            if (string.Equals(dateTimeValA, "") || string.Equals(dateTimeValB, ""))
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
                else if (DateTime.Compare(DateTime.Parse(dateTimeValA), DateTime.Parse(dateTimeValB)) < 0)
                {
                    return 1;
                }
            }
            return 0;

        }
    }
}
