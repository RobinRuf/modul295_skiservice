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

        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<ServiceOrderModel> ServiceOrders { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<StatusModel> Statuses { get; set; }
        public DbSet<PriorityModel> Priorities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API for DB definition (Properties, Keys, Relations ...)

            // Employees
            modelBuilder.Entity<EmployeeModel>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<EmployeeModel>()
                .HasMany(e => e.ServiceOrders)
                .WithOne(so => so.Employee)
                .HasForeignKey(so => so.EmployeeID);

            // Customers
            modelBuilder.Entity<CustomerModel>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<CustomerModel>()
                .HasMany(c => c.ServiceOrders)
                .WithOne(so => so.Customer)
                .HasForeignKey(so => so.CustomerID);

            // Priorities
            modelBuilder.Entity<PriorityModel>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<PriorityModel>()
                .HasMany(c => c.ServiceOrders)
                .WithOne(so => so.PriorityInfo)
                .HasForeignKey(so => so.Priority);

            // Services
            modelBuilder.Entity<ServiceModel>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<ServiceModel>()
                .HasMany(c => c.ServiceOrders)
                .WithOne(so => so.Service)
                .HasForeignKey(so => so.ServiceType);

            // Statuses
            modelBuilder.Entity<StatusModel>()
                .HasKey(e => e.ID);

            modelBuilder.Entity<StatusModel>()
                .HasMany(c => c.ServiceOrders)
                .WithOne(so => so.StatusInfo)
                .HasForeignKey(so => so.Status);

            // ServiceOrders
            modelBuilder.Entity<ServiceOrderModel>()
                .HasKey(e => e.OrderID);

            modelBuilder.Entity<ServiceOrderModel>()
                .HasOne(so => so.Employee)
                .WithMany(e => e.ServiceOrders)
                .HasForeignKey(so => so.EmployeeID) 
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ServiceOrderModel>()
                .HasOne(so => so.Customer) 
                .WithMany(c => c.ServiceOrders)
                .HasForeignKey(so => so.CustomerID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ServiceOrderModel>()
                .HasOne(so => so.PriorityInfo)
                .WithMany(e => e.ServiceOrders)
                .HasForeignKey(so => so.Priority)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ServiceOrderModel>()
                .HasOne(so => so.StatusInfo)
                .WithMany(e => e.ServiceOrders)
                .HasForeignKey(so => so.Status)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ServiceOrderModel>()
                .HasOne(so => so.Service)
                .WithMany(e => e.ServiceOrders)
                .HasForeignKey(so => so.ServiceType)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
