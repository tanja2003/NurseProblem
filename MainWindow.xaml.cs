using Google.OrTools.Sat;
using NurseProblem.ViewModels;
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
        public CalendarViewModel VM { get; set; } = new CalendarViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = VM;

        }

        private void Button_Click_Month(object sender, RoutedEventArgs e)
        {
            DataContext = new NurseScheduleViewModel();

        }


        //public void GenerateColumnsForDays(int numDays)
        //{
        //    MonthDataGrid.Columns.Clear();

        //    // Erste Spalte: Nurse Name
        //    MonthDataGrid.Columns.Add(new DataGridTextColumn
        //    {
        //        Header = "Nurse",
        //        Binding = new Binding("NurseName")
        //    });

        //    // Dynamische Tages-Spalten
        //    for (int day = 0; day < numDays; day++)
        //    {
        //        MonthDataGrid.Columns.Add(new DataGridTextColumn
        //        {
        //            Header = $"Tag {day + 1}",
        //            Binding = new Binding($"Days[{day}]") // Zugriff auf Array-Index
        //        });
        //    }
        //}


    }



}