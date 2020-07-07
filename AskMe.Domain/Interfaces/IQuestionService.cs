using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> Create(Question question, int lessonId);
        Task<Question> GetQuestionById(int questionId);
        Task<List<Question>> GetQuestions(int lessonId);
    }
}
