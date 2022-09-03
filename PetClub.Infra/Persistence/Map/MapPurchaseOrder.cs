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
    public class MapPurchaseOrder : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrder");
            builder.HasOne(p => p.UsersPartners).WithMany().HasForeignKey(p => p.IdUsersPartners);
            builder.HasOne(p => p.PaymentMethod).WithMany().HasForeignKey(p => p.IdPaymentMethod);
        }
    }
}
