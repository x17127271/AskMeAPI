using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class QuestionService : IQuestionService
    {
        // Variables to use for dependency injection
        private readonly IAskMeRepository _askMeRepository;
        // Constructor
        public QuestionService(IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }
        /// <summary>
        /// This method creates a new Question.
        /// </summary>
        /// <param name="question">Question</param>
        /// <param name="lessonId">integer</param>
        /// <returns>Question</returns>
        public async Task<Question> Create(Question question, int lessonId)
        {
            // calls repository to create a new Question
            return await _askMeRepository.AddQuestion(question, lessonId);
        }
        /// <summary>
        /// This method returns a list of Quesitons
        /// for a given Lesson.
        /// </summary>
        /// <param name="lessonId">integer</param>
        /// <returns>List<Question></returns>
        public async Task<List<Question>> GetQuestions(int lessonId)
        {
            // calls repository to search questions by lesson id
            return await _askMeRepository.GetQuestions(lessonId);

        }
        /// <summary>
        /// This method returns an existing Question.
        /// </summary>
        /// <param name="questionId">integer</param>
        /// <returns>Quesiton</returns>
        public async Task<Question> GetQuestionById(int questionId)
        {
            // calls repository to search a question by id
            return await _askMeRepository.GetQuestionById(questionId);
        }
        /// <summary>
        /// This method updates an existing Question.
        /// </summary>
        /// <param name="question">Question</param>
        /// <returns>Boolean</returns>
        public async Task<bool> UpdateQuestion(Question question)
        {
            // calls repository to upate an existing question
            return await _askMeRepository.UpdateQuestion(question);
        }
    }
}
