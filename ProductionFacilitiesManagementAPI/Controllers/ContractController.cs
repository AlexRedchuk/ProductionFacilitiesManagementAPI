
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionFacilitiesManagement.API.Requests;
using ProductionFacilitiesManagement.API.Responses;
using ProductionFacilitiesManagement.DAL.Interfaces.Repositories;
using ProductionFacilitiesManagement.DAL.Models;


namespace ProductionFacilitiesManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractRepository _repo;
        private readonly IProductionFacilityRepository _productFacilityRepo;
        private readonly IEquipmentTypeRepository _equipmentTypeRepo;
        private readonly IContractEquipmentTypeRepository _contractEquipmentTypeRepo;
        private readonly ILogger<ContractController> _logger;

        public ContractController(IContractRepository repo, IProductionFacilityRepository productFacilityRepo, IEquipmentTypeRepository equipmentTypeRepo, ILogger<ContractController> logger, IContractEquipmentTypeRepository contractEquipmentTypeRepo)
        {
            _repo = repo;
            _productFacilityRepo = productFacilityRepo;
            _equipmentTypeRepo = equipmentTypeRepo;
            _logger = logger;
            _contractEquipmentTypeRepo = contractEquipmentTypeRepo;

        }

        // Additional Task 3: Include role authorize
        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public async Task<IActionResult> Add(ContractAddRequest entity)
        {
            try
            {
                double equipmentAreaNeeded = 0;

                var facility = await _productFacilityRepo.GetByCodeAsync(entity.ProductFacilityCode);


                if(facility == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        statusCode = 400,
                        message = "Facility does not exist"
                    });
                };

                foreach (var el in entity.equipmentTypeQuantities)
                {
                    var equipmentType = await _equipmentTypeRepo.GetByCodeAsync(el.EquipmentTypeCode);
                    equipmentAreaNeeded += equipmentType.Area * el.equipmentQuantity;
                }

                if (facility.EquipmentArea - facility.AreaUsed < equipmentAreaNeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        statusCode = 400,
                        message = "Chosen equipment does not fit to the production facility"
                    });
                }
                facility.AreaUsed = equipmentAreaNeeded;
                
                var contract = new Contract();
                contract.Number = entity.ContractNumber;
                contract.ProductionFacilityCode = facility.Code;
                var createdEntity = await _repo.AddAsync(contract);
                await _productFacilityRepo.Update(facility);
                foreach (var el in entity.equipmentTypeQuantities)
                {
                    var note = new ContractEquipmentType();
                    note.ContractNumber = contract.Number;
                    note.equipmentTypeCode = el.EquipmentTypeCode;
                    note.Quantity = el.equipmentQuantity;
                    await _contractEquipmentTypeRepo.AddAsync(note);
                }
                return CreatedAtAction(nameof(Add), createdEntity);
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

       

        [HttpDelete("{number}")]
        public async Task<IActionResult> Delete(string number)
        {
            try
            {
                var existingEntity = await _repo.GetByCodeAsync(number);
                if (existingEntity == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
                    });
                }

                await _repo.Remove(existingEntity);
                return NoContent();
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var entities = await _repo.GetAllAsync();
                var list = new List<ContractResponse>();
                foreach (var el in entities)
                {
                    var result = new ContractResponse();
                    result.equipmentTypeToQuantities = new List<EquipmentTypeToQuantity>();
                    result.facility = await _productFacilityRepo.GetByCodeAsync(el.ProductionFacilityCode);
                    var notes = await _contractEquipmentTypeRepo.FindByContract(el.Number);
                    foreach (var note in notes)
                    {
                        var obj = new EquipmentTypeToQuantity();
                        obj.equipmentType = await _equipmentTypeRepo.GetByCodeAsync(note.equipmentTypeCode);
                        obj.quantity = note.Quantity;
                        result.equipmentTypeToQuantities.Add(obj);
                    }
                    list.Add(result);
                }
                return Ok(list);
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

        [HttpGet("{number}")]
        public async Task<IActionResult> GetByNumber(string number)
        {
            try
            {
                var entity = await _repo.GetByCodeAsync(number);
                if (entity == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
                    });
                }

                var result = new ContractResponse();
                result.equipmentTypeToQuantities = new List<EquipmentTypeToQuantity>();
                result.facility = await _productFacilityRepo.GetByCodeAsync(entity.ProductionFacilityCode);
                var notes = await _contractEquipmentTypeRepo.FindByContract(entity.Number);
                foreach (var note in notes)
                {
                    var obj = new EquipmentTypeToQuantity();
                    obj.equipmentType = await _equipmentTypeRepo.GetByCodeAsync(note.equipmentTypeCode);
                    obj.quantity = note.Quantity;
                    result.equipmentTypeToQuantities.Add(obj);
                }
                return Ok(result);
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