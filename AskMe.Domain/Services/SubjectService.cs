using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class SubjectService : ISubjectService
    {
        // Variables to use for dependency injection
        private readonly IAskMeRepository _askMeRepository;
        // Constructor
        public SubjectService(
            IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }
        /// <summary>
        /// This method creates a new Subject
        /// for a given user.
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="userId">Integer</param>
        /// <returns>Subject</returns>
        public async Task<Subject> Create(Subject subject, int userId)
        {            
            // calls repository to create a new subject
            return await _askMeRepository.AddSubject(subject, userId);
        }
        /// <summary>
        /// This method returns an existing Subject.
        /// </summary>
        /// <param name="subjectId">Integer</param>
        /// <returns>Subject</returns>
        public async Task<Subject> GetSubjectById(int subjectId)
        {
            // call repository to get an existing subject by id
            return await _askMeRepository.GetSubjectById(subjectId);
        }
        /// <summary>
        /// This method returns a list of subjects
        /// for a given user.
        /// </summary>
        /// <param name="userId">Integer</param>
        /// <returns>List<subject></returns>
        public async Task<List<Subject>> GetSubjects(int userId)
        {
            // calls repository to search a subject by userId
            return await _askMeRepository.GetSubjects(userId);
        }
        /// <summary>
        /// This method updates an existing Subject.
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <returns>Boolean</returns>
        public async Task<bool> UpdateSubject(Subject subject)
        {
            // calls reository to update an existing subject
            return await _askMeRepository.UpdateSubject(subject);
        }
    }
}
