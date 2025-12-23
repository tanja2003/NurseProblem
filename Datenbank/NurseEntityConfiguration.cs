using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NurseProblem.Models.DbModelle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NurseProblem.Datenbank
{
    public class NurseEntityConfiguration: IEntityTypeConfiguration<NurseEntity>
    {
        public void Configure(EntityTypeBuilder<NurseEntity> builder)
        {
            builder.ToTable("Nurses");

            // No empty Values
            builder.Property(n => n.FirstName).IsRequired().HasMaxLength(60);
            builder.Property(n => n.LastName).IsRequired().HasMaxLength(100);
            builder.Property(n => n.EmploymentStatus).IsRequired().HasMaxLength(50);

            builder.Property(n => n.WorkingHours).IsRequired();
            // No negative Workinghours
            builder.HasCheckConstraint(
                "CK_Nurse_WorkingHours",
                "WorkingHours >= 0"
            );

            // no invalid enum-Values
            builder.Property(n => n.UnavailableDays).HasConversion<int>().IsRequired();
        }
    }
}
