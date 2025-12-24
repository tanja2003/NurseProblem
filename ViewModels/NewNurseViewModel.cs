using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using NurseProblem.Datenbank;
using NurseProblem.Models;
using NurseProblem.Models.DbModelle;
using NurseProblem.Models.UiModelle;
using NurseProblem.UseCases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace NurseProblem.ViewModels
{
    [Flags]
    public enum WeekDays
    {
        None = 0,
        Monday = 1 << 0,
        Tuesday = 1 << 1,
        Wednesday = 1 << 2,
        Thursday = 1 << 3,
        Friday = 1 << 4,
        Saturday = 1 << 5,
        Sunday = 1 << 6
    }

    partial class NewNurseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        //private readonly Nurse _nurse = new();
        private readonly NewNurseUseCase _addNurseUseCase;
        private Nurse _nurse = new Nurse(0, "", "", 0, WeekDays.None, "");
        public Nurse Nurse
        {
            get => _nurse;
            set
            {
                if (_nurse == value) return;
                _nurse = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? RequestClose;
        public ObservableCollection<string> Status { get; } = new ObservableCollection<string>{
            "Vollzeitkraft", "Student", "FSJ"
        };

        public bool CanNotWorkMonday
        {
            get => _nurse.UnavailableDays.HasFlag(WeekDays.Monday);
            set
            {
                if (value) _nurse.UnavailableDays |= WeekDays.Monday;
                else _nurse.UnavailableDays &= ~WeekDays.Monday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkTuesday
        {
            get => _nurse.UnavailableDays.HasFlag(WeekDays.Tuesday);
            set
            {
                if (value) _nurse.UnavailableDays |= WeekDays.Tuesday;
                else _nurse.UnavailableDays &= ~WeekDays.Tuesday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkWednesday
        {
            get => _nurse.UnavailableDays.HasFlag(WeekDays.Wednesday);
            set
            {
                if (value) _nurse.UnavailableDays |= WeekDays.Wednesday;
                else _nurse.UnavailableDays &= ~WeekDays.Wednesday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkThursday
        {
            get => _nurse.UnavailableDays.HasFlag(WeekDays.Thursday);
            set
            {
                if (value) _nurse.UnavailableDays |= WeekDays.Thursday;
                else _nurse.UnavailableDays &= ~WeekDays.Thursday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkFriday
        {
            get => _nurse.UnavailableDays.HasFlag(WeekDays.Friday);
            set
            {
                if (value) _nurse.UnavailableDays |= WeekDays.Friday;
                else _nurse.UnavailableDays &= ~WeekDays.Friday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkSaturday
        {
            get => _nurse.UnavailableDays.HasFlag(WeekDays.Saturday);
            set
            {
                if (value) _nurse.UnavailableDays |= WeekDays.Saturday;
                else _nurse.UnavailableDays &= ~WeekDays.Saturday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkSunday
        {
            get => _nurse.UnavailableDays.HasFlag(WeekDays.Sunday);
            set
            {
                if (value) _nurse.UnavailableDays |= WeekDays.Sunday;
                else _nurse.UnavailableDays &= ~WeekDays.Sunday;
                OnPropertyChanged();
            }
        }


        public string FirstName
        {
            get => _nurse.FirstName;
            set
            {
                if (_nurse.FirstName == value) return;
                _nurse.FirstName = value;
                OnPropertyChanged();
                OnPropertyChanged("Item[]");
            }
        }
        public string LastName
        {
            get => _nurse.LastName;
            set
            {
                if (_nurse.LastName == value) return;
                _nurse.LastName = value;
                OnPropertyChanged();
                OnPropertyChanged("Item[]");
            }
        }
        public double WorkingHours
        {
            get => _nurse.WorkingHours;
            set
            {
                if (_nurse.WorkingHours == value) return;
                _nurse.WorkingHours = value;
                OnPropertyChanged(nameof(WorkingHours));
                OnPropertyChanged("Item[]");
            }
        }
        public string EmploymentStatus
        {
            get => _nurse.EmploymentStatus;
            set
            {
                if (_nurse.EmploymentStatus == value) return;
                _nurse.EmploymentStatus = value;
                OnPropertyChanged(nameof(EmploymentStatus));
                OnPropertyChanged("Item[]");
            }
        }


        private bool _validateOnSubmit;
        public bool ValidateOnSubmit
        {
            get => _validateOnSubmit;
            set
            {
                _validateOnSubmit = value;
                OnPropertyChanged(null);
            }
        }


        
        public NewNurseViewModel(NewNurseUseCase addNurseUseCase)
        {
            _addNurseUseCase = addNurseUseCase;
            // _nurse = new Nurse();
            SaveCommand = new RelayCommand(async () => await SaveNurseAsync());
            CancelCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow() => RequestClose?.Invoke();

        private void SaveNurseToDb()
        {
            ValidateOnSubmit = true;
            CommandManager.InvalidateRequerySuggested();
            if (!IsValid())
                return;
            
            try
            {
                ValidateNurse();

                using var context = new ScheduleDbContext();
                context.Database.EnsureCreated();
                context.Nurses.Add(new Models.DbModelle.NurseEntity
                {
                    Id = _nurse.Id,
                    FirstName = _nurse.FirstName,
                    LastName = _nurse.LastName,
                    WorkingHours = _nurse.WorkingHours,
                    UnavailableDays = _nurse.UnavailableDays,
                    EmploymentStatus = _nurse.EmploymentStatus
                });

                context.SaveChanges();
                // Nachricht ans Parent senden
                WeakReferenceMessenger.Default.Send(new NurseSavedMessage(_nurse));
                CloseWindow();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Ungültige Daten – Datenbank hat abgelehnt");
            }




        }

        private async Task SaveNurseAsync()
        {
            try
            {
                await _addNurseUseCase.ExecuteAsync(_nurse);
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Validation

        /// <summary>
        /// UI/Validation
        /// IDataErrorInfo
        /// for Userfeedback
        ///  Availabilty can be empty --> only stores exceptions
        /// </summary>
        public string Error => null;
        public string this[string propertyName]
        {
            get
            {
                if (!ValidateOnSubmit)
                    return null;

                switch (propertyName)
                {
                    case nameof(_nurse.FirstName):
                        if (string.IsNullOrWhiteSpace(_nurse.FirstName)) return "Vorname ist erforderlich";
                        break;
                    case nameof(_nurse.LastName):
                        if (string.IsNullOrWhiteSpace(_nurse.LastName)) return "Nachname ist erforderlich";
                        break;
                    case nameof(_nurse.WorkingHours):
                        if (_nurse.WorkingHours <= 0) return "Arbeitsstunden müssen größer als oder gleich 0 sein";
                        break;
                    case nameof(_nurse.EmploymentStatus):
                        if (string.IsNullOrWhiteSpace(_nurse.EmploymentStatus)) return "Status ist erforderlich";
                        break;
                }
                return null;
            }
        }
        public bool IsValid()
        {
            ValidateOnSubmit = true;

            return string.IsNullOrWhiteSpace(this[nameof(_nurse.FirstName)])
                && string.IsNullOrWhiteSpace(this[nameof(_nurse.LastName)])
                && string.IsNullOrWhiteSpace(this[nameof(_nurse.WorkingHours)])
                && string.IsNullOrWhiteSpace(this[nameof(_nurse.EmploymentStatus)]);
        }

        

        private void ValidateNurse()
        {
            if (string.IsNullOrWhiteSpace(_nurse.FirstName))
                throw new InvalidOperationException("Vorname ist erforderlich");
            if (string.IsNullOrWhiteSpace(_nurse.LastName))
                throw new InvalidOperationException("Nachname ist erforderlich");
            if (_nurse.WorkingHours < 0)
                throw new InvalidOperationException("Arbeitsstunden müssen größer 0 sein");
            if (string.IsNullOrWhiteSpace(_nurse.EmploymentStatus))
                throw new InvalidOperationException("Arbeitsstatus ist erforderlich");
        }

        # endregion



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(
            [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
