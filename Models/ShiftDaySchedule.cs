using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NurseProblem.Converter;

namespace NurseProblem.Models
{
    public class ShiftSlot
    {
        public int? NurseId { get; set; }
        public string NurseName { get; set; }
    }

    public class DaySchedule
    {
        public DateTime Date { get; set; }
        public List<ShiftSlot> Früh { get; set; }
        public List<ShiftSlot> Spät { get; set; }
        public List<ShiftSlot> Nacht { get; set; }
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;

        public string WeekdayName => Date.ToString("ddd");

        public DaySchedule(int slots)
        {
            Früh = Enumerable.Range(0, slots).Select(_ => new ShiftSlot()).ToList();
            Spät = Enumerable.Range(0, slots).Select(_ => new ShiftSlot()).ToList();
            Nacht = Enumerable.Range(0, slots).Select(_ => new ShiftSlot()).ToList();
        }
    }

    //public class DaySchedule
    //{
    //    public DateTime Date { get; set; }

    //    public List<int?> FrühNurseId { get; set; }
    //    public List<int?> SpätNurseId { get; set; }
    //    public List<int?> NachtNurseId { get; set; }
    //    public List<string> FrühName {  get; set; }
    //    public List<string> SpätName {  get; set; }
    //    public List<string> NachtName {  get; set; }
    //    public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;

    //    public string WeekdayName => Date.ToString("ddd");

    //    public DaySchedule(int workerSlotsPerShift)
    //    {
    //        FrühName = Enumerable.Repeat<string>(null, workerSlotsPerShift).ToList();
    //        SpätName = Enumerable.Repeat<string>(null, workerSlotsPerShift).ToList();
    //        NachtName = Enumerable.Repeat<string>(null, workerSlotsPerShift).ToList();

    //        FrühNurseId = new int?[workerSlotsPerShift].ToList();
    //        SpätNurseId = new int?[workerSlotsPerShift].ToList();
    //        NachtNurseId = new int?[workerSlotsPerShift].ToList();
    //    }
    //}



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
