using NurseProblem.DomainLayer.Enums;
using NurseProblem.InterfaceAdaptersLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.DomainLayer.UiModelle
{
    public class Nurse 
    {
        public int Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public double WorkingHours { get; set; }
        public WeekDays UnavailableDays { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }

        public Nurse() { }
        public Nurse(int id, string name)
        {
            Id = id;
            FirstName = name;
        }
        public Nurse( string firstName, string lastName, double workingHours, WeekDays unavailableDays, EmploymentStatus employmentStatus) 
        {

            //Id = id;
            FirstName = firstName;
            LastName = lastName;
            WorkingHours = workingHours;
            UnavailableDays = unavailableDays;
            EmploymentStatus = employmentStatus;
        }

        internal void SetId(int id)
        {
            Id = id;
        }
    }

}
