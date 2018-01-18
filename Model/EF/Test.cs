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
            QuestionTests = new HashSet<QuestionTest>();
            TestResultDetails = new HashSet<TestResultDetail>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int CodeTestID { get; set; }

        [NotMapped]
        public string CodeTest { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public int? SubjectID { get; set; }

        public int? NumberOfQuestions { get; set; }

        public int? Time { get; set; }

        public int? NumberOfTurns { get; set; }

        public int? ExamID { get; set; }

        public int? ScoreLadderID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(128)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? Status { get; set; }

        [NotMapped]
        public int QuizSelection { get; set; }

        public virtual Exam Exam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionTest> QuestionTests { get; set; }

        public virtual ScoreLadder ScoreLadder { get; set; }

        public virtual Subject Subject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }

        public virtual User User { get; set; }
    }
}
