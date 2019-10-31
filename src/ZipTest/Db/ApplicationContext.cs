using Microsoft.EntityFrameworkCore;
using ZipTest.Db.Model;

namespace ZipTest.Db
{
    /// <summary>
    /// The db context for the Zip test.
    /// </summary>
    public class ApplicationContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContext"/> class.
        /// </summary>
        /// <param name="options">The db context options.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Users> Users { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.MonthlySalary).HasColumnType("money");

                entity.Property(e => e.MonthlyExpenses).HasColumnType("money");

                entity.Property(e => e.LastModifiedDt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Accounts>(entity =>
            {
                entity.ToTable("Accounts");

                entity.HasKey(e => e.AccountId);

                entity.Property(e => e.AccountId).HasColumnName("AccountId");

                entity.Property(e => e.AccountUserId).HasColumnName("AccountUserId");

                entity.Property(e => e.MonthlySalary).HasColumnType("money");

                entity.Property(e => e.MonthlyExpenses).HasColumnType("money");

                entity.Property(e => e.LastModifiedDt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedDt)
                    .HasColumnType("datetime");

                entity.HasOne(e => e.User).WithOne(e => e.Account).HasForeignKey<Accounts>(f => f.AccountUserId);
            });
        }
    }
}
