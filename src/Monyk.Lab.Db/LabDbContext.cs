using Microsoft.EntityFrameworkCore;
using Monyk.Lab.Models;

namespace Monyk.Lab.Db
{
    public class LabDbContext : DbContext
    {
        public LabDbContext(DbContextOptions<LabDbContext> options) : base(options)
        {
        }

        public DbSet<ActionEntity> Actions { get; set; }
        public DbSet<ActionGroupEntity> ActionGroups { get; set; }
    }
}
