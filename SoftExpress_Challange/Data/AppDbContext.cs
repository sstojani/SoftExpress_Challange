using Microsoft.EntityFrameworkCore;
using SoftExpress_Challange.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Statistika> Statistikas { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // One-to-Many relationship configuration: A User can have many Statistika records.
        modelBuilder.Entity<Statistika>()
            .HasOne(s => s.User)          // Each Statistika has one User
            .WithMany(u => u.Statistika)  // A User can have many Statistika records
            .HasForeignKey(s => s.UserId) // Foreign key is UserId in Statistika
            .OnDelete(DeleteBehavior.Cascade); // Optional: specify delete behavior

        // Ensure the UserId column in Statistika is a Guid in the database
        modelBuilder.Entity<Statistika>()
            .Property(s => s.UserId)
            .HasColumnType("uniqueidentifier");  // For SQL Server, this is a Guid (uniqueidentifier)
    }
}
