using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
            return await _askMeRepository.AddQuestion(question, lessonId).ConfigureAwait(false);
        }

        public async Task<List<Question>> GetQuestions(int lessonId)
        {
            return await _askMeRepository.GetQuestions(lessonId).ConfigureAwait(false);

        }

        public async Task<Question> GetQuestionById(int questionId)
        {
            return await _askMeRepository.GetQuestionById(questionId).ConfigureAwait(false);
        }
    }
}
