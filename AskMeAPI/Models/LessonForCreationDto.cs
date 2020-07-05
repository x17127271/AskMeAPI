using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.API.Models
{
    public class LessonForCreationDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
    }
}
