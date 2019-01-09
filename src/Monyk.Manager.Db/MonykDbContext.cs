using Microsoft.EntityFrameworkCore;
using Monyk.Manager.Db.Entities;

namespace Monyk.Manager.Db
{
    public class MonykDbContext : DbContext
    {
        public MonykDbContext(DbContextOptions<MonykDbContext> options) : base(options)
        {
        }

        public DbSet<MonitorEntity> Monitors { get; set; }
    }
}
