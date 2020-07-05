using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface ILessonService
    {
        Task<Lesson> Create(Lesson lesson, int subjectId);
        Task<Lesson> GetLessonById(int lessonId);
        Task<List<Lesson>> GetLessons(int subjectId);
    }
}
