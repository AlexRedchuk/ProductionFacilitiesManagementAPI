namespace ProductionFacilitiesManagement.API.Requests
{
    public class ContractAddRequest
    {
        public string ContractNumber { get; set; }
        public string ProductFacilityCode { get; set; }

        public IEnumerable<EquipmentTypeQuantity> equipmentTypeQuantities { get; set; }
    }

    public class EquipmentTypeQuantity
    {
        public string EquipmentTypeCode { get; set; }
        
        public int equipmentQuantity { get; set; }
    }
}
