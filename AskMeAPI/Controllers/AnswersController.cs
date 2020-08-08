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
    [Route("api/questions/{questionId}/answers")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        // Variables to use for dependency injection
        private readonly IMapper _mapper;
        private readonly IAnswerService _answerService;
        // constructor
        public AnswersController(IMapper mapper, IAnswerService answerService)
        {
            _mapper = mapper;
            _answerService = answerService;
        }
        /// <summary>
        /// This method returns a list of Answers
        /// for a given Question.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="questionId">Integer</param>
        /// <returns>List<AnswerDto></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAnswers(int questionId)
        {
            // calls answer service to search list of answers by questionId
            var answers = await _answerService.GetAnswers(questionId);
            // returns 200ok
            return Ok(_mapper.Map<List<AnswerDto>>(answers));
        }

        /// <summary>
        /// This method returns an existing Answer resource.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="answerId">Integer</param>
        /// <returns>AnswerDto</returns>
        [HttpGet("{answerId}", Name = "GetAnswer")]
        public async Task<IActionResult> GetAnswer(int answerId)
        {
            // calls answer service to search answer by id
            var answer = await _answerService.GetAnswerById(answerId);
            // if answer is null
            if (answer == null)
            {
                // retuns 404 not found
                return NotFound();
            }
            // maps domain model to resource
            //returns 200ok and the resource
            return Ok(_mapper.Map<AnswerDto>(answer));
        }
        /// <summary>
        /// This method creates a new Answer.
        /// HTTP POST verb.
        /// </summary>
        /// <param name="model">AnswerForCreationDto</param>
        /// <returns>AnswerDto</returns>
        [HttpPost()]
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerForCreationDto model)
        {
            // maps resource to a domain model
            var answer = _mapper.Map<Answer>(model);

            try
            {
                // create answer
                var answerCreated = await _answerService.Create(answer, model.QuestionId);
                return Ok(_mapper.Map<AnswerDto>(answerCreated));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// This method updates ans existing Answer.
        /// HTTP PUT verb.
        /// </summary>
        /// <param name="answerDto">AnswerDto</param>
        /// <returns>NoContent</returns>
        [HttpPut("{answerId}")]
        public async Task<IActionResult> UpdateAnswer(AnswerDto answerDto)
        {
            // maps resource to a domain model
            var answer = _mapper.Map<Answer>(answerDto);
            // calls answer service to create a new answer
            _ = await _answerService.UpdateAnswer(answer);
            // returns 204 no content
            return NoContent();
        }
    }
}
