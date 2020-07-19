using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IResultService
    {
        Task<List<Result>> GetResults(int examId);
        Task<bool> ProcessExamResult(List<ExamResult> examResult);
    }
}
