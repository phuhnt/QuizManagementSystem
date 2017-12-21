﻿namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

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

        [Required(ErrorMessage = "Tên tài khoản không được để trống.")]
        [Display(Name = "Tên tài khoản")]
        [StringLength(128, ErrorMessage = "Số ký tự tối đa là 128.")]
        [MinLength(6, ErrorMessage = "Tài khoản phải có ít nhất 6 ký tự.")]
        [RegularExpression("([A-Za-z0-9_@]{6,32})", ErrorMessage = "Tên tài khoản không hợp lệ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password, ErrorMessage = "Mật khẩu không hợp lệ.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [NotMapped]
        [DataType(DataType.Password, ErrorMessage = "Mật khẩu không hợp lệ.")]
        [System.ComponentModel.DataAnnotations.Compare("PasswordHash", ErrorMessage = "Mật khẩu chưa trùng khớp.")]
        public string ConfirmPaswordHash { get; set; }

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

        [Display(Name = "Hình đại diện")]
        [StringLength(500)]
        public string Avatar { get; set; }

        [Display(Name = "Trạng thái tài khoản")]
        public bool? Status { get; set; }

        public string SelectedRole { get; set; }

        public string SelectedStatus { get; set; }

        public virtual Class Class { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Exam> Exams { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }

        [Display(Name = "Loại tài khoản")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }
    }
}
