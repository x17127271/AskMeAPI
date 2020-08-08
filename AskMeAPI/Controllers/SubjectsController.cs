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
        // Variables to use for dependency injection
        private readonly IMapper _mapper;
        private readonly ISubjectService _subjectService;
        // constructor
        public SubjectsController(IMapper mapper, ISubjectService subjectService)
        {
            _mapper = mapper;
            _subjectService = subjectService;
        }

        /// <summary>
        /// This method returns an existing Subjects by userId.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="userId">Integer</param>
        /// <returns>List<Subjects>()</returns>
        [HttpGet()]
        public async Task<IActionResult> GetSubjects(int userId)
        {
            // call subject service to get the subjects by userId
            var subject = await _subjectService.GetSubjects(userId);

            // returns the list of subjects 200ok
            return Ok(subject);
        }
        /// <summary>
        /// This method returns an existing Subject resource.
        /// HTTP GET verb.
        /// </summary>
        /// <param name="subjectId">Integer</param>
        /// <returns>SubjectDto</returns>
        [HttpGet("{subjectId}", Name = "GetSubject")]
        public async Task<IActionResult> GetSubject(int subjectId)
        {
            // call subject service to get a Subject by id
            var subject = await _subjectService.GetSubjectById(subjectId);
            //if subject is null
            if (subject == null)
            {
                // returns not found 404
                return NotFound();
            }
            // if subject exists returns 200ok and the subject resource details
            return Ok(_mapper.Map<SubjectDto>(subject));
        }
        /// <summary>
        /// This method creates a new subject.
        /// HTTP POST verb.
        /// </summary>
        /// <param name="model">SubjectForCreationDto</param>
        /// <returns>SubjectDto</returns>
        [HttpPost()]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectForCreationDto model)
        {
            // maps the resource to a domain model
            var subject = _mapper.Map<Subject>(model);

            try
            {
                // calls subject service to create a new subject
                var subjectCreated = await _subjectService.Create(subject, model.UserId);
                // returns 200ok and the subject created
                return Ok(_mapper.Map<SubjectDto>(subjectCreated));
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
        /// <summary>
        /// This method is used to updated an existing subject.
        /// HTTP PUT verb.
        /// </summary>
        /// <param name="subjectDto">SubjectDto</param>
        /// <returns>NoContent204</returns>
        [HttpPut("{subjectId}")]
        public async Task<IActionResult> UpdateSubject(SubjectDto subjectDto)
        {
            // maps the resource to a domain model
            var subject = _mapper.Map<Subject>(subjectDto);
            // call subject service to update an existing subject.
            _ = await _subjectService.UpdateSubject(subject);

            return NoContent();
        }
    }
}
