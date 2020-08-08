using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class AnswerService : IAnswerService
    {
        // Variables to use for dependency injection
        private readonly IAskMeRepository _askMeRepository;
        // Constructor
        public AnswerService(IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }
        /// <summary>
        /// This method creates a new answer
        /// for a given question.
        /// </summary>
        /// <param name="answer">Answer</param>
        /// <param name="questionId">integer</param>
        /// <returns>Answer</returns>
        public async Task<Answer> Create(Answer answer, int questionId)
        {
            // calls repository to create a new answer for a given question
            return await _askMeRepository.AddAnswer(answer, questionId);
        }
        /// <summary>
        /// This method returns an existing answer.
        /// </summary>
        /// <param name="answerId">integer</param>
        /// <returns>Answer</returns>
        public async Task<Answer> GetAnswerById(int answerId)
        {
            // calls repository to search an existing answer by id
            return await _askMeRepository.GetAnswerById(answerId);
        }
        /// <summary>
        /// This method returns a list of answers
        /// for a given question.
        /// </summary>
        /// <param name="questionId">integer</param>
        /// <returns>List<Answer></returns>
        public async Task<List<Answer>> GetAnswers(int questionId)
        {
            // calss repository to search answers by question id
            return await _askMeRepository.GetAnswers(questionId);

        }
        /// <summary>
        /// This method updates an existing answer.
        /// </summary>
        /// <param name="answer">Answer</param>
        /// <returns>boolean</returns>
        public async Task<bool> UpdateAnswer(Answer answer)
        {
            // calls repository to update an existing answer
            return await _askMeRepository.UpdateAnswer(answer);
        }
    }
}
