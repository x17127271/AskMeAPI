using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System;
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

        public async Task<bool> AddExamAutoQuestions(Exam exam, List<int> subjects, int questionPerSubject)
        {
            throw new NotImplementedException();
        }

        public async Task<ExamQuestion> AddExamQuestion(int examId, int questionId)
        {
            return await _askMeRepository.AddExamQuestion(examId, questionId).ConfigureAwait(false);
        }

        public async Task<bool> AddExamQuestions(Exam exam, List<int> questions)
        {
            if (!questions.Any())
            {
                var questionsForExam = await _askMeRepository.GetRandomQuestionsBySubject(exam.SubjectId, exam.TotalQuestions).ConfigureAwait(false);

                questions = questionsForExam.Select(q => q.Id).ToList();
            }

            return await _askMeRepository.AddExamQuestions(exam, questions).ConfigureAwait(false);
        }

        public async Task<Exam> GetExamById(int examId)
        {
            return await _askMeRepository.GetExamById(examId).ConfigureAwait(false);
        }

        public async Task<List<Exam>> GetExams(int userId)
        {
            return await _askMeRepository.GetExams(userId).ConfigureAwait(false);
        }

        public async Task<ExamQuestions> GetExamQuestions(int examId)
        {
            return await _askMeRepository.GetExamQuestions(examId).ConfigureAwait(false);

        }
    }
}
