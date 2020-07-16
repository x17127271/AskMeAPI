using System.Collections.Generic;

namespace AskMe.API.Models
{
    public class ExamResultDto
    {
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public List<ExamAnswerResultDto> Answers { get; set; }
    }
}
