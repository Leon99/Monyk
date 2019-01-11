using Microsoft.EntityFrameworkCore;
using Monyk.GroundControl.Db.Entities;

namespace Monyk.GroundControl.Db
{
    public class MonykDbContext : DbContext
    {
        public MonykDbContext(DbContextOptions<MonykDbContext> options) : base(options)
        {
        }

        public DbSet<MonitorEntity> Monitors { get; set; }
    }
}
