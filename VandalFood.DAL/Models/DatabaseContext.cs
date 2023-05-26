using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VandalFood.DAL.Models;

public partial class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContactType> ContactTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerContact> CustomerContacts { get; set; }

    public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }

    public virtual DbSet<Operator> Operators { get; set; }

    public virtual DbSet<OrderContact> OrderContats { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<RoleType> RoleTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\aprox\\source\\repos\\VandalFood\\VandalFood.DAL\\LocalDatabase\\FoodDatabase.mdf");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContactT__3214EC079635637C");

            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0724D8156A");

            entity.Property(e => e.LeftName)
                .HasMaxLength(64)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.Login)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<CustomerContact>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.ContactTypeId }).HasName("PK__Customer__95D07A3978AD67EE");

            entity.Property(e => e.Value)
                .HasMaxLength(128)
                .UseCollation("Cyrillic_General_CI_AS");

            entity.HasOne(d => d.ContactType).WithMany(p => p.CustomerContacts)
                .HasForeignKey(d => d.ContactTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerC__Conta__5535A963");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerContacts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerC__Custo__5441852A");
        });

        modelBuilder.Entity<CustomerOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07AF6283E0");

            entity.Property(e => e.CustomerName)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Operator).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerO__Opera__59063A47");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.CustomerOrders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerO__Order__59FA5E80");
        });

        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operator__3214EC071484D6B3");

            entity.Property(e => e.LeftName)
                .HasMaxLength(64)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.Login)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.RightName)
                .HasMaxLength(64)
                .UseCollation("Cyrillic_General_CI_AS");

            entity.HasOne(d => d.RoleType).WithMany(p => p.Operators)
                .HasForeignKey(d => d.RoleTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Operators__RoleT__48CFD27E");
        });

        modelBuilder.Entity<OrderContact>(entity =>
        {
            entity.HasKey(e => new { e.CustomerOrderId, e.ContactTypeId }).HasName("PK__OrderCon__1985BE5DCE6BC722");

            entity.Property(e => e.Value)
                .HasMaxLength(128)
                .UseCollation("Cyrillic_General_CI_AS");

            entity.HasOne(d => d.ContactType).WithMany(p => p.OrderContacts)
                .HasForeignKey(d => d.ContactTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderCont__Conta__5DCAEF64");

            entity.HasOne(d => d.CustomerOrder).WithMany(p => p.OrderContacts)
                .HasForeignKey(d => d.CustomerOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderCont__Custo__5CD6CB2B");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.CustomerOrderId, e.ProductId }).HasName("PK__OrderIte__E3BB6CD0A35874EF");

            entity.Property(e => e.Amount).HasDefaultValueSql("((1))");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.CustomerOrder).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.CustomerOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Custo__6383C8BA");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Price__628FA481");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3214EC0784D210DC");

            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC078B8507BD");

            entity.Property(e => e.Description)
                .HasMaxLength(256)
                .HasDefaultValueSql("('-')")
                .UseCollation("Cyrillic_General_CI_AS");

            entity.Property(e => e.Title)
                .HasMaxLength(64)
                .UseCollation("Cyrillic_General_CI_AS");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Produc__5165187F");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductT__3214EC072AA4A94C");

            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        modelBuilder.Entity<RoleType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleType__3214EC0722A1FF9C");

            entity.Property(e => e.Title)
                .HasMaxLength(32)
                .UseCollation("Cyrillic_General_CI_AS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
