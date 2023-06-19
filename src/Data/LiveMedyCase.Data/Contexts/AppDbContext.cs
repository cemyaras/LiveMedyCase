using LiveMedyCase.Data.Model.Entities;
using LiveMedyCase.Data.Seeds;
using Microsoft.EntityFrameworkCore;

namespace LiveMedyCase.Data.Contexts
{
	public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
			builder.Entity<User>()
		        .HasMany(x => x.Referral)
		        .WithOne(x => x.ReffererUser)
		        .HasForeignKey(x => x.ReffererUserId)
		        .HasPrincipalKey(x => x.Id);

			builder.Entity<User>()
				.HasMany(x => x.Referrer)
				.WithOne(e => e.User)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id);

			builder.Seed();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Referral> Referrals { get; set; }
	}
}
