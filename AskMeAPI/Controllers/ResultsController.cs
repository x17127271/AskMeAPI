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
        // Variables to use for dependency injection
        private readonly IResultService _resultService;
        private readonly IMapper _mapper;
        // constructor
        public ResultsController(IMapper mapper, IResultService resultService)
        {
            _mapper = mapper;
            _resultService = resultService;
        }
        /// <summary>
        /// This method returns a list of Result resources for a given exam.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="examId">Integer</param>
        /// <returns>List<Result>()</returns>
        [HttpGet()]
        public async Task<IActionResult> GetResults(int examId)
        {
            // calls result service to get results by examId
            var results = await _resultService.GetResults(examId);

            // maps list of results model domain to a list of results resource
            //200ok
            return Ok(_mapper.Map<List<ResultDto>>(results));
        }
        /// <summary>
        /// This method returns True if the results of a given Exam
        /// have been processed.
        /// </summary>
        /// <param name="model">List<ExamResultDto>()</param>
        /// <returns>Boolean</returns>
        [HttpPost()]
        public async Task<IActionResult> ProcessExamResult([FromBody] List<ExamResultDto> model)
        {
            // maps resource to domain model
            var examResult = _mapper.Map<List<ExamResult>>(model);

            try
            {
                // calls result service to process exam results
                var isResultProcess = await _resultService.ProcessExamResult(examResult);
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
