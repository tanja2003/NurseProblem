using Google.OrTools.ConstraintSolver;
using NurseProblem.Converter;
using NurseProblem.Models;
using NurseProblem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace NurseProblem.ViewModels
{
    public class CalendarViewModel
    {
        public ObservableCollection<DaySchedule> Days { get; set; }
        public ObservableCollection<ObservableCollection<DaySchedule>> Weeks { get; set; } = new();
        public ICommand CalculateCommand { get; }
        public Schedule Schedule { get; private set; } = new();

        private readonly CpSatSchedulerService _service;



        public CalendarViewModel()
        {
            _service = new CpSatSchedulerService(10, 30, 3);

            CalculateCommand = new RelayCommand(Calculate);

            Days = new ObservableCollection<DaySchedule>();
           
        }
        private void Calculate()
        {
            Schedule = _service.Solve();
            OnPropertyChanged(nameof(Schedule));
            BuildMonthSchedule(2025,1);
        }
        public void BuildMonthSchedule(int year, int month)
        {
            Days.Clear();
            Weeks.Clear();

            int daysInMonth = DateTime.DaysInMonth(year, month);

           

            // Fill Every Day

            // initalize all days
            for (int d = 1; d <= daysInMonth; d++)
            {
                Days.Add(new DaySchedule(3)  // TODO: Dynamiclly
                {
                    Date = new DateTime(year, month, d),
                });
            }

            // include Assignments
            foreach (var a in Schedule.Assignments)
            {
                var day = Days[a.Day];  

                switch (a.Shift)
                {
                    case 0:
                        day.Früh[a.WorkerSlots].NurseId = a.NurseId;
                        day.Früh[a.WorkerSlots].NurseName = Dictionarys.NurseNames[a.NurseId];

                        break;

                    case 1:
                        day.Spät[a.WorkerSlots].NurseName = Dictionarys.NurseNames[a.NurseId];
                        day.Spät[a.WorkerSlots].NurseId = a.NurseId;
                        break;

                    case 2:
                        day.Nacht[a.WorkerSlots].NurseName = Dictionarys.NurseNames[a.NurseId];
                        day.Nacht[a.WorkerSlots].NurseId = a.NurseId;
                        break;
                }
            }


            // Generate Weeks
            Weeks.Clear();

            ObservableCollection<DaySchedule> currentWeek = new();
            var firstDayOfMonth = new DateTime(year, month, 1);
            int startOffset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7; // Montag=0, Sonntag=6

            // empty blocks before first day
            for (int i = 0; i < startOffset; i++)
            {
                currentWeek.Add(null); 
            }

            foreach (var day in Days)
            {
                if (day.Früh != null && day.Spät != null && day.Nacht != null)
                {
                    currentWeek.Add(day);
                }
                else 
                {
                    currentWeek.Add(null);
                }

                if (currentWeek.Count == 7)
                {
                    Weeks.Add(currentWeek);
                    currentWeek = new ObservableCollection<DaySchedule>();
                }
            }

            // fill up last week
            while (currentWeek.Count < 7 && currentWeek.Count > 0)
                currentWeek.Add(null);

            if (currentWeek.Count > 0)
                Weeks.Add(currentWeek);

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
