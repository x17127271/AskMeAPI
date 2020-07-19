using AskMe.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AskMe.Data.DbContexts
{
    public class AskMeDbContext : DbContext
    {
        public AskMeDbContext(DbContextOptions<AskMeDbContext> options)
            : base(options)
        {

        }

        //datasabe sets
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<SubjectEntity> Subjects { get; set; }
        public DbSet<LessonEntity> Lessons { get; set; }
        public DbSet<QuestionEntity> Questions { get; set; }
        public DbSet<AnswerEntity> Answers { get; set; }
        public DbSet<ExamEntity> Exams { get; set; }
        public DbSet<ExamsQuestions> ExamsQuestions { get; set; }
        public DbSet<ResultEntity> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                       
        }
    }
}
