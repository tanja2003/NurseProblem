using NurseProblem.Converter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.DomainLayer.UiModelle
{
    public class DaySchedule
    {
        public DateTime Date { get; set; }
        public ObservableCollection<ShiftSlot> Früh { get; set; } 

        public ObservableCollection<ShiftSlot> Spät { get; set; }
        public ObservableCollection<ShiftSlot> Nacht { get; set; }
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;

        public string WeekdayName => Date.ToString("ddd");

        public DaySchedule(int slots)
        {
            Früh = new ObservableCollection<ShiftSlot>(
                Enumerable.Range(0, slots).Select(_ => new ShiftSlot())
            );

            Spät = new ObservableCollection<ShiftSlot>(
                Enumerable.Range(0, slots).Select(_ => new ShiftSlot())
            );

            Nacht = new ObservableCollection<ShiftSlot>(
                Enumerable.Range(0, slots).Select(_ => new ShiftSlot())
            );
        }
        public DaySchedule() { }
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
