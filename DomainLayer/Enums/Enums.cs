using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.DomainLayer.Enums
{
    [Flags]
    public enum WeekDays
    {
        None = 0,
        Monday = 1 << 0,
        Tuesday = 1 << 1,
        Wednesday = 1 << 2,
        Thursday = 1 << 3,
        Friday = 1 << 4,
        Saturday = 1 << 5,
        Sunday = 1 << 6
    }
    public enum EmploymentStatus
    {
        None = 0,
        [Description("Vollzeit")]
        FullTimeEmployee,

        [Description("Teilzeit")]
        PartTimeEmployee = 2,

        [Description("Student")]
        Student = 3,

        [Description("FSJ")]
        FSJ = 4,
    }
}
