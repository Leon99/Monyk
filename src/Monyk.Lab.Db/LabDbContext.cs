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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionGroupActionEntity>()
                .HasKey(_ => new { _.ActionId, _.ActionGroupId});

            modelBuilder.Entity<ActionGroupActionEntity>()
                .HasOne(_ => _.Action)
                .WithMany(p => p.ActionGroupActions)
                .HasForeignKey(pt => pt.ActionId);

            modelBuilder.Entity<ActionGroupActionEntity>()
                .HasOne(pt => pt.ActionGroup)
                .WithMany(t => t.ActionGroupActions)
                .HasForeignKey(pt => pt.ActionGroupId);
        }
    }
}
