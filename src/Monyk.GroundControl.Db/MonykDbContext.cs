using Microsoft.EntityFrameworkCore;
using Monyk.GroundControl.Models;

namespace Monyk.GroundControl.Db
{
    public class MonykDbContext : DbContext
    {
        public MonykDbContext(DbContextOptions<MonykDbContext> options) : base(options)
        {
        }

        public DbSet<Monitor> Monitors { get; set; }
    }
}
