using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeTracker.Api.Filters;
using RecipeTracker.Api.Requests.User;
using RecipeTracker.Api.Services;
using RecipeTracker.Core.Models;
using RecipeTracker.Data;
using RequestInjector.NetCore;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;

namespace RecipeTracker.Api
{
    public class Startup
    {
        IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IRequest), typeof(AddUserRequest))
                .AddClasses()
                .AsSelf()
                .WithScopedLifetime());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(7);
            });
            services.AddOptions();
            var connectionString = Configuration.GetConnectionString("RecipeTrackerConnection");
            var lockoutOptions = new LockoutOptions()
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromDays(99999),
                MaxFailedAccessAttempts = 5
            };

            services.AddDbContext<RecipeTrackerDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(option =>
            {
                option.Lockout = lockoutOptions;
                option.User = new UserOptions { RequireUniqueEmail = true };
                option.Password.RequireDigit = false;
                option.Password.RequiredLength = 12;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<RecipeTrackerDbContext>()
            .AddDefaultTokenProviders();

            var provider = services.BuildServiceProvider();
            services.AddMvc(config =>
            {
                config.ModelMetadataDetailsProviders.Add(new RequestInjectionMetadataProvider());
                config.ModelBinderProviders.Insert(0, new QueryModelBinderProvider(provider));
                config.Filters.Add(new ValidationFilter());
                config.Filters.Add(new AuthorizeFilter());
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new RequestInjectionHandler<IRequest>(provider));
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                options.SerializerSettings.DateFormatString = "MM/dd/yy HH:mm";
            })
            .AddFluentValidation(c =>
            {
                c.RegisterValidatorsFromAssemblyContaining<AddUserRequest>();
                c.RegisterValidatorsFromAssemblyContaining<ApplicationUserValidator>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Recipe Tracker API", Version = "v1" });
            });
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<ProfileService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

                using (var scope = scopeFactory.CreateScope())
                {
                    context.Items.Add("scope", scope);

                    await next.Invoke();
                }
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseIdentityServer();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecipeTracker API");
                });
            }
            app.UseMvc();
        }
    }
}
