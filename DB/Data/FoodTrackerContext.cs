using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace DB.Data;

public partial class FoodTrackerContext : DbContext
{
    public FoodTrackerContext(DbContextOptions<FoodTrackerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<MealItem> MealItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2BC2960B25");

            entity.HasIndex(e => e.CategoryName, "UQ__Categori__8517B2E0EFCDF3DF").IsUnique();

            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meals__ACF6A65DA214300B");

            entity.Property(e => e.MealId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("MealID");
            entity.Property(e => e.MealDate).HasColumnType("datetime");
            entity.Property(e => e.MealName).HasMaxLength(100);
        });

        modelBuilder.Entity<MealItem>(entity =>
        {
            entity.HasKey(e => new { e.MealId, e.PurchaseId }).HasName("PK__MealItem__5A4600E0A935BE42");

            entity.Property(e => e.MealId).HasColumnName("MealID");
            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.QuantityUsed).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.Meal).WithMany(p => p.MealItems)
                .HasForeignKey(d => d.MealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MealItems__MealI__236943A5");

            entity.HasOne(d => d.Purchase).WithMany(p => p.MealItems)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MealItems__Purch__245D67DE");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED04619E0C");

            entity.HasIndex(e => new { e.CategoryId, e.ProductName }, "UX_Products_CategoryID_ProductName").IsUnique();

            entity.Property(e => e.ProductId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UnitId).HasColumnName("UnitID");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__17F790F9");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK__Products__UnitID__18EBB532");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Purchase__6B0A6BDE3ECB5388");

            entity.Property(e => e.PurchaseId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PurchaseID");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");
            entity.Property(e => e.PriceTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            entity.Property(e => e.QuantityTotal).HasColumnType("decimal(10, 3)");

            entity.HasOne(d => d.Product).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Purchases__Produ__1DB06A4F");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PK__Units__44F5EC95575B6C67");

            entity.HasIndex(e => e.UnitName, "UQ__Units__B5EE66784F7F58EA").IsUnique();

            entity.Property(e => e.UnitId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UnitID");
            entity.Property(e => e.UnitName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
