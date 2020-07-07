using AskMe.Domain.Models;
using System.Collections.Generic;

namespace AskMe.API.Models
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int LessonId { get; set; }
        //public List<Answer> Answers { get; set; }
        
    }
}
