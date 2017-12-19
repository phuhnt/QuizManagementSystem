namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Answer")]
    public partial class Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? QuestionID { get; set; }

        [StringLength(1000)]
        public string AnswerText { get; set; }

        [StringLength(10)]
        public string CorrectAnswer { get; set; }

        public virtual Question Question { get; set; }
    }
}
