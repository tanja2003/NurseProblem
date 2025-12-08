using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace NurseProblem.Converter
{
    public class NurseToColorConverter : IValueConverter
    {
        private static readonly Brush[] Colors = new Brush[]
        {
        Brushes.LightBlue,
        Brushes.LightGreen,
        Brushes.LightPink,
        Brushes.LightSalmon,
        Brushes.LightYellow,
        Brushes.LightCyan,
        Brushes.Lavender,
        Brushes.LightCoral,
        Brushes.LightGoldenrodYellow,
        Brushes.LightSeaGreen,
        Brushes.LightSkyBlue
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int nurseId)
            {
                return Colors[nurseId % Colors.Length];
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
