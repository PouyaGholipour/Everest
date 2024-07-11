using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Helper
{
    public static class PersianDate
    {
        public static DateTime PersianDateTime(DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            var persianDate = new DateTime(persianCalendar.GetYear(dateTime), persianCalendar.GetMonth(dateTime), persianCalendar.GetDayOfMonth(dateTime));
            return persianDate;
        }
    }
}
