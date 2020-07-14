using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskMe.Data.Entities
{
    public class ExamsQuestions
    {
        [Key]
        public int Id { get; set; }        
        public int QuestionId { get; set; }
        public int ExamId { get; set; }
        [ForeignKey("ExamId")]
        public ExamEntity ExamEntity { get; set; }
        [ForeignKey("QuestionId")]
        public QuestionEntity QuestionEntity { get; set; }
    }
}
