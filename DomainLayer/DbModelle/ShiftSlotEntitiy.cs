using NurseProblem.DomainLayer.UiModelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.DomainLayer.DbModelle
{
    public class ShiftSlotEntity
    {
        public int Id { get; set; }

        public ShiftType Shift {  get; set; }
        public int SlotNumber { get; set; }

        public int? NurseId { get; set; }
        public string NurseName { get; set; }

        // Foreign Key
        public int DayEntityId { get; set; }
        public DayEntity Day { get; set; }
    }

}
