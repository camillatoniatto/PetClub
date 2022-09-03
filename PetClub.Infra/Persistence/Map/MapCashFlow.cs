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
    public class MapCashFlow : IEntityTypeConfiguration<CashFlow>
    {
        public void Configure(EntityTypeBuilder<CashFlow> builder)
        {
            builder.ToTable("CashFlow");
            builder.Property(p => p.LaunchValue).IsRequired();
            builder.Property(p => p.NetValue).IsRequired();

            builder.HasOne(p => p.UserCreate).WithMany().HasForeignKey(p => p.IdUserCreate);
            builder.HasOne(p => p.PurchaseOrder).WithMany().HasForeignKey(p => p.IdPurchaseOrder);
            builder.HasOne(p => p.PaymentMethod).WithMany().HasForeignKey(p => p.IdPaymentMethod);
            builder.HasOne(p => p.UserWriteOff).WithMany().HasForeignKey(p => p.IdUserWriteOff);
            builder.HasOne(p => p.UserInactivate).WithMany().HasForeignKey(p => p.IdUserInactivate);
        }
    }
}
