using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IResultService
    {
        Task<bool> ProcessExamResult(List<ExamResult> examResult);
    }
}
