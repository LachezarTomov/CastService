namespace CastService.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using CastService.Data.Common.Models;

    public class InstalatedEquipment : AuditInfo, IDeletableEntity
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        [MaxLength(16)]
        public string SerialNumber { get; set; }

        [ForeignKey("Id")]
        public int InstalationId { get; set; }

        public virtual Instalation Instalation { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
