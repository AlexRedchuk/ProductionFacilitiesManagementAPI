﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionFacilitiesManagement.DAL.Interfaces.Repositories;
using ProductionFacilitiesManagement.DAL.Models;

namespace ProductionFacilitiesManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentTypeController : ControllerBase
    {
        private readonly IEquipmentTypeRepository _repo;
        private readonly ILogger<EquipmentTypeController> _logger;

        public EquipmentTypeController(IEquipmentTypeRepository repo, ILogger<EquipmentTypeController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // Additional Task 3: Include role authorize
        [HttpPost]
        [Authorize(Roles = "Admin,Operator")]
        public async Task<IActionResult> Add(EquipmentType entity)
        {
            try
            {
                var createdEntity = await _repo.AddAsync(entity);
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

        [HttpPut]
        public async Task<IActionResult> Update(EquipmentType entityToUpdate)
        {
            try
            {
                var existingEntity = await _repo.GetByCodeAsync(entityToUpdate.Code);
                if (existingEntity == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
                    });
                }
                existingEntity.Description = entityToUpdate.Description;
                existingEntity.Area = entityToUpdate.Area;
                await _repo.Update(existingEntity);
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

        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                var existingEntity = await _repo.GetByCodeAsync(code);
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
                return Ok(entities);
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

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                var entity = await _repo.GetByCodeAsync(code);
                if (entity == null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = "record not found"
                    });
                }
                return Ok(entity);
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