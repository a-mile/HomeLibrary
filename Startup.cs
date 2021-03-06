﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using HomeLibrary.Models;
using HomeLibrary.Services;
using Microsoft.AspNetCore.Identity;
using Hangfire;
using Hangfire.MemoryStorage;
using Autofac;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using HomeLibrary.Infrastructure;
using AutoMapper;
using HomeLibrary.Data;
using System.Security.Claims;

namespace HomeLibrary
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddControllersAsServices();

            services.AddSession();
            services.AddMemoryCache();

            services.Configure<DataProtectionTokenProviderOptions>(options =>{
                options.TokenLifespan = TimeSpan.FromHours(24);
            });

            services.AddHangfire(config => config.UseMemoryStorage());

            services.AddAutoMapper(); 

            var builder = new ContainerBuilder();
            var assembly = typeof(ApplicationDbContext).GetTypeInfo().Assembly;                       

            builder.Populate(services);
            builder.RegisterAssemblyTypes(assembly);
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();  
            builder.RegisterType<MailKitEmailSender>().As<IEmailSender>();              

            ApplicationContainer = builder.Build();

            GlobalConfiguration.Configuration.UseActivator(new AutofacJobActivator(ApplicationContainer));

            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime,
            DbInitializer dbInitializer)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseIdentity();

            app.UseHangfireServer();
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate<DatabaseCleaner>(
                x=>x.DeleteUnconfirmedAccounts(), Cron.Hourly);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            dbInitializer.Initialize();

            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
