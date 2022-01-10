using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class DateTimeExtension
    {
        public static DateTime? ConvertToDateTimeFormat(this string time, string format = "dd/MM/yyyy")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(time) || time == "Invalid date")
                {
                    return null;
                }
                return DateTime.ParseExact(time, format, CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}
