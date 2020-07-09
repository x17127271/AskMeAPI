namespace AskMe.Data.Entities
{
    public class ExamsQuestions
    {
        public int QuestionId { get; set; }
        public int ExamId { get; set; }
        public ExamEntity ExamEntity { get; set; }
        public QuestionEntity questionEntity { get; set; }
    }
}
