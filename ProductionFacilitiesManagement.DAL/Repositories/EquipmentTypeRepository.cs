using Microsoft.EntityFrameworkCore;
using ProductionFacilitiesManagement.DAL.DbContext;
using ProductionFacilitiesManagement.DAL.Interfaces.Repositories;
using ProductionFacilitiesManagement.DAL.Models;


namespace ProductionFacilitiesManagement.DAL.Repositories;

public class EquipmentTypeRepository : IEquipmentTypeRepository
{

    private readonly ProductionFacilityManagementContext _ctx;
    public EquipmentTypeRepository(ProductionFacilityManagementContext ctx)
    {
        _ctx = ctx;

    }

    public async Task<EquipmentType> AddAsync(EquipmentType entity)
    {
        _ctx.EquipmentTypes.Add(entity);
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public void Dispose()
    {

    }

    public async Task<IEnumerable<EquipmentType>> GetAllAsync()
    {
        return await _ctx.EquipmentTypes.ToListAsync();
    }

    public async Task<EquipmentType> GetByCodeAsync(string code)
    {
        return await _ctx.EquipmentTypes.FindAsync(code);
    }

    public async Task Remove(EquipmentType entity)
    {
        _ctx.EquipmentTypes.Remove(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task Update(EquipmentType entity)
    {
        _ctx.EquipmentTypes.Update(entity);
        await _ctx.SaveChangesAsync();
    }
}