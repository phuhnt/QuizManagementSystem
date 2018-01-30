namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class Exam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam()
        {
            TestResultDetails = new HashSet<TestResultDetail>();
            Tests = new HashSet<Test>();
            Classes = new HashSet<Class>();
        }

        public int Id { get; set; }

        [StringLength(500)]
        public string Titile { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(500)]
        public string NoteEncode { get; set; }

        public int? SubjectID { get; set; }

        public int? NumberOfTurns { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        [StringLength(256)]
        public string Link { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(128)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? Status { get; set; }

        [NotMapped]
        public int[] SelectedClassID { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Class> Classes { get; set; }
    }
}
