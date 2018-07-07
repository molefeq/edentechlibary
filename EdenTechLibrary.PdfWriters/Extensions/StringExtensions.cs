using EdenTechLibrary.PdfWriters.Utilities;

using System;
using System.Globalization;

namespace EdenTechLibrary.PdfWriters.Extensions
{
    public static class StringExtensions
    {
        public static DateTime StringToDateTime(this string dateText)
        {
            DateTime date;

            if (!string.IsNullOrEmpty(dateText) && DateTime.TryParseExact(dateText, Constants.DATE_FORMAT, CultureInfo.CurrentUICulture, DateTimeStyles.None, out date))
            {
                return date;
            }

            return DateTime.MinValue;
        }

        public static decimal StringToDecimal(this string decimalText)
        {
            decimal decimalValue;

            if (!string.IsNullOrEmpty(decimalText) && Decimal.TryParse(decimalText, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, CultureInfo.CurrentUICulture, out decimalValue))
            {
                return decimalValue;
            }

            return decimal.MinValue;
        }
    }
}
