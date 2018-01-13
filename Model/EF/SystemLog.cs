namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemLog")]
    public partial class SystemLog
    {
        public int Id { get; set; }

        [StringLength(500)]
        public string EventName { get; set; }

        [StringLength(128)]
        public string PerformedBy { get; set; }

        public TimeSpan? ExTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExDate { get; set; }

        [StringLength(50)]
        public string ClientIP { get; set; }
    }
}
