namespace CastService.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using CastService.Data.Common.Models;
    
    public class Protocol: AuditInfo, IDeletableEntity
    {
        private ICollection<ChangedEquipment> changedEquipment;

        public Protocol()
        {
            this.changedEquipment = new HashSet<ChangedEquipment>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        public string ObjectType { get; set; }

        [MaxLength(16)]
        [Required]
        public string ObjectNumber { get; set; }

        [MaxLength(30)]
        public string ObjectDriver { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string PerformedDiagnostic { get; set; }

        [Column(TypeName = "ntext")]
        [MaxLength]
        public string DetectedFauls { get; set; }

        [Column(TypeName = "Date")]
        public DateTime ProtocolDate { get; set; }

        [MaxLength(5)]
        [Required]
        public string StartTime { get; set; }

        [MaxLength(5)]
        [Required]
        public string EndTime { get; set; }

        public bool IsWarrantyService { get; set; }

        public bool WithSubscriptionService { get; set; }

        [MaxLength(30)]
        public string PersonMadeRequest { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? RequestDate { get; set; }

        public bool HasCustomerProtocol { get; set; }

        [MaxLength(10)]
        public string InvoiceNumber { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? InvoiceDate { get; set; }

        [MaxLength(50)]
        public string Other { get; set; }

        [MaxLength(30)]
        public string WarrantyCardNumber { get; set; }

        public int WorkInHours { get; set; }

        public decimal PricePerHour { get; set; }

        public decimal PriceForChangedEguipment { get; set; }

        public decimal DistanceInKm { get; set; }

        public decimal PricePerKm { get; set; }

        [Column(TypeName = "ntext")]
        public string Note { get; set; }

        [MaxLength(30)]
        public string CustomerRepresentative { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        [Index]
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<ChangedEquipment> ChangedEquipment
        {
            get { return this.changedEquipment; }
            set { this.changedEquipment = value; }
        }
    }
}
