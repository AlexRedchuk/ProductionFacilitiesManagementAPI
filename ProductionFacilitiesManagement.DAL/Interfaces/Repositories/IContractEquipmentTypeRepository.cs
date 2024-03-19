
using ProductionFacilitiesManagement.DAL.Models;

namespace ProductionFacilitiesManagement.DAL.Interfaces.Repositories;

public interface IContractEquipmentTypeRepository : IRepositoryBase<ContractEquipmentType>
{
    public Task<ContractEquipmentType> FindByContractNumberAndEquipmentCode(string contractNumber, string EquipmentTypeCode);

    public Task<IEnumerable<ContractEquipmentType>> FindByContract(string contractNumber);
}


