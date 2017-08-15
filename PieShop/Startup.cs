using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PieShop.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PieShop
{
    public class Startup
    {
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<AppDbContext>(options =>
										 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>();

            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IPieRepository, PieRepository>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddMvc();

			//session specific
			services.AddMemoryCache();
			services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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
                app.UseExceptionHandler("/Pie/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseIdentity();

            app.UseMvc(routes =>
            {
				routes.MapRoute(
                    name: "categoryFilter",
                    template: "Pie/{action}/{category?}",
                    defaults: new { Controller = "Pie", action = "List" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Seed(app);
        }
    }
}
