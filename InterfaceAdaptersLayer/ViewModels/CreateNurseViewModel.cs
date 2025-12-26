using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using NurseProblem.FrameworkLayer.Datenbank;
using NurseProblem.DomainLayer;
using NurseProblem.DomainLayer.DbModelle;
using NurseProblem.DomainLayer.UiModelle;
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
using NurseProblem.Services.Interfaces;
using NurseProblem.DomainLayer.Enums;

namespace NurseProblem.InterfaceAdaptersLayer.ViewModels
{
    

    partial class CreateNurseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly CreateNurseUseCase _addNurseUseCase;
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? RequestClose;
        public IEnumerable<EmploymentStatus> EmploymentStatuses =>
            Enum.GetValues(typeof(EmploymentStatus))
                .Cast<EmploymentStatus>()
                .Where(e => e != EmploymentStatus.None);

        public bool CanNotWorkMonday
        {
            get => UnavailableDays.HasFlag(WeekDays.Monday);
            set
            {
                if (value) UnavailableDays |= WeekDays.Monday;
                else    UnavailableDays &= ~WeekDays.Monday;
            }
        }
        public bool CanNotWorkTuesday
        {
            get =>  UnavailableDays.HasFlag(WeekDays.Tuesday);
            set
            {
                if (value) UnavailableDays |= WeekDays.Tuesday;
                else UnavailableDays &= ~WeekDays.Tuesday;
            }
        }
        public bool CanNotWorkWednesday
        {
            get => UnavailableDays.HasFlag(WeekDays.Wednesday);
            set
            {
                if (value)  UnavailableDays |= WeekDays.Wednesday;
                else        UnavailableDays &= ~WeekDays.Wednesday;
            }
        }
        public bool CanNotWorkThursday
        {
            get => UnavailableDays.HasFlag(WeekDays.Thursday);
            set
            {
                if (value)      UnavailableDays |= WeekDays.Thursday;
                else    UnavailableDays &= ~WeekDays.Thursday;
            }
        }
        public bool CanNotWorkFriday
        {
            get =>  UnavailableDays.HasFlag(WeekDays.Friday);
            set
            {
                if (value)  UnavailableDays |= WeekDays.Friday;
                else UnavailableDays &= ~WeekDays.Friday;
            }
        }
        public bool CanNotWorkSaturday
        {
            get => UnavailableDays.HasFlag(WeekDays.Saturday);
            set
            {
                if (value) UnavailableDays |= WeekDays.Saturday;
                else UnavailableDays &= ~WeekDays.Saturday;
            }
        }
        public bool CanNotWorkSunday
        {
            get => UnavailableDays.HasFlag(WeekDays.Sunday);
            set
            {
                if (value) UnavailableDays |= WeekDays.Sunday;
                else UnavailableDays &= ~WeekDays.Sunday;
            }
        }


        private string _firstName = "";
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName == value) return;
                _firstName = value;
                OnPropertyChanged();
                OnPropertyChanged("Item[]");
            }
        }

        private string _lastName = "";
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                OnPropertyChanged();
                OnPropertyChanged("Item[]");
            }
        }

        private double _weeklyHours;
        public double WeeklyHours
        {
            get => _weeklyHours;
            set
            {
                if (_weeklyHours == value) return;
                _weeklyHours = value;
                OnPropertyChanged(nameof(WeeklyHours));
                OnPropertyChanged("Item[]");
            }
        }

        private EmploymentStatus _employmentStatus;
        public EmploymentStatus EmploymentStatus
        {
            get => _employmentStatus;
            set
            {
                if (_employmentStatus == value) return;
                _employmentStatus = value;
                OnPropertyChanged(nameof(EmploymentStatus));
                OnPropertyChanged("Item[]");
            }
        }

        private WeekDays _unavailableDays;
        public WeekDays UnavailableDays
        {
            get => _unavailableDays;
            set
            {
                if (_unavailableDays == value) return;
                _unavailableDays = value;
                OnPropertyChanged();
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

        private IErrorDialogService _errorDialog;
        
        /// <summary>
        /// ViewModel just knows the UseCase
        /// </summary>
        public CreateNurseViewModel(CreateNurseUseCase addNurseUseCase, IErrorDialogService errorDialogService)
        {
            _addNurseUseCase = addNurseUseCase;
            _errorDialog = errorDialogService;
            SaveCommand = new RelayCommand(async () => await SaveNurseAsyncInDb());
            CancelCommand = new RelayCommand(CloseWindow);
        }

        private void CloseWindow() => RequestClose?.Invoke();

        private async Task SaveNurseAsyncInDb()
        {
            if (!IsValid())
                return;
            try
            {
                // Nurse Entity will be build in UseCase layer
                CommandManager.InvalidateRequerySuggested();
                var command = new CreateNurseCommand(
                    FirstName,
                    LastName,
                    WeeklyHours,
                    UnavailableDays,
                    EmploymentStatus
                );
                var nurse = await _addNurseUseCase.ExecuteSaveNurseAsync(command);
                WeakReferenceMessenger.Default.Send(new NurseSavedMessage(nurse));
                CloseWindow();
            }
            catch (DomainException ex)
            {
                _errorDialog.ShowError(ex.Message, "Fehler");
            }
        }

        #region Validation

        /// <summary>
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
                    case nameof(FirstName):
                        if (string.IsNullOrWhiteSpace(FirstName)) return "Vorname ist erforderlich";
                        break;
                    case nameof(LastName):
                        if (string.IsNullOrWhiteSpace(LastName)) return "Nachname ist erforderlich";
                        break;
                    case nameof(WeeklyHours):
                        if (WeeklyHours <= 0) return "Arbeitsstunden müssen größer als oder gleich 0 sein";
                        break;
                    case nameof(EmploymentStatus):
                        if (EmploymentStatus == EmploymentStatus.None) return "Status ist erforderlich";
                        break;
                }
                return null;
            }
        }
        public bool IsValid()
        {
            ValidateOnSubmit = true;

            return string.IsNullOrWhiteSpace(this[nameof(FirstName)])
                && string.IsNullOrWhiteSpace(this[nameof(LastName)])
                && string.IsNullOrWhiteSpace(this[nameof(WeeklyHours)])
                && string.IsNullOrWhiteSpace(this[nameof(EmploymentStatus)]);
        }
        # endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
