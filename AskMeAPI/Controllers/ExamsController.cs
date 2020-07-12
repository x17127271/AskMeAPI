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
    [Route("api/users/{userId}/exams")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IExamService _examService;

        public ExamsController(IMapper mapper, IExamService examService)
        {
            _mapper = mapper;
            _examService = examService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetExams(int userId)
        {
            var exams = await _examService.GetExams(userId).ConfigureAwait(false);

            return Ok(exams);
        }

        [HttpGet("{examId}/questions", Name = "GetExamQuestions")]
        public async Task<IActionResult> GetExamQuestions(int examId)
        {
            var examQuestions = await _examService.GetExamQuestions(examId).ConfigureAwait(false);

            if (examQuestions == null)
            {
                return NotFound();
            }

            return Ok(examQuestions);
        }

        [HttpGet("{examId}", Name = "GetExam")]
        public async Task<IActionResult> GetExam(int examId)
        {
            var exam = await _examService.GetExamById(examId).ConfigureAwait(false);

            if (exam == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ExamDto>(exam));
        }

        [HttpPost()]
        public async Task<IActionResult> CreateExam([FromBody] ExamQuestionsForCreationDto model)
        {
            // map model to entity
            var exam = _mapper.Map<Exam>(model);

            try
            {
                // create lesson
                var isCreated = await _examService.AddExamQuestions(exam, model.questions).ConfigureAwait(false);
                return Ok(isCreated);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
