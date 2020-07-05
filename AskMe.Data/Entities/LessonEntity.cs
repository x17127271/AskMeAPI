using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AskMe.Data.Entities
{
    public class LessonEntity
    {
        public LessonEntity()
        {
            Questions = new List<QuestionEntity>();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [ForeignKey("SubjectId")]
        public SubjectEntity SubjectEntity { get; set; }
        public int SubjectId { get; set; }
        public ICollection<QuestionEntity> Questions { get; set; }
    }
}
