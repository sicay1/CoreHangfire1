using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.LiteDB;
using Hangfire.Console;
//using Hangfire.Extensions.Configuration;


namespace CoreHangfire1
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHangfire(config =>
            {
                // Hangfire.console output current only support mongoDb, MSSQL
                //config.UseLiteDbStorage(Configuration.GetConnectionString("litedb1"), new LiteDbStorageOptions { }); 
                
                //config.UseSqlServerStorage(Configuration.GetConnectionString("sqldb"));
                config.UseSqlServerStorage(Configuration.GetConnectionString("sqldb"), new Hangfire.SqlServer.SqlServerStorageOptions {
                    
                });

                config.UseConsole(new ConsoleOptions
                {
                    BackgroundColor = "#f5f5f5",
                    TextColor = "#5cb85c",
                    PollInterval = 500,
                    TimestampColor = "Drakgray",
                });
                config.UseColouredConsoleLogProvider();
               //config.UseLog4NetLogProvider();
            });
            
            //services.AddHangfire(x => x.UseLiteDbStorage(Configuration.GetConnectionString("litedb1")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                Queues = new[] { "queue1111", "queue2222", "DEFAULT" },
                ServerName = "SuperCollector",
                WorkerCount = Environment.ProcessorCount * 2,
            });
            app.UseHangfireDashboard("/hf", new DashboardOptions
            {
                AppPath = "/",
                DisplayStorageConnectionString = false,

            });

            //app.UseHangfireDashboard("/hangfire", Configuration.GetHangfireDashboardOptions());
            //app.UseHangfireServer(Configuration.GetHangfireBackgroundJobServerOptions());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
