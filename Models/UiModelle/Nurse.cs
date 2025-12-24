using NurseProblem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Models.UiModelle
{
    public class Nurse: IDataErrorInfo, INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    OnPropertyChanged("Item[]");
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    OnPropertyChanged("Item[]");
                }
            }
        }

        private double _workingHours;
        public double WorkingHours
        {
            get => _workingHours;
            set
            {
                if (_workingHours != value)
                {
                    _workingHours = value;
                    OnPropertyChanged(nameof(WorkingHours));
                    OnPropertyChanged("Item[]");
                }
            }
        }
        public WeekDays UnavailableDays { get; set; }

        private string _employmentStatus;
        public string EmploymentStatus
        {
            get => _employmentStatus;
            set
            {
                if (_employmentStatus != value)
                {
                    _employmentStatus = value;
                    OnPropertyChanged(nameof(EmploymentStatus));
                    OnPropertyChanged("Item[]");
                }
            }
        }

        public Nurse(int id, string name)
        {
            Id = id;
            FirstName = name;
        }
        public Nurse () { }

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
                    case nameof(WorkingHours):
                        if (WorkingHours <= 0) return "Arbeitsstunden müssen größer als oder gleich 0 sein";
                        break;
                    case nameof(EmploymentStatus):
                        if (string.IsNullOrWhiteSpace(EmploymentStatus)) return "Status ist erforderlich";
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
                && string.IsNullOrWhiteSpace(this[nameof(WorkingHours)])
                && string.IsNullOrWhiteSpace(this[nameof(EmploymentStatus)]);
        }

        private bool _validateOnSubmit;
        public bool ValidateOnSubmit
        {
            get => _validateOnSubmit;
            set
            {
                if (_validateOnSubmit != value)
                {
                    _validateOnSubmit = value;
                    OnPropertyChanged(null); 
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
