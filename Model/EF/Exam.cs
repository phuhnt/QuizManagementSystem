namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Exam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Exam()
        {
            Tests = new HashSet<Test>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(128)]
        public string Titile { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public bool? Status { get; set; }

        [StringLength(256)]
        public string Link { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(128)]
        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
    }
}
