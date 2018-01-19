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
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Kind> Kinds { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SchoolYear> SchoolYears { get; set; }
        public virtual DbSet<ScoreLadder> ScoreLadders { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SystemLog> SystemLogs { get; set; }
        public virtual DbSet<TestResultDetail> TestResultDetails { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryQuiz>()
                .HasMany(e => e.Questions)
                .WithOptional(e => e.CategoryQuiz)
                .HasForeignKey(e => e.CategoryID);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Exams)
                .WithMany(e => e.Classes)
                .Map(m => m.ToTable("Examinee").MapLeftKey("ClassID").MapRightKey("ExamID"));

            modelBuilder.Entity<Exam>()
                .Property(e => e.Link)
                .IsFixedLength();

            modelBuilder.Entity<Test>()
                .HasMany(e => e.Quizs)
                .WithMany(e => e.Tests)
                .Map(m => m.ToTable("QuizTest").MapLeftKey("TestID").MapRightKey("QuizID"));

            modelBuilder.Entity<Test>()
                .HasMany(e => e.TestResultDetails)
                .WithRequired(e => e.Test)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Avatar)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.TestResultDetails)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tests)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.CreatedBy);
        }
    }
}
