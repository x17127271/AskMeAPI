using System;
using System.Collections.Generic;
using System.Text;

namespace AskMe.Data.Entities
{
    public class AnswerEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsAccepted { get; set; }
        public QuestionEntity Question { get; set; }
    }
}
