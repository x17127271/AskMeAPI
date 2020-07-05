using System;
using System.Collections.Generic;
using System.Text;

namespace AskMe.Domain.Models
{
    public class Lesson
    {
        public Lesson()
        {
            Questions = new List<Question>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Subject Subject { get; set; }
        public List<Question> Questions { get; set; }
    }
}
