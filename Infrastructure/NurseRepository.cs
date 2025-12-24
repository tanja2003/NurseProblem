using CommunityToolkit.Mvvm.Messaging;
using NurseProblem.Datenbank;
using NurseProblem.Models;
using NurseProblem.Models.UiModelle;
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

        public async Task AddAsync(Nurse nurse)
        {
            CommandManager.InvalidateRequerySuggested();
            _context.Nurses.Add(new Models.DbModelle.NurseEntity
            {
                Id = nurse.Id,
                FirstName = nurse.FirstName,
                LastName = nurse.LastName,
                WorkingHours = nurse.WorkingHours,
                UnavailableDays = nurse.UnavailableDays,
                EmploymentStatus = nurse.EmploymentStatus
            });
            
            await _context.SaveChangesAsync();
            WeakReferenceMessenger.Default.Send(new NurseSavedMessage(nurse));
        }

        public async Task<Nurse?> GetByIdAsync(int id)
        {
            var entity = await _context.Nurses.FindAsync(id);
            if (entity == null) return null;
            return new Nurse(entity.Id, entity.FirstName, entity.LastName, entity.WorkingHours, entity.UnavailableDays, entity.EmploymentStatus);
        }
    }
}
