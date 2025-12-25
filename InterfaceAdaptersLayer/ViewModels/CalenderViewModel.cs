using Google.OrTools.ConstraintSolver;
using Microsoft.EntityFrameworkCore;
using NurseProblem.Converter;
using NurseProblem.FrameworkLayer.Datenbank;
using NurseProblem.DomainLayer.DbModelle;
using NurseProblem.DomainLayer.SolverModelle;
using NurseProblem.DomainLayer.UiModelle;
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
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace NurseProblem.InterfaceAdaptersLayer.ViewModels
{
    public class CalendarViewModel
    {
        public string MonthName { get; set; } = "Januar";
        public ObservableCollection<DaySchedule> Days { get; set; } = new();
        public ObservableCollection<ObservableCollection<DaySchedule>> Weeks { get; set; } = new();

        public CalendarViewModel()
        {
            LoadOrCreateMonth(2026, 2);  // TODO
        }

        public void LoadOrCreateMonth(int year, int month)
        {
            using var context = new ScheduleDbContext();

            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            var fullPath = Path.GetFullPath(context.Database.GetDbConnection().DataSource);
            Debug.WriteLine(fullPath);


            var test = context.Days;
            var test2 = context.ShiftSlots;
            // Select *
            var dayEntities = context.Days // From Days d
                .Include(d => d.ShiftSlots) // Left Join ShiftSlots s on s.DayEntityId = d.id
                //.Where(d => d.Date >= start && d.Date < end) // where d.Date >= @start and d.Date < @end
                .OrderBy(d => d.Date) // order by Date
                .ToList();

            if (dayEntities.Any())
            {
                LoadFromDb(dayEntities);
            }
        }

        private void LoadFromDb(List<DayEntity> entities, int slotsPerShift=2) // TODO
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
            if (!Days.Any())
                return;
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

            while (currentWeek.Count < 7)
            {
                currentWeek.Add(null);
            }

            Weeks.Add(currentWeek);
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
