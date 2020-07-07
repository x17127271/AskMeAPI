using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.API.Models
{
    public class QuestionForCreationDto
    {
        public string Title { get; set; }
        public int LessonId { get; set; }
    }
}
