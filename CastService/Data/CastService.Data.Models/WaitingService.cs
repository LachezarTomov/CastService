namespace CastService.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using CastService.Data.Common.Models;

    public class WaitingService : AuditInfo, IDeletableEntity
    {
      
        [Key]
        public int Id { get; set; }

        [MaxLength(16)]
        [Required]
        public string ObjectNumber { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Column(TypeName = "Date")]
        public DateTime RequestDate { get; set; }

        [MaxLength(50)]
        public string SubmittedInfo { get; set; }

        [Column(TypeName = "ntext")]
        public string ProblemDescription { get; set; }

        [Column(TypeName = "Date")]
        public DateTime PlannedDate { get; set; }

        public string PlannedSpecialist { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
