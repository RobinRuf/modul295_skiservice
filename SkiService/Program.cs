
using Microsoft.EntityFrameworkCore;
using SkiService.Models;

namespace SkiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MySQL COnfiguration
            var connectionString = builder.Configuration.GetConnectionString("DBConnection"); // Stelle sicher, dass du einen Verbindungsstring in deiner appsettings.json hast
            builder.Services.AddDbContext<SkiServiceContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // exclude Swagger from only accessable in Development Mode, because Docker runs with release
            // and we need access to Swagger (in Production we would not do this, because nobody should have 
            // access to our API Endpoints)
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles(); // Gives the ability of serving static web pages
            app.UseRouting(); // added for static website support

            app.UseAuthorization();

            app.MapControllers();
            app.MapGet("/", () => Results.Redirect("/index.html"));

            app.Run();
        }
    }
}