using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchoolLifeAPI.Models.Extensions
{
    // Class for Extensions methods that help to check Date.
    public static class DateExtensions
    {
        private static DateTime ConvertStringToDate(this string date)
        {
            try
            {
                var dateParts = date.Split('/').Select(int.Parse).ToList();
                DateTime givenDate = new DateTime(dateParts[2], dateParts[1], dateParts[0]);
                return givenDate;
            }
            catch (FormatException ex)
            {
                throw new FormatException("Invalid date format", ex);
            }
        }

        public static bool IsFutureDate(this string date)
        {
            try
            {
                return date.ConvertStringToDate().Date > DateTime.Today;
            }
            catch (FormatException ex)
            {
                throw new FormatException("Invalid date format", ex);
            }
        }

        public static bool IsWeekend(this string date)
        {
            var weekendDays = new List<string> { "Fri", "Sat" };
            string dayName = date.ConvertStringToDate().ToString("ddd");
            return weekendDays.Contains(dayName);
        }

        public static string ToTime (this string date) => date.ConvertStringToDate().ToShortTimeString();

    }
}