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
            Tests = new HashSet<Test>();
        }

        public int Id { get; set; }

        public int? SubjectsID { get; set; }

        public int? CategoryID { get; set; }

        public int? KindID { get; set; }

        public int? LevelID { get; set; }

        [StringLength(1000)]
        public string ContentQuestion { get; set; }

        [StringLength(1000)]
        public string ContentQuestionEncode { get; set; }

        [StringLength(500)]
        public string AnswerText { get; set; }

        [StringLength(500)]
        public string AnswerTextEncode { get; set; }

        [StringLength(128)]
        public string KeyAnswer { get; set; }

        public int? UserID { get; set; }

        public DateTime? DateCreated { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? Status { get; set; }

        public virtual CategoryQuiz CategoryQuiz { get; set; }

        public virtual Kind Kind { get; set; }

        public virtual Level Level { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
    }
}
