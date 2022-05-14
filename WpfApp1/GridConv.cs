using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace WpfApp1
{
    public class GridConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return $"{values[0]:0.0000}-{values[1]:0.0000}";
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "0-0";
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            try
            {
                string val = value as string;
                string[] values = val.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length != 2)
                {
                    throw new InvalidOperationException("More or less than 2 values");
                }
                float val1 = float.Parse(values[0]);
                float val2 = float.Parse(values[1]);
                object[] res = new object[2];
                res[0] = val1;
                res[1] = val2;
                return res;
            }
            catch (Exception error)
            {
                MessageBox.Show($"Unexpected error: {error.Message}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                float val1 = 3.5F;
                float val2 = 3.9F;
                object[] res = new object[2];
                res[0] = val1;
                res[1] = val2;
                return res;
            }
        }
    }
}
