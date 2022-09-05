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
    public class MapUsersPartners : IEntityTypeConfiguration<UsersPartners>
    {
        public void Configure(EntityTypeBuilder<UsersPartners> builder)
        {
            builder.ToTable("UsersPartners");
            builder.Property(p => p.IdPartner).IsRequired();
            builder.HasOne(p => p.User).WithMany(x => x.UsersPartners).HasForeignKey(x => x.IdUser);
        }
    }
}
