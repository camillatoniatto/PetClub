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
    public class MapPurchaseOrderItem : IEntityTypeConfiguration<PurchaseOrderItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
            builder.ToTable("PurchaseOrderItem");
            builder.HasOne(p => p.PurchaseOrder).WithMany(a => a.PurchaseOrderItem).HasForeignKey(p => p.IdPurchaseOrder);
            builder.HasOne(p => p.Service).WithMany().HasForeignKey(p => p.IdService);
        }
    }
}
