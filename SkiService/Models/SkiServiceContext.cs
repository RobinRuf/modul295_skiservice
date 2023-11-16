using Microsoft.EntityFrameworkCore;

namespace Modul295_SkiService_WebAPI.Models
{
    public class SkiServiceContext: DbContext
    {
        public SkiServiceContext(DbContextOptions<SkiServiceContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Ersetze diesen Connection String mit deinen tatsächlichen MariaDB-Verbindungsinformationen
                var connectionString = "server=localhost;port=3306;database=skiservice;user=root;password=HelloWorld1995@";
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            }
        }
    }
}
