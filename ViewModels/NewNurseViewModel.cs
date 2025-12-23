using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using NurseProblem.Datenbank;
using NurseProblem.Models;
using NurseProblem.Models.DbModelle;
using NurseProblem.Models.UiModelle;
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

    partial class NewNurseViewModel : INotifyPropertyChanged
    {
        public Nurse Nurse { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public event Action? RequestClose;
        public ObservableCollection<string> Status { get; } = new ObservableCollection<string>{
            "Vollzeitkraft", "Student", "FSJ"
        };

        public bool CanNotWorkMonday
        {
            get => Nurse.UnavailableDays.HasFlag(WeekDays.Monday);
            set
            {
                if (value) Nurse.UnavailableDays |= WeekDays.Monday;
                else Nurse.UnavailableDays &= ~WeekDays.Monday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkTuesday
        {
            get => Nurse.UnavailableDays.HasFlag(WeekDays.Tuesday);
            set
            {
                if (value) Nurse.UnavailableDays |= WeekDays.Tuesday;
                else Nurse.UnavailableDays &= ~WeekDays.Tuesday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkWednesday
        {
            get => Nurse.UnavailableDays.HasFlag(WeekDays.Wednesday);
            set
            {
                if (value) Nurse.UnavailableDays |= WeekDays.Wednesday;
                else Nurse.UnavailableDays &= ~WeekDays.Wednesday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkThursday
        {
            get => Nurse.UnavailableDays.HasFlag(WeekDays.Thursday);
            set
            {
                if (value) Nurse.UnavailableDays |= WeekDays.Thursday;
                else Nurse.UnavailableDays &= ~WeekDays.Thursday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkFriday
        {
            get => Nurse.UnavailableDays.HasFlag(WeekDays.Friday);
            set
            {
                if (value) Nurse.UnavailableDays |= WeekDays.Friday;
                else Nurse.UnavailableDays &= ~WeekDays.Friday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkSaturday
        {
            get => Nurse.UnavailableDays.HasFlag(WeekDays.Saturday);
            set
            {
                if (value) Nurse.UnavailableDays |= WeekDays.Saturday;
                else Nurse.UnavailableDays &= ~WeekDays.Saturday;
                OnPropertyChanged();
            }
        }
        public bool CanNotWorkSunday
        {
            get => Nurse.UnavailableDays.HasFlag(WeekDays.Sunday);
            set
            {
                if (value) Nurse.UnavailableDays |= WeekDays.Sunday;
                else Nurse.UnavailableDays &= ~WeekDays.Sunday;
                OnPropertyChanged();
            }
        }

       




        private string _selectedEmploymentStatus;
        public string SelectedEmploymentStatus 
        {
            get => _selectedEmploymentStatus;
            set
            {
                if (_selectedEmploymentStatus != value)
                {
                    _selectedEmploymentStatus = value;
                    OnPropertyChanged(nameof(SelectedEmploymentStatus));
                    if (Nurse != null)
                        Nurse.EmploymentStatus = value;
                }
            }
        }
        private bool _validateOnSubmit;
        public bool ValidateOnSubmit
        {
            get => _validateOnSubmit;
            set
            {
                _validateOnSubmit = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Nurse.FirstName));
                OnPropertyChanged(nameof(Nurse.LastName));
                OnPropertyChanged(nameof(Nurse.WorkingHours));
                OnPropertyChanged(nameof(Nurse.EmploymentStatus));
                OnPropertyChanged(nameof(Nurse.UnavailableDays));
            }
        }

        public NewNurseViewModel ()
        {
            Nurse = new Nurse();
            SaveCommand = new RelayCommand(SaveNurseToDb );
            CancelCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow()
        {
            RequestClose?.Invoke();
        }

        private void SaveNurseToDb()
        {
            Nurse.ValidateOnSubmit = true;
            CommandManager.InvalidateRequerySuggested();
            if (!Nurse.IsValid())
                return;
            
            try
            {
                ValidateNurse();

                using var context = new ScheduleDbContext();
                context.Database.EnsureCreated();
                context.Nurses.Add(new Models.DbModelle.NurseEntity
                {
                    Id = Nurse.Id,
                    FirstName = Nurse.FirstName,
                    LastName = Nurse.LastName,
                    WorkingHours = Nurse.WorkingHours,
                    UnavailableDays = Nurse.UnavailableDays,
                    EmploymentStatus = Nurse.EmploymentStatus
                });

                context.SaveChanges();
                // Nachricht ans Parent senden
                WeakReferenceMessenger.Default.Send(new NurseSavedMessage(Nurse));
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

        #region Validation

        /// <summary>
        /// UI/Validation
        /// IDataErrorInfo
        /// for Userfeedback
        /// </summary>
        

        private void ValidateNurse()
        {
            if (string.IsNullOrWhiteSpace(Nurse.FirstName))
                throw new InvalidOperationException("Vorname ist erforderlich");
            if (string.IsNullOrWhiteSpace(Nurse.LastName))
                throw new InvalidOperationException("Nachname ist erforderlich");
            if (Nurse.WorkingHours < 0)
                throw new InvalidOperationException("Arbeitsstunden müssen größer 0 sein");
            if (string.IsNullOrWhiteSpace(Nurse.EmploymentStatus))
                throw new InvalidOperationException("Arbeitsstatus ist erforderlich");
        }

        # endregion



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(
            [System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
