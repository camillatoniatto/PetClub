﻿using PetClub.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.SqlServer;
using System;
using PetClub.CrossCutting.Identity.Configuration;
using PetClub.CrossCutting.Ioc;
using Datletica.Api.Configurations;

namespace PetClub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            AutoMapperSetup.AddAutoMapperSetup(services);
            SwaggerSetup.AddSwaggerSetup(services);
            DependencyInjectionSetup.AddDependencyInjectionSetup(services, Configuration);
            SecuritySetup.AddSecuritySetup(services, Configuration);
            ApiSetup.AddApiSetup(services, Configuration);
            //HangfireSetup.AddHangfireSetup(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //var builder = WebApplication.CreateBuilder(args);

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            //app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseRouting();
            //app.UseAuthorization();
            //app.MapRazorPages();
            //app.Run();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerSetup();
            app.UseMvcConfiguration();
            //app.UseHangfireDashboard();
            //app.UseHangfireServer();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHangfireDashboard();
            //});
        }
    }
}
