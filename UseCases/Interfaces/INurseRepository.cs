using NurseProblem.Models.UiModelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.UseCases.Interfaces
{
    public interface INurseRepository
    {
        Task AddAsync(Nurse nurse);
        Task<Nurse?> GetByIdAsync(int id);
    }
}
