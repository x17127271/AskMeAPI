using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IAnswerService
    {
        Task<Answer> Create(Answer answer, int questionId);
        Task<Answer> GetAnswerById(int answerId);
        Task<List<Answer>> GetAnswers(int questionId);
    }
}
