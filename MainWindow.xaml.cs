using Google.OrTools.Sat;
using NurseProblem.Converter;
using NurseProblem.Datenbank;
using NurseProblem.Models.UiModelle;
using NurseProblem.ViewModels;
using NurseProblem.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NavigationService = NurseProblem.Services.NavigationService;

namespace NurseProblem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // View
        // Display UI
        // Set DataContext
        // NO business logic
        // NO calculations
        
        public MainWindow()
        {
            InitializeComponent();
            var db = new ScheduleDbContext();
            var navigation = new NavigationService(db);
            DataContext = new StartViewModel(navigation);

        }

        private void Button_Click_Month(object sender, RoutedEventArgs e)
        {
            DataContext = new NurseScheduleViewModel();

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var day = border?.DataContext as ShiftSlot;

            //var vm = new DayCalenderViewModel(day);
            //var win = new DayWindow(vm);
            //win.Show();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}