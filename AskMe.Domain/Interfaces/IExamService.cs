using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IExamService
    {
        Task<bool> AddExamQuestions(Exam exam, List<int> questions);        
        Task<ExamQuestion> AddExamQuestion(int examId, int questionId);
        Task<Exam> GetExamById(int examId);
        Task<List<Exam>> GetExams(int userId);
        Task<ExamQuestions> GetExamQuestions(int examId);
    }
}
