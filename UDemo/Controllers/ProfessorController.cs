using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using UDemo.Model;

namespace UDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {
        private readonly Repository<Professors> _professorRepository;
        private readonly ILogger<ProfessorsController> _logger;

        public ProfessorsController(Repository<Professors> professorRepository, ILogger<ProfessorsController> logger)
        {
            _professorRepository = professorRepository ?? throw new ArgumentNullException(nameof(professorRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult GetAllProfessors()
        {
            _logger.LogInformation("GetAllProfessors invoked");
            List<Professors> results;

            try
            {
                results = _professorRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all professors");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error: Failed to retrieve all professors");
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetProfessor(int id)
        {
            _logger.LogInformation($"GetProfessor invoked for id: {id}");
            Professors professor;

            try
            {
                professor = _professorRepository.GetById(id);
                if (professor == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve professor with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to retrieve professor with id: {id}");
            }

            return Ok(professor);
        }

        [HttpPost]
        public IActionResult CreateProfessor([FromBody] Professors professor)
        {
            _logger.LogInformation("CreateProfessor invoked");

            try
            {
                _professorRepository.Create(professor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create professor");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error: Failed to create professor");
            }

            return CreatedAtAction(nameof(GetProfessor), new { id = professor.ProfessorID }, professor);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProfessor(int id, [FromBody] Professors professor)
        {
            if (id != professor.ProfessorID)
            {
                return BadRequest();
            }

            _logger.LogInformation($"UpdateProfessor invoked for id: {id}");

            try
            {
                _professorRepository.Update(professor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update professor with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to update professor with id: {id}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProfessor(int id)
        {
            _logger.LogInformation($"DeleteProfessor invoked for id: {id}");

            try
            {
                var professor = _professorRepository.GetById(id);
                if (professor == null)
                {
                    return NotFound();
                }
                _professorRepository.Delete(professor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete professor with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to delete professor with id: {id}");
            }

            return NoContent();
        }
    }
}
