using NurseProblem.Models.UiModelle;
using NurseProblem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NurseProblem
{
    /// <summary>
    /// Interaktionslogik für CalenderWindwow.xaml
    /// </summary>
    public partial class CalenderWindwow : Window
    {
        public CalenderWindwow()
        {
            InitializeComponent();
            DataContext = new CalendarViewModel();
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is StackPanel sp && sp.DataContext is DaySchedule day)
            {
                var win = new DayWindow(new DayCalenderViewModel(day));

                if (win.ShowDialog() == true)
                {
                    // kein extra Speichern nötig → gleiche Instance von "day" wurde bearbeitet
                }
            }
        }
    }
}
