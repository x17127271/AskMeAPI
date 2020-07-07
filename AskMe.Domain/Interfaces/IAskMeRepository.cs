using AskMe.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Domain.Interfaces
{
    public interface IAskMeRepository
    {
        bool Save();
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
    }
}
