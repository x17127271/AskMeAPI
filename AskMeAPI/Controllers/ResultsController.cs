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
    [Route("api/exams/{examId}/results")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IResultService _resultService;
        private readonly IMapper _mapper;
        public ResultsController(IMapper mapper, IResultService resultService)
        {
            _mapper = mapper;
            _resultService = resultService;
        }


        [HttpPost()]
        public async Task<IActionResult> ProcessExamResult([FromBody] List<ExamResultDto> model)
        {
            // map model to entity
            var examResult = _mapper.Map<List<ExamResult>>(model);

            try
            {
                // process exam results
                var isResultProcess = await _resultService.ProcessExamResult(examResult).ConfigureAwait(false);
                return Ok(isResultProcess);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
