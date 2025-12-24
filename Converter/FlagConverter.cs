using NurseProblem.Models;
using NurseProblem.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NurseProblem.Converter
{
    public class FlagEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue && parameter is string flagName)
            {
                var flag = (Enum)Enum.Parse(enumValue.GetType(), flagName);
                return enumValue.HasFlag(flag);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Use MultiBinding for ConvertBack");
        }
    }



}
