using Microsoft.EntityFrameworkCore;
using ProductionFacilitiesManagement.DAL.DbContext;
using ProductionFacilitiesManagement.DAL.Interfaces.Repositories;
using ProductionFacilitiesManagement.DAL.Models;

namespace ProductionFacilitiesManagement.DAL.Repositories;

public class ContractRepository : IContractRepository
{

    private readonly ProductionFacilityManagementContext _ctx;
    public ContractRepository(ProductionFacilityManagementContext ctx)
    {
        _ctx = ctx;

    }

    public async Task<Contract> AddAsync(Contract entity)
    {
        _ctx.Contracts.Add(entity);
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public void Dispose()
    {

    }

    public async Task<IEnumerable<Contract>> GetAllAsync()
    {
        return await _ctx.Contracts.ToListAsync();
    }

    public async Task<Contract> GetByCodeAsync(string code)
    {
        return await _ctx.Contracts.FindAsync(code);
    }

    public async Task Remove(Contract entity)
    {
        _ctx.Contracts.Remove(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task Update(Contract entity)
    {
        _ctx.Contracts.Update(entity);
        await _ctx.SaveChangesAsync();
    }


}