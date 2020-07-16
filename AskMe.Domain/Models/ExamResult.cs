using System.Collections.Generic;

namespace AskMe.Domain.Models
{
    public class ExamResult
    {
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public List<ExamAnswerResult> Answers { get; set; }
    }
}
