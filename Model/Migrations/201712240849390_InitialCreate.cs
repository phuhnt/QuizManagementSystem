namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answer", "QuestionID", "dbo.Questions");
            DropForeignKey("dbo.Questions", "LevelID", "dbo.Levels");
            DropForeignKey("dbo.CodeTest", "TestID", "dbo.Tests");
            DropForeignKey("dbo.TestResultDetail", "TestID", "dbo.Tests");
            DropForeignKey("dbo.QuestionTest", "QuestionID", "dbo.Tests");
            DropForeignKey("dbo.Tests", "SubjectID", "dbo.Subjects");
            DropIndex("dbo.Answer", new[] { "QuestionID" });
            DropPrimaryKey("dbo.Tests");
            DropPrimaryKey("dbo.Subjects");
            AlterColumn("dbo.Levels", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Tests", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Subjects", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Levels", "Id");
            AddPrimaryKey("dbo.Tests", "Id");
            AddPrimaryKey("dbo.Subjects", "Id");
            AddForeignKey("dbo.Questions", "LevelID", "dbo.Levels", "Id");
            AddForeignKey("dbo.CodeTest", "TestID", "dbo.Tests", "Id");
            AddForeignKey("dbo.TestResultDetail", "TestID", "dbo.Tests", "Id");
            AddForeignKey("dbo.QuestionTest", "QuestionID", "dbo.Tests", "Id");
            AddForeignKey("dbo.Tests", "SubjectID", "dbo.Subjects", "Id");
            DropTable("dbo.Answer");
            DropTable("dbo.sysdiagrams");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionID = c.Int(),
                        AnswerText = c.String(maxLength: 1000),
                        CorrectAnswer = c.String(maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Tests", "SubjectID", "dbo.Subjects");
            DropForeignKey("dbo.QuestionTest", "QuestionID", "dbo.Tests");
            DropForeignKey("dbo.TestResultDetail", "TestID", "dbo.Tests");
            DropForeignKey("dbo.CodeTest", "TestID", "dbo.Tests");
            DropForeignKey("dbo.Questions", "LevelID", "dbo.Levels");
            DropPrimaryKey("dbo.Subjects");
            DropPrimaryKey("dbo.Tests");
            //DropPrimaryKey("dbo.Levels");
            AlterColumn("dbo.Subjects", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Tests", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Levels", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Subjects", "Id");
            AddPrimaryKey("dbo.Tests", "Id");
            AddPrimaryKey("dbo.Levels", "Id");
            CreateIndex("dbo.Answer", "QuestionID");
            AddForeignKey("dbo.Tests", "SubjectID", "dbo.Subjects", "Id");
            AddForeignKey("dbo.QuestionTest", "QuestionID", "dbo.Tests", "Id");
            AddForeignKey("dbo.TestResultDetail", "TestID", "dbo.Tests", "Id");
            AddForeignKey("dbo.CodeTest", "TestID", "dbo.Tests", "Id");
            AddForeignKey("dbo.Questions", "LevelID", "dbo.Levels", "Id");
            AddForeignKey("dbo.Answer", "QuestionID", "dbo.Questions", "Id");
        }
    }
}
