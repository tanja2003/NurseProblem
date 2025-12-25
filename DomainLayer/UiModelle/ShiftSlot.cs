using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.DomainLayer.UiModelle
{
    public class ShiftSlot
    {
        public int SlotNumber { get; set; }
        public int? NurseId { get; set; }
        public string NurseName { get; set; }

        public Nurse? SelectedNurse
        {
            get => _selectedNurse;
            set
            {
                _selectedNurse = value;
                NurseId = value?.Id;
                NurseName = value?.LastName;
            }
        }
        private Nurse? _selectedNurse;
    }

    public enum ShiftType
    {
        Früh = 0,
        Spät = 1,
        Nacht = 2
    }

}
