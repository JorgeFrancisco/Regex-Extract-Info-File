using System;
using System.Globalization;

namespace ExtractInfoSaveSettlementsLog.Utils.String
{
    public static class FormatString
    {
        public static string FormatToTimeString(TimeSpan timeSpan, bool enableMillisecondsPrecision = true)
        {
            if (enableMillisecondsPrecision)
            {
                return $"{timeSpan:hh\\:mm\\:ss\\.fff}";
            }

            return $"{timeSpan:hh\\:mm\\:ss}";
        }

        public static string FormatToTimeString(double timeInSeconds, bool enableMillisecondsPrecision = true)
        {
            TimeSpan time = timeInSeconds <= 0 ? default(TimeSpan) : TimeSpan.FromSeconds(timeInSeconds);

            return FormatToTimeString(time, enableMillisecondsPrecision);
        }

        public static string FormatToIntString(double number)
        {
            var nfi = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

            return number.ToString("#,##0", nfi);
        }

        public static string FormatToDoubleString(double number)
        {
            var nfi = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

            return number.ToString("#,##0.00", nfi);
        }
    }
}
