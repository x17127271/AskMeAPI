using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IAskMeRepository _askMeRepository;
        public SubjectService(
            IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }

        public async Task<Subject> Create(Subject subject, int userId)
        {
            return await _askMeRepository.AddSubject(subject, userId).ConfigureAwait(false);
        }

        public async Task<Subject> GetSubjectById(int subjectId)
        {
            return await _askMeRepository.GetSubjectById(subjectId).ConfigureAwait(false);
        }

        public async Task<List<Subject>> GetSubjects(int userId)
        {
            return await _askMeRepository.GetSubjects(userId).ConfigureAwait(false);
        }

        public async Task<bool> UpdateSubject(Subject subject)
        {
            return await _askMeRepository.UpdateSubject(subject).ConfigureAwait(false);
        }
    }
}
