using NurseProblem.Models.SolverModelle;
using NurseProblem.Models.UiModelle;
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace NurseProblem
{
    // ViewModel
    public class NurseScheduleViewModel : INotifyPropertyChanged
    {
        private readonly CpSatSchedulerService _service;

        public NurseScheduleViewModel()
        {
            // Beispiel: 6 Nurses, 10 Tage, 3 Schichten
            _service = new CpSatSchedulerService(4, 4, 3, 2025, 1);

            CalculateCommand = new RelayCommand(Calculate);

            MonthSchedule = new ObservableCollection<NurseDaySchedule>();
            SelectedNurseAssignments = new ObservableCollection<Assignment>();

            // Default Nurse 0 für Einzelplan
            SelectedNurse = 0;
        }

        public Schedule Schedule { get; private set; } = new();

        public ObservableCollection<NurseDaySchedule> MonthSchedule { get; }

        public ObservableCollection<Assignment> SelectedNurseAssignments { get; }

        private int _selectedNurse;
        public int SelectedNurse
        {
            get => _selectedNurse;
            set
            {
                _selectedNurse = value;
                OnPropertyChanged();
                UpdateSelectedNurseAssignments();
            }
        }

        public ICommand CalculateCommand { get; }

        private void Calculate()
        {
            Schedule = _service.Solve();
            OnPropertyChanged(nameof(Schedule));

            BuildMonthSchedule();

            UpdateSelectedNurseAssignments();
        }


        private void BuildMonthSchedule()
        {
            MonthSchedule.Clear();


            for (int d = 0; d < _service.NumDays; d++)
            {
                var dayRow = new NurseDaySchedule(_service.NumShifts)
                {
                    NurseName = $"Tag {d + 1}" // optional: als Label der Zeile
                };

                foreach (var a in Schedule.Assignments.Where(x => x.Day == d))
                {
                    // a.Shift gibt die Schichtnummer
                    // a.NurseId gibt die zugewiesene Nurse
                    dayRow.Days[a.Shift] = $"Nurse {a.NurseId + 1}"; // oder Name aus Dictionary
                }

                MonthSchedule.Add(dayRow);
            }

            OnPropertyChanged(nameof(MonthSchedule));
        }


        private void UpdateSelectedNurseAssignments()
        {
            SelectedNurseAssignments.Clear();
            foreach (var a in Schedule.Assignments.Where(x => x.NurseId == SelectedNurse))
            {
                SelectedNurseAssignments.Add(a);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
