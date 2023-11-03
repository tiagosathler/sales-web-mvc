using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Services;
using System.Globalization;

namespace SalesWebMVC
{
    public static class Program
    {
        private const string DEFAULT_CONNECTION = "SalesWebMVCContext";
        private const string MIGRATION_ASSEMBLY = "SalesWebMVC";

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            WebApplication app = ConfigureApp(builder);

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            string connection = builder.Configuration.GetConnectionString(DEFAULT_CONNECTION)!;

            IServiceCollection services = builder.Services;

            services.AddDbContext<SalesWebMVCContext>(options =>
                    options.UseMySql(connection, ServerVersion.AutoDetect(connection), builder =>
                        builder.MigrationsAssembly(MIGRATION_ASSEMBLY)));

            services.AddScoped<SeedingService>();
            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<SalesRecordsService>();

            services.AddControllersWithViews();
        }

        private static WebApplication ConfigureApp(WebApplicationBuilder builder)
        {
            WebApplication app = builder.Build();

            RequestLocalizationOptions localizationOptions = BuilderRequestLocalizationOptions();

            app.UseRequestLocalization(localizationOptions);

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

            return app;
        }

        private static RequestLocalizationOptions BuilderRequestLocalizationOptions()
        {
            CultureInfo enUSCultureInfo = new("en-US");

            List<CultureInfo> culturesInfo = new() { enUSCultureInfo };

            return new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(enUSCultureInfo),
                SupportedCultures = culturesInfo,
                SupportedUICultures = culturesInfo,
            };
        }
    }
}