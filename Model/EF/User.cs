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
            Roles = new HashSet<Role>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên tài khoản")]
        [StringLength(128)]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        public string PaswordHash { get; set; }

        [NotMapped]
        [Required(ErrorMessage ="Vui lòng nhập lại mật khẩu.")]
        [Compare("PasswordHash", ErrorMessage ="Mật khẩu chưa trùng khớp.")]
        public string ConfirmPaswordHash { get; set; }

        [Display(Name = "Địa chỉ email")]
        [StringLength(256)]
        public string Email { get; set; }

        [Display(Name = "Ngày sinh")]
        [Column(TypeName = "date")]
        public DateTime? DayOfBirth { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Ngày tham gia")]
        [Column(TypeName = "date")]
        public DateTime? DateOfParticipation { get; set; }

        [Display(Name = "Họ và tên")]
        [StringLength(256)]
        public string FullName { get; set; }

        [Display(Name = "Giới tính")]
        [StringLength(10)]
        public string Sex { get; set; }

        [Display(Name = "Lớp")]
        public int? ClassID { get; set; }

        [Display(Name ="Hình đại diện")]
        [StringLength(500)]
        public string Avatar { get; set; }

        [Display(Name ="Trạng thái tài khoản")]
        public bool? Status { get; set; }

        public virtual Class Class { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Exam> Exams { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
