using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AskMe.Data.Entities
{
    public class SubjectEntity
    {
        public SubjectEntity()
        {
            Lessons = new List<LessonEntity>();
        }

        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<LessonEntity> Lessons { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public int UserId { get; set; }
    }
}
