using System.ComponentModel.DataAnnotations;

namespace ProductionFacilitiesManagement.DAL.Models;

public class ProductionFacility
{

    public string Code { get; set; }

    public string Description { get; set; }

    public double EquipmentArea { get; set; }

    public double AreaUsed { get; set; } = 0;

}
