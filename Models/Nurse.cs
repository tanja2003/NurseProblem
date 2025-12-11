using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Models
{
    public class Nurse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Nurse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}
