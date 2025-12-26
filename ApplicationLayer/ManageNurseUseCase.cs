using NurseProblem.DomainLayer.UiModelle;
using NurseProblem.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.ApplicationLayer
{
    public class ManageNurseUseCase
    {
        private readonly INurseRepository _repository;

        public ManageNurseUseCase(INurseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Nurse?> GetNurseByIdAsync(int id)
        {

            var nurseId = await _repository.GetByIdAsync(id);
            return nurseId;
        }
    }
}
