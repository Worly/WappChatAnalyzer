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

namespace WappChatAnalyzer
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
            services.AddDbContext<MainDbContext>();

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

            services.AddControllers();

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
