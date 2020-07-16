namespace AskMe.Domain.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int ExamId { get; set; }       
        public Exam Exam { get; set; }
        public int TotalSuccess { get; set; }
        public int TotalFailed { get; set; }
    }
}
