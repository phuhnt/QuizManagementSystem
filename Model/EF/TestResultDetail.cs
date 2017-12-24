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

        public double? Score { get; set; }

        public int? NumberOfWrong { get; set; }

        public int? NumberOfCorrect { get; set; }

        public int? NumberOfIgnored { get; set; }

        public int? TimeToTake { get; set; }

        public DateTime? ActualStartTime { get; set; }

        public DateTime? ActualEndTime { get; set; }

        public TimeSpan? ActualExamTime { get; set; }

        public int? UserID { get; set; }

        public int? TestID { get; set; }

        public virtual Test Test { get; set; }

        public virtual User User { get; set; }
    }
}
