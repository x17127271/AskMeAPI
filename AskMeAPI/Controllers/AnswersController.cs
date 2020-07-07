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
    [Route("api/questions/{questionId}/answers")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAnswerService _answerService;

        public AnswersController(IMapper mapper, IAnswerService answerService)
        {
            _mapper = mapper;
            _answerService = answerService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAnswers(int questionId)
        {
            var answers = await _answerService.GetAnswers(questionId).ConfigureAwait(false);

            return Ok(answers);
        }


        [HttpGet("{answerId}", Name = "GetAnswer")]
        public async Task<IActionResult> GetAnswer(int answerId)
        {
            var answer = await _answerService.GetAnswerById(answerId).ConfigureAwait(false);

            if (answer == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AnswerDto>(answer));
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAnswer([FromBody] AnswerForCreationDto model)
        {
            // map model to entity
            var answer = _mapper.Map<Answer>(model);

            try
            {
                // create answer
                var answerCreated = await _answerService.Create(answer, model.QuestionId).ConfigureAwait(false);
                return Ok(_mapper.Map<AnswerDto>(answerCreated));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
