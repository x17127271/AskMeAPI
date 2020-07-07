using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskMe.Data.Entities
{
    public class AnswerEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsAccepted { get; set; }
        [ForeignKey("QuestionId")]
        public QuestionEntity Question { get; set; }
        public int QuestionId { get; set; }
    }
}
