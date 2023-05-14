using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeFinance.DataAccess.EFBasic.Util
{
    internal class DateConverter
    {
        readonly TimeZoneInfo _timeZone;
        public DateConverter(TimeZoneInfo timeZoneInfo)
        {
            _timeZone = timeZoneInfo;
        }

        public DateTime ToLocalDateTime(DateTime dateTime)
        {
            var dateTimeUtc = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTime(dateTimeUtc, _timeZone);
        }
        public DateTime ToUtcDateTime(DateTime localDateTime)
        {
            var dateTimeUnspec = DateTime.SpecifyKind(localDateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, _timeZone);
        }

    }
}
