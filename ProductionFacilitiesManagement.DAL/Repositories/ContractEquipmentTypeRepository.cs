using Microsoft.EntityFrameworkCore;
using ProductionFacilitiesManagement.DAL.DbContext;
using ProductionFacilitiesManagement.DAL.Interfaces.Repositories;
using ProductionFacilitiesManagement.DAL.Models;

namespace ProductionFacilitiesManagement.DAL.Repositories;

public class ContractEquipmentTypeRepository : IContractEquipmentTypeRepository
{

    private readonly ProductionFacilityManagementContext _ctx;
    public ContractEquipmentTypeRepository(ProductionFacilityManagementContext ctx)
    {
        _ctx = ctx;

    }

    public async Task<ContractEquipmentType> AddAsync(ContractEquipmentType entity)
    {
        _ctx.ContractEquipmentTypes.Add(entity);
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public void Dispose()
    {

    }

    public async Task<IEnumerable<ContractEquipmentType>> GetAllAsync()
    {
        return await _ctx.ContractEquipmentTypes.ToListAsync();
    }

    public async Task<ContractEquipmentType> GetByCodeAsync(string code)
    {
        return await _ctx.ContractEquipmentTypes.FindAsync(code);
    }

    public async Task Remove(ContractEquipmentType entity)
    {
        _ctx.ContractEquipmentTypes.Remove(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task Update(ContractEquipmentType entity)
    {
        _ctx.ContractEquipmentTypes.Update(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task<ContractEquipmentType> FindByContractNumberAndEquipmentCode(string contractNumber, string EquipmentTypeCode)
    {
        return await _ctx.ContractEquipmentTypes.FirstOrDefaultAsync(o => o.ContractNumber == contractNumber && o.equipmentTypeCode == EquipmentTypeCode);
    }

    public async Task<IEnumerable<ContractEquipmentType>> FindByContract (string contractNumber)
    {
        return await _ctx.ContractEquipmentTypes.Where(o => o.ContractNumber == contractNumber).ToListAsync();
    }
}