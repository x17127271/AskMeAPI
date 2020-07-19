using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class ResultService : IResultService
    {
        private readonly IAskMeRepository _askMeRepository;

        public ResultService(IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }

        public async Task<List<Result>> GetResults(int examId)
        {
            return await _askMeRepository.GetResults(examId).ConfigureAwait(false);             
        }

        public async Task<bool> ProcessExamResult(List<ExamResult> examResult)
        {
            // get list of questions and answer for the current examId from the database
            var examId = examResult.First().ExamId;
            var currentQuestions = await _askMeRepository.GetExamQuestions(examId).ConfigureAwait(false);
            // create variables to store total success, total failed
            var totalSuccess = 0;
            var totalFailed = 0;
            // iterate exam result sent and compare it if the answers are success
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
            // save the results in the database
            var result = new Result
            {
                ExamId = examId,
                TotalFailed = totalFailed,
                TotalSuccess = totalSuccess
            };

            return await _askMeRepository.AddResults(result).ConfigureAwait(false);
        }
    }
}
