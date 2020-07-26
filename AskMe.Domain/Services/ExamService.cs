using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class ExamService : IExamService
    {
        private readonly IAskMeRepository _askMeRepository;

        public ExamService(IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }        

        public async Task<ExamQuestion> AddExamQuestion(int examId, int questionId)
        {
            return await _askMeRepository.AddExamQuestion(examId, questionId);
        }

        public async Task<bool> AddExamQuestions(Exam exam, List<int> questions)
        {
            if (questions == null || !questions.Any())
            {
                var questionsForExam = await _askMeRepository.GetRandomQuestionsBySubject(exam.SubjectId, exam.TotalQuestions);

                questions = questionsForExam.Select(q => q.Id).ToList();
            }

            return await _askMeRepository.AddExamQuestions(exam, questions);
        }

        public async Task<Exam> GetExamById(int examId)
        {
            return await _askMeRepository.GetExamById(examId);
        }

        public async Task<List<Exam>> GetExams(int userId)
        {
            return await _askMeRepository.GetExams(userId);
        }

        public async Task<ExamQuestions> GetExamQuestions(int examId)
        {
            return await _askMeRepository.GetExamQuestions(examId);

        }
    }
}
