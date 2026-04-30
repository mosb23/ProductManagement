using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Domain.Entities;
using ProductManagement_V2.Infrastructuree.Interceptors;
using System.Linq.Expressions;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly AuditSaveChangesInterceptor _auditInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        AuditSaveChangesInterceptor auditInterceptor)
        : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductStatusHistory> ProductStatusHistories => Set<ProductStatusHistory>();
    public DbSet<UserWithRoleView> UsersWithRoles => Set<UserWithRoleView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(SoftDeleteEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
            }
        }

        modelBuilder.Entity<Product>()
            .HasMany(p => p.StatusHistories)
            .WithOne(h => h.Product)
            .HasForeignKey(h => h.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ApplicationUser>(builder =>
        {
            builder.Property(x => x.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            builder.Property(x => x.IsActive)
                   .IsRequired();
        });

        modelBuilder.Entity<UserWithRoleView>(builder =>
        {
            builder.HasNoKey();
            builder.ToView("View_UsersWithRoles", "dbo");
        });
    }

    private static LambdaExpression GetIsDeletedRestriction(Type type)
    {
        var param = Expression.Parameter(type, "e");
        var prop = Expression.Property(param, nameof(SoftDeleteEntity.IsDeleted));
        var condition = Expression.Equal(prop, Expression.Constant(false));
        return Expression.Lambda(condition, param);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
    }
}
