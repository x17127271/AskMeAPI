using System.Collections.Generic;

namespace AskMe.API.Models
{
    public class ExamQuestionsForCreationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public List<int> questions { get; set; }
    }
}
