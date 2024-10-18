using CyberBilby.Shared.Configuration;
using CyberBilby.Shared.Database;
using CyberBilby.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CyberBilby.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DotEnv.Load(Environment.CurrentDirectory);
            DotEnv.Ensure("MYSQL_CONNECTION");

            var connectionString = DotEnv.Get("MYSQL_CONNECTION");

            builder.Services.AddRazorPages();

            builder.Services.AddDbContext<BilbyDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            builder.Services.AddScoped<IBlogRepository, MySqlBlogRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
