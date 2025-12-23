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
    class FlagConverter: IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            var current = (WeekDays)values;
            var flag = (WeekDays)Enum.Parse(typeof(WeekDays), parameter.ToString());

            return current.HasFlag(flag);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
