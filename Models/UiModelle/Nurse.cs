using NurseProblem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Models.UiModelle
{
    public class Nurse 
    {
        public int Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public double WorkingHours { get; set; }
        public WeekDays UnavailableDays { get; set; }
        public string EmploymentStatus { get; set; }

        public Nurse() { }
        public Nurse(int id, string name)
        {
            Id = id;
            FirstName = name;
        }
        public Nurse(int id, string firstName, string lastName, double workingHours, WeekDays unavailableDays, string employmentStatus) 
        {

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            WorkingHours = workingHours;
            UnavailableDays = unavailableDays;
            EmploymentStatus = employmentStatus;
        }
    }

}
