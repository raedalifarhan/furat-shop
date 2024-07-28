namespace API.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static string ToCustomFormat(this DateTime dateTime)
        {
            return dateTime.ToString("dd.MM.yyyy hh:mm tt");
        }
    }

}