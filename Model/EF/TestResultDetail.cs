namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestResultDetail")]
    public partial class TestResultDetail
    {
        public int Id { get; set; }

        public int UserID { get; set; }

        public int TestID { get; set; }

        public int? ExamID { get; set; }

        public double? Score { get; set; }

        public int? NumberOfWrong { get; set; }

        public int? NumberOfCorrect { get; set; }

        public int? NumberOfIgnored { get; set; }

        public int? TimeToTake { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ActualTestDate { get; set; }

        public TimeSpan? ActualStartTime { get; set; }

        public TimeSpan? ActualEndTime { get; set; }

        [StringLength(500)]
        public string UserAnswer { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual Test Test { get; set; }

        public virtual User User { get; set; }
    }
}
