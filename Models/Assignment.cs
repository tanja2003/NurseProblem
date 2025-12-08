using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Models
{
    public class Assignment
    {
        public int NurseId { get; }
        public int Day { get; }
        public int Shift { get; }

        

        public Assignment(int nurseId, int day, int shift)
        {
            NurseId = nurseId;
            Day = day;
            Shift = shift;
        }

        //public string NurseName => $"Nurse {NurseId}"; // oder aus Dictionary
        //public string DayName => Day switch
        //{
        //    0 => "Montag",
        //    1 => "Dienstag",
        //    2 => "Mittwoch",
        //    _ => $"Day {Day}"
        //};
        //public string ShiftName => Shift switch
        //{
        //    0 => "Früh",
        //    1 => "Spät",
        //    2 => "Nacht",
        //    _ => $"Shift {Shift}"
        //};
    }

}
