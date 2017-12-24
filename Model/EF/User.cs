namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Exams = new HashSet<Exam>();
            Questions = new HashSet<Question>();
            TestResultDetails = new HashSet<TestResultDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string ConfirmPasswordHash { get; set; }

        public string NewPasswordHash { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DayOfBirth { get; set; }

        public string Phone { get; set; }

        public DateTime? DateOfParticipation { get; set; }

        [StringLength(256)]
        public string FullName { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        public int? ClassID { get; set; }

        [StringLength(500)]
        public string Avatar { get; set; }

        public bool? Status { get; set; }

        public int? RoleID { get; set; }

        [StringLength(128)]
        public string CreateBy { get; set; }

        [StringLength(128)]
        public string ModifiedBy { get; set; }

        public virtual Class Class { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Exam> Exams { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }
    }
}
