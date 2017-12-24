namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QuizManagementSystemDbContext : DbContext
    {
        public QuizManagementSystemDbContext()
            : base("name=QuizManagementSystemDbContext")
        {
        }

        public virtual DbSet<CategoryQuiz> CategoryQuizs { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<CodeTest> CodeTests { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Kind> Kinds { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionTest> QuestionTests { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ScoreLadder> ScoreLadders { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<TestResultDetail> TestResultDetails { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryQuiz>()
                .HasMany(e => e.Questions)
                .WithOptional(e => e.CategoryQuiz)
                .HasForeignKey(e => e.CategoryID);

            modelBuilder.Entity<Exam>()
                .Property(e => e.Link)
                .IsFixedLength();

            modelBuilder.Entity<Question>()
                .HasMany(e => e.QuestionTests)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Test>()
                .Property(e => e.UserID)
                .IsFixedLength();

            modelBuilder.Entity<Test>()
                .HasMany(e => e.QuestionTests)
                .WithRequired(e => e.Test)
                .HasForeignKey(e => e.QuestionID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Avatar)
                .IsFixedLength();
        }
    }
}
