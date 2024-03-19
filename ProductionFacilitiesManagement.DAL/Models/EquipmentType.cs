using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionFacilitiesManagement.DAL.Models;

public class EquipmentType
{

    public string Code { get; set; }

    public string Description { get; set; }

    public double Area { get; set; }

}
