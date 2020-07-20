using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAskMeRepository _askMeRepository;
        public AnswerService(IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }

        public async Task<Answer> Create(Answer answer, int questionId)
        {
            return await _askMeRepository.AddAnswer(answer, questionId).ConfigureAwait(false);
        }

        public async Task<Answer> GetAnswerById(int answerId)
        {
            return await _askMeRepository.GetAnswerById(answerId).ConfigureAwait(false);
        }

        public async Task<List<Answer>> GetAnswers(int questionId)
        {
            return await _askMeRepository.GetAnswers(questionId).ConfigureAwait(false);

        }

        public async Task<bool> UpdateAnswer(Answer answer)
        {
            return await _askMeRepository.UpdateAnswer(answer).ConfigureAwait(false);
        }
    }
}
