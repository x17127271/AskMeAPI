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
        private readonly IMapper _mapper;
        private readonly ILessonService _lessonService;

        public LessonsController(IMapper mapper, ILessonService lessonService)
        {
            _mapper = mapper;
            _lessonService = lessonService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetLessons(int subjectId)
        {
            var lessons = await _lessonService.GetLessons(subjectId).ConfigureAwait(false);

            return Ok(lessons);
        }


        [HttpGet("{lessonId}", Name = "GetLesson")]
        public async Task<IActionResult> GetLesson(int lessonId)
        {
            var lesson = await _lessonService.GetLessonById(lessonId).ConfigureAwait(false);

            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LessonDto>(lesson));
        }

        [HttpPost()]
        public async Task<IActionResult> CreateLesson([FromBody] LessonForCreationDto model)
        {
            // map model to entity
            var lesson = _mapper.Map<Lesson>(model);

            try
            {
                // create lesson
                var lessonCreated = await _lessonService.Create(lesson, model.SubjectId).ConfigureAwait(false);
                return Ok(_mapper.Map<LessonDto>(lessonCreated));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{lessonId}")]
        public async Task<IActionResult> UpdateLesson(LessonDto lessonDto)
        {
            var lesson = _mapper.Map<Lesson>(lessonDto);

            _ = await _lessonService.UpdateLesson(lesson).ConfigureAwait(false);

            return NoContent();
        }
    }
}
