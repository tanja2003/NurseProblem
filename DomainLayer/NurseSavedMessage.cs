using NurseProblem.DomainLayer.UiModelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.DomainLayer
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
