using NurseProblem.InterfaceAdaptersLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.DomainLayer.DbModelle
{
    public class NurseEntity
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Vorname ist erforderlich")]
        [MaxLength(60)]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Nachname ist erforderlich")]
        [MaxLength(100)]
        public string LastName { get; set; }


        [Required]
        [Range(1, 168, ErrorMessage = "Arbeitsstunden müssen > 0 sein")]      
        public double WorkingHours { get; set; }


        public WeekDays UnavailableDays { get; set; }



        [Required(ErrorMessage = "Arbeitsstatus ist erforderlich")]
        [MaxLength(50)]
        public EmploymentStatus EmploymentStatus { get; set; }
    }

}
