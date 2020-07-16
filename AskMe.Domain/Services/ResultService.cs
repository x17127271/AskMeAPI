using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
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

        public async Task<bool> ProcessExamResult(List<ExamResult> examResult)
        {

            return true;
        }
    }
}
