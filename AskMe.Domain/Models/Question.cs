﻿using System.Collections.Generic;

namespace AskMe.Domain.Models
{
    public class Question
    {
        public Question()
        {
            Answers = new List<Answer>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public Lesson Lesson { get; set; }
        public int LessonId { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
