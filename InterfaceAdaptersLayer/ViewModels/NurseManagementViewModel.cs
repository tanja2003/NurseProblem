using CommunityToolkit.Mvvm.Messaging;
using NurseProblem.Converter;
using NurseProblem.FrameworkLayer.Datenbank;
using NurseProblem.DomainLayer;
using NurseProblem.DomainLayer.DbModelle;
using NurseProblem.DomainLayer.UiModelle;
using NurseProblem.Services.Interfaces;
using NurseProblem.Styles.Themes;
using NurseProblem.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NurseProblem.InterfaceAdaptersLayer.ViewModels
{
    public class NurseManagementViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<Nurse> _filteredNurses; 
        public ObservableCollection<Nurse> FilteredNurses
        {
            get => _filteredNurses;
            set { _filteredNurses = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Nurse> Nurses { get; } = [];
        public int Id { get; set; } = new();


        private Nurse? _selectedNurse;
        public Nurse? SelectedNurse
        {
            get => _selectedNurse;
            set { _selectedNurse = value; OnPropertyChanged(); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        private bool _isDarkMode;
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set {  _isDarkMode = value; OnPropertyChanged(); ThemeManager.Apply(value); }
        }



        private readonly INavigationService _navigationService;
        public ICommand OpenNurseDetailsCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand OpenNewNurseCommand { get; }


        public NurseManagementViewModel(INavigationService navigationService) 
        {
            _navigationService = navigationService;
            Nurses = new ObservableCollection<Nurse>();

            OpenNurseDetailsCommand = new RelayCommand(OpenNurseDetails);
            // SaveCommand = new RelayCommand(SaveNurseToDb);
            OpenNewNurseCommand = new RelayCommand(OpenNewNurse);
            WeakReferenceMessenger.Default.Register<NurseSavedMessage>(this, (r, m) =>
            {
                Nurses.Add(m.Nurse);
            });
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
                        FirstName = nurseEntity.FirstName,
                        LastName = nurseEntity.LastName,
                        WorkingHours = nurseEntity.WorkingHours,
                        UnavailableDays = nurseEntity.UnavailableDays,
                        EmploymentStatus = nurseEntity.EmploymentStatus,
                        Id = nurseEntity.Id
                    };
                    Nurses.Add(NewNurse);
                }
            }
            FilteredNurses = Nurses;
        }
        private void SaveNurseToDb()
        {
            using var context = new ScheduleDbContext();
            context.Nurses.Add(new NurseEntity
            {
                FirstName = nameof(Nurse),
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
            window.ShowDialog(); 
        }

        private void OpenNewNurse()
        {
            _navigationService.OpenNewNurse();
        }


        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredNurses = new ObservableCollection<Nurse>(Nurses);
            }
            else
            {
                var lower = SearchText.ToLower();
                FilteredNurses = new ObservableCollection<Nurse>();
                foreach (var nurse in Nurses)
                {
                    Debug.Assert(nurse != null, "Nurse has no values!");
                    if ( nurse.FirstName != null && nurse.LastName != null)
                    {
                        if (nurse.FirstName.ToLower().Contains(lower) || nurse.LastName.ToLower().Contains(lower))
                        {
                            FilteredNurses.Add(nurse);
                        }
                    }

                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
