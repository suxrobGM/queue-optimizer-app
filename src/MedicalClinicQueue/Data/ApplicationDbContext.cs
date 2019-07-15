using Microsoft.EntityFrameworkCore;
using MedicalClinicQueue.Models;

namespace MedicalClinicQueue.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ServiceItem> ServiceItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=app_db.sqlite");
            }
        }        
    }
}
