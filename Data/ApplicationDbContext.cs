using AidaProfileManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AidaProfileManager.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles => Set<Profile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var profile = modelBuilder.Entity<Profile>();
        profile.HasIndex(x => x.NormalizedUsername).IsUnique();
        profile.HasIndex(x => x.NormalizedEmail).IsUnique();
        profile.Property(x => x.FullName).HasMaxLength(120).IsRequired();
        profile.Property(x => x.Username).HasMaxLength(30).IsRequired();
        profile.Property(x => x.NormalizedUsername).HasMaxLength(30).IsRequired();
        profile.Property(x => x.Email).HasMaxLength(254).IsRequired();
        profile.Property(x => x.NormalizedEmail).HasMaxLength(254).IsRequired();
        profile.Property(x => x.PhoneNumber).HasMaxLength(24).IsRequired();
        profile.Property(x => x.Gender).HasMaxLength(40).IsRequired();
        profile.Property(x => x.Bio).HasMaxLength(500);
        profile.Property(x => x.PasswordHash).IsRequired();
    }
}
