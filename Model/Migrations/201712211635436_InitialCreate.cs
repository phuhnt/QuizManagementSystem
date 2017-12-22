namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        QuestionID = c.Int(),
                        AnswerText = c.String(maxLength: 1000),
                        CorrectAnswer = c.String(maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionID)
                .Index(t => t.QuestionID);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SubjectsID = c.Int(),
                        Category = c.Int(),
                        KindID = c.Int(),
                        LevelID = c.Int(),
                        ContentQuestion = c.String(maxLength: 1000),
                        AnswerID = c.Int(),
                        UserID = c.Int(),
                        DateCreated = c.DateTime(),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kinds", t => t.KindID)
                .ForeignKey("dbo.Levels", t => t.LevelID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.KindID)
                .Index(t => t.LevelID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Kinds",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Levels",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(maxLength: 128),
                        Note = c.String(maxLength: 256),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionTest",
                c => new
                    {
                        TestID = c.Int(nullable: false),
                        QuestionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TestID, t.QuestionID })
                .ForeignKey("dbo.Tests", t => t.QuestionID)
                .ForeignKey("dbo.Questions", t => t.QuestionID)
                .Index(t => t.QuestionID);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CodeTestID = c.Int(nullable: false),
                        Title = c.String(maxLength: 500),
                        SubjectID = c.Int(),
                        NumberOfQuestions = c.Int(),
                        Time = c.Time(precision: 7),
                        NumberOfTurns = c.Int(),
                        ExamID = c.Int(),
                        ScoreLadderID = c.Int(),
                        TestDay = c.DateTime(storeType: "date"),
                        StartTime = c.Time(precision: 7),
                        EndTime = c.Time(precision: 7),
                        UserID = c.String(maxLength: 32, fixedLength: true),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exams", t => t.ExamID)
                .ForeignKey("dbo.Subjects", t => t.SubjectID)
                .ForeignKey("dbo.ScoreLadder", t => t.ScoreLadderID)
                .Index(t => t.SubjectID)
                .Index(t => t.ExamID)
                .Index(t => t.ScoreLadderID);
            
            CreateTable(
                "dbo.CodeTest",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.Int(),
                        CreatedDay = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tests", t => t.Code)
                .Index(t => t.Code);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Titile = c.String(maxLength: 128),
                        Note = c.String(maxLength: 500),
                        Status = c.Boolean(),
                        Link = c.String(maxLength: 256, fixedLength: true),
                        UserID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 128),
                        PasswordHash = c.String(nullable: false),
                        Email = c.String(maxLength: 256),
                        DayOfBirth = c.DateTime(storeType: "date"),
                        Phone = c.String(),
                        DateOfParticipation = c.DateTime(storeType: "date"),
                        FullName = c.String(maxLength: 256),
                        Sex = c.String(maxLength: 10),
                        ClassID = c.Int(),
                        Avatar = c.String(maxLength: 500, fixedLength: true),
                        Status = c.Boolean(),
                        SelectedRole = c.String(),
                        SelectedStatus = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class", t => t.ClassID)
                .Index(t => t.ClassID);
            
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 128),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(maxLength: 256),
                        Note = c.String(maxLength: 256),
                        Status = c.Boolean(),
                        ClassID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class", t => t.ClassID)
                .Index(t => t.ClassID);
            
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
                        Id = c.Int(nullable: false),
                        Score = c.Double(),
                        NumberOfWrong = c.Int(),
                        NumberOfCorrect = c.Int(),
                        NumberOfIgnored = c.Int(),
                        TimeToTake = c.Int(),
                        ActualStartTime = c.DateTime(),
                        ActualEndTime = c.DateTime(),
                        ActualExamTime = c.Time(precision: 7),
                        UserID = c.Int(),
                        TestID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tests", t => t.TestID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.TestID);
            
            CreateTable(
                "dbo.ScoreLadder",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(maxLength: 256),
                        Score = c.Double(),
                        Note = c.String(maxLength: 256),
                        Status = c.Boolean(),
                        RoundingFactor = c.Double(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleID, t.UserID })
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.RoleID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionTest", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.Tests", "ScoreLadderID", "dbo.ScoreLadder");
            DropForeignKey("dbo.QuestionTest", "QuestionID", "dbo.Tests");
            DropForeignKey("dbo.TestResultDetail", "UserID", "dbo.Users");
            DropForeignKey("dbo.TestResultDetail", "TestID", "dbo.Tests");
            DropForeignKey("dbo.UserRoles", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Questions", "UserID", "dbo.Users");
            DropForeignKey("dbo.Exams", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "ClassID", "dbo.Class");
            DropForeignKey("dbo.Tests", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.Subjects", "ClassID", "dbo.Class");
            DropForeignKey("dbo.Tests", "ExamID", "dbo.Exams");
            DropForeignKey("dbo.CodeTest", "Code", "dbo.Tests");
            DropForeignKey("dbo.Questions", "LevelID", "dbo.Levels");
            DropForeignKey("dbo.Questions", "KindID", "dbo.Kinds");
            DropForeignKey("dbo.Answer", "QuestionID", "dbo.Questions");
            DropIndex("dbo.UserRoles", new[] { "UserID" });
            DropIndex("dbo.UserRoles", new[] { "RoleID" });
            DropIndex("dbo.TestResultDetail", new[] { "TestID" });
            DropIndex("dbo.TestResultDetail", new[] { "UserID" });
            DropIndex("dbo.Subjects", new[] { "ClassID" });
            DropIndex("dbo.Users", new[] { "ClassID" });
            DropIndex("dbo.Exams", new[] { "UserID" });
            DropIndex("dbo.CodeTest", new[] { "Code" });
            DropIndex("dbo.Tests", new[] { "ScoreLadderID" });
            DropIndex("dbo.Tests", new[] { "ExamID" });
            DropIndex("dbo.Tests", new[] { "SubjectID" });
            DropIndex("dbo.QuestionTest", new[] { "QuestionID" });
            DropIndex("dbo.Questions", new[] { "UserID" });
            DropIndex("dbo.Questions", new[] { "LevelID" });
            DropIndex("dbo.Questions", new[] { "KindID" });
            DropIndex("dbo.Answer", new[] { "QuestionID" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.ScoreLadder");
            DropTable("dbo.TestResultDetail");
            DropTable("dbo.Roles");
            DropTable("dbo.Subjects");
            DropTable("dbo.Class");
            DropTable("dbo.Users");
            DropTable("dbo.Exams");
            DropTable("dbo.CodeTest");
            DropTable("dbo.Tests");
            DropTable("dbo.QuestionTest");
            DropTable("dbo.Levels");
            DropTable("dbo.Kinds");
            DropTable("dbo.Questions");
            DropTable("dbo.Answer");
        }
    }
}
