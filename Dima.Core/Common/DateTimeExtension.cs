namespace Dima.Core.Common;

public static class DateTimeExtension
{
    extension(DateTime date)
    {
        public DateTime GetFirstDay(int? year = null, int? month = null)
        {
            return new DateTime(year ?? date.Year, month ?? date.Month, 1);
        }

        public DateTime GetLastDay(int? year = null, int? month = null)
        {
            return new DateTime(year ?? date.Year, month ?? date.Month, 1).AddMonths(1).AddDays(-1);
        }
    }
}