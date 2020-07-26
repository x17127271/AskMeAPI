using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IAskMeRepository
    {
        Task<bool> Save();
        ValueTask<User> AddUser(User user);
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUserName(string userName);
        Task<List<User>> GetUsers();
        Task<Subject> AddSubject(Subject subject, int userId);
        Task<Subject> GetSubjectById(int subjectId);
        Task<List<Subject>> GetSubjects(int userId);
        Task<Lesson> AddLesson(Lesson lesson, int subjectId);
        Task<Lesson> GetLessonById(int lessonId);
        Task<List<Lesson>> GetLessons(int subjectId);
        Task<List<Question>> GetQuestions(int lessonId);
        Task<Question> GetQuestionById(int questionId);
        Task<Question> AddQuestion(Question question, int lessonId);
        Task<List<Answer>> GetAnswers(int questionId);
        Task<Answer> GetAnswerById(int answerId);
        Task<Answer> AddAnswer(Answer answer, int questionId);
        Task<Exam> GetExamById(int examId);
        Task<List<Exam>> GetExams(int userId);
        Task<ExamQuestion> AddExamQuestion(int examId, int questionId);
        Task<bool> AddExamQuestions(Exam exam, List<int> questions);
        Task<ExamQuestions> GetExamQuestions(int examId);
        Task<List<Question>> GetRandomQuestionsBySubject(int subjectId, int totalQuestions);
        Task<bool> AddResults(Result result);
        Task<List<Result>> GetResults(int examId);
        Task<bool> UpdateSubject(Subject subject);
        Task<bool> UpdateLesson(Lesson lesson);
        Task<bool> UpdateQuestion(Question question);
        Task<bool> UpdateAnswer(Answer answer);
    }
}
