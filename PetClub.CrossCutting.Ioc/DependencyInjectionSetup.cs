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
using PetClub.AppService.AppServices.NotifierAppService;
using PetClub.AppService.AppServices.CashFlowAppService;
using PetClub.AppService.AppServices.PaymentMethodAppService;
using PetClub.AppService.AppServices.PetAppService;
using PetClub.AppService.AppServices.PurchaseOrderAppService;
using PetClub.AppService.AppServices.PurchaseOrderItemAppService;
using PetClub.AppService.AppServices.SchedulerAppService;
using PetClub.AppService.AppServices.ServiceAppService;
using PetClub.AppService.AppServices.UsersPartnersAppService;
using PetClub.AppService.AppServices.DashboardAppService;

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
            services.AddScoped<IRepositoryCashFlow, RepositoryCashFlow>();
            services.AddScoped<IRepositoryPaymentMethod, RepositoryPaymentMethod>();
            services.AddScoped<IRepositoryPet, RepositoryPet>();
            services.AddScoped<IRepositoryPurchaseOrder, RepositoryPurchaseOrder>();
            services.AddScoped<IRepositoryPurchaseOrderItem, RepositoryPurchaseOrderItem>();
            services.AddScoped<IRepositoryScheduler, RepositoryScheduler>();
            services.AddScoped<IRepositoryService, RepositoryService>();
            services.AddScoped<IRepositoryUsersPartners, RepositoryUsersPartners>();
            services.AddScoped<IRepositoryRefreshTokenData, RepositoryRefreshTokenData>();


            // ADD TRANSIENT
            services.AddTransient<IAppServiceUser, AppServiceUser>();
            services.AddTransient<IAppServiceRefreshToken, AppServiceRefreshToken>();
            services.AddTransient<INotifierAppService, NotifierAppService>();
            services.AddTransient<IAppServiceCashFlow, AppServiceCashFlow>();
            services.AddTransient<IAppServiceDashboard, AppServiceDashboard>();
            services.AddTransient<IAppServicePaymentMethod, AppServicePaymentMethod>();
            services.AddTransient<IAppServicePet, AppServicePet>();
            services.AddTransient<IAppServicePurchaseOrder, AppServicePurchaseOrder>();
            services.AddTransient<IAppServicePurchaseOrderItem, AppServicePurchaseOrderItem>();
            services.AddTransient<IAppServiceScheduler, AppServiceScheduler>();
            services.AddTransient<IAppServiceService, AppServiceService>();
            services.AddTransient<IAppServiceUsersPartners, AppServiceUsersPartners>();
        }
    }
}
