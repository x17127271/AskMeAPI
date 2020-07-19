using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface ISubjectService
    {
        Task<Subject> Create(Subject subject, int userId);
        Task<Subject> GetSubjectById(int subjectId);
        Task<List<Subject>> GetSubjects(int userId);
        Task<bool> UpdateSubject(Subject subject);
    }
}
