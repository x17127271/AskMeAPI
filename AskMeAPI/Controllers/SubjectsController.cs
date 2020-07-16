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
    [Route("api/users/{userId}/subjects")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISubjectService _subjectService;

        public SubjectsController(IMapper mapper, ISubjectService subjectService)
        {
            _mapper = mapper;
            _subjectService = subjectService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetSubjects(int userId)
        {
            var subject = await _subjectService.GetSubjects(userId).ConfigureAwait(false);

            
            return Ok(subject);
        }

        [HttpGet("{subjectId}", Name = "GetSubject")]
        public async Task<IActionResult> GetSubject(int subjectId)
        {
            var subject = await _subjectService.GetSubjectById(subjectId).ConfigureAwait(false);

            if (subject == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SubjectDto>(subject));
        }

        [HttpPost()]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectForCreationDto model)
        {
            // map model to entity
            var subject = _mapper.Map<Subject>(model);

            try
            {
                // create subject
                var subjectCreated = await _subjectService.Create(subject, model.UserId).ConfigureAwait(false);
                return Ok(_mapper.Map<SubjectDto>(subjectCreated));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
