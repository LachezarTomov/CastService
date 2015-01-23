namespace CastService.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using CastService.Data.Common.Models;

    public class Instalation : AuditInfo, IDeletableEntity
    {
        private ICollection<InstalatedEquipment> instalatedEquipment;

        public Instalation()
        {
            this.instalatedEquipment = new HashSet<InstalatedEquipment>();
        }

        public int Id { get; set; }

        [MaxLength(50)]
        public string ObjectName { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string DetectedFaults { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string AdditionalActivities { get; set; }

        [Column(TypeName = "Date")]
        public DateTime InstalationDate { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        public string GuessedTime { get; set; }

        public bool HasProtocol { get; set; }

        public string InvoiceNumber { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? InvoiceDate { get; set; }

        public string WarrantyCardNumber { get; set; }

        public string Other { get; set; }

        [Column(TypeName = "ntext")]
        public string Note { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<InstalatedEquipment> InstalatedEquipment
        {
            get { return this.instalatedEquipment; }
            set { this.instalatedEquipment = value; }
        }

    }
}
