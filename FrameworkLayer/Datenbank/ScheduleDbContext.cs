using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NurseProblem.DomainLayer.DbModelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace NurseProblem.FrameworkLayer.Datenbank
{
    public class ScheduleDbContext : DbContext
    {
        public DbSet<DayEntity> Days { get; set; }
        public DbSet<ShiftSlotEntity> ShiftSlots { get; set; }
        public DbSet<NurseEntity> Nurses { get; set; }
        public ScheduleDbContext(DbContextOptions<ScheduleDbContext> options) : base(options)
        {
        }
        public ScheduleDbContext() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=schedule.db");  // Datei
            // optionsBuilder.UseNpgsql("Host=localhost;Database=ScheduleDB;Username=postgres;Password=1234"); // Server/Cloud --> andere Pakete installieren
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DayEntity>()
                .HasMany(d => d.ShiftSlots)
                .WithOne(s => s.Day)
                .HasForeignKey(s => s.DayEntityId);
            modelBuilder.Entity<ShiftSlotEntity>()
                .HasOne(s => s.Day)
                .WithMany(d => d.ShiftSlots)
                .HasForeignKey(s => s.DayEntityId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.ApplyConfiguration(new NurseEntityConfiguration());
        }
    }

}
