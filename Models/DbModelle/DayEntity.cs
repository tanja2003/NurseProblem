using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Models.DbModelle
{
    public class DayEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ShiftSlotEntity> ShiftSlots { get; set; } = [];
    }

}
