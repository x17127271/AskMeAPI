using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskMe.Data.Entities
{
    public class ResultEntity
    {
        [Key]
        public int Id { get; set; }
        public int ExamId { get; set; }
        [ForeignKey("ExamId")]
        public ExamEntity Exam { get; set; }
        public int TotalSuccess { get; set; }
        public int TotalFailed { get; set; }
    }
}
