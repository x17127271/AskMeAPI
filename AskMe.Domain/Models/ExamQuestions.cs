using System;
using System.Collections.Generic;
using System.Text;

namespace AskMe.Domain.Models
{
    public class ExamQuestions
    {
        public Exam Exam { get; set; }
        public List<Question> Questions { get; set; }
    }
}
