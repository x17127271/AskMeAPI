
namespace AskMe.API.Models
{
    public class AnswerDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsAccepted { get; set; }
        public int QuestionId { get; set; }
    }
}
