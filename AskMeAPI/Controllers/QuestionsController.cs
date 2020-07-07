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
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;

        public QuestionsController(IMapper mapper, IQuestionService questionService)
        {
            _mapper = mapper;
            _questionService = questionService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetQuestions(int lessonId)
        {
            var questions = await _questionService.GetQuestions(lessonId).ConfigureAwait(false);


            return Ok(_mapper.Map<List<QuestionDto>>(questions));
        }


        [HttpGet("{questionId}", Name = "GetQuestion")]
        public async Task<IActionResult> GetQuestion(int questionId)
        {
            var question = await _questionService.GetQuestionById(questionId).ConfigureAwait(false);

            if (question == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<QuestionDto>(question));
        }

        [HttpPost()]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionForCreationDto model)
        {
            // map model to entity
            var question = _mapper.Map<Question>(model);

            try
            {
                // create question
                var questionCreated = await _questionService.Create(question, model.LessonId).ConfigureAwait(false);
                return Ok(_mapper.Map<QuestionDto>(questionCreated));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

