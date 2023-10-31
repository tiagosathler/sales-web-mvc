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

            string connection = builder.Configuration.GetConnectionString(DEFAULT_CONNECTION)!;

            builder.Services.AddDbContext<SalesWebMVCContext>(
                options => options
                    .UseMySql(connection, ServerVersion.AutoDetect(connection), builder => builder.MigrationsAssembly(MIGRATION_ASSEMBLY)));

            builder.Services.AddScoped<SeedingService>();

            builder.Services.AddControllersWithViews();

            WebApplication app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                Console.WriteLine("Not in development environment mode!");
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                Console.WriteLine("It's in development environment mode!");
                using IServiceScope serviceScoped = app.Services.CreateScope();
                IServiceProvider serviceProvider = serviceScoped.ServiceProvider;
                SeedingService seedingService = serviceProvider.GetRequiredService<SeedingService>();
                seedingService.Seed();
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