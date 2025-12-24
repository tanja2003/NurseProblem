using NurseProblem.Models.UiModelle;
using NurseProblem.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.UseCases
{
    public class NewNurseUseCase
    {
        
        private readonly INurseRepository _repository;

        public NewNurseUseCase(INurseRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(Nurse nurse)
        {
            // Hier könnten zusätzliche Validierungen oder Business Rules hin
            await _repository.AddAsync(nurse);
        }
    }
}
