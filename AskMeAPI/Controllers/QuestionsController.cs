using System;
using System.Collections.Generic;
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
    [Route("api/lessons/{lessonId}/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        // Variables to use for dependency injection
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;
        // constructor
        public QuestionsController(IMapper mapper, IQuestionService questionService)
        {
            _mapper = mapper;
            _questionService = questionService;
        }
        /// <summary>
        /// This method returns a list of Question resources
        /// for a given Lesson.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="lessonId">Integer</param>
        /// <returns>List<Question>()</returns>
        [HttpGet()]
        public async Task<IActionResult> GetQuestions(int lessonId)
        {
            // calls question service to get a list of existing questions by lesson
            var questions = await _questionService.GetQuestions(lessonId);

            // maps domain model to resource
            // returns the list of question
            //200ok
            return Ok(_mapper.Map<List<QuestionDto>>(questions));
        }

        /// <summary>
        /// This method returns an existing Question resource.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="questionId">Integer</param>
        /// <returns>QuestionDto</returns>
        [HttpGet("{questionId}", Name = "GetQuestion")]
        public async Task<IActionResult> GetQuestion(int questionId)
        {
            // calls question service to search a question by id
            var question = await _questionService.GetQuestionById(questionId);
            // if question is null
            if (question == null)
            {
                // returns not found 404
                return NotFound();
            }
            // if exists returns 200ok
            //maps domain model to resource
            return Ok(_mapper.Map<QuestionDto>(question));
        }
        /// <summary>
        /// This method creates a new Question.
        /// HTTP POST verb.
        /// </summary>
        /// <param name="model">QuestionForCreationDto</param>
        /// <returns>QuestionDto</returns>
        [HttpPost()]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionForCreationDto model)
        {
            // maps resource to domain model
            var question = _mapper.Map<Question>(model);

            try
            {
                // create question
                var questionCreated = await _questionService.Create(question, model.LessonId);
                // retuns 200ok and the question resource
                return Ok(_mapper.Map<QuestionDto>(questionCreated));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// This method updates an existing Question.
        /// HTTP PUT verb.
        /// </summary>
        /// <param name="questionDto">Integer</param>
        /// <returns>NoContent204</returns>
        [HttpPut("{questionId}")]
        public async Task<IActionResult> UpdateQuestion(QuestionDto questionDto)
        {
            // maps resource to domain model
            var question = _mapper.Map<Question>(questionDto);
            // calls question service to update the question
            _ = await _questionService.UpdateQuestion(question);
            // returns 204 no content
            return NoContent();
        }
    }
}

