using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SkiService.Models;

namespace SkiService.Models
{
    public class SkiServiceContext: DbContext
    {
        public SkiServiceContext(DbContextOptions<SkiServiceContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
    }
}
