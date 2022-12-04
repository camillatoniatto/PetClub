using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using PetClub.AppService.AppServices.UriAppService.Interfaces;
using PetClub.AppService.AppServices.UtilAppService;

namespace PetClub.Configurations
{
    public static class ApiSetup
    {
        public static void AddApiSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSingleton<IUriAppService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), "/");
                return new UriAppService(absoluteUri);
            });

            services.AddCors();
            services.AddControllers();

            //services.AddTransient<IAppServiceEmails, AppServiceEmails>(email =>
            //   new AppServiceEmails(
            //       configuration["EmailConfiguration:SmtpServer"],
            //       configuration.GetValue<int>("EmailConfiguration:Port"),
            //       configuration["EmailConfiguration:Username"],
            //       configuration["EmailConfiguration:Password"],
            //       configuration.GetValue<bool>("EmailConfiguration:EnableSSL")
            //       ));

            services.AddMvc(option => option.EnableEndpointRouting = false);

            //services.AddAWSService<IAmazonS3>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddSignalR();

        }
        public static void UseMvcConfiguration(this IApplicationBuilder app)
        {

            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseApiVersioning();
            app.UseAuthorization();
            app.UseMvc();
        }
    }
}
