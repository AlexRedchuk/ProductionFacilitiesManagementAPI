using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionFacilitiesManagement.DAL.Models;
public class Contract
{

    public string Number { get; set; }

    public ProductionFacility ProductionFacility { get; set; }

    public string ProductionFacilityCode { get; set; }


    /* Possibly we can add user id for better experience, but not mentioned in task */
}
