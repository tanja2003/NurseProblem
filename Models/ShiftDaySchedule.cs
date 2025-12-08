using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NurseProblem.Converter;

namespace NurseProblem.Models
{
    public class DaySchedule
    {
        public DateTime Date { get; set; }

        public int? FrühNurseId { get; set; }
        public int? SpätNurseId { get; set; }
        public int? NachtNurseId { get; set; }
        public string FrühName {  get; set; }
        public string SpätName {  get; set; }
        public string NachtName {  get; set; }
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;

        public string WeekdayName => Date.ToString("ddd");
    }



    public class NurseDaySchedule
    {
        public string NurseName { get; set; } = "";
        public string[] Days { get; set; }

        public NurseDaySchedule(int numDays)
        {
            Days = new string[numDays];
        }
    }
}
