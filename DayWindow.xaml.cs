using NurseProblem.Models;
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
    /// Interaktionslogik für DayWindow.xaml
    /// </summary>
    public partial class DayWindow : Window
    {
        public DayWindow(DayCalenderViewModel day)
        {
            InitializeComponent();
            DataContext = day;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //DialogResult = false;
            Close();
        }
    }
}
