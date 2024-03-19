using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionFacilitiesManagement.DAL.Models;

// Additional Task 1-2: To combine task 1 and 2 solutions created another table to contole N:M relation 
public class ContractEquipmentType
{
    public Contract Contract { get; set; }
    public string ContractNumber { get; set; }
    public EquipmentType equipmentType { get; set; }
    public string equipmentTypeCode { get; set; }
    public int Quantity { get; set; }
}
