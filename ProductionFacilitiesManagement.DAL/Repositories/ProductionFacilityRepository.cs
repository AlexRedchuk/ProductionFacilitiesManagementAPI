using Microsoft.EntityFrameworkCore;
using ProductionFacilitiesManagement.DAL.DbContext;
using ProductionFacilitiesManagement.DAL.Interfaces.Repositories;
using ProductionFacilitiesManagement.DAL.Models;

namespace ProductionFacilitiesManagement.DAL.Repositories;

public class ProductionFacilityRepository : IProductionFacilityRepository
{

    private readonly ProductionFacilityManagementContext _ctx;
    public ProductionFacilityRepository(ProductionFacilityManagementContext ctx)
    {
        _ctx = ctx;

    }

    public async Task<ProductionFacility> AddAsync(ProductionFacility entity)
    {
        _ctx.ProductionFacilities.Add(entity);
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public void Dispose()
    {
       
    }

    public async Task<IEnumerable<ProductionFacility>> GetAllAsync()
    {
        return await _ctx.ProductionFacilities.ToListAsync();
    }

    public async Task<ProductionFacility> GetByCodeAsync(string code)
    {
        return await _ctx.ProductionFacilities.FindAsync(code);
    }

    public async Task Remove(ProductionFacility entity)
    {
        _ctx.ProductionFacilities.Remove(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task Update(ProductionFacility entity)
    {
        _ctx.ProductionFacilities.Update(entity);
        await _ctx.SaveChangesAsync();
    }
}
