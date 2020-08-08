using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class ResultService : IResultService
    {
        // Variables to use for dependency injection
        private readonly IAskMeRepository _askMeRepository;
        // Constructor
        public ResultService(IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }
        /// <summary>
        /// This method get a list of resulst
        /// for a given exam.
        /// </summary>
        /// <param name="examId">Integer</param>
        /// <returns>List<Result></returns>
        public async Task<List<Result>> GetResults(int examId)
        {
            // calls repository to get a list of results by exmanId
            return await _askMeRepository.GetResults(examId);             
        }
        /// <summary>
        /// This method process an exam.
        /// </summary>
        /// <param name="examResult">List<ExamResult></param>
        /// <returns>Boolean</returns>
        public async Task<bool> ProcessExamResult(List<ExamResult> examResult)
        {
            // gets list of questions and answer for the current examId from the database
            var examId = examResult.First().ExamId;
            var currentQuestions = await _askMeRepository.GetExamQuestions(examId);
            // creates variables to store total success, total failed
            var totalSuccess = 0;
            var totalFailed = 0;
            // iterates exam result sent and compare it if the answers are success
            foreach(var question in examResult)
            {
                var isQuestionSuccess = true;
                var currentQuestion = currentQuestions.Questions.FirstOrDefault(q => q.Id == question.QuestionId);
                foreach(var answer in question.Answers)
                {
                    var currentAnswer = currentQuestion.Answers.FirstOrDefault(a => a.Id == answer.AnswerId);
                    if(answer.AnswerValue != currentAnswer.IsAccepted)
                    {
                        isQuestionSuccess = false;
                    }                  
                }

                _ = isQuestionSuccess ? ++totalSuccess : ++totalFailed;
            }
            // saves the results in the database
            var result = new Result
            {
                ExamId = examId,
                TotalFailed = totalFailed,
                TotalSuccess = totalSuccess
            };

            return await _askMeRepository.AddResults(result);
        }
    }
}
