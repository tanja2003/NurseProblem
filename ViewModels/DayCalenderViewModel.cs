using NurseProblem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NurseProblem.ViewModels
{
    public class DayCalenderViewModel : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }
        public ObservableCollection<Nurse> AllNurses { get; set; }

        public int? FrühSelected { get; set; }
        public Nurse SpätSelected { get; set; }
        public Nurse NachtSelected { get; set; }

        public ICommand SaveCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private DaySchedule _day;

        public DayCalenderViewModel(DaySchedule day)
        {
            _day = day;
            Date = day.Date;
            AllNurses = []; // TODO

            // Vorbelegung
            FrühSelected = day.Früh[0].NurseId;
            //SpätSelected = GetNurseById(day.Spät[0].NurseId);
            //NachtSelected = GetNurseById(day.Nacht[0].NurseId);

            SaveCommand = new RelayCommand(Save);
        }

        private Nurse GetNurseById(int? id)
        {
            if (id == null) return null;
            return AllNurses.FirstOrDefault(n => n.Id == id);
        }

        private void Save()
        {
            //_day.FrühNurseId = FrühSelected?.Id;
            //_day.SpätNurseId = SpätSelected?.Id;
            //_day.NachtNurseId = NachtSelected?.Id;

            //_day.FrühName = FrühSelected?.Name;
            //_day.SpätName = SpätSelected?.Name;
            //_day.NachtName = NachtSelected?.Name;

            //DialogResult = true;
        }
    }

}
