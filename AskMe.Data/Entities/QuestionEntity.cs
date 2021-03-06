﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskMe.Data.Entities
{
    public class QuestionEntity
    {
        public QuestionEntity()
        {
            Answers = new List<AnswerEntity>();
            ExamQuestions = new List<ExamsQuestions>();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        [ForeignKey("LessonId")]
        public LessonEntity Lesson { get; set; }
        public int LessonId { get; set; }

        public ICollection<AnswerEntity> Answers { get; set; }
        public ICollection<ExamsQuestions> ExamQuestions { get; set; }

    }
}
