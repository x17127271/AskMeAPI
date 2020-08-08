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
        // Variables to use for dependency injection
        private AskMeDbContext _context;
        private readonly IMapper _mapper;
        // Constructor
        public AskMeRepository(
            AskMeDbContext context,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper;
        }
        /// <summary>
        /// This method dispose the connection to the database
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // to free memory using garbage collector
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// This method checks if the database context exist
        /// before dispose.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
        /// <summary>
        /// This method is used to save changes on the database
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Save()
        {
            return  (await _context.SaveChangesAsync()  > 0);
        }
        /// <summary>
        /// This method returns an existing user from the database.
        /// </summary>
        /// <param name="userId">integer</param>
        /// <returns>User</returns>
        public async Task<User> GetUserById(int userId)
        {            
            var entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == userId);
            return _mapper.Map<User>(entity);
        }
        /// <summary>
        /// This method returns a list of existing users
        /// from the database.
        /// </summary>
        /// <returns>List<User>()</returns>
        public async Task<List<User>> GetUsers()
        {
            var entities = await _context.Users.AsNoTracking().ToListAsync();
            return _mapper.Map<List<User>>(entities);
        }
        /// <summary>
        /// This method creates a new user on the database.
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User</returns>
        public async ValueTask<User> AddUser(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            // maps domain model to entity
            var entity = _mapper.Map<UserEntity>(user);

            var entityCreated = await _context.Users.AddAsync(entity);
            await Save();
            return _mapper.Map<User>(entityCreated.Entity);
        }
        /// <summary>
        /// This method search an existing user by name.
        /// </summary>
        /// <param name="userName">string</param>
        /// <returns>User</returns>
        public async Task<User> GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var entity = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == userName);

            return _mapper.Map<User>(entity);
        }
        /// <summary>
        /// This method checks if the user already exists on the database.
        /// </summary>
        /// <param name="userId">integer</param>
        /// <returns>boolean</returns>
        public async Task<bool> UserExists(int userId) => await _context.Users.FindAsync(userId)  != null;
        /// <summary>
        /// This method checks if the subject already exists on the database.
        /// </summary>
        /// <param name="subjectId">integer</param>
        /// <returns>boolean</returns>
        public async Task<bool> SubjectExists(int subjectId) => await _context.Subjects.FindAsync(subjectId)  != null;
        /// <summary>
        /// This method creates a new subject on the database
        /// for a given user.
        /// </summary>
        /// <param name="subject">subject</param>
        /// <param name="userId">integer</param>
        /// <returns>subject</returns>
        public async Task<Subject> AddSubject(Subject subject, int userId)
        {
            if(await UserExists(userId)  == false)
            {
                throw new ArgumentNullException(nameof(User));
            }
            
            subject.UserId = userId;
            // maps domain model to entity
            var entity = _mapper.Map<SubjectEntity>(subject);

            var entityCreated = await _context.Subjects.AddAsync(entity);
            await Save();
            // maps entity to domain model
            return _mapper.Map<Subject>(entityCreated.Entity);
        }
        /// <summary>
        /// This method updates a given subject
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <returns>boolean</returns>
        public async Task<bool> UpdateSubject(Subject subject)
        {
            var subjectForUpdate = await _context.Subjects
                .FirstOrDefaultAsync(s => s.Id == subject.Id && s.UserId == subject.UserId);
            
            if(subjectForUpdate == null)
            {
                throw new ArgumentNullException(nameof(Subject));
            }

            _mapper.Map(subject, subjectForUpdate);
            return await Save();
        }
        /// <summary>
        /// This method search an existing subject by id on the database.
        /// </summary>
        /// <param name="subjectId">integer</param>
        /// <returns>subject</returns>
        public async Task<Subject> GetSubjectById(int subjectId)
        {
            var subject = await _context.Subjects.AsNoTracking().FirstOrDefaultAsync(subject => subject.Id == subjectId);
            if(subject == null)
            {
                throw new ArgumentNullException(nameof(Subject));
            }
            // maps entity to domain model
            return _mapper.Map<Subject>(subject);
        }
        /// <summary>
        /// This method search existing subjects
        /// for a given user.
        /// </summary>
        /// <param name="userId">integer</param>
        /// <returns>List<Subject></returns>
        public async Task<List<Subject>> GetSubjects(int userId)
        {
            if (await UserExists(userId)  == false)
            {
                throw new ArgumentNullException(nameof(User));
            }

            var subjects = await _context.Subjects.AsNoTracking()
                .Where(subject => subject.User.Id == userId).ToListAsync();
            // maps entity to domain model
            return _mapper.Map<List<Subject>>(subjects);
        }
        /// <summary>
        /// This method checks if the lesson already exists on the database by id.
        /// </summary>
        /// <param name="lessonId">integer</param>
        /// <returns>boolean</returns>
        public async Task<bool> LessonExists(int lessonId) => await _context.Lessons.FindAsync(lessonId)  != null;
        /// <summary>
        /// This method creates a new lesson on the database
        /// for a given subject.
        /// </summary>
        /// <param name="lesson">lesson</param>
        /// <param name="subjectId">integer</param>
        /// <returns>lesson</returns>
        public async Task<Lesson> AddLesson(Lesson lesson, int subjectId)
        {
            if (await SubjectExists(subjectId)  == false)
            {
                throw new ArgumentNullException(nameof(Subject));
            }

            lesson.SubjectId = subjectId;
            
            var entity = _mapper.Map<LessonEntity>(lesson);

            var entityCreated = await _context.Lessons.AddAsync(entity);
            await Save();
            return _mapper.Map<Lesson>(entityCreated.Entity);
        }
        /// <summary>
        /// This method search a lesson by id on the database.
        /// </summary>
        /// <param name="lessonId">integer</param>
        /// <returns>lesson</returns>
        public async Task<Lesson> GetLessonById(int lessonId)
        {
            var lesson = await _context.Lessons.AsNoTracking()
                .FirstOrDefaultAsync(lesson => lesson.Id == lessonId);

            if (lesson == null)
            {
                throw new ArgumentNullException(nameof(Lesson));
            }

            return _mapper.Map<Lesson>(lesson);
        }
        /// <summary>
        /// This method search lessons on the database
        /// for a given subject.
        /// </summary>
        /// <param name="subjectId">integer</param>
        /// <returns>List<Lesson>()</returns>
        public async Task<List<Lesson>> GetLessons(int subjectId)
        {
            if (await SubjectExists(subjectId)  == false)
            {
                throw new ArgumentNullException(nameof(Subject));
            }

            var lessons = await _context.Lessons.AsNoTracking()
                .Where(lesson => lesson.SubjectEntity.Id == subjectId).ToListAsync();

            return _mapper.Map<List<Lesson>>(lessons);
        }
        /// <summary>
        /// This method checks if a question already exists on the database by id.
        /// </summary>
        /// <param name="questionId">integer</param>
        /// <returns>boolean</returns>
        public async Task<bool> QuestionExists(int questionId) => await _context.Questions.FindAsync(questionId)  != null;
        /// <summary>
        /// This method creates a new question on the database
        /// for a given lesson.
        /// </summary>
        /// <param name="question">question</param>
        /// <param name="lessonId">integer</param>
        /// <returns>question</returns>
        public async Task<Question> AddQuestion(Question question, int lessonId)
        {
            if (await LessonExists(lessonId)  == false)
            {
                throw new ArgumentNullException(nameof(Lesson));
            }

            question.LessonId = lessonId;

            var entity = _mapper.Map<QuestionEntity>(question);

            var entityCreated = await _context.Questions.AddAsync(entity);
            await Save();
            return _mapper.Map<Question>(entityCreated.Entity);
        }
        /// <summary>
        /// This method creates an exisitng lesson on the database.
        /// </summary>
        /// <param name="lesson">lesson</param>
        /// <returns>boolean</returns>
        public async Task<bool> UpdateLesson(Lesson lesson)
        {
            var lessonForUpdate = await _context.Lessons
               .FirstOrDefaultAsync(s => s.Id == lesson.Id && s.SubjectId == lesson.SubjectId);

            if (lessonForUpdate == null)
            {
                throw new ArgumentNullException(nameof(Subject));
            }

            _mapper.Map(lesson, lessonForUpdate);
            return await Save();
        }
        /// <summary>
        /// This method search a question by id on the database.
        /// </summary>
        /// <param name="questionId">integer</param>
        /// <returns>question</returns>
        public async Task<Question> GetQuestionById(int questionId)
        {
            var question = await _context.Questions.AsNoTracking()
                .FirstOrDefaultAsync(question => question.Id == questionId);
            if (question == null)
            {
                throw new ArgumentNullException(nameof(Question));
            }

            return _mapper.Map<Question>(question);
        }
        /// <summary>
        /// This method search on the database questions
        /// for a given lesson.
        /// </summary>
        /// <param name="lessonId">integer</param>
        /// <returns>List<Question>()</returns>
        public async Task<List<Question>> GetQuestions(int lessonId)
        {
            if (await LessonExists(lessonId)  == false)
            {
                throw new ArgumentNullException(nameof(Lesson));
            }

            var questions = await _context.Questions.AsNoTracking()
                .Where(question => question.Lesson.Id == lessonId).ToListAsync();

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

        public async Task<bool> UpdateQuestion(Question question)
        {
            var questionForUpdate = await _context.Questions
               .FirstOrDefaultAsync(q => q.Id == question.Id && q.LessonId == question.LessonId);

            if (questionForUpdate == null)
            {
                throw new ArgumentNullException(nameof(Question));
            }

            _mapper.Map(question, questionForUpdate);
            return await Save();
        }

        public async Task<Answer> AddAnswer(Answer answer, int questionId)
        {
            if (await QuestionExists(questionId)  == false)
            {
                throw new ArgumentNullException(nameof(Question));
            }

            answer.QuestionId = questionId;

            var entity = _mapper.Map<AnswerEntity>(answer);

            var entityCreated = await _context.Answers.AddAsync(entity);
            await Save();
            return _mapper.Map<Answer>(entityCreated.Entity);
        }

        public async Task<bool> UpdateAnswer(Answer answer)
        {
            var answerForUpdate = await _context.Answers
               .FirstOrDefaultAsync(a => a.Id == answer.Id && a.QuestionId == answer.QuestionId);

            if (answerForUpdate == null)
            {
                throw new ArgumentNullException(nameof(Answer));
            }

            _mapper.Map(answer, answerForUpdate);
            return await Save();
        }

        public async Task<Answer> GetAnswerById(int answerId)
        {
            var answer = await _context.Answers.AsNoTracking()
                .FirstOrDefaultAsync(answer => answer.Id == answerId);

            if (answer == null)
            {
                throw new ArgumentNullException(nameof(Answer));
            }

            return _mapper.Map<Answer>(answer);
        }
        /// <summary>
        /// This method search an existing answer on the database
        /// for a given question.
        /// </summary>
        /// <param name="questionId">integer</param>
        /// <returns>List<Answer>()</returns>
        public async Task<List<Answer>> GetAnswers(int questionId)
        {
            if (await QuestionExists(questionId)  == false)
            {
                throw new ArgumentNullException(nameof(Question));
            }

            var answers = await _context.Answers.AsNoTracking()
                .Where(answer => answer.QuestionId == questionId).ToListAsync();

            return _mapper.Map<List<Answer>>(answers);
        }
        /// <summary>
        /// This method search an existing exam on the database by id.
        /// </summary>
        /// <param name="examId">integer</param>
        /// <returns>exam</returns>
        public async Task<Exam> GetExamById(int examId)
        {
            var exam = await _context.Exams.AsNoTracking()
                .FirstOrDefaultAsync(exam => exam.Id == examId);

            if (exam == null)
            {
                throw new ArgumentNullException(nameof(Exam));
            }

            return _mapper.Map<Exam>(exam);
        }

        public async Task<List<Exam>> GetExams(int userId)
        {
            if (await UserExists(userId)  == false)
            {
                throw new ArgumentNullException(nameof(User));
            }

            var exams = await _context.Exams.AsNoTracking()
                .Where(exam => exam.UserId == userId).ToListAsync();

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
                await _context.AddAsync(examQuestionEntity);
            await Save();
            return _mapper.Map<ExamQuestion>(examQuestionCreated.Entity);
        }

        // to add questions to a new exam
        public async Task<bool> AddExamQuestions(Exam exam,List<int> questions)
        {
            //create exam
            var examEntity = _mapper.Map<ExamEntity>(exam);
            
            var examCreated = await _context.Exams.AddAsync(examEntity);
            await Save();
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

            await _context.AddRangeAsync(examQuestionsEntities);
            return await Save();
        }
        /// <summary>
        /// This method search questions on the database
        /// for a given exam.
        /// </summary>
        /// <param name="examId">integer</param>
        /// <returns>examQuestions</returns>
        public async Task<ExamQuestions> GetExamQuestions(int examId)
        {
            var examQuestion = await _context.Exams.AsNoTracking().Where(e => e.Id == examId)
                .Select(e => new
                {
                    Exam = e,
                    Questions = e.ExamQuestions.Select(eq => eq.QuestionEntity)
                }).FirstOrDefaultAsync();

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
        /// <summary>
        /// This method checks if an exam already exists on the database by id.
        /// </summary>
        /// <param name="examId">integer</param>
        /// <returns>boolean</returns>
        public async Task<bool> ExamExists(int examId) => await _context.Exams.FindAsync(examId)  != null;

        /// <summary>
        /// This method creates a new result on the database.
        /// </summary>
        /// <param name="result">result</param>
        /// <returns>boolean</returns>
        public async Task<bool> AddResults(Result result)
        {
            // maps domain model to entity
            var resultEntity = _mapper.Map<ResultEntity>(result);

            await _context.Results.AddAsync(resultEntity);

            return await Save();
        }
        /// <summary>
        /// This method search results on the database
        /// for a given exam.
        /// </summary>
        /// <param name="examId">integer</param>
        /// <returns>List<Result>()</returns>
        public async Task<List<Result>> GetResults(int examId)
        {
            if (await ExamExists(examId)  == false)
            {
                throw new ArgumentNullException(nameof(Exam));
            }

            var resultEntities = await _context.Results.AsNoTracking()
                .Where(r => r.ExamId == examId).ToListAsync();

            return _mapper.Map<List<Result>>(resultEntities);
        }
    }
}
