namespace CastService.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using CastService.Data.Common.Models;

    public class Installation : AuditInfo, IDeletableEntity
    {
        private ICollection<InstalledEquipment> instalatedEquipment;

        public Installation()
        {
            this.instalatedEquipment = new HashSet<InstalledEquipment>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string ObjectName { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string DetectedFaults { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string AdditionalActivities { get; set; }

        [Column(TypeName = "Date")]
        public DateTime InstallationDate { get; set; }

        [MaxLength(5)]
        [Required]
        public string StartTime { get; set; }

        [MaxLength(5)]
        [Required]
        public string EndTime { get; set; }

        [MaxLength(10)]
        public string GuessedTime { get; set; }

        public bool HasProtocol { get; set; }

        [MaxLength(10)]
        public string InvoiceNumber { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? InvoiceDate { get; set; }

        [MaxLength(30)]
        public string WarrantyCardNumber { get; set; }

        [MaxLength(50)]
        public string Other { get; set; }

        [Column(TypeName = "ntext")]
        public string Note { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<InstalledEquipment> InstalatedEquipment
        {
            get { return this.instalatedEquipment; }
            set { this.instalatedEquipment = value; }
        }

    }
}
