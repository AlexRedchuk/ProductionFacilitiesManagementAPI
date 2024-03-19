using ProductionFacilitiesManagement.DAL.Models;

namespace ProductionFacilitiesManagement.API.Responses
{
    public class ContractResponse
    {
        public ProductionFacility facility {  get; set; }

        public List<EquipmentTypeToQuantity> equipmentTypeToQuantities { get; set; }
    }

    public class EquipmentTypeToQuantity
    {
        public EquipmentType equipmentType { get; set; }

        public int quantity { get; set; }
    }
}
