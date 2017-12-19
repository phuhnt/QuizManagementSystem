namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CodeTest")]
    public partial class CodeTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? Code { get; set; }

        public DateTime? CreatedDay { get; set; }

        public virtual Test Test { get; set; }
    }
}
