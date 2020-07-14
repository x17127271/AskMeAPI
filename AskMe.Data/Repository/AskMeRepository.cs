using AskMe.Data.DbContexts;
using AskMe.Data.Entities;
using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMe.Data.Repository
{
    public class AskMeRepository : IAskMeRepository, IDisposable
    {
        private readonly AskMeDbContext _context;
        private readonly IMapper _mapper;

        public AskMeRepository(
            AskMeDbContext context,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool Save()
        {
            return (_context.SaveChanges() > 0);
        }

        public async Task<User> GetUserById(int userId)
        {            
            var entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == userId).ConfigureAwait(false);
            return _mapper.Map<User>(entity);
        }

        public async Task<List<User>> GetUsers()
        {
            var entities = await _context.Users.AsNoTracking().ToListAsync().ConfigureAwait(false);
            return _mapper.Map<List<User>>(entities);
        }

        public async ValueTask<User> AddUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var entity = _mapper.Map<UserEntity>(user);

            var entityCreated = await _context.Users.AddAsync(entity).ConfigureAwait(false);
            Save();
            return _mapper.Map<User>(entityCreated.Entity);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var entity = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == userName).ConfigureAwait(false);

            return _mapper.Map<User>(entity);
        }

        public bool UserExists(int userId) => _context.Users.Find(userId) != null;

        public bool SubjectExists(int subjectId) => _context.Subjects.Find(subjectId) != null;

        public async Task<Subject> AddSubject(Subject subject, int userId)
        {
            if(!UserExists(userId))
            {
                throw new ArgumentNullException(nameof(User));
            }
            
            subject.UserId = userId;

            var entity = _mapper.Map<SubjectEntity>(subject);

            var entityCreated = await _context.Subjects.AddAsync(entity).ConfigureAwait(false);
            Save();
            return _mapper.Map<Subject>(entityCreated.Entity);
        }

        public async Task<Subject> GetSubjectById(int subjectId)
        {
            var subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(subject => subject.Id == subjectId).ConfigureAwait(false);
            if(subject == null)
            {
                throw new ArgumentNullException(nameof(Subject));
            }

            return _mapper.Map<Subject>(subject);
        }

        public async Task<List<Subject>> GetSubjects(int userId)
        {
            if (!UserExists(userId))
            {
                throw new ArgumentNullException(nameof(User));
            }

            var subjects = await _context.Subjects.AsNoTracking()
                .Where(subject => subject.User.Id == userId).ToListAsync().ConfigureAwait(false);
            
            return _mapper.Map<List<Subject>>(subjects);
        }

        public bool LessonExists(int lessonId) => _context.Lessons.Find(lessonId) != null;

        public async Task<Lesson> AddLesson(Lesson lesson, int subjectId)
        {
            if (!SubjectExists(subjectId))
            {
                throw new ArgumentNullException(nameof(Subject));
            }

            lesson.SubjectId = subjectId;

            var entity = _mapper.Map<LessonEntity>(lesson);

            var entityCreated = await _context.Lessons.AddAsync(entity).ConfigureAwait(false);
            Save();
            return _mapper.Map<Lesson>(entityCreated.Entity);
        }

        public async Task<Lesson> GetLessonById(int lessonId)
        {
            var lesson = await _context.Lessons.AsNoTracking()
                .FirstOrDefaultAsync(lesson => lesson.Id == lessonId).ConfigureAwait(false);

            if (lesson == null)
            {
                throw new ArgumentNullException(nameof(Lesson));
            }

            return _mapper.Map<Lesson>(lesson);
        }

        public async Task<List<Lesson>> GetLessons(int subjectId)
        {
            if (!SubjectExists(subjectId))
            {
                throw new ArgumentNullException(nameof(Subject));
            }

            var lessons = await _context.Lessons.AsNoTracking()
                .Where(lesson => lesson.SubjectEntity.Id == subjectId).ToListAsync().ConfigureAwait(false);

            return _mapper.Map<List<Lesson>>(lessons);
        }

        public bool QuestionExists(int questionId) => _context.Questions.Find(questionId) != null;

        public async Task<Question> AddQuestion(Question question, int lessonId)
        {
            if (!LessonExists(lessonId))
            {
                throw new ArgumentNullException(nameof(Lesson));
            }

            question.LessonId = lessonId;

            var entity = _mapper.Map<QuestionEntity>(question);

            var entityCreated = await _context.Questions.AddAsync(entity).ConfigureAwait(false);
            Save();
            return _mapper.Map<Question>(entityCreated.Entity);
        }

        public async Task<Question> GetQuestionById(int questionId)
        {
            var question = await _context.Questions.AsNoTracking()
                .FirstOrDefaultAsync(question => question.Id == questionId).ConfigureAwait(false);
            if (question == null)
            {
                throw new ArgumentNullException(nameof(Question));
            }

            return _mapper.Map<Question>(question);
        }

        public async Task<List<Question>> GetQuestions(int lessonId)
        {
            if (!LessonExists(lessonId))
            {
                throw new ArgumentNullException(nameof(Lesson));
            }

            var questions = await _context.Questions.AsNoTracking()
                .Where(question => question.Lesson.Id == lessonId).ToListAsync().ConfigureAwait(false);

            return _mapper.Map<List<Question>>(questions);
        }

        public async Task<List<Question>> GetRandomQuestionsBySubject(int subjectId, int totalQuestions)
        {
            var lessons = await _context.Lessons.AsNoTracking()
                .Where(lesson => lesson.SubjectId == subjectId).ToListAsync();

            var questionsPerLesson = totalQuestions / lessons.Count;
            var questions = new List<QuestionEntity>();
            foreach (var lesson in lessons)
            {
                var totalQuestionsPerLesson = _context.Questions.Count(q => q.LessonId == lesson.Id);
                if (questionsPerLesson > totalQuestionsPerLesson)
                {
                    questionsPerLesson = totalQuestionsPerLesson;
                }

                var randomQuestions = await _context.Questions.AsNoTracking()
                    .Where(q => q.LessonId == lesson.Id).OrderBy(q => Guid.NewGuid()).Take(questionsPerLesson).ToListAsync();
                
                questions.AddRange(randomQuestions);
            }           

            return _mapper.Map<List<Question>>(questions);
        }

        public async Task<Answer> AddAnswer(Answer answer, int questionId)
        {
            if (!QuestionExists(questionId))
            {
                throw new ArgumentNullException(nameof(Question));
            }

            answer.QuestionId = questionId;

            var entity = _mapper.Map<AnswerEntity>(answer);

            var entityCreated = await _context.Answers.AddAsync(entity).ConfigureAwait(false);
            Save();
            return _mapper.Map<Answer>(entityCreated.Entity);
        }

        public async Task<Answer> GetAnswerById(int answerId)
        {
            var answer = await _context.Answers.AsNoTracking()
                .FirstOrDefaultAsync(answer => answer.Id == answerId).ConfigureAwait(false);

            if (answer == null)
            {
                throw new ArgumentNullException(nameof(Answer));
            }

            return _mapper.Map<Answer>(answer);
        }       

        public async Task<List<Answer>> GetAnswers(int questionId)
        {
            if (!QuestionExists(questionId))
            {
                throw new ArgumentNullException(nameof(Question));
            }

            var answers = await _context.Answers.AsNoTracking()
                .Where(answer => answer.QuestionId == questionId).ToListAsync().ConfigureAwait(false);

            return _mapper.Map<List<Answer>>(answers);
        }

        public async Task<Exam> GetExamById(int examId)
        {
            var exam = await _context.Exams.AsNoTracking()
                .FirstOrDefaultAsync(exam => exam.Id == examId).ConfigureAwait(false);

            if (exam == null)
            {
                throw new ArgumentNullException(nameof(Exam));
            }

            return _mapper.Map<Exam>(exam);
        }

        public async Task<List<Exam>> GetExams(int userId)
        {
            if (!UserExists(userId))
            {
                throw new ArgumentNullException(nameof(User));
            }

            var exams = await _context.Exams.AsNoTracking()
                .Where(exam => exam.UserId == userId).ToListAsync().ConfigureAwait(false);

            return _mapper.Map<List<Exam>>(exams);
        }

        // to add an exisiting question to an existing exam
        public async Task<ExamQuestion> AddExamQuestion(int examId, int questionId)
        {            
            var examQuestionEntity = new ExamsQuestions
            {
                ExamId = examId,
                QuestionId = questionId
            };

            var examQuestionCreated =
                await _context.AddAsync(examQuestionEntity).ConfigureAwait(false);
            Save();
            return _mapper.Map<ExamQuestion>(examQuestionCreated.Entity);
        }

        // to add questions to a new exam
        public async Task<bool> AddExamQuestions(Exam exam,List<int> questions)
        {
            //create exam
            var examEntity = _mapper.Map<ExamEntity>(exam);
            
            var examCreated = await _context.Exams.AddAsync(examEntity).ConfigureAwait(false);
            Save();
            //add questions to the exam
            var examQuestionsEntities = new List<ExamsQuestions>();
            foreach (var question in questions)
            {
                var examQuestion = new ExamsQuestions
                {
                    ExamId = examCreated.Entity.Id,
                    QuestionId = question
                };

                examQuestionsEntities.Add(examQuestion);
            }

            await _context.AddRangeAsync(examQuestionsEntities).ConfigureAwait(false);
            return Save();
        }

        public async Task<ExamQuestions> GetExamQuestions(int examId)
        {
            var examQuestion = await _context.Exams.AsNoTracking().Where(e => e.Id == examId)
                .Select(e => new
                {
                    Exam = e,
                    Questions = e.ExamQuestions.Select(eq => eq.QuestionEntity)
                }).FirstOrDefaultAsync().ConfigureAwait(false);

            foreach (var question in examQuestion.Questions)
            {
                question.Answers = await _context.Answers.AsNoTracking().Where(a => a.QuestionId == question.Id).ToListAsync();
            };

            return new ExamQuestions
            {
                Exam = _mapper.Map<Exam>(examQuestion.Exam),
                Questions = _mapper.Map<List<Question>>(examQuestion.Questions)
            };
        }
    }
}
