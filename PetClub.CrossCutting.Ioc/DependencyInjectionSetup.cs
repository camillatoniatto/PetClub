using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetClub.AppService.AppServices.UserAppService;
using PetClub.AppService.AppServices.ServiceRefreshTokenAppService;
using PetClub.Domain.Interfaces;
using PetClub.Infra.Persistence;
using PetClub.Infra.Persistence.Repositores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetClub.CrossCutting.Ioc
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjectionSetup(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ContextFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<PetClubContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // ADD SCOPED
            services.AddScoped<IRepositoryUser, RepositoryUser>();
            services.AddScoped<IRepositoryRefreshTokenData, RepositoryRefreshTokenData>();

            // ADD TRANSIENT
            services.AddTransient<IAppServiceUser, AppServiceUser>();
            services.AddTransient<IAppServiceRefreshToken, AppServiceRefreshToken>();
        }
    }
}
