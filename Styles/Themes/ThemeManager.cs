using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NurseProblem.Styles.Themes
{
    public static class ThemeManager
    {
        public static void Apply(bool dark)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri(
                    dark
                    ? "Themes/Dark.xaml"
                    : "Themes/Light.xaml",
                    UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }

}
