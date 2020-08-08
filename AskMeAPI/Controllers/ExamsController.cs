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
        // Variables to use for dependency injection
        private readonly IMapper _mapper;
        private readonly IExamService _examService;
        // constructor
        public ExamsController(IMapper mapper, IExamService examService)
        {
            _mapper = mapper;
            _examService = examService;
        }
        /// <summary>
        /// This method returns a lis of Exams resource
        /// for a given User.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="userId">Integer</param>
        /// <returns>List<Exam></returns>
        [HttpGet()]
        public async Task<IActionResult> GetExams(int userId)
        {
            // calls exam service to search exams by userId
            var exams = await _examService.GetExams(userId);
            // returns 200ok
            // list of exams
            return Ok(exams);
        }
        /// <summary>
        /// This methos returns list of Question resource
        /// for a given Exam.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="examId">Integer</param>
        /// <returns>List<ExamQuestion></returns>
        [HttpGet("{examId}/questions", Name = "GetExamQuestions")]
        public async Task<IActionResult> GetExamQuestions(int examId)
        {
            // calls exam service to search question by examId
            var examQuestions = await _examService.GetExamQuestions(examId);
            // if the list is null
            if (examQuestions == null)
            {
                // returns 404 not found
                return NotFound();
            }
            // returns 200ok
            return Ok(examQuestions);
        }
        /// <summary>
        /// This method returns an existing Exam.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="examId">Integer</param>
        /// <returns>ExamDto</returns>
        [HttpGet("{examId}", Name = "GetExam")]
        public async Task<IActionResult> GetExam(int examId)
        {
            // calls exam service to search exam by id
            var exam = await _examService.GetExamById(examId);
            // if Exam is null
            if (exam == null)
            {
                // returns 404 not found
                return NotFound();
            }
            // maps domain model to reosurce
            //200ok
            return Ok(_mapper.Map<ExamDto>(exam));
        }

        /// <summary>
        /// This method creates a new Exam.
        /// HTTP POST verb.
        /// </summary>
        /// <param name="model">ExamQuestionsForCreationDto</param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> CreateExam([FromBody] ExamQuestionsForCreationDto model)
        {
            // maps resource to domain model
            var exam = _mapper.Map<Exam>(model);

            try
            {
                // create lesson
                var isCreated = await _examService.AddExamQuestions(exam, model.questions);
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
