using Google.OrTools.ConstraintSolver;
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
        public String MonthName { get; set; } = "Januar";
        public ObservableCollection<DaySchedule> Days { get; set; } = new();
        public ObservableCollection<ObservableCollection<DaySchedule>> Weeks { get; set; } = new();
        public ICommand CalculateCommand { get; }
        public Schedule Schedule { get; private set; } = new();

        private readonly CpSatSchedulerService _service;



        public CalendarViewModel()
        {
            _service = new CpSatSchedulerService(10, 30, 3);  // TODO start Window to enter the infos

            CalculateCommand = new RelayCommand(Calculate);
            LoadOrCreateMonth(2026, 1);  // TODO
        }

        public void LoadOrCreateMonth(int year, int month)
        {
            using var context = new ScheduleDbContext();

            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var dayEntities = context.Days
                .Include(d => d.ShiftSlots)
                .Where(d => d.Date >= start && d.Date < end)
                .OrderBy(d => d.Date)
                .ToList();

            if (dayEntities.Any())
            {
                LoadFromDb(dayEntities);
            }
            else
            {
                BuildMonthSchedule(year, month);
                SaveMonthToDb();
            }
        }

        private void LoadFromDb(List<DayEntity> entities, int slotsPerShift=3)
        {
            Days.Clear();
            Weeks.Clear();

            foreach (var entity in entities)
            {
                var day = new DaySchedule(slotsPerShift)
                {
                    Date = entity.Date
                };
                day.Früh = new ObservableCollection<ShiftSlot>(
                    entity.ShiftSlots
                        .Where(s => s.Shift == ShiftType.Früh)
                        .OrderBy(s => s.SlotNumber)
                        .Select(ToShiftSlot)
                );
                day.Spät = new ObservableCollection<ShiftSlot>(
                    entity.ShiftSlots
                        .Where(s => s.Shift == ShiftType.Spät)
                        .OrderBy(s => s.SlotNumber)
                        .Select(ToShiftSlot)
                );
                day.Nacht = new ObservableCollection<ShiftSlot>(
                    entity.ShiftSlots
                        .Where(s => s.Shift == ShiftType.Nacht)
                        .OrderBy(s => s.SlotNumber)
                        .Select(ToShiftSlot)
                );
                Days.Add( day );
            }
            BuildWeeks();
        }

        private static ShiftSlot ToShiftSlot(ShiftSlotEntity e)
        {
            return new ShiftSlot
            {
                SlotNumber = e.SlotNumber,
                NurseId = e.NurseId,
                NurseName = e.NurseName
            };
        }


        private void BuildWeeks()
        {
            Weeks.Clear();
            ObservableCollection<DaySchedule> currentWeek = new();
            var firstDayOfMonth = Days.First().Date;
            int startOffset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;

            for ( int i = 0; i < startOffset;  i++ )
            {
                currentWeek.Add(null);
            }

            foreach ( var day in Days )
            {
                currentWeek.Add(day);

                if (currentWeek.Count == 7)
                {
                    Weeks.Add(currentWeek);
                    currentWeek = new ObservableCollection<DaySchedule>();
                }
            }

            while (currentWeek.Count > 7)
            {
                currentWeek.Add(null);
            }

            Weeks.Add(currentWeek);
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
                        Date = day.Date
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
                foreach(var slot in day.Spät)
                {
                    AddSlot(context, day.Date, ShiftType.Spät, slot);
                }
                foreach(var slot in day.Nacht)
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
        private void AddSlot(ScheduleDbContext context, DateTime date, ShiftType shift, ShiftSlot slot)
        {
            if (slot == null)
                throw new InvalidOperationException("Slot konnte nicht erstellt werden");

            var dayEntity = context.Days
                .Include(d => d.ShiftSlots)
                .SingleOrDefault(d => d.Date == date);

            if (dayEntity == null)
            {
                dayEntity = new DayEntity
                {
                    Date = date,
                    ShiftSlots = new List<ShiftSlotEntity>()
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
                    Day = dayEntity,
                    Shift = shift,
                    SlotNumber = slot.SlotNumber,
                };
                dayEntity.ShiftSlots.Add(slotEntity);
            }
            slotEntity.NurseId = slot.NurseId;
            slotEntity.NurseName = slot.NurseName;
        }

        // On Click on Button "Schedule berechnen"
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
