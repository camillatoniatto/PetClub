using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PetClub.Infra.Persistence
{
    public class ContextFactory : IDesignTimeDbContextFactory<PetClubContext>
    {
        public PetClubContext CreateDbContext(string[] args)
        {
            string connectionString = GetDefaultConnectionString();
            var builder = new DbContextOptionsBuilder<PetClubContext>();
            builder.UseSqlServer(connectionString);
            return new PetClubContext(builder.Options);
        }

        private string GetDefaultConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            return connectionString;
        }
    }
}
