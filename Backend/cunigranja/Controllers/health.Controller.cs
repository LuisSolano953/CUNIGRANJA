﻿using cunigranja.Models;
using cunigranja.Services;
using Microsoft.AspNetCore.Mvc;
using cunigranja.Functions;

namespace cunigranja.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class HealthController : Controller
    {
        public readonly HealthServices _Services;
        public IConfiguration _configuration { get; set; }
        public GeneralFunctions FunctionsGeneral;

        public HealthController(IConfiguration configuration, HealthServices healthServices)
        {
            FunctionsGeneral = new GeneralFunctions(configuration);
            _Services = healthServices;
            _configuration = configuration;
        }

        // GET: api/Health/AllHealth

        // POST: api/Health/CreateHealth
        [HttpPost("CreateHealth")]
        public IActionResult Add(HealthModel entity)
        {
            try
            {
                _Services.Add(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                FunctionsGeneral.AddLog(ex.Message);
                return StatusCode(500, ex.ToString());
            }
        }
        [HttpGet("AllHealth")]
        public ActionResult<IEnumerable<HealthModel>> GetHealth()
        {
            return Ok(_Services.GetHealth());
        }

        // GET: api/Health/ConsulHealth?id=1
        [HttpGet("ConsulHealth")]
        public ActionResult<HealthModel> GetHealthById(int id)
        {
            var health = _Services.GetHealthById(id);
            if (health != null)
            {
                return Ok(health);
            }
            else
            {
                return NotFound("Health record not found.");
            }
        }

        // POST: api/Health/UpdateHealth
        [HttpPost("UpdateHealth")]
        public IActionResult UpdateHealth(HealthModel entity)
        {
            try
            {
                if (entity.Id_health <= 0) // Verifica que el ID sea válido
                {
                    return BadRequest("Invalid health ID.");
                }

                _Services.Update(entity);
                return Ok("Health record updated successfully.");
            }
            catch (Exception ex)
            {
                FunctionsGeneral.AddLog(ex.Message);
                return StatusCode(500, ex.ToString());
            }
        }

        // DELETE: api/Health/DeleteHealth?id=1
        [HttpDelete("DeleteHealth")]
        public IActionResult DeleteHealthById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid health ID.");
                }

                var result = _Services.DeleteById(id);

                if (result)
                {
                    return Ok("Health record deleted successfully.");
                }
                else
                {
                    return NotFound("Health record not found.");
                }
            }
            catch (Exception ex)
            {
                FunctionsGeneral.AddLog(ex.Message);
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
