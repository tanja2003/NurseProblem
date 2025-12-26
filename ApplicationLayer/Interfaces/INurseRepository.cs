
using NurseProblem.DomainLayer.UiModelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.UseCases.Interfaces
{
    public interface INurseRepository
    {
        Task AddNurseAsync(Nurse nurse);
        Task<Nurse?> GetByIdAsync(int id);
    }
}
