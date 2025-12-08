using Google.OrTools.ModelBuilder;
using Google.OrTools.Sat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NurseProblem
{
    public class SolverClass
    {
        // Variables
        public int NumNurses { get; }
        public int NumDays { get; }
        public int NumShifts { get; }

        public int[] AllNurses => Enumerable.Range(0, NumNurses).ToArray();
        public int[] AllDays => Enumerable.Range(0, NumDays).ToArray();
        public int[] AllShifts => Enumerable.Range(0, NumShifts).ToArray();

        public SolverClass(int nurses, int days, int shifts) 
        {
            // 1. data
            NumNurses = nurses;
            NumDays = days;
            NumShifts = shifts;

            SolveProblem();
        }

        public void SolveProblem()
        {
            // 2. Declare Model
            CpModel model = new();
            model.Model.Variables.Capacity = NumNurses * NumDays * NumShifts;

            // 3. create Variables
            Dictionary<(int, int, int), BoolVar> shifts = new(NumNurses * NumDays * NumShifts);
            foreach (int n in AllNurses)
            {
                foreach (int d in AllDays)
                {
                    foreach (int s in AllShifts)
                    {
                        shifts.Add((n, d, s), model.NewBoolVar($"shifts_n{n}d{d}s{s}"));
                    }
                }
            }
        }
        
    }
}
