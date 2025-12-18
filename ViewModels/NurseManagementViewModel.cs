using NurseProblem.Converter;
using NurseProblem.Datenbank;
using NurseProblem.Models.DbModelle;
using NurseProblem.Models.UiModelle;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NurseProblem.ViewModels
{
    public class NurseManagementViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Nurse> Nurses { get; set; } = [];
        public int Id { get; set; } = new();
        private string _nurseName;
        public string NurseName
        {
            get => _nurseName;
            set
            {
                if (_nurseName != value)
                {
                    _nurseName = value;
                    OnPropertyChanged(nameof(NurseName));
                }
            }
        }

        private Nurse? _selectedNurse;
        public Nurse? SelectedNurse
        {
            get => _selectedNurse;
            set
            {
                _selectedNurse = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenNurseDetailsCommand { get; }
        public ICommand SaveCommand { get; }

        public NurseManagementViewModel() {
            OpenNurseDetailsCommand = new RelayCommand(OpenNurseDetails);
            SaveCommand = new RelayCommand(SaveNurseToDb);

            Nurses = new ObservableCollection<Nurse>();
            ListNurses();
        }
        private void ListNurses()
        {
            using var context = new ScheduleDbContext();
            var nurseEntities = context.Nurses.ToList();
            if(nurseEntities.Any())
            {
                Nurses.Clear();
                foreach(var nurseEntity in nurseEntities)
                {
                    var NewNurse = new Nurse()
                    {
                        Name = nurseEntity.Name
                    };
                    Nurses.Add(NewNurse);
                }
            }
            //var nurse2 = Dictionarys.NurseNames;

            //foreach (var nurse in nurse2)
            //{
            //    var NewNurse = new Nurse();
            //    NewNurse.Id = nurse.Key;
            //    NewNurse.Name = nurse.Value;
            //    Nurses.Add(NewNurse);
            //}
        }
        private void LoadNurseFromDb()
        {

        }
        private void SaveNurseToDb()
        {
            using var context = new ScheduleDbContext();
            context.Nurses.Add(new NurseEntity
            {
                Name = NurseName
            });
            context.SaveChanges();
        }
        private void OpenNurseDetails()
        {
            var vm = new NurseDetailViewModel(SelectedNurse!);
            var window = new NurseDetailWindow
            {
                DataContext = vm
            };
            window.ShowDialog(); // modal → ideal für Bearbeiten/Löschen
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
