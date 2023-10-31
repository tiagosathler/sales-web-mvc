using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;

namespace SalesWebMVC
{
    public static class Program
    {
        private const string DEFAULT_CONNECTION = "SalesWebMVCContext";
        private const string MIGRATION_ASSEMBLY = "SalesWebMVC";

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;

            string connection = configuration.GetConnectionString(DEFAULT_CONNECTION)!;

            builder.Services.AddDbContext<SalesWebMVCContext>(
                options => options
                    .UseMySql(connection, ServerVersion.AutoDetect(connection), builder => builder.MigrationsAssembly(MIGRATION_ASSEMBLY)));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}