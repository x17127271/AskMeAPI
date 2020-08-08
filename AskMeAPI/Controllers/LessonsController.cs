using System;
using System.Threading.Tasks;
using AskMe.API.Models;
using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AskMe.API.Controllers
{
    [Authorize]
    [Route("api/subjects/{subjectId}/lessons")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        // Variables to use for dependency injection
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;
        // constructor
        public LessonsController(IMapper mapper, ILessonService lessonService)
        {
            _mapper = mapper;
            _lessonService = lessonService;
        }
        /// <summary>
        /// This method returns a list of Lesson resource
        /// for a given Subjet.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="subjectId">Integer</param>
        /// <returns>List<Lesson>()</returns>
        [HttpGet()]
        public async Task<IActionResult> GetLessons(int subjectId)
        {
            // calls lesson service to get list of lesson for a given subject
            var lessons = await _lessonService.GetLessons(subjectId);
            // returns 200ok and the list of lessons
            return Ok(lessons);
        }

        /// <summary>
        /// This method returns an existing Lesson.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="lessonId">Integer</param>
        /// <returns>LessonDto</returns>
        [HttpGet("{lessonId}", Name = "GetLesson")]
        public async Task<IActionResult> GetLesson(int lessonId)
        {
            // calls lessons service to search a lesson by id
            var lesson = await _lessonService.GetLessonById(lessonId);
            // if lesson is null
            if (lesson == null)
            {
                // returns 404 not found
                return NotFound();
            }
            // maps domail model to resource
            //200ok
            return Ok(_mapper.Map<LessonDto>(lesson));
        }
        /// <summary>
        /// This method creates a new Lesson.
        /// HTTP POST verb.
        /// </summary>
        /// <param name="model">LessonForCreationDto</param>
        /// <returns>LessonDto</returns>
        [HttpPost()]
        public async Task<IActionResult> CreateLesson([FromBody] LessonForCreationDto model)
        {
            // maps resource to domain model
            var lesson = _mapper.Map<Lesson>(model);

            try
            {
                // calls lesson service to create a new lesson
                var lessonCreated = await _lessonService.Create(lesson, model.SubjectId);
                // maps domain model to resource
                // returns 200ok
                return Ok(_mapper.Map<LessonDto>(lessonCreated));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// This method updates an existing Lesson.
        /// HTTP PUT verb.
        /// </summary>
        /// <param name="lessonDto">LessonDto</param>
        /// <returns>NoContent204</returns>
        [HttpPut("{lessonId}")]
        public async Task<IActionResult> UpdateLesson(LessonDto lessonDto)
        {
            var lesson = _mapper.Map<Lesson>(lessonDto);

            _ = await _lessonService.UpdateLesson(lesson);

            return NoContent();
        }
    }
}
