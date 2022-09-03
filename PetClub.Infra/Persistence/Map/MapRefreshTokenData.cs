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
    public class MapRefreshTokenData : IEntityTypeConfiguration<RefreshTokenData>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenData> builder)
        {
            builder.ToTable("RefreshTokenData");
            builder.Property(x => x.IdUser).IsRequired();
            builder.Property(x => x.RefreshToken).IsRequired();
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.IdUser);
        }
    }
}
