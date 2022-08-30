using Microsoft.EntityFrameworkCore;
using PetClub.Domain.Entities;
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

        public DbSet<User> User { get; set; }
        public DbSet<User> RefreshTokenData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new MapAddress());
        }
    }
}
