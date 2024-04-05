using System;
using Classifieds.Data;
using Classifieds.Data.Entities;
using Classifieds.Web.Services;
using Classifieds.Web.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EntityFrameworkServiceCollectionExtensions =
    Microsoft.Extensions.DependencyInjection.EntityFrameworkServiceCollectionExtensions;
using IdentityEntityFrameworkBuilderExtensions =
    Microsoft.Extensions.DependencyInjection.IdentityEntityFrameworkBuilderExtensions;
using IdentityServiceCollectionUIExtensions =
    Microsoft.Extensions.DependencyInjection.IdentityServiceCollectionUIExtensions;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using ServiceCollectionServiceExtensions = Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions;

namespace Classifieds.Web
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
            EntityFrameworkServiceCollectionExtensions.AddDbContext<ApplicationDbContext>(services, options =>
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"))
            );

            ServiceCollectionServiceExtensions.AddTransient<IEmailSender>(services,
                s => new EmailSender("localhost", 25, "no-reply@classified.com"));

            IdentityEntityFrameworkBuilderExtensions
                .AddEntityFrameworkStores<ApplicationDbContext>(IdentityServiceCollectionUIExtensions
                    .AddDefaultIdentity<User>(services, options =>
                    {
                        options.Password.RequireDigit = true;
                        options.Password.RequiredLength = 8;
                        options.Password.RequireUppercase = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.SignIn.RequireConfirmedAccount = true;

                        options.Lockout.AllowedForNewUsers = true;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                        options.Lockout.MaxFailedAccessAttempts = 3;
                    })
                    .AddRoles<IdentityRole>())
                .AddPasswordValidator<PasswordValidatorService>()
                .AddClaimsPrincipalFactory<CustomClaimsService>();

            // add policies
            services.AddAuthorization(options =>
            {
                // when filter is not specified, the default policy is used
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.AddPolicy(Policies.IsMinimumAge,
                    policy => policy.RequireClaim(UserClaims.IsMinimumAge, "true"));
            });

            // services.AddRazorPages().AddMvcOptions(q => q.Filters.Add(new AuthorizeFilter()));
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}