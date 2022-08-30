using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.Domain.Entities;
using PetClub.CrossCutting.Identity.Models;

namespace PetClub.CrossCutting.Identity.Persistence.Map
{
    public class MapApplicationUser : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(p => p.FullName).HasMaxLength(200).IsRequired();
            builder.Property(p => p.Cpf).HasMaxLength(14).IsRequired();
        }
    }
}
