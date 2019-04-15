using Microsoft.EntityFrameworkCore;
using Monyk.GroundControl.Models;

namespace Monyk.GroundControl.Db
{
    public class GroundControlDbContext : DbContext
    {
        public GroundControlDbContext(DbContextOptions<GroundControlDbContext> options) : base(options)
        {
        }

        public DbSet<MonitorEntity> Monitors { get; set; }
    }
}
