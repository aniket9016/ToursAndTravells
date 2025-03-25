using Microsoft.EntityFrameworkCore;

namespace ToursAndTravells.Models
{
    public class TandTDBContext : DbContext
    {
        public TandTDBContext(DbContextOptions<TandTDBContext> options) : base(options)
        { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> State { get; set; }  
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasMany(c => c.States)
                .WithOne(s => s.Country)
                .HasForeignKey(s => s.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Users)
                .WithOne(u => u.Country)
                .HasForeignKey(u => u.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<State>()
                .HasMany(s => s.Users)
                .WithOne(u => u.State)
                .HasForeignKey(u => u.StateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
