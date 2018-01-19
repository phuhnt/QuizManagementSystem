namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ScoreLadder")]
    public partial class ScoreLadder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ScoreLadder()
        {
            Tests = new HashSet<Test>();
        }

        public int Id { get; set; }

        [StringLength(256)]
        public string Title { get; set; }

        public double? Score { get; set; }

        [StringLength(256)]
        public string Note { get; set; }

        public double? RoundingFactor { get; set; }

        public double? ScorePassed { get; set; }

        public bool? Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
    }
}
