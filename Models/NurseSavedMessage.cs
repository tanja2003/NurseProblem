using NurseProblem.Models.UiModelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Models
{
    public class NurseSavedMessage
    {
        public Nurse Nurse { get; }

        public NurseSavedMessage(Nurse nurse)
        {
            Nurse = nurse;
        }
    }
}
