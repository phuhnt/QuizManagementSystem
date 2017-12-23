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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
        [Display(Name = "Tên tài khoản")]
        [StringLength(128, ErrorMessage = "Số ký tự tối đa là 128.")]
        [MinLength(6, ErrorMessage = "Tài khoản phải có ít nhất 6 ký tự.")]
        [RegularExpression("([A-Za-z0-9_@-]{6,32})", ErrorMessage = "Tên tài khoản không hợp lệ")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [RegularExpression("([A-Za-z0-9_@-]{6,32})", ErrorMessage = "Mật khẩu không hợp lệ")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string PasswordHash { get; set; }

        [RegularExpression("([A-Za-z0-9_@-]{6,32})", ErrorMessage = "Mật khẩu không hợp lệ")]
        [Display(Name = "Nhập lại mật khẩu")]
        public string ConfirmPasswordHash { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [RegularExpression("([A-Za-z0-9_@-]{6,32})", ErrorMessage = "Mật khẩu không hợp lệ")]
        public string NewPasswordHash { get; set; }

        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Địa chỉ email")]
        [StringLength(256)]
        public string Email { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Ngày sinh không hợp lệ.")]
        [Display(Name = "Ngày sinh")]
        [Column(TypeName = "date")]
        public DateTime? DayOfBirth { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Ngày tham gia")]
        public DateTime? DateOfParticipation { get; set; }

        [Display(Name = "Họ và tên")]
        [StringLength(256)]
        public string FullName { get; set; }

        [Display(Name = "Giới tính")]
        [StringLength(10)]
        public string Sex { get; set; }

        [Display(Name = "Lớp")]
        public int? ClassID { get; set; }

        [Display(Name ="Ảnh đại diện")]
        [StringLength(500)]
        public string Avatar { get; set; }

        [Display(Name = "Trạng thái tài khoản")]
        public bool? Status { get; set; }

        [Display(Name = "Loại tài khoản")]
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
