
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SkiService.Models;
using SkiService.Services;
using System.Text;
using Serilog;

namespace SkiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Serilog Config
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/api_.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            // Add Token Service
            builder.Services.AddScoped<ITokenService, TokenService>();

            // MySQL COnfiguration
            var connectionString = builder.Configuration.GetConnectionString("DBConnection"); // Stelle sicher, dass du einen Verbindungsstring in deiner appsettings.json hast
            builder.Services.AddDbContext<SkiServiceContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT", Version = "v1" });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader());
            });

            // JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

            var app = builder.Build();

            // exclude Swagger from only accessable in Development Mode, because Docker runs with release
            // and we need access to Swagger (in Production we would not do this, because nobody should have 
            // access to our API Endpoints)
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT v1"));

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles(); // Gives the ability of serving static web pages
            app.UseRouting(); // added for static website support

            app.UseCors("AllowAllOrigins");

            // Auth
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapGet("/", () => Results.Redirect("/index.html"));

            app.Run();
        }
    }
}