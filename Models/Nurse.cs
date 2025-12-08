using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Models
{
    public class Nurse
    {
        public int Id { get; }
        public string Name { get; }

        public Nurse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}
