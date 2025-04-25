using ECardWebsite.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for each entity
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ECardTemplate> ECardTemplates { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Offer> Offers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fix decimal precision warning for Offer
        modelBuilder.Entity<Offer>()
            .Property(o => o.DiscountPercentage)
            .HasPrecision(5, 2); // e.g., 999.99 max value

        // CATEGORY - TEMPLATE: One-to-Many
        modelBuilder.Entity<ECardTemplate>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Templates)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // USER - FEEDBACK: One-to-Many
        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.User)
            .WithMany(u => u.Feedbacks)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // USER - SUBSCRIPTION: One-to-Many
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // SUBSCRIPTION - OFFER: Many-to-One (optional)
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Offer)
            .WithMany(o => o.Subscriptions)
            .HasForeignKey(s => s.OfferId)
            .OnDelete(DeleteBehavior.SetNull);

        // USER - TRANSACTION: One-to-Many
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // TEMPLATE - TRANSACTION: One-to-Many
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Template)
            .WithMany()
            .HasForeignKey(t => t.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
