using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetClub.Domain.Entities;

namespace PetClub.Infra.Persistence.Map
{
    public class MapPet : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("Pet");
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Specie).IsRequired();
            builder.Property(p => p.Genre).IsRequired();
            builder.HasOne(p => p.User).WithMany(x => x.Pet).HasForeignKey(x => x.IdUser);
        }
    }
}
