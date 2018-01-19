namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryQuiz",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        CreatedDay = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectsID = c.Int(),
                        CategoryID = c.Int(),
                        KindID = c.Int(),
                        LevelID = c.Int(),
                        ContentQuestion = c.String(maxLength: 1000),
                        ContentQuestionEncode = c.String(maxLength: 1000),
                        AnswerText = c.String(maxLength: 500),
                        AnswerTextEncode = c.String(maxLength: 500),
                        KeyAnswer = c.String(maxLength: 128),
                        UserID = c.Int(),
                        DateCreated = c.DateTime(),
                        ModifiedBy = c.String(),
                        ModifiedDate = c.DateTime(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kinds", t => t.KindID)
                .ForeignKey("dbo.Levels", t => t.LevelID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .ForeignKey("dbo.CategoryQuiz", t => t.CategoryID)
                .Index(t => t.CategoryID)
                .Index(t => t.KindID)
                .Index(t => t.LevelID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Kinds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        Note = c.String(maxLength: 256),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CodeTest = c.Int(nullable: false),
                        Title = c.String(maxLength: 500),
                        SubjectID = c.Int(),
                        NumberOfQuestions = c.Int(),
                        Time = c.Int(),
                        NumberOfTurns = c.Int(),
                        ExamID = c.Int(),
                        ScoreLadderID = c.Int(),
                        FromDate = c.DateTime(storeType: "date"),
                        ToDate = c.DateTime(storeType: "date"),
                        StartTime = c.Time(precision: 7),
                        EndTime = c.Time(precision: 7),
                        CreatedBy = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Exams", t => t.ExamID)
                .ForeignKey("dbo.ScoreLadder", t => t.ScoreLadderID)
                .Index(t => t.SubjectID)
                .Index(t => t.ExamID)
                .Index(t => t.ScoreLadderID)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Titile = c.String(maxLength: 500),
                        Note = c.String(maxLength: 500),
                        NoteEncode = c.String(maxLength: 500),
                        Link = c.String(maxLength: 256, fixedLength: true),
                        UserID = c.Int(),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.String(maxLength: 128),
                        ModifiedDate = c.DateTime(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        GradeID = c.Int(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grades", t => t.GradeID)
                .Index(t => t.GradeID);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GradeName = c.String(maxLength: 50),
                        SchoolYearID = c.Int(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SchoolYear", t => t.SchoolYearID)
                .Index(t => t.SchoolYearID);
            
            CreateTable(
                "dbo.SchoolYear",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartYear = c.Int(nullable: false),
                        EndYear = c.Int(nullable: false),
                        NameOfSchoolYear = c.String(maxLength: 50),
                        bit = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Note = c.String(maxLength: 256),
                        Status = c.Boolean(),
                        GradeID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Grades", t => t.GradeID)
                .Index(t => t.GradeID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 128),
                        PasswordHash = c.String(),
                        ConfirmPasswordHash = c.String(),
                        NewPasswordHash = c.String(),
                        Email = c.String(maxLength: 256),
                        DayOfBirth = c.DateTime(storeType: "date"),
                        Phone = c.String(),
                        DateOfParticipation = c.DateTime(),
                        FullName = c.String(maxLength: 256),
                        Sex = c.String(maxLength: 10),
                        ClassID = c.Int(),
                        Avatar = c.String(maxLength: 500, fixedLength: true),
                        Status = c.Boolean(),
                        RoleID = c.Int(),
                        CreateBy = c.String(maxLength: 128),
                        ModifiedBy = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class", t => t.ClassID)
                .ForeignKey("dbo.Roles", t => t.RoleID)
                .Index(t => t.ClassID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestResultDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        TestID = c.Int(nullable: false),
                        ExamID = c.Int(),
                        Score = c.Double(),
                        NumberOfWrong = c.Int(),
                        NumberOfCorrect = c.Int(),
                        NumberOfIgnored = c.Int(),
                        TimeToTake = c.Int(),
                        ActualTestDate = c.DateTime(storeType: "date"),
                        ActualStartTime = c.Time(precision: 7),
                        ActualEndTime = c.Time(precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exams", t => t.ExamID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .ForeignKey("dbo.Tests", t => t.TestID)
                .Index(t => t.UserID)
                .Index(t => t.TestID)
                .Index(t => t.ExamID);
            
            CreateTable(
                "dbo.ScoreLadder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 256),
                        Score = c.Double(),
                        Note = c.String(maxLength: 256),
                        RoundingFactor = c.Double(),
                        ScorePassed = c.Double(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventName = c.String(maxLength: 500),
                        PerformedBy = c.String(maxLength: 128),
                        ExTime = c.Time(precision: 7),
                        ExDate = c.DateTime(storeType: "date"),
                        ClientIP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Examinee",
                c => new
                    {
                        ClassID = c.Int(nullable: false),
                        ExamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClassID, t.ExamID })
                .ForeignKey("dbo.Class", t => t.ClassID, cascadeDelete: true)
                .ForeignKey("dbo.Exams", t => t.ExamID, cascadeDelete: true)
                .Index(t => t.ClassID)
                .Index(t => t.ExamID);
            
            CreateTable(
                "dbo.QuizTest",
                c => new
                    {
                        TestID = c.Int(nullable: false),
                        QuizID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TestID, t.QuizID })
                .ForeignKey("dbo.Tests", t => t.TestID, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuizID, cascadeDelete: true)
                .Index(t => t.TestID)
                .Index(t => t.QuizID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Questions", "CategoryID", "dbo.CategoryQuiz");
            DropForeignKey("dbo.TestResultDetail", "TestID", "dbo.Tests");
            DropForeignKey("dbo.Tests", "ScoreLadderID", "dbo.ScoreLadder");
            DropForeignKey("dbo.QuizTest", "QuizID", "dbo.Questions");
            DropForeignKey("dbo.QuizTest", "TestID", "dbo.Tests");
            DropForeignKey("dbo.Tests", "ExamID", "dbo.Exams");
            DropForeignKey("dbo.Tests", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.TestResultDetail", "UserID", "dbo.Users");
            DropForeignKey("dbo.TestResultDetail", "ExamID", "dbo.Exams");
            DropForeignKey("dbo.Users", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Questions", "UserID", "dbo.Users");
            DropForeignKey("dbo.Exams", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "ClassID", "dbo.Class");
            DropForeignKey("dbo.Tests", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "GradeID", "dbo.Grades");
            DropForeignKey("dbo.Grades", "SchoolYearID", "dbo.SchoolYear");
            DropForeignKey("dbo.Class", "GradeID", "dbo.Grades");
            DropForeignKey("dbo.Examinee", "ExamID", "dbo.Exams");
            DropForeignKey("dbo.Examinee", "ClassID", "dbo.Class");
            DropForeignKey("dbo.Questions", "LevelID", "dbo.Levels");
            DropForeignKey("dbo.Questions", "KindID", "dbo.Kinds");
            DropIndex("dbo.QuizTest", new[] { "QuizID" });
            DropIndex("dbo.QuizTest", new[] { "TestID" });
            DropIndex("dbo.Examinee", new[] { "ExamID" });
            DropIndex("dbo.Examinee", new[] { "ClassID" });
            DropIndex("dbo.TestResultDetail", new[] { "ExamID" });
            DropIndex("dbo.TestResultDetail", new[] { "TestID" });
            DropIndex("dbo.TestResultDetail", new[] { "UserID" });
            DropIndex("dbo.Users", new[] { "RoleID" });
            DropIndex("dbo.Users", new[] { "ClassID" });
            DropIndex("dbo.Subjects", new[] { "GradeID" });
            DropIndex("dbo.Grades", new[] { "SchoolYearID" });
            DropIndex("dbo.Class", new[] { "GradeID" });
            DropIndex("dbo.Exams", new[] { "UserID" });
            DropIndex("dbo.Tests", new[] { "CreatedBy" });
            DropIndex("dbo.Tests", new[] { "ScoreLadderID" });
            DropIndex("dbo.Tests", new[] { "ExamID" });
            DropIndex("dbo.Tests", new[] { "SubjectID" });
            DropIndex("dbo.Questions", new[] { "UserID" });
            DropIndex("dbo.Questions", new[] { "LevelID" });
            DropIndex("dbo.Questions", new[] { "KindID" });
            DropIndex("dbo.Questions", new[] { "CategoryID" });
            DropTable("dbo.QuizTest");
            DropTable("dbo.Examinee");
            DropTable("dbo.SystemLog");
            DropTable("dbo.ScoreLadder");
            DropTable("dbo.TestResultDetail");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Subjects");
            DropTable("dbo.SchoolYear");
            DropTable("dbo.Grades");
            DropTable("dbo.Class");
            DropTable("dbo.Exams");
            DropTable("dbo.Tests");
            DropTable("dbo.Levels");
            DropTable("dbo.Kinds");
            DropTable("dbo.Questions");
            DropTable("dbo.CategoryQuiz");
        }
    }
}
