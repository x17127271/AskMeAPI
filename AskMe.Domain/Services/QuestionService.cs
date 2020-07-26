using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IAskMeRepository _askMeRepository;
        public QuestionService(IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }

        public async Task<Question> Create(Question question, int lessonId)
        {
            return await _askMeRepository.AddQuestion(question, lessonId);
        }

        public async Task<List<Question>> GetQuestions(int lessonId)
        {
            return await _askMeRepository.GetQuestions(lessonId);

        }

        public async Task<Question> GetQuestionById(int questionId)
        {
            return await _askMeRepository.GetQuestionById(questionId);
        }

        public async Task<bool> UpdateQuestion(Question question)
        {
            return await _askMeRepository.UpdateQuestion(question);
        }
    }
}
