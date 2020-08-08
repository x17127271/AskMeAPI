using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AskMe.Domain.Services
{
    public class LessonService : ILessonService
    {
        // Variables to use for dependency injection
        private readonly IAskMeRepository _askMeRepository;
        // Constructor
        public LessonService(
            IAskMeRepository askMeRepository)
        {
            _askMeRepository = askMeRepository;
        }
        /// <summary>
        /// This method creates a new Lesson
        /// for a given subject.
        /// </summary>
        /// <param name="lesson">Lesson</param>
        /// <param name="subjectId">integer</param>
        /// <returns>Lesson</returns>
        public async Task<Lesson> Create(Lesson lesson, int subjectId)
        {
            // calls repository to create a new lesson
            return await _askMeRepository.AddLesson(lesson, subjectId);
        }
        /// <summary>
        /// This method returns an existing Lesson.
        /// </summary>
        /// <param name="lessonId">integer</param>
        /// <returns>Lesson</returns>
        public async Task<Lesson> GetLessonById(int lessonId)
        {
            // calls repostory to search a lesson by id
            return await _askMeRepository.GetLessonById(lessonId);
        }
        /// <summary>
        /// This method returns a list of lessons
        /// for a given subject.
        /// </summary>
        /// <param name="subjectId">integer</param>
        /// <returns>List<Lesson></returns>
        public async Task<List<Lesson>> GetLessons(int subjectId)
        {
            // calls repository to search lessons by subjectId
            return await _askMeRepository.GetLessons(subjectId);
        }
        /// <summary>
        /// This method updates an existing lesson.
        /// </summary>
        /// <param name="lesson">Lesson</param>
        /// <returns>boolean</returns>
        public async Task<bool> UpdateLesson(Lesson lesson)
        {
            // calls repository to update an existing lesson
            return await _askMeRepository.UpdateLesson(lesson);
        }
    }
}
