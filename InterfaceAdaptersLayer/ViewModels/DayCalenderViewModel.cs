using Microsoft.EntityFrameworkCore;
using NurseProblem.Converter;
using NurseProblem.DomainLayer.DbModelle;
using NurseProblem.DomainLayer.UiModelle;
using NurseProblem.FrameworkLayer.Datenbank;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NurseProblem.InterfaceAdaptersLayer.ViewModels
{
    public class DayCalenderViewModel : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }
        public ObservableCollection<Nurse> AllNurses { get; set; }
        public ObservableCollection<int> Numbers { get; set; }
        public int? FrühNumbers { get; set; }
        public int? SpätNumbers { get; set; }
        public int? NachtNumbers { get; set; }

        public int? FrühSelected { get; set; }
        public Nurse SpätSelected { get; set; }
        public Nurse NachtSelected { get; set; }

        public ICommand SaveCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private DaySchedule _day;

        public ObservableCollection<ShiftSlot> FrühWorkerSlots { get; } =
            new ObservableCollection<ShiftSlot>();
        public ObservableCollection<ShiftSlot> SpätWorkerSlots { get; } =
            new ObservableCollection<ShiftSlot>();
        public ObservableCollection<ShiftSlot> NachtWorkerSlots { get; } =
            new ObservableCollection<ShiftSlot>();

        private int _selectFrühSlotCount;
        private int _selectSpätSlotCount;
        private int _selectNachtSlotCount;
        public int SelectFrühSlotCount
        {
            get => _selectFrühSlotCount;
            set
            {
                if (_selectFrühSlotCount != value)
                {
                    _selectFrühSlotCount = value;
                    OnPropertyChanged();
                    UpdateWorkerSlots(_selectFrühSlotCount, FrühWorkerSlots);
                }
            }
        }
        public int SelectSpätSlotCount
        {
            get => _selectSpätSlotCount;
            set
            {
                if (_selectSpätSlotCount != value)
                {
                    _selectSpätSlotCount = value;
                    OnPropertyChanged();
                    UpdateWorkerSlots(_selectSpätSlotCount, SpätWorkerSlots);
                }
            }
        }
        public int SelectNachtSlotCount
        {
            get => _selectNachtSlotCount;
            set
            {
                if (_selectNachtSlotCount != value)
                {
                    _selectNachtSlotCount = value;
                    OnPropertyChanged();
                    UpdateWorkerSlots(_selectNachtSlotCount, NachtWorkerSlots);
                }
            }
        }

        public DayCalenderViewModel(DaySchedule day)
        {
            _day = day;
            Date = day.Date;
            ConvertDictionaryInNurse();

            Numbers = new ObservableCollection<int>(
                Enumerable.Range(1, 10)
            );
            SaveCommand = new RelayCommand(Save);
        }

        private void ConvertDictionaryInNurse()
        {
            AllNurses = new ObservableCollection<Nurse>();
            foreach (var nurse in Dictionarys.NurseNames)
            {
                Nurse n = new(nurse.Key, nurse.Value);
                AllNurses.Add(n);
            }
        }

        private void Save()
        {
            using var db = new ScheduleDbContext();

            var entity = db.Days
                .Include(d => d.ShiftSlots)
                .Single(d => d.Date == _day.Date);

            SaveShift(db, entity, ShiftType.Früh, _day.Früh);
            SaveShift(db, entity, ShiftType.Spät, _day.Spät);
            SaveShift(db, entity, ShiftType.Nacht, _day.Nacht);

            db.SaveChanges();
        }

        private void SaveShift(ScheduleDbContext context, DayEntity dayEntity, ShiftType shift, ObservableCollection<ShiftSlot> slots)
        {
            foreach (var slot in slots)
            {
                var entity = dayEntity.ShiftSlots
            .FirstOrDefault(s =>
                s.Shift == shift &&
                s.SlotNumber == slot.SlotNumber);

                if (entity == null)
                {
                    entity = new ShiftSlotEntity
                    {
                        Shift = shift,
                        SlotNumber = slot.SlotNumber,
                        Day = dayEntity
                    };

                    dayEntity.ShiftSlots.Add(entity);
                }

                entity.NurseId = slot.NurseId;
                entity.NurseName = slot.NurseName;
            }
        }

        // --- Dynamisch erzeugte Slots ---
       
        private void UpdateWorkerSlots(int count, ObservableCollection<ShiftSlot> workerSlots)
        {
            workerSlots.Clear();

            for (int i = 1; i <= count; i++)
            {
                workerSlots.Add(new ShiftSlot
                {
                    SlotNumber = i
                });
            }
        }

        protected void OnPropertyChanged(
            [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
