namespace SportsStore.Domain.Concrete
{
    using SportsStore.Domain.Entities;
    using System.Data.Entity;

    public class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}