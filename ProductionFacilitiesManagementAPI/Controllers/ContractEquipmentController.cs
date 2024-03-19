using Microsoft.AspNetCore.Mvc;
using ProductionFacilitiesManagement.API.Requests;
using ProductionFacilitiesManagement.DAL.Interfaces.Repositories;
using ProductionFacilitiesManagement.DAL.Models;

namespace ProductionFacilitiesManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractEquipmentController : ControllerBase
    {
        private readonly IContractRepository _contractRepo;
        private readonly IProductionFacilityRepository _productFacilityRepo;
        private readonly IEquipmentTypeRepository _equipmentTypeRepo;
        private readonly IContractEquipmentTypeRepository _contractEquipmentTypeRepo;
        private readonly ILogger<ContractController> _logger;

        public ContractEquipmentController(IContractRepository repo, IProductionFacilityRepository productFacilityRepo, IEquipmentTypeRepository equipmentTypeRepo, IContractEquipmentTypeRepository contractEquipmentTypeRepo, ILogger<ContractController> logger)
        {
            _contractRepo = repo;
            _productFacilityRepo = productFacilityRepo;
            _equipmentTypeRepo = equipmentTypeRepo;
            _contractEquipmentTypeRepo = contractEquipmentTypeRepo;
            _logger = logger;
        }

        // Additional Task 2: Adding equipment to contract algorithm

        [HttpPost("Add")]
        public async Task<IActionResult> AddEquipment(ContractEquipmentAddRequest entity)
        {
            
            try
            {
                var contract = await _contractRepo.GetByCodeAsync(entity.ContractNumber);
                if (contract == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        statusCode = 400,
                        message = "Contract does not exist"
                    });
                }
                double areaToUse = 0;
                foreach (var el in entity.equipmentTypeQuantities)
                {
                    var equipmentType = await _equipmentTypeRepo.GetByCodeAsync(el.EquipmentTypeCode);
                    if (equipmentType == null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new
                        {
                            statusCode = 400,
                            message = "Equipment does not exist"
                        });
                    }
                    areaToUse += equipmentType.Area * el.equipmentQuantity;
                }
                var facility = await _productFacilityRepo.GetByCodeAsync(contract.ProductionFacilityCode);
                if(facility.AreaUsed + areaToUse > facility.EquipmentArea)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        statusCode = 400,
                        message = "Chosen equipment does not fit to the production facility"
                    });
                }

                foreach (var el in entity.equipmentTypeQuantities)
                {
                    var note = await _contractEquipmentTypeRepo.FindByContractNumberAndEquipmentCode(entity.ContractNumber, el.EquipmentTypeCode);
                    if (note == null)
                    {
                        var newNote = new ContractEquipmentType();
                        newNote.ContractNumber = entity.ContractNumber;
                        newNote.equipmentTypeCode = el.EquipmentTypeCode;
                        newNote.Quantity = el.equipmentQuantity;
                        await _contractEquipmentTypeRepo.AddAsync(newNote);
                    }
                    else
                    {
                        note.Quantity = note.Quantity + el.equipmentQuantity;
                        await _contractEquipmentTypeRepo.Update(note);
                    }
                }

                facility.AreaUsed = facility.AreaUsed + areaToUse;
                await _productFacilityRepo.Update(facility);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    statusCode = 500,
                    message = ex.Message
                });
            }
        }
    }
}
