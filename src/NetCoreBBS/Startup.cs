using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreBBS.Models;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;
using NetCoreBBS.Middleware;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.WebEncoders;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace NetCoreBBS
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password = new PasswordOptions() {
                    RequireNonAlphanumeric = false,
                    RequireUppercase=false
                };
            }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
            // Add framework services.
            services.AddMvc();

            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<UserServices>();
            services.AddMemoryCache();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    authBuilder =>
                    {
                        authBuilder.RequireClaim("Admin", "Allowed");
                    });
            });
            //文字被编码 https://github.com/aspnet/HttpAbstractions/issues/315
            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.ConfigureNLog("nlog.config");
            loggerFactory.AddNLog();

            app.UseRequestIPMiddleware();

            InitializeNetCoreBBSDatabase(app.ApplicationServices);
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseIdentity();
            app.UseStatusCodePages();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void InitializeNetCoreBBSDatabase(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<DataContext>();
                db.Database.EnsureCreated();
            }
        }
    }
}
