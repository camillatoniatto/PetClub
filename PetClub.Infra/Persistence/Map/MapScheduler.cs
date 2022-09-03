using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PetClub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Infra.Persistence.Map
{
    public class MapScheduler : IEntityTypeConfiguration<Scheduler>
    {
        public void Configure(EntityTypeBuilder<Scheduler> builder)
        {
            builder.ToTable("Scheduler");
            builder.HasOne(p => p.UsersPartners).WithMany().HasForeignKey(p => p.IdUsersPartners);
            builder.HasOne(p => p.Pet).WithMany().HasForeignKey(p => p.IdPet);
        }
    }
}
