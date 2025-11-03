using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemandManagement.Domain.Entities;
using DemandManagement.Domain.ValueObjects;

namespace DemandManagement.Persistence.Data;

public class DemandManagementDbContext : DbContext
{
    public DemandManagementDbContext(DbContextOptions<DemandManagementDbContext> options) : base(options) { }

    public DbSet<Demand> Demands => Set<Demand>();
    public DbSet<DemandType> DemandTypes => Set<DemandType>();
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<AssociatedDocument> AssociatedDocuments => Set<AssociatedDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Demand
        modelBuilder.Entity<Demand>(entity =>
        {
            entity.ToTable("Demands");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Id)
                .HasConversion(
                    id => id.Value,
                    value => DemandId.From(value))
                .ValueGeneratedNever();

            entity.Property(d => d.Title).IsRequired().HasMaxLength(200);
            entity.Property(d => d.Description).HasMaxLength(2000);

            entity.Property(d => d.DemandTypeId)
                .HasConversion(id => id.Value, value => DemandTypeId.From(value));

            entity.Property(d => d.StatusId)
                .HasConversion(id => id.Value, value => StatusId.From(value));

            entity.Property(d => d.RequestingUserId)
                .HasConversion(id => id.Value, value => UserId.From(value));

            entity.Property(d => d.AssignedToId)
                .HasConversion(id => id.HasValue ? id.Value.Value : (Guid?)null, 
                               value => value.HasValue ? UserId.From(value.Value) : null);

            entity.OwnsOne(d => d.Priority, p =>
            {
                p.Property(pr => pr.Level).HasColumnName("PriorityLevel").IsRequired();
            });

            entity.OwnsOne(d => d.Audit, a =>
            {
                a.Property(au => au.CreatedDate).HasColumnName("CreatedDate").IsRequired();
                a.Property(au => au.UpdatedDate).HasColumnName("UpdatedDate").IsRequired();
            });

            entity.HasMany(d => d.Documents)
                .WithOne()
                .HasForeignKey("DemandId")
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DemandType
        modelBuilder.Entity<DemandType>(entity =>
        {
            entity.ToTable("DemandTypes");
            entity.HasKey(dt => dt.Id);
            entity.Property(dt => dt.Id)
                .HasConversion(id => id.Value, value => DemandTypeId.From(value))
                .ValueGeneratedNever();
            entity.Property(dt => dt.Name).IsRequired().HasMaxLength(100);
            entity.Property(dt => dt.Description).HasMaxLength(500);
            entity.Property(dt => dt.ServiceLevel).HasMaxLength(100);
        });

        // Status
        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Statuses");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id)
                .HasConversion(id => id.Value, value => StatusId.From(value))
                .ValueGeneratedNever();
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.SequenceOrder).IsRequired();
            entity.Property(s => s.IsFinal).IsRequired();
            entity.Property(s => s.IsInitial).IsRequired();
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .HasConversion(id => id.Value, value => UserId.From(value))
                .ValueGeneratedNever();

            entity.OwnsOne(u => u.FullName, fn =>
            {
                fn.Property(f => f.Value).HasColumnName("FullName").IsRequired().HasMaxLength(200);
            });

            entity.OwnsOne(u => u.CorporateEmail, ce =>
            {
                ce.Property(c => c.Value).HasColumnName("CorporateEmail").IsRequired().HasMaxLength(200);
            });

            entity.Property(u => u.RoleId)
                .HasConversion(id => id.Value, value => RoleId.From(value));

            entity.Property(u => u.Department).HasMaxLength(100);
        });

        // Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                .HasConversion(id => id.Value, value => RoleId.From(value))
                .ValueGeneratedNever();
            entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            entity.Property(r => r.Description).HasMaxLength(500);
        });

        // AssociatedDocument
        modelBuilder.Entity<AssociatedDocument>(entity =>
        {
            entity.ToTable("AssociatedDocuments");
            entity.HasKey(ad => ad.Id);
            entity.Property(ad => ad.Id)
                .HasConversion(id => id.Value, value => DocumentId.From(value))
                .ValueGeneratedNever();

            entity.Property(ad => ad.DemandId)
                .HasConversion(id => id.Value, value => DemandId.From(value));

            entity.Property(ad => ad.FileName).IsRequired().HasMaxLength(255);
            entity.Property(ad => ad.FileType).IsRequired().HasMaxLength(50);
            entity.Property(ad => ad.Path).IsRequired().HasMaxLength(500);
            entity.Property(ad => ad.UploadDate).IsRequired();

            entity.Property(ad => ad.UploadedBy)
                .HasConversion(id => id.Value, value => UserId.From(value));
        });
    }
}
