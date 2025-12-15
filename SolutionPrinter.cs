using Google.OrTools.ConstraintSolver;
using Google.OrTools.Sat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment = NurseProblem.Models.SolverModelle.Assignment;

namespace NurseProblem
{
    public class SolutionPrinter : CpSolverSolutionCallback
    {
        public List<Assignment> assignments { get; } = new();
        public SolutionPrinter(int[] allNurses, int[] allDays, int[] allShifts,
                               Dictionary<(int, int, int), BoolVar> shifts, int limit)
        {
            solutionCount_ = 0;
            allNurses_ = allNurses;
            allDays_ = allDays;
            allShifts_ = allShifts;
            shifts_ = shifts;
            solutionLimit_ = limit;
        }

        public override void OnSolutionCallback()
        {
            assignments.Clear();
            foreach (int d in allDays_)
            {
                foreach (int n in allNurses_)
                {
                    foreach (int s in allShifts_)
                    {
                        if (Value(shifts_[(n, d, s)]) == 1)
                        {
                            assignments.Add(new Assignment(n, d,d, s)); // TODO: WorkerPerShift
                        }
                    }
                }
            }
            solutionCount_++;
            if (solutionCount_ >= solutionLimit_)
            {
                Console.WriteLine($"Stop search after {solutionLimit_} solutions");
                StopSearch();
            }
            
        }

        public int SolutionCount()
        {
            return solutionCount_;
        }

        private int solutionCount_;
        private int[] allNurses_;
        private int[] allDays_;
        private int[] allShifts_;
        private Dictionary<(int, int, int), BoolVar> shifts_;
        private int solutionLimit_;
    }
}
