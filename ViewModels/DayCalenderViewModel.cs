using NurseProblem.Converter;
using NurseProblem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NurseProblem.ViewModels
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

        public ObservableCollection<WorkerSlot> FrühWorkerSlots { get; } =
            new ObservableCollection<WorkerSlot>();
        public ObservableCollection<WorkerSlot> SpätWorkerSlots { get; } =
            new ObservableCollection<WorkerSlot>();
        public ObservableCollection<WorkerSlot> NachtWorkerSlots { get; } =
            new ObservableCollection<WorkerSlot>();

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

            // Vorbelegung
            //FrühSelected = day.Früh[0].SlotNumber;
            //SpätSelected = GetNurseById(day.Spät);
            //NachtSelected = GetNurseById(day.Nacht[0].NurseId);

            SaveCommand = new RelayCommand(Save);
        }

        private void ConvertDictionaryInNurse()
        {
            AllNurses = new ObservableCollection<Nurse>();
            foreach (var nurse in Dictionarys.NurseNames)
            {
                Nurse n = new Nurse(nurse.Key, nurse.Value);
                AllNurses.Add(n);
            }
        }

        private Nurse GetNurseById(int? id)
        {
            if (id == null) return null;
            return AllNurses.FirstOrDefault(n => n.Id == id);
        }

        private void Save()
        {

            _day.Früh.Clear();

            foreach (var w in FrühWorkerSlots)
            {
                _day.Früh.Add(new ShiftSlot
                {
                    NurseId = w.SelectedNurse?.Id,
                    NurseName = w.SelectedNurse?.Name
                });
            }
            if (Application.Current.Windows.OfType<DayWindow>()
                .FirstOrDefault(w => w.DataContext == this) is DayWindow win)
            {
                win.DialogResult = true;
                win.Close();
            }
            //_day.Früh = FrühSelected;
            //_day.SpätNurseId = SpätSelected?.Id;
            //_day.NachtNurseId = NachtSelected?.Id;

            //_day.FrühName = FrühSelected?.Name;
            //_day.SpätName = SpätSelected?.Name;
            //_day.NachtName = NachtSelected?.Name;

            //DialogResult = true;
        }


        // --- Dynamisch erzeugte Slots ---
       
        private void UpdateWorkerSlots(int count, ObservableCollection<WorkerSlot> workerSlots)
        {
            workerSlots.Clear();

            for (int i = 1; i <= count; i++)
            {
                workerSlots.Add(new WorkerSlot
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
