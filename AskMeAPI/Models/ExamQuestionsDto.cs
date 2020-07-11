using AskMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.API.Models
{
    public class ExamQuestionsDto
    {
        public Exam Exam { get; set; }
        public List<Question> Questions { get; set; }
    }
}
