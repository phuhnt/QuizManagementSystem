namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            Answers = new HashSet<Answer>();
            QuestionTests = new HashSet<QuestionTest>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? SubjectsID { get; set; }

        [Display(Name ="Loại câu hỏi")]
        public int? CategoryID { get; set; }

        public int? KindID { get; set; }

        public int? LevelID { get; set; }

        [Display(Name ="Nội dung câu hỏi")]
        [StringLength(1000)]
        public string ContentQuestion { get; set; }

        public int? AnswerID { get; set; }

        public int? UserID { get; set; }

        public DateTime? DateCreated { get; set; }

        [Display(Name ="Trạng thái")]
        public bool? Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual CategoryQuiz CategoryQuiz { get; set; }

        public virtual Kind Kind { get; set; }

        public virtual Level Level { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuestionTest> QuestionTests { get; set; }
    }
}
