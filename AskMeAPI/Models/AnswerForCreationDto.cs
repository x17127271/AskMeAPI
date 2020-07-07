
namespace AskMe.API.Models
{
    public class AnswerForCreationDto
    {
        public string Title { get; set; }
        public bool IsAccepted { get; set; }
        public int QuestionId { get; set; }
    }
}
