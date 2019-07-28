using Microsoft.EntityFrameworkCore;
using Monyk.Lab.Models;

namespace Monyk.Lab.Db
{
    public class LabDbContext : DbContext
    {
        public LabDbContext(DbContextOptions<LabDbContext> options) : base(options)
        {
        }

        public DbSet<ReactionEntity> Reactions { get; set; }
        public DbSet<ReactionSetEntity> ReactionSets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReactionSetReactionEntity>()
                .HasKey(_ => new { ReactionId = _.ReactionId, ReactionSetId = _.ReactionSetId});

            modelBuilder.Entity<ReactionSetReactionEntity>()
                .HasOne(_ => _.Reaction)
                .WithMany(p => p.ReactionSetReactions)
                .HasForeignKey(pt => pt.ReactionId);

            modelBuilder.Entity<ReactionSetReactionEntity>()
                .HasOne(pt => pt.ReactionSet)
                .WithMany(t => t.ReactionSetReactions)
                .HasForeignKey(pt => pt.ReactionSetId);
        }
    }
}
