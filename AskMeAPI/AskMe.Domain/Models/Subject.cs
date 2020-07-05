using System;
using System.Collections.Generic;
using System.Text;

namespace AskMe.Domain.Models
{
    public class Subject
    {
        public Subject()
        {
            Lessons = new List<Lesson>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
