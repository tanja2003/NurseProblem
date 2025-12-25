using CommunityToolkit.Mvvm.Messaging;
using NurseProblem.DomainLayer;
using NurseProblem.DomainLayer.DbModelle;
using NurseProblem.DomainLayer.UiModelle;
using NurseProblem.FrameworkLayer.Datenbank;
using NurseProblem.UseCases.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NurseProblem.Infrastructure
{
    public class NurseRepository : INurseRepository
    {
        private readonly ScheduleDbContext _context;

        public NurseRepository(ScheduleDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Repository: I know where and how to store the nurse
        /// Encapsulate technology
        /// </summary>
        /// <param name="nurse">nurse to be stored</param>
        /// <returns></returns>
        public async Task AddNurseToDbAsync(Nurse nurse)
        {
            CommandManager.InvalidateRequerySuggested();
            var entity = new NurseEntity
            {
                FirstName = nurse.FirstName,
                LastName = nurse.LastName,
                WorkingHours = nurse.WorkingHours,
                UnavailableDays = nurse.UnavailableDays,
                EmploymentStatus = nurse.EmploymentStatus
            };
            _context.Nurses.Add(entity);

            await _context.SaveChangesAsync();

            nurse.SetId(entity.Id);
            WeakReferenceMessenger.Default.Send(new NurseSavedMessage(nurse));
        }

        public async Task<Nurse?> GetByIdAsync(int id)
        {
            var entity = await _context.Nurses.FindAsync(id);
            if (entity == null) return null;
            return new Nurse(entity.FirstName, entity.LastName, entity.WorkingHours, entity.UnavailableDays, entity.EmploymentStatus);
        }
    }
}
