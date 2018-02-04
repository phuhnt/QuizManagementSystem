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
            TestResultDetails = new HashSet<TestResultDetail>();
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }

        public int? CodeTest { get; set; }

        [NotMapped]
        public string CodeTestArr { get; set; }
        [StringLength(500)]
        public string Title { get; set; }

        public int? NumberOfQuestions { get; set; }

        public int? Time { get; set; }

        public int? ExamID { get; set; }

        public int? ScoreLadderID { get; set; }

        public bool? MixAnswer { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(128)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? Status { get; set; }

        [NotMapped]
        public int QuizSelection { get; set; }  //Phương án chọn câu hỏi

        [NotMapped]
        public int FixedOrChanged { get; set; } //Phương thức cố định hay thay đổi câu hỏi

        [NotMapped]
        public int Mix { get; set; }    // Đảo câu hỏi, đảo đáp án, hay đảo cả 2

        [NotMapped]
        public List<string> UserAnswer { get; set; }
        public virtual Exam Exam { get; set; }

        public virtual ScoreLadder ScoreLadder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }
    }
}
