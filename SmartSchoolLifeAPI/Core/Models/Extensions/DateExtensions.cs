using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SmartSchoolLifeAPI.Core.Models.Extensions
{
    // Class for Extensions methods that help to check Date.
    public static class DateExtensions
    {
        public static DateTime ConvertStringToDate(this string date)
        {
            try
            {
                string pattern = @"[/\-]"; // Define the pattern to match "/" or "-" 😎;
                var dateParts = Regex.Split(date, pattern).Select(int.Parse).ToList();
                var day = dateParts[0];
                var month = dateParts[1];
                var year = dateParts[2];
                if (day.ToString().Length > 2 || day.ToString().Length < 1 || month.ToString().Length > 2 || month.ToString().Length < 1 || year.ToString().Length != 4)
                    throw new FormatException("Invalid date format");
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

        public static string ToTime(this string date) => date.ConvertStringToDate().ToShortTimeString();

    }
}