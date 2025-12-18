using Microsoft.EntityFrameworkCore;
using NurseProblem.Converter;
using NurseProblem.Datenbank;
using NurseProblem.Models.DbModelle;
using NurseProblem.Models.SolverModelle;
using NurseProblem.Models.UiModelle;
using NurseProblem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace NurseProblem.ViewModels
{
    public class CalculateScheduleViewModel: INotifyPropertyChanged
    {
        public String MonthName { get; set; } = "Januar";
        public ObservableCollection<DaySchedule> Days { get; set; } = new();
        public ObservableCollection<ObservableCollection<DaySchedule>> Weeks { get; set; } = new();
        public ICommand CalculateCommand { get; }
        public ICommand SaveCommand { get; }
        public Schedule Schedule { get; private set; } = new();

        private int _month; 
        private bool _isCalendarVisible;
        public bool IsCalendarVisible
        {
            get => _isCalendarVisible;
            set
            {
                _isCalendarVisible = value;
                OnPropertyChanged();
            }
        }

        private readonly CpSatSchedulerService _service;

        public CalculateScheduleViewModel()
        {
            _month = 1;
            _service = new CpSatSchedulerService(25, 28, 3, 2025, _month);  // TODO start Window to enter the infos
            CalculateCommand = new RelayCommand(Calculate);
            SaveCommand = new RelayCommand(SaveMonthSchedule);
        }

        // On Click on Button "Schedule berechnen"
        private void Calculate()
        {
            Schedule = _service.Solve();

            OnPropertyChanged(nameof(Schedule));
            BuildMonthSchedule(2025, _month);
        }

        // On CLick on Button "Save MonthSchedule"
        private void SaveMonthSchedule()
        {
            SaveMonthToDb();
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
                Days.Add(new DaySchedule(2)  // TODO: Dynamiclly
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
            IsCalendarVisible = true;

        }

        private void SaveMonthToDb()
        {
            using var context = new ScheduleDbContext();

            foreach (var day in Days)
            {
                if (!context.Days.Any(d => d.Date == day.Date))
                {
                    context.Days.Add(new DayEntity
                    {
                        Date = day.Date,
                    });
                }
            }

            context.SaveChanges();

            foreach (var day in Days)
            {

                foreach (var slot in day.Früh)
                {
                    AddSlot(context, day.Date, ShiftType.Früh, slot);
                }
                foreach (var slot in day.Spät)
                {
                    AddSlot(context, day.Date, ShiftType.Spät, slot);
                }
                foreach (var slot in day.Nacht)
                {
                    AddSlot(context, day.Date, ShiftType.Nacht, slot);
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Mapping UI --> DB
        /// </summary>
        /// <param name="day"></param>
        /// <param name="shift"></param>
        /// <param name="slot"></param>
        private static void AddSlot(ScheduleDbContext context, DateTime date, ShiftType shift, ShiftSlot slot)
        {
            if (slot == null)
                throw new InvalidOperationException("Slot konnte nicht erstellt werden");

            var dayEntity = context.Days  // from days d
                .Include(d => d.ShiftSlots)  // left join ShiftSlots s on s.dayEntityId = d.id
                .SingleOrDefault(d => d.Date == date); // order by date

            if (dayEntity == null)
            {
                dayEntity = new DayEntity
                {
                    Date = date,
                    // ShiftSlot must be added with slotEntity
                };

                context.Days.Add(dayEntity);
                context.SaveChanges();
            }

            var slotEntity = dayEntity.ShiftSlots
                .FirstOrDefault(s =>
                s.Shift == shift &&
                s.SlotNumber == slot.SlotNumber);

            if (slotEntity == null)
            {
                slotEntity = new ShiftSlotEntity
                {
                    DayEntityId = dayEntity.Id, // important to store also in Day Table
                    Day = dayEntity,
                    Shift = shift,
                    SlotNumber = slot.SlotNumber,
                };
                dayEntity.ShiftSlots.Add(slotEntity);
            }
            slotEntity.NurseId = slot.NurseId;
            slotEntity.NurseName = slot.NurseName;

            context.SaveChanges();
            var fullPath = Path.GetFullPath(context.Database.GetDbConnection().DataSource);
            Debug.WriteLine(fullPath);
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
