using WappChatAnalyzer.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WappChatAnalyzer.Services;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using WappChatAnalyzer.Services.Workspaces;
using Microsoft.EntityFrameworkCore;

namespace WappChatAnalyzer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            WebHostEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MainDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("Main");
                if (WebHostEnvironment.IsProduction() && Environment.GetEnvironmentVariable("POSTGRES_HOSTNAME") != null)
                {
                    var hostname = Environment.GetEnvironmentVariable("POSTGRES_HOSTNAME");
                    var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
                    if (port == null)
                        throw new ArgumentNullException("POSTGRES_PORT");

                    var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
                    if (user == null)
                        throw new ArgumentNullException("POSTGRES_USER");

                    var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
                    if (password == null)
                        throw new ArgumentNullException("POSTGRES_PASSWORD");

                    connectionString = $"Server={hostname};Port={port};Database=WappChatAnalyzer;Userid={user};Password={password}";
                }

                options.UseNpgsql(connectionString);
            });

            //services.AddSingleton<IMessageService, LocalMessageService>();
            services.AddSingleton<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IWorkspaceService, WorkspaceService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IStatisticCacheService, StatisticCacheService>();

            services.AddSingleton<IEmojiService, EmojiService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped<IStatisticFuncsService, StatisticFuncsService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICustomStatisticService, CustomStatisticService>();

            services
                .AddControllers(o => o.UseDateOnlyTimeOnlyStringConverters())
                .AddJsonOptions(o => o.UseDateOnlyTimeOnlyStringConverters());

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "WappChatAnalyzer-frontend/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseMiddleware<Auth.JwtMiddleware>();
            app.UseMiddleware<WorkspaceMiddleware>();

            // global cors policy
            if (env.IsDevelopment())
            {
                app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "WappChatAnalyzer-frontend";

                if (env.IsDevelopment() && Environment.GetEnvironmentVariable("NO_FRONTEND") != "true")
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
