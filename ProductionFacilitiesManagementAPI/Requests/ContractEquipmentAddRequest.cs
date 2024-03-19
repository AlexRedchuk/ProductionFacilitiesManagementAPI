namespace ProductionFacilitiesManagement.API.Requests
{
    public class ContractEquipmentAddRequest
    {
        public string ContractNumber { get; set; }

        public IEnumerable<EquipmentTypeQuantity> equipmentTypeQuantities { get; set; }
    }
}
