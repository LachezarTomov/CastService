namespace CastService.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using CastService.Data.Common.Models;

    public class InstallatedEquipment : AuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EquipmentId { get; set; }

        public virtual Equipment Equipment { get; set; }

        [Required]
        public int Quantity { get; set; }

        [MaxLength(16)]
        public string SerialNumber { get; set; }

        [Required]
        public int InstallationId { get; set; }

        public virtual Installation Installation { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
