using Microsoft.EntityFrameworkCore;

namespace Sales.Models
{
    public class SalesContext : DbContext
    {
        public SalesContext(DbContextOptions<SalesContext> options) : base(options) { }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Seller> Sellers { get; set; }
        public virtual DbSet<SalesRecord> SalesRecords { get; set; }

    }
}
