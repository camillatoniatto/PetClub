using PetClub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Datletica.Infra.Persistence.Map
{
    public class MapUser : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AspNetUsers");
            builder.Ignore(p => p.DateCreation);
            builder.Ignore(p => p.RecordSituation);
            builder.Property(p => p.FullName).HasMaxLength(256).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(256).IsRequired().HasColumnName("Email");
        }
    }
}
