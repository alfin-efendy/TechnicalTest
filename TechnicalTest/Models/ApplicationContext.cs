using Microsoft.EntityFrameworkCore;

namespace TechnicalTest.Models;


 public class ApplicationContext : DbContext
{
    /// <summary>
    /// Represents the application context for the database.
    /// </summary>
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet of Users.
    /// </summary>
    public virtual DbSet<User> Users { get; set; }

    /// <summary>
    /// Configures the model for the database.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define the User entity
        modelBuilder.Entity<User>(entity =>
        {
            // Define the primary key
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            // Define the properties email, first name, and last name
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);

            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(255);

            entity.Property(e => e.LastName).IsRequired().HasMaxLength(255);
        });
    }
}
