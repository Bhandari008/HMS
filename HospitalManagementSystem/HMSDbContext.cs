using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem
{
    public class HMSDbContext:IdentityDbContext<ApplicationUser>
    {
        public HMSDbContext(DbContextOptions<HMSDbContext> options) : base(options)
        {

        }
        public DbSet<DepartmentModel> Department { get; set; }
        public DbSet<AppointmentModel> Appointment { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DepartmentModel>().ToTable("Department");
            builder.Entity<AppointmentModel>().ToTable("Appointment");
            base.OnModelCreating(builder);
        }
    }
}
