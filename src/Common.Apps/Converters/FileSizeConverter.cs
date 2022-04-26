using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Common.Apps.Converters
{
    public class FileSizeConverter : IValueConverter
    {
        readonly string[] suffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "BB", "NB", "DB", "CB", "XB" };
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;

            var r = FormatBytes((long)value);
            return $"{r.Item1}{r.Item2}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Tuple<string, string> FormatBytes(long bytes)
        {
            if (bytes == 0)
                return new Tuple<string, string>("0", suffix[0]);

            int i;
            double convertedSize = bytes;
            string s = string.Empty;
            for (i = 0; i < suffix.Length - 1 && convertedSize > 1024; i++)
            {
                convertedSize = convertedSize / 1024.0;
                s = suffix[i + 1];
            }
            string fractional = GetFractional(convertedSize);
            return new Tuple<string, string>(String.Format("{0:0." + fractional + "}", convertedSize), s);
        }
        private string GetFractional(double bytes, int? maxCount = null, int? maxFractional = null)
        {
            StringBuilder sb = new();
            int length = (bytes).ToString().Length;
            if (maxCount == null)
                maxCount = length;
            if (maxFractional == null)
                maxFractional = length;

            int i;
            for (i = 0; i < maxCount; length++)
            {
                sb.Append("#");
                if (sb.Length >= maxFractional)
                    break;
            }

            return sb.ToString();
        }
    }
}
