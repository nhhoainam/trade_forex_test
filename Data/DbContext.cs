using HandleMessage.Model;
using Microsoft.EntityFrameworkCore;

namespace HandleMessage.Data;
public class TradingSystemContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseSqlServer("Server=202.55.134.227;Database=Trading;User=sa;Password=123456aA@$;TrustServerCertificate=true;");
        optionsBuilder.UseMySQL("Server=103.232.55.72;Database=TradingSystem;User=root;Password=root123");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Account>()
            .HasOne(e => e.User)
            .WithMany(p => p.accounts)
            .HasForeignKey(e => e.IdUser)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Account_User");
    }
}