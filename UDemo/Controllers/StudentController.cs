using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using UDemo.Model;

namespace UDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly Repository<Students> _studentRepository;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(Repository<Students> studentRepository, ILogger<StudentsController> logger)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            _logger.LogInformation("GetAllStudents invoked");
            List<Students> results;

            try
            {
                results = _studentRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all students");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error: Failed to retrieve all students");
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            _logger.LogInformation($"GetStudent invoked for id: {id}");
            Students student;

            try
            {
                student = _studentRepository.GetById(id);
                if (student == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve student with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to retrieve student with id: {id}");
            }

            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] Students student)
        {
            _logger.LogInformation("CreateStudent invoked");

            try
            {
                _studentRepository.Create(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create student");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error: Failed to create student");
            }

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Students student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            _logger.LogInformation($"UpdateStudent invoked for id: {id}");

            try
            {
                _studentRepository.Update(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update student with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to update student with id: {id}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            _logger.LogInformation($"DeleteStudent invoked for id: {id}");

            try
            {
                var student = _studentRepository.GetById(id);
                if (student == null)
                {
                    return NotFound();
                }
                _studentRepository.Delete(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete student with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to delete student with id: {id}");
            }

            return NoContent();
        }
    }
}
