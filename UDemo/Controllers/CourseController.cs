using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using UDemo.Model;

namespace UDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly Repository<Courses> _courseRepository;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(Repository<Courses> courseRepository, ILogger<CoursesController> logger)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            _logger.LogInformation("GetAllCourses invoked");
            List<Courses> results;

            try
            {
                results = _courseRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve all courses");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error: Failed to retrieve all courses");
            }

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCourse(int id)
        {
            _logger.LogInformation($"GetCourse invoked for id: {id}");
            Courses course;

            try
            {
                course = _courseRepository.GetById(id);
                if (course == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to retrieve course with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to retrieve course with id: {id}");
            }

            return Ok(course);
        }

        [HttpPost]
        public IActionResult CreateCourse([FromBody] Courses course)
        {
            _logger.LogInformation("CreateCourse invoked");

            try
            {
                var existingCourse = _courseRepository.GetById(course.CourseId);
                if (existingCourse != null)
                {
                    return Conflict("A course with the same ID already exists.");
                }

                _courseRepository.Create(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create course");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server error: Failed to create course");
            }

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, course);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] Courses course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _logger.LogInformation($"UpdateCourse invoked for id: {id}");

            try
            {
                _courseRepository.Update(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update course with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to update course with id: {id}");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            _logger.LogInformation($"DeleteCourse invoked for id: {id}");

            try
            {
                var course = _courseRepository.GetById(id);
                if (course == null)
                {
                    return NotFound();
                }
                _courseRepository.Delete(course);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete course with id: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server error: Failed to delete course with id: {id}");
            }

            return NoContent();
        }
    }
}
