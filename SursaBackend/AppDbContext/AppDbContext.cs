using Microsoft.EntityFrameworkCore;
using SursaBackend.Models;

namespace SursaBackend.AppDbContext
{
    public class SursaBackendDbContext: DbContext

    {
        public SursaBackendDbContext(DbContextOptions options) : base(options) 
        {
        } 
        public DbSet<User> Users { get; set; }
        public DbSet<CreateProduct> Creates { get; set; }

      
    }

}
