using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using HiloGame.IdentityServer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//using Serilog;

namespace HiloGame.IdentityServer
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=25432;Database=identityserver;Username=postgres;Password=postgres";

            builder.Services.AddRazorPages();

            var isBuilder = builder.Services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(connectionString,
                        sql => sql.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseNpgsql(connectionString,
                        sql => sql.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
                })
                .AddTestUsers(TestUsers.Users);


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWasm", policy =>
                {
                    policy.WithOrigins("https://localhost:7045")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            InitializeDatabase(app);

            //app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseCors("AllowWasm");
            app.UseIdentityServer();
            app.UseAuthorization();


            app.MapRazorPages();

            return app;
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

            var configContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            configContext.Database.Migrate();

            if (!configContext.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    configContext.Clients.Add(client.ToEntity());
                }
                configContext.SaveChanges();
            }

            if (!configContext.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    configContext.IdentityResources.Add(resource.ToEntity());
                }
                configContext.SaveChanges();
            }

            if (!configContext.ApiScopes.Any())
            {
                foreach (var scope in Config.ApiScopes)
                {
                    configContext.ApiScopes.Add(scope.ToEntity());
                }
                configContext.SaveChanges();
            }

            var persistedGrantContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            persistedGrantContext.Database.Migrate();
        }
    }
}