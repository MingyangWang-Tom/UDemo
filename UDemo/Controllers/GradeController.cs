using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using UDemo.Model;

namespace UDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly Repository<Grades> _gradeRepository;
        private readonly ILogger<GradesController> _logger;

        public GradesController(Repository<Grades> gradeRepository, ILogger<GradesController> logger)
        {
            _gradeRepository = gradeRepository ?? throw new ArgumentNullException(nameof(gradeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult GetAllGrades()
        {
            _logger.LogInformation("GetAllGrades invoked");
            List<Grades> results;

            try
            {
                results = _gradeRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all grades");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error: Failed to retrieve all grades");
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetGrade(int id)
        {
            _logger.LogInformation($"GetGrade invoked for id: {id}");
            Grades grade;

            try
            {
                grade = _gradeRepository.GetById(id);
                if (grade == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve grade with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to retrieve grade with id: {id}");
            }

            return Ok(grade);
        }

        [HttpPost]
        public IActionResult CreateGrade([FromBody] Grades grade)
        {
            _logger.LogInformation("CreateGrade invoked");

            try
            {
                _gradeRepository.Create(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create grade");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error: Failed to create grade");
            }

            return CreatedAtAction(nameof(GetGrade), new { id = grade.GradeID }, grade);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateGrade(int id, [FromBody] Grades grade)
        {
            if (id != grade.GradeID)
            {
                return BadRequest();
            }

            _logger.LogInformation($"UpdateGrade invoked for id: {id}");

            try
            {
                _gradeRepository.Update(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update grade with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to update grade with id: {id}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGrade(int id)
        {
            _logger.LogInformation($"DeleteGrade invoked for id: {id}");

            try
            {
                var grade = _gradeRepository.GetById(id);
                if (grade == null)
                {
                    return NotFound();
                }
                _gradeRepository.Delete(grade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete grade with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to delete grade with id: {id}");
            }

            return NoContent();
        }
    }
}
