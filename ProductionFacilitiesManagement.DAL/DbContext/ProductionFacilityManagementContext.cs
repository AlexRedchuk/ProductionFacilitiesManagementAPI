
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductionFacilitiesManagement.DAL.Models;
using System.Reflection.Emit;

namespace ProductionFacilitiesManagement.DAL.DbContext;

public class ProductionFacilityManagementContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public ProductionFacilityManagementContext(IConfiguration configuratation)
    {
        _configuration = configuratation;
        _connectionString = _configuration.GetConnectionString("default");

    }
    public DbSet<ProductionFacility> ProductionFacilities { get; set; }

    public DbSet<EquipmentType> EquipmentTypes { get; set; }

    public DbSet<Contract> Contracts { get; set; }

    public DbSet<ContractEquipmentType> ContractEquipmentTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ProductionFacility>(entity =>
        {
            entity.HasKey(a => a.Code);
            entity.Property(a => a.Description).IsRequired();
            entity.Property(a => a.EquipmentArea).IsRequired();
            entity.Property(a => a.AreaUsed).HasDefaultValue(0);
        });

        builder.Entity<EquipmentType>(entity =>
        {
            entity.HasKey(a => a.Code);
            entity.Property(a => a.Description).IsRequired();
            entity.Property(a => a.Area).IsRequired();
        });

        builder.Entity<Contract>(entity =>
        {
            entity.HasKey(a => a.Number);
            entity.HasOne(a => a.ProductionFacility)
            .WithOne()
            .HasForeignKey<Contract>(a => a.ProductionFacilityCode);
        });

        builder.Entity<ContractEquipmentType>(entity =>
        {
            entity.HasKey(pb => new { pb.ContractNumber, pb.equipmentTypeCode });

            entity.HasOne(pb => pb.Contract)
            .WithMany()
            .HasForeignKey(pb => pb.ContractNumber)
            .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pb => pb.equipmentType)
            .WithMany()
            .HasForeignKey(pb => pb.equipmentTypeCode)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(_connectionString);
    }


}