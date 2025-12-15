using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Models.SolverModelle
{
    public class Schedule
    {
        public List<Assignment> Assignments { get; set; }

        public Schedule(List<Assignment> assignments)
        {
            Assignments = assignments;
        }

        // Optional: Default-Konstruktor
        public Schedule()
        {
            Assignments = new List<Assignment>();
        }
    }


}
