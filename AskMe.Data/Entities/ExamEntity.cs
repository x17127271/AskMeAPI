using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskMe.Data.Entities
{
    public class ExamEntity
    {
        public ExamEntity()
        {
            ExamQuestions = new List<ExamsQuestions>();
            Results = new List<ResultEntity>();
        }
        [Key]
        public int Id { get; set; }       
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<ExamsQuestions> ExamQuestions { get; set; }
        public ICollection<ResultEntity> Results { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public int UserId { get; set; }

    }
}
