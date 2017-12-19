namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Test
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Test()
        {
            CodeTests = new HashSet<CodeTest>();
            QuestionTests = new HashSet<QuestionTest>();
            TestResultDetails = new HashSet<TestResultDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int CodeTestID { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public int? SubjectID { get; set; }

        public int? NumberOfQuestions { get; set; }

        public TimeSpan? Time { get; set; }

        public int? NumberOfTurns { get; set; }

        public int? ExamID { get; set; }

        public int? ScoreLadderID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TestDay { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        [StringLength(32)]
        public string UserID { get; set; }

        public bool? Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CodeTest> CodeTests { get; set; }

        public virtual Exam Exam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionTest> QuestionTests { get; set; }

        public virtual ScoreLadder ScoreLadder { get; set; }

        public virtual Subject Subject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }
    }
}
