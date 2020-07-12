namespace AskMe.Domain.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int SubjectId { get; set; }
        public int TotalQuestions { get; set; }
    }
}
