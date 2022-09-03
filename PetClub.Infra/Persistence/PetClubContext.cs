using Datletica.Infra.Persistence.Map;
using Microsoft.EntityFrameworkCore;
using PetClub.Domain.Entities;
using PetClub.Infra.Persistence.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.Infra.Persistence
{
    public class PetClubContext : DbContext
    {
        public PetClubContext(DbContextOptions<PetClubContext> options) : base(options)
        {
        }

        public DbSet<CashFlow> CashFlow { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItem { get; set; }
        public DbSet<Pet> Pet { get; set; }
        public DbSet<RefreshTokenData> RefreshTokenData { get; set; }
        public DbSet<Scheduler> Scheduler { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UsersPartners> UsersPartners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new MapCashFlow());
            modelBuilder.ApplyConfiguration(new MapPaymentMethod());
            modelBuilder.ApplyConfiguration(new MapPet());
            modelBuilder.ApplyConfiguration(new MapPurchaseOrder());
            modelBuilder.ApplyConfiguration(new MapPurchaseOrderItem());
            modelBuilder.ApplyConfiguration(new MapRefreshTokenData());
            modelBuilder.ApplyConfiguration(new MapScheduler());
            modelBuilder.ApplyConfiguration(new MapService());
            modelBuilder.ApplyConfiguration(new MapUser());
            modelBuilder.ApplyConfiguration(new MapUsersPartners());
        }
    }
}
