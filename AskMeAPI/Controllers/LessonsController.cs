using System;
using System.Threading.Tasks;
using AskMe.API.Models;
using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AskMe.API.Controllers
{
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
            var lesson = await _lessonService.GetLessons(subjectId).ConfigureAwait(false);

            return Ok(lesson);
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
    }
}
