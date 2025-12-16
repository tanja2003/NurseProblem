using Google.OrTools.Sat;
using NurseProblem.Models.SolverModelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment = NurseProblem.Models.SolverModelle.Assignment;

namespace NurseProblem.Services
{
    public class CpSatSchedulerService
    {
        // Variables
        public int NumNurses { get; }
        public int NumDays { get; }
        public int NumShifts { get; }

        public int NumWorkerSlot { get; }

        public int[] AllNurses => Enumerable.Range(0, NumNurses).ToArray();
        public int[] AllDays => Enumerable.Range(0, NumDays).ToArray();
        public int[] AllShifts => Enumerable.Range(0, NumShifts).ToArray();
        public int[] AllWorkerSlot => Enumerable.Range(0, NumWorkerSlot).ToArray();

        private List<(int, int)> weekendDays = new List<(int sat, int sun)>();

        public CpSatSchedulerService(int numNurses, int numDays, int numShifts, int year, int month)
        {
            // 1. data
            NumNurses = numNurses;
            NumDays = numDays;
            NumShifts = numShifts;
            NumWorkerSlot = 1;
            CalculateWeekendDays(year, month);
        }

        private void CalculateWeekendDays(int year, int month)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);
            
            for (int d = 1; d <= DateTime.DaysInMonth(year, month); d++)
            {
                var day = new DateTime(year, month, d);
                if ( day.DayOfWeek == DayOfWeek.Saturday)
                {
                    int saturday = d - 1;
                    int sunday = d < DateTime.DaysInMonth(year, month) ? d : -1;
                    weekendDays.Add((saturday, sunday));
                }
            }
        }

        public Schedule Solve()
        {
            // 2. declare Model
            CpModel model = new();
            model.Model.Variables.Capacity = NumNurses * NumDays * NumShifts * NumWorkerSlot;

            // 3. create Variables
            Dictionary<(int, int, int, int), BoolVar> shifts = new(NumNurses * NumDays * NumShifts * NumWorkerSlot);
            foreach (int n in AllNurses)
            {
                foreach (int d in AllDays)
                {
                    foreach (int w in AllWorkerSlot)
                    {
                        foreach (int s in AllShifts)
                        {
                            shifts.Add((n, d, w, s), model.NewBoolVar($"shifts_n{n}d{d}w{w}s{s}"));
                        }    
                    }
                }
            }

            // 4. add constraints

            // 4.1 Each shift is assigned to Number of workers per shift every day
            List<ILiteral> nurses = new(); 
            foreach (int d in AllDays)
            {
                foreach (int s in AllShifts)
                {
                    foreach (int w in AllWorkerSlot)
                    {
                        foreach (int n in AllNurses)
                        {
                            nurses.Add(shifts[(n, d, w, s)]);
                        }

                        // Genau 1 Nurse pro Tag-Schicht-WorkerSlot
                        model.AddExactlyOne(nurses);
                        nurses.Clear();
                    }
                }
            }


            // 4.2 Each nurse works at most one shift per day
            foreach (int n in AllNurses)
            {
                foreach (int d in AllDays)
                {
                    foreach (int w in AllWorkerSlot)
                    {
                        foreach (int s in AllShifts)
                        {
                            nurses.Add(shifts[(n, d,w, s)]);
                        }
                    }
                    model.AddAtMostOne(nurses);
                    nurses.Clear();
                }
            }

            // 4.3 
            // Try to distribute the shifts evenly, so that each nurse works
            // minShiftsPerNurse shifts. If this is not possible, because the total
            // number of shifts is not divisible by the number of nurses, some nurses will
            // be assigned one more shift.
            int minShiftsPerNurse = (NumShifts * NumDays * NumWorkerSlot) / NumNurses;
            int maxShiftsPerNurse;
            if ((NumShifts * NumDays * NumWorkerSlot) % NumNurses == 0)
            {
                maxShiftsPerNurse = minShiftsPerNurse;
            }
            else
            {
                maxShiftsPerNurse = minShiftsPerNurse + 1;
            }


            foreach (int n in AllNurses)
            {
                List<IntVar> shiftsWorkedByNurse = [];
                foreach (int d in AllDays)
                {
                    foreach (int w in AllWorkerSlot)
                    {
                        foreach (int s in AllShifts)
                        {
                            shiftsWorkedByNurse.Add(shifts[(n, d, w, s)]);
                        }
                    }
                }

                // min ≤ Σ shifts[n,d,s] ≤ max
                model.AddLinearConstraint(LinearExpr.Sum(shiftsWorkedByNurse), minShiftsPerNurse, maxShiftsPerNurse);
            }

            // 4.4  Each Nurse works at max number on Weekends in Month
            //      Min  1 shift on Sa or So --> Weekend is marked as "worked"
            //      If Nurse Works on Sa, So or both the count of worked weekend is +1

            foreach(int n in AllNurses)
            {
                var weekendAssignment = new List<BoolVar>();
                foreach (var (sat,sun) in weekendDays)
                {
                    var satShifts = AllWorkerSlot.SelectMany(w => AllShifts.Select(s => shifts[(n, sat, w, s)])).ToList();
                    var sunShifts = sun >= 0 ? AllWorkerSlot.SelectMany(w => AllShifts.Select(s => shifts[(n, sun, w, s)])).ToList() : new List<BoolVar>();

                    var weekendWorked = model.NewBoolVar($"N{n}_Weekend_{sat}");
                    model.AddMaxEquality(weekendWorked, satShifts.Concat(sunShifts).ToArray());
                    weekendAssignment.Add(weekendWorked);
                }
                model.Add(LinearExpr.Sum(weekendAssignment) <= 2);
                
            }

            // Each Nurse works at max Number on Weekend in Month


            // 5. solve model
            CpSolver solver = new CpSolver();
            solver.StringParameters += "linearization_level:0 ";
            var status = solver.Solve(model);

            List<Assignment> assignments = [];

            if (status == CpSolverStatus.Optimal || status == CpSolverStatus.Feasible)
            {
                foreach (int n in AllNurses)
                {
                    foreach (int d in AllDays)
                    {
                        foreach (int w in AllWorkerSlot)
                        {
                            foreach (int s in AllShifts)
                            {
                                if (solver.Value(shifts[(n, d, w, s)]) == 1)
                                {
                                    assignments.Add(new Assignment(n, d,w, s));
                                }
                            }
                        }
                    }
                }
            }
           

            return new Schedule(assignments);
        }
    }
}
