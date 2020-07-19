using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class LessonService : ILessonService
    {
        private readonly IAskMeRepository _askMeRepository;
        public LessonService(
            IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }

        public async Task<Lesson> Create(Lesson lesson, int subjectId)
        {
            return await _askMeRepository.AddLesson(lesson, subjectId).ConfigureAwait(false);
        }

        public async Task<Lesson> GetLessonById(int lessonId)
        {
            return await _askMeRepository.GetLessonById(lessonId).ConfigureAwait(false);
        }

        public async Task<List<Lesson>> GetLessons(int subjectId)
        {
            return await _askMeRepository.GetLessons(subjectId).ConfigureAwait(false);
        }

        public async Task<bool> UpdateLesson(Lesson lesson)
        {
            return await _askMeRepository.UpdateLesson(lesson).ConfigureAwait(false);
        }
    }
}
