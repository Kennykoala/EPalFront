using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Build_School_Project_No_4.Services
{
    public class TimestampService
    {
        public  static long UtcDateTimeToUnix(DateTime x)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long result = (x.ToUniversalTime() - unixStart).Ticks;
            return result;
        }
        public static DateTime UnixToDateTime(long datestamp)
        {
            DateTime result = DateTimeOffset.FromUnixTimeMilliseconds(datestamp).DateTime;
            return result;
        }
        public static DateTime UnixToLocalDateTime(long datestamp)
        {
            DateTime result = DateTimeOffset.FromUnixTimeMilliseconds(datestamp).DateTime.ToLocalTime();
            return result;
        }
    }
}