using NurseProblem.DomainLayer;
using NurseProblem.DomainLayer.Enums;
using NurseProblem.DomainLayer.UiModelle;
using NurseProblem.InterfaceAdaptersLayer.ViewModels;
using NurseProblem.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace NurseProblem.UseCases
{
    public record CreateNurseCommand(
    string FirstName,
    string LastName,
    double WeeklyHours,
    WeekDays UnavailableDays,
    EmploymentStatus EmploymentStatus
);

    public class CreateNurseUseCase
    {
        /// <summary>
        ///  Rules which need to fit before storing a new Nurse:
        ///  Eine Pflegekraft darf keine negativen Stunden haben
        ///  Max.Wochenstunden(z.B. 40 / 48)
        ///  Teilzeit ≠ Vollzeit
        ///  Name darf nicht doppelt existieren
        ///  Status beeinflusst erlaubte Stunden
        ///  UnavailableDays darf nicht alle Tage enthalten
        /// </summary>
        private readonly INurseRepository _repository;

        public CreateNurseUseCase(INurseRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// UseCase: Store this nurse
        /// don't know anything about technology
        /// </summary>
        /// <param name="nurse"> nurse to be stored</param>
        /// <returns></returns>
        public async Task<Nurse> ExecuteSaveNurseAsync(CreateNurseCommand command)
        {
            Validate(command);

            var nurse = new Nurse(
                firstName: command.FirstName,
                lastName: command.LastName,
                workingHours: command.WeeklyHours,
                unavailableDays: command.UnavailableDays,
                employmentStatus: command.EmploymentStatus              
                );
            await _repository.AddNurseAsync(nurse);
            return nurse;
        }

        private void Validate(CreateNurseCommand command)
        {
            if (command.EmploymentStatus == EmploymentStatus.FullTimeEmployee && command.WeeklyHours > 40) throw new DomainException("Vollzeitkrafte durfen maximal 40 Stunden arbeiten");


        }

    }
}
